using OMInsurance.Configuration;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace OMInsurance.WebApps.Models
{
    public class FundFileHistoryModel
    {

        public FundFileHistoryModel(FundFileHistory item, List<User> listUser, List<ReferenceItem> listStatus)
        {
            StatusID = item.StatusID;
            Status = listStatus.Where(a => a.Id == item.StatusID).Select(b => b.Name).FirstOrDefault();
            Date = item.Date;
            UserID = item.UserID;
            UserName = listUser.Where(a => a.Id == item.UserID).FirstOrDefault().Fullname;
            FileName = item.FileName;
            FileUrl = item.FileUrl;
            IsExistFile = System.IO.File.Exists(Path.Combine(ConfiguraionProvider.FileStorageFolder, item.FileUrl));
            if (IsExistFile)
            {
                Size = GetFileSizeToString((float) new System.IO.FileInfo(Path.Combine(ConfiguraionProvider.FileStorageFolder, item.FileUrl)).Length);
            }
        }

        public FundFileHistoryModel(long count)
        {
            this.StatusID = StatusID;
            Status = Status;
            Date = Date;
            UserID = UserID;
            UserName = UserName;
            FileName = FileName;
            FileUrl = FileUrl;
            IsExistFile = IsExistFile;
            Size = Size;
            Count = count;
        }

        public long StatusID { get; set; }

        [DisplayName("Статус")]
        public string Status { get; set; }

        [DisplayName("Дата создания")]
        public DateTime Date { get; set; }
        public long UserID { get; set; }

        [DisplayName("Пользователь")]
        public string UserName { get; set; }

        [DisplayName("Имя файла")]
        public string FileName { get; set; }

        [DisplayName("Ссылка")]
        public string FileUrl { get; set; }

        //существует ли файл на диске?
        public bool IsExistFile { get; set; }

        [DisplayName("Размер")]
        public string Size { get; set; }

        [DisplayName("Кол-во заявок внутри")]
        public long Count { get; set; }


        private string GetFileSizeToString(float size)
        {
            if ((size / 1024 / 1024 / 1024) > 1)
            {
                return string.Format("{0:F2} Гб", size / 1024 / 1024 / 1024);
            }
            else
            {
                if ((size / 1024 / 1024) > 1)
                {
                    return string.Format("{0:F2} Мб", size / 1024 / 1024);
                }
                else
                {
                    if ((size / 1024) > 1)
                    {
                        return string.Format("{0:F2} Кб", size / 1024);
                    }
                    else
                    {
                        return string.Format("{0:F2} байт", size);
                    }
                }
            }
        }

    }
}