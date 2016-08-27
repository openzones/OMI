using OMInsurance.BusinessLogic.Core;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OMInsurance.Log
{
    public class LogMessageQueue
    {
        /// <summary>
		/// Size of cache of queue.
		/// </summary>
		private const int EventGroupIdsCacheSize = 20;

        private List<LogMessage> _eventsWaitingQueue;
        private List<LogMessage> _eventsWorkQueue;

		private SpinLock _eventsLock;
		private bool _processingEventsWorkQueue;

		private SpinLock _eventGroupIdsLock;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public LogMessageQueue()
		{
            _eventsWaitingQueue = new List<LogMessage>();
            _eventsWorkQueue = new List<LogMessage>();
			_processingEventsWorkQueue = false;
			_eventsLock = new SpinLock(false);

			_eventGroupIdsLock = new SpinLock(false);
		}

		/// <summary>
		/// Finalizer. Writes all unprocessed events to log message.
		/// </summary>
        ~LogMessageQueue()
		{
            List<LogMessage> unsavedEventsData = new List<LogMessage>();
			if (_eventsWaitingQueue != null)
			{
				unsavedEventsData.AddRange(_eventsWaitingQueue);
			}
			if (_eventsWorkQueue != null)
			{
				unsavedEventsData.AddRange(_eventsWorkQueue);
			}

			if (unsavedEventsData.Count > 0)
			{
				StringBuilder logMessage = new StringBuilder();
				logMessage.AppendLine("The following service audit events have not been saved due to forced service stop:");

                foreach (LogMessage eventData in unsavedEventsData)
				{
					logMessage.AppendLine(eventData.ToString());
				}

				FileLog.Fatal(logMessage.ToString());
			}
		}

		/// <summary>
		/// Enqueues new audit event for saving.
		/// </summary>
		/// <param name="eventData">Audit event data.</param>
		public void EnqueueEvent(LogMessage eventData)
		{
			if (eventData == null)
			{
				throw new ArgumentNullException("eventData");
			}

			bool lockTaken = false;
			_eventsLock.Enter(ref lockTaken);
			try
			{
				_eventsWaitingQueue.Add(eventData);
				if (!_processingEventsWorkQueue)
				{
					_processingEventsWorkQueue = true;
					SwapEventsQueues();
					ThreadPool.QueueUserWorkItem(ProcessEventsWorkQueue);
				}
			}
			finally
			{
				if (lockTaken)
				{
					_eventsLock.Exit(true);
				}
			}
		}

		/// <summary>
		/// Swaps waiting and working events queues.
		/// </summary>
		/// <remarks>This method is not thread-safe, so it should be called only inside events lock.</remarks>
		private void SwapEventsQueues()
		{
			try
			{
                List<LogMessage> oldWorkQueue = _eventsWorkQueue;
				_eventsWorkQueue = _eventsWaitingQueue;
				_eventsWaitingQueue = oldWorkQueue;
			}
			catch (ThreadAbortException)
			{
				// If queues swap was interrupped, then re-create waiting queue from scratch
				if (object.ReferenceEquals(_eventsWorkQueue, _eventsWaitingQueue))
				{
                    _eventsWaitingQueue = new List<LogMessage>();
				}
				throw;
			}
		}

		/// <summary>
		/// Takes events from working queue and saves them to the database.
		/// </summary>
		/// <param name="state">Ignored async state.</param>
		private void ProcessEventsWorkQueue(object state = null)
		{
			try
			{
				while (_eventsWorkQueue.Count > 0)
				{
					try
					{
						LogBusinessLogic.Create(_eventsWorkQueue);
					}
					catch (Exception ex)
					{
						// Write unsaved events to the log
						if (_eventsWorkQueue.Count > 0)
						{
							StringBuilder logMessage = new StringBuilder();
							logMessage.AppendLine("Some of the following errors were not saved:");

							foreach (LogMessage eventData in _eventsWorkQueue)
							{
								logMessage.AppendLine(eventData.ToString());
							}

							FileLog.Fatal(logMessage.ToString(), ex);
						}
					}

					// Mark work queue as completed; if waiting queue is not empty, then swap queues and continue processing
					bool lockTaken = false;
					_eventsLock.Enter(ref lockTaken);
					try
					{
						_eventsWorkQueue.Clear();

						if (_eventsWaitingQueue.Count > 0)
						{
							SwapEventsQueues();
						}
						else
						{
							_processingEventsWorkQueue = false;
							return;
						}
					}
					finally
					{
						if (lockTaken)
						{
							_eventsLock.Exit(true);
						}
					}
				}
			}
			catch (Exception ex)
			{
				// Reset processing flag, so other thread can start processing when it will be possible
				Volatile.Write(ref _processingEventsWorkQueue, false);
				FileLog.Fatal(ex);
			}
		}
    }
}
