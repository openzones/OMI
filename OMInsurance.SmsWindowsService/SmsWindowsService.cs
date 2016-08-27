using System;
using System.IO;
using System.ServiceProcess;
using System.Collections.Generic;
using OMInsurance.SmsSender;
using OMInsurance.SmsSender.ServiceReferenceSms;
using OMInsurance.BusinessLogic;
using OMInsurance.Interfaces;
using OMInsurance.Entities.SMS;
using OMInsurance.Entities.Core;
using Quartz;
using Quartz.Impl;
using System.Threading;

namespace OMInsurance.SmsWindowsService
{
    public partial class SmsWindowsService : ServiceBase
    {
        public SmsWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                JobScheduler.Start();
                WriteFileLog("Service стартовал " + DateTime.Now);
            }
            catch (Exception exp)
            {
                WriteFileLog("Scheduler не запустился");
                WriteFileLog(exp.Message);
            }
        }

        protected override void OnStop()
        {
            WriteFileLog("Service остановлен " + DateTime.Now);
        }

        public static void WriteFileLog(string log)
        {
            StreamWriter file;
            using (file = new StreamWriter(new FileStream(Constants.LogFileSmsWindowsService, System.IO.FileMode.Append)))
            {
                file.WriteLine(log);
                file.Flush();
                file.Close();
            }
        }

        /// <summary>
        /// Собираем данные в главную таблицу SMSBase
        /// </summary>
        public class Job1 : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                try
                {
                    ISmsBusinessLogic smsBusinessLogic;
                    smsBusinessLogic = new SmsBusinessLogic();
                    SmsTemplate sms = smsBusinessLogic.SmsTemplate_Get();
                    SmsBase.SmsBaseSet set = new SmsBase.SmsBaseSet(sms);
                    smsBusinessLogic.SMSBaseSet(set);
                    WriteFileLog("Job1 выполнен " + DateTime.Now);
                }
                catch (Exception exp)
                {
                    WriteFileLog("Job1 не выполнен " + DateTime.Now);
                    WriteFileLog(exp.Message);
                }
            }
        }

        /// <summary>
        /// Отправляем смс
        /// </summary>
        public class Job2 : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                try
                {
                    ISmsBusinessLogic smsBusinessLogic;
                    smsBusinessLogic = new SmsBusinessLogic();
                    List<SMSMessage> listSms = new List<SMSMessage>();
                    listSms = smsBusinessLogic.GetList(0);
                    if(listSms != null && listSms.Count > 0)
                    {
                        //Отправляем по 10 штук с задержкой в 10 сек
                        int count = 10;
                        int delay = 10000;
                        List<SMSMessage> sendList = new List<SMSMessage>();

                        int i = 0;
                        foreach(var item in listSms)
                        {
                            sendList.Add(item);
                            if (i < count)
                            {
                                i++;
                            }
                            else
                            {
                                i = 0;
                                List<SmsResult> results = SmsSender.SmsSender.Send(sendList);
                                if (results != null && results.Count > 0)
                                {
                                    smsBusinessLogic.SetMessageResult(results);
                                    WriteFileLog("Отправлено " + results.Count + " смс. " + DateTime.Now);
                                }
                                sendList.Clear();
                                Thread.Sleep(delay);
                            }
                        }
                        if (sendList != null && sendList.Count > 0)
                        {
                            List<SmsResult> resultLast = SmsSender.SmsSender.Send(sendList);
                            if (resultLast != null && resultLast.Count > 0)
                            {
                                smsBusinessLogic.SetMessageResult(resultLast);
                                WriteFileLog("Отправлено " + resultLast.Count + " смс. " + DateTime.Now);
                            }
                        } 
                    }
                    else
                    {
                        WriteFileLog("Рассылка(Job2) не началась, т.к. список получателей пуст. " + DateTime.Now);
                    }
                }
                catch (Exception exp)
                {
                    WriteFileLog("Рассылка(Job2) не выполнена " + DateTime.Now);
                    WriteFileLog(exp.Message);
                }
            }
        }

        public class JobScheduler
        {
            public static void Start()
            {
                IScheduler scheduler1 = StdSchedulerFactory.GetDefaultScheduler();
                scheduler1.Start();
                IScheduler scheduler2 = StdSchedulerFactory.GetDefaultScheduler();
                scheduler2.Start();

                //Запускаем каждые 3 часа
                IJobDetail job = JobBuilder.Create<Job1>()
                    .Build();
                ITrigger trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithDailyTimeIntervalSchedule(x => x.WithIntervalInHours(3)
                    .OnEveryDay())
                    .Build();
                scheduler1.ScheduleJob(job, trigger);

                //Рассылаем смс в 10.00 в Вт, Ср, Чт
                IJobDetail job2 = JobBuilder.Create<Job2>()
                    .Build();
                ITrigger hour = TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule
                      (s =>
                        s.OnDaysOfTheWeek(new DayOfWeek[3] { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday })
                        .WithIntervalInHours(24)
                        //.OnEveryDay()
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(10, 00))
                      )
                    .Build();
                scheduler2.ScheduleJob(job2, hour);

                //IJobDetail job_1 = JobBuilder.Create<Job1>()
                //    .Build();
                //ITrigger trigger_1 = TriggerBuilder.Create()
                //    .StartNow()
                //    .WithSimpleSchedule(x => x
                //        .WithIntervalInSeconds(15)
                //        .RepeatForever())
                //    .Build();
                //scheduler1.ScheduleJob(job_1, trigger_1);

                //Запускаем в 00:10 - формирование основной таблицы SMS
                //IJobDetail job = JobBuilder.Create<Job1>()
                //    .Build();
                //ITrigger trigger = TriggerBuilder.Create()
                //   .WithDailyTimeIntervalSchedule
                //      (s =>
                //         s.WithIntervalInHours(24)
                //        .OnEveryDay()
                //        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 10))
                //      )
                //    .Build();
                //scheduler.ScheduleJob(job, trigger);

                //IJobDetail job_2 = JobBuilder.Create<Job2>()
                //    .Build();
                //ITrigger trigger_2 = TriggerBuilder.Create()
                //    .StartNow()
                //    .WithSimpleSchedule(x => x
                //        .WithIntervalInSeconds(70)
                //        .RepeatForever())
                //    .Build();
                //scheduler2.ScheduleJob(job_2, trigger_2);

            }
        }
    }
}
