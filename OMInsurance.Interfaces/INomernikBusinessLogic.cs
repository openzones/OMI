using OMInsurance.Entities;
using System.Collections.Generic;

namespace OMInsurance.Interfaces
{
    public interface INomernikBusinessLogic
    {
        List<NOMP> GetDataFromNOMPdbf(string dbfFilePath, Nomernik.History nompHistory);
        List<STOP> GetDataFromSTOPdbf(string dbfFilePath, Nomernik.History stopHistory);

        /// <summary>
        /// Получаем общее кол-во записей(строк) в dbf
        /// </summary>
        /// <param name="dbfFilePath"></param>
        /// <returns></returns>
        long? GetAllRowCount(string dbfFilePath);

        /// <summary>
        /// Получаем кол-во записей(строк) в файле dbf только по нашей компании
        /// </summary>
        /// <param name="dbfFilePath"></param>
        /// <returns></returns>
        long? GetOurRowCount(string dbfFilePath, string param);
        Nomernik.History NOMPHistory_Get();
        Nomernik.History STOPHistory_Get();
        List<NomernikForClient> NomernikClientNOMP_Get(long ClientID);
        List<NomernikForClient> NomernikClientSTOP_Get(long ClientID);
    }
}
