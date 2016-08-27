using OMInsurance.Entities;
using System;
using System.ComponentModel;

namespace OMInsurance.WebApps.Models
{
    public class ClientNomernikModel
    {
        public ClientNomernikModel(NomernikForClient item)
        {
            this.S_CARD = item.S_CARD;
            this.N_CARD = item.N_CARD;
            this.ENP = item.ENP;
            this.VSN = item.VSN;
            this.LPU_ID = item.LPU_ID;
            this.DATE_IN = item.DATE_IN;
            this.SPOS = item.SPOS;
            this.ClientID = item.ClientID;
            this.LoadDate = item.LoadDate;
            this.FileDate = item.FileDate;
            this.FIO = item.Lastname + " " + item.Firstname.Remove(1) + ". " + item.Secondname.Remove(1) + ".";
            this.SCENARIO = item.SCENARIO;
            this.QZ = item.QZ;
            this.DATE_END = item.DATE_END;
            this.DATE_ARC = item.DATE_ARC;
            this.IST = item.IST;
        }

        //[DisplayName("")]
        public string S_CARD { get; set; }

        public string N_CARD { get; set; }

        public string ENP { get; set; }

        public string VSN { get; set; }

        public long? LPU_ID { get; set; }

        public DateTime? DATE_IN { get; set; }

        public int? SPOS { get; set; }

        public long? ClientID { get; set; }

        [DisplayName("Сценарий")]
        public string SCENARIO { get; set; }
        public long? QZ { get; set; }
        public DateTime? DATE_END { get; set; }
        public DateTime? DATE_ARC { get; set; }
        public string IST { get; set; }

        [DisplayName("Дата загрузки")]
        public DateTime LoadDate { get; set; }

        [DisplayName("Дата номерника")]
        public DateTime FileDate { get; set; }

        [DisplayName("Загрузил номерник")]
        public string FIO { get; set; }
    }
}