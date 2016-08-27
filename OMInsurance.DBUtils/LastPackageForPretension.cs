using OMInsurance.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace OMInsurance.DBUtils
{
    public class LastPackageForPretension
    {
        /// <summary>
        /// Внимание! Используются dynamic переменные.
        /// </summary>
        /// <param name="pretension"></param>
        /// <param name="client"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public FileWrapper SaveToUralsibDBF(
            dynamic pretension,
            dynamic client,
            string filepath)
        {
            FileWrapper file = new FileWrapper();
            string dbfName = Path.Combine(ConfiguraionProvider.FileStorageFolder, filepath);
            //string fptName = Path.Combine(ConfiguraionProvider.FileStorageFolder, filepath.Replace("dbf", "FPT"));
            File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "DBFTemplates", "lastPackage.dbf"), dbfName, true);
            //File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "DBFTemplates", "lastPackage.FPT"), fptName, true);
            string filename = Path.GetFileName(filepath);

            #region command
            string textCommand =
            string.Format("INSERT INTO \"{0}\" " +
            " (RECID, "
            + "LPU_ID, "
            + "S_POL, "
            + "N_POL, "
            + "Q, "
            + "AKT_E, "
            + "DATE_E, "
            + "RESERV) " +
            " VALUES " +
            " (?, "//@RECID, "
            + "?, "//@LPU_ID, "
            + "?, "//@S_POL, "
            + "?, "//@N_POL, "
            + "?, "//@Q, "
            + "?, "//@AKT_E, "
            + "?, "//@DATE_E, "
            + "?)"//@RESERV)"
            , filepath);

            #endregion

            using (OleDbConnection conn = new OleDbConnection(string.Format("Provider=VFPOLEDB.1;Data Source={0}", dbfName)))
            {
                conn.Open();
                ProcessRow(pretension, client, textCommand, conn);
            }

            file = new FileWrapper() { Filename = Path.GetFileName(dbfName), Content = File.ReadAllBytes(dbfName) };
            //files.Add(new FileWrapper() { Filename = Path.GetFileName(fptName), Content = File.ReadAllBytes(fptName) });
            File.Delete(dbfName);
            //File.Delete(fptName);

            return file;
        }

        private static void ProcessRow(
            dynamic pretension,
            dynamic client,
            string textCommand, OleDbConnection conn)
        {
            using (OleDbCommand command = new OleDbCommand())
            {
                command.CommandText = textCommand;
                command.Connection = conn;

                #region Parameters
                command.Parameters.Add(new OleDbParameter() { ParameterName = "RECID", OleDbType = OleDbType.Char, Value = pretension.Generator });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "LPU_ID", OleDbType = OleDbType.Numeric, Value = pretension.LPU_ID ?? 0 });
                command.Parameters.Add(new OleDbParameter()
                {
                    ParameterName = "S_POL",
                    OleDbType = OleDbType.Char,
                    Value = System.Linq.Enumerable.LastOrDefault<dynamic>(client.Visits).PolicySeries ?? string.Empty
                });
                command.Parameters.Add(new OleDbParameter()
                {
                    ParameterName = "N_POL",
                    OleDbType = OleDbType.Char,
                    Value = System.Linq.Enumerable.LastOrDefault<dynamic>(client.Visits).PolicyNumber ??
                        System.Linq.Enumerable.LastOrDefault<dynamic>(client.Visits).UnifiedPolicyNumber ??
                            string.Empty
                });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "Q", OleDbType = OleDbType.Char, Value = "P2" });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "AKT_E", OleDbType = OleDbType.Char, Value = pretension.M_nakt.TrimStart('№') });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "DATE_E", OleDbType = OleDbType.Date, Value = pretension.M_dakt ?? DateTime.Now });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "RESERV", OleDbType = OleDbType.Char, Value = string.Empty });
                #endregion
                command.ExecuteNonQuery();
            }
        }
    }
}
