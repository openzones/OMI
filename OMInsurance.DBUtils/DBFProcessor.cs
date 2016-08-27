using OMInsurance.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

namespace OMInsurance.DBUtils
{
    public class DBFProcessor
    {
        public List<FileWrapper> SaveToUralsibDBF(SqlDataReader reader, string filepath, List<long> ids)
        {
            List<FileWrapper> files = new List<FileWrapper>();
            string dbfName = Path.Combine(ConfiguraionProvider.FileStorageFolder, filepath);
            string fptName = Path.Combine(ConfiguraionProvider.FileStorageFolder, filepath.Replace("dbf", "FPT"));
            File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "DBFTemplates", "uralsib_internal.dbf"), dbfName, true);
            File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "DBFTemplates", "uralsib_internal.FPT"), fptName, true);
            string filename = Path.GetFileName(filepath);

            #region command
            string textCommand =
            string.Format("INSERT INTO \"{0}\" " +
            " (nz, "
            + "[status], "
            + "sn_card, "
            + "VS, "
            + "enp, "
            + "q, "
            + "dp, "
            + "dt, "
            + "fam, "
            + "d_type1, "
            + "im, "
            + "d_type2, "
            + "ot, "
            + "d_type3, "
            + "dr, "
            + "w, "
            + "jt, "
            + "mcod, "
            + "kl, "
            + "cont, "
            + "pricin, "
            + "c_doc, "
            + "s_doc, "
            + "n_doc, "
            + "d_doc, "
            + "podr_doc, "
            + "ss, "
            + "gr, "
            + "mr, "
            + "dat_reg, "
            + "[form], "
            + "predst, "
            + "spos, "
            + "d_type4, "
            + "[comment], "
            + "isrereg, "
            + "p_doc, "
            + "vs_data, "
            + "true_dr, "
            + "tranz, "
            + "recid_chld, "
            + "gz_data, "
            + "[pv], "
            + "os_doc, "
            + "on_doc, "
            + "od_doc, "
            + "odr, "
            + "ow, "
            + "is2fio, "
            + "is2doc, "
            + "oper, "
            + "enp2, "
            + "ogrn_old2, "
            + "okato_old2, "
            + "dp_old2, "
            + "scn, "
            + "gzk_flag, "
            + "doc_flag, "
            + "uec_flag, "
            + "e_doc, "
            + "ktg, "
            + "act, "
            + "perm2id, "
            + "x_doc, "
            + "p_doc2 , "
            + "s_card2, "
            + "n_card2, "
            + "s_card, "
            + "n_card, "
            + "recid_pv , "
            + "recid_smo , "
            + "ul , "
            + "d, "
            + "kor , "
            + "str , "
            + "kv , "
            + "c_okato, "
            + "ra_name, "
            + "np_c, "
            + "np_name, "
            + "ul_c, "
            + "ul_name, "
            + "dom2, "
            + "kor2, "
            + "str2, "
            + "kv2 , "
            + "c_perm, "
            + "s_perm, "
            + "n_perm, "
            + "d_perm, "
            + "ogrn_old , "
            + "okato_old , "
            + "dp_old, "
            + "ofam, "
            + "oim , "
            + "oot , "
            + "oc_doc, "
            + "prfam, "
            + "prim, "
            + "prot, "
            + "prc_doc, "
            + "prs_doc, "
            + "prn_doc, "
            + "prd_doc, "
            + "prpodr, "
            + "prtel1, "
            + "prtel2, "
            + "prpinf, "
            + "wrkcode, "
            + "wrkname) " +
            " VALUES " +
            " (?, "//@nz, "
            + "?, "//@status, "
            + "?, "//@sn_card, "
            + "?, "//@vs, "
            + "?, "//@enp, "
            + "?, "//@q, "
            + "?, "//@dp, "
            + "?, "//@dt, "
            + "?, "//@fam, "
            + "?, "//@d_type1, "
            + "?, "//@im, "
            + "?, "//@d_type2, "
            + "?, "//@ot, "
            + "?, "//@d_type3, "
            + "?, "//@dr, "
            + "?, "//@w, "
            + "?, "//@jt, "
            + "?, "//@mcod, "
            + "?, "//@kl, "
            + "?, "//@cont, "
            + "?, "//@pricin, "
            + "?, "//@c_doc, "
            + "?, "//@s_doc, "
            + "?, "//@n_doc, "
            + "?, "//@d_doc, "
            + "?, "//@podr_doc, "
            + "?, "//@ss, "
            + "?, "//@gr, "
            + "?, "//@mr, "
            + "?, "//@dat_reg, "
            + "?, "//@form, "
            + "?, "//@predst, "
            + "?, "//@spos, "
            + "?, "//@d_type4, "
            + "?, "//@comment, "
            + "?, "//@isrereg, "
            + "?, "//@p_doc, "
            + "?, "//@vs_data, "
            + "?, "//@true_dr, "
            + "?, "//@tranz, "
            + "?, "//@recid_chld, "
            + "?, "//@gz_data, "
            + "?, "//@pv, "
            + "?, "//@os_doc, "
            + "?, "//@on_doc, "
            + "?, "//@od_doc, "
            + "?, "//@odr, "
            + "?, "//@ow, "
            + "?, "//@is2fio, "
            + "?, "//@is2doc, "
            + "?, "//@oper, "
            + "?, "//@enp2, "
            + "?, "//@ogrn_old2, "
            + "?, "//@okato_old2, "
            + "?, "//@dp_old2, "
            + "?, "//@scn, "
            + "?, "//@gzk_flag, "
            + "?, "//@doc_flag, "
            + "?, "//@uec_flag, "
            + "?, "//@e_doc, "
            + "?, "//@ktg, "
            + "?, "//@act, "
            + "?, "//@perm2id, "
            + "?, "//@x_doc, "
            + "?, "//@p_doc2 , "
            + "?, "//@s_card2, "
            + "?, "//@n_card2, "
            + "?, "//@s_card, "
            + "?, "//@n_card, "
            + "?, "//@recid_pv , "
            + "?, "//@recid_smo , "
            + "?, "//@ul , "
            + "?, "//@d, "
            + "?, "//@kor , "
            + "?, "//@str , "
            + "?, "//@kv , "
            + "?, "//@c_okato, "
            + "?, "//@ra_name, "
            + "?, "//@np_c, "
            + "?, "//@np_name, "
            + "?, "//@ul_c, "
            + "?, "//@ul_name, "
            + "?, "//@dom2, "
            + "?, "//@kor2, "
            + "?, "//@str2, "
            + "?, "//@kv2 , "
            + "?, "//@c_perm, "
            + "?, "//@s_perm, "
            + "?, "//@n_perm, "
            + "?, "//@d_perm, "
            + "?, "//@ogrn_old , "
            + "?, "//@okato_old , "
            + "?, "//@dp_old, "
            + "?, "//@ofam, "
            + "?, "//@oim , "
            + "?, "//@oot , "
            + "?, "//@oc_doc, "
            + "?, "//@prfam, "
            + "?, "//@prim, "
            + "?, "//@prot, "
            + "?, "//@prc_doc, "
            + "?, "//@prs_doc, "
            + "?, "//@prn_doc, "
            + "?, "//@prd_doc, "
            + "?, "//@prpodr, "
            + "?, "//@prtel1, "
            + "?, "//@prtel2, "
            + "?, "//@prpinf, "
            + "?, "//@wrkcode, "
            + "?)"//@wrkname)"
            , filepath);

            #endregion

            while (reader.Read())
            {
                using (OleDbConnection conn = new OleDbConnection(string.Format("Provider=VFPOLEDB.1;Data Source={0}", dbfName)))
                {
                    conn.Open();
                    ProcessRow(reader, files, ids, textCommand, conn);
                }
            }

            files.Add(new FileWrapper() { Filename = Path.GetFileName(dbfName), Content = File.ReadAllBytes(dbfName) });
            files.Add(new FileWrapper() { Filename = Path.GetFileName(fptName), Content = File.ReadAllBytes(fptName) });
            File.Delete(dbfName);
            File.Delete(fptName);

            return files;
        }

        private static void ProcessRow(SqlDataReader reader, List<FileWrapper> files, List<long> ids, string textCommand, OleDbConnection conn)
        {
            using (OleDbCommand command = new OleDbCommand())
            {
                command.CommandText = textCommand;
                command.Connection = conn;
                #region Parameters
                command.Parameters.Add(new OleDbParameter() { ParameterName = "nz", OleDbType = OleDbType.Char, Value = reader["nz"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "status", OleDbType = OleDbType.Numeric, Value = reader["status"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "sn_card", OleDbType = OleDbType.Char, Value = reader["sn_card"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "VS", OleDbType = OleDbType.Char, Value = reader["vs"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "enp", OleDbType = OleDbType.Char, Value = reader["enp"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "q", OleDbType = OleDbType.Char, Value = reader["q"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "dp", OleDbType = OleDbType.Date, Value = reader["dp"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "dt", OleDbType = OleDbType.Date, Value = reader["dt"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "fam", OleDbType = OleDbType.Char, Value = reader["fam"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "d_type1", OleDbType = OleDbType.Char, Value = reader["d_type1"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "im", OleDbType = OleDbType.Char, Value = reader["im"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "d_type2", OleDbType = OleDbType.Char, Value = reader["d_type2"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "ot", OleDbType = OleDbType.Char, Value = reader["ot"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "d_type3", OleDbType = OleDbType.Char, Value = reader["d_type3"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "dr", OleDbType = OleDbType.Date, Value = reader["dr"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "w", OleDbType = OleDbType.Numeric, Value = reader["w"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "jt", OleDbType = OleDbType.Char, Value = reader["jt"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "mcod", OleDbType = OleDbType.Char, Value = reader["mcod"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "kl", OleDbType = OleDbType.Numeric, Value = reader["kl"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "cont", OleDbType = OleDbType.Char, Value = reader["cont"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "pricin", OleDbType = OleDbType.Char, Value = reader["pricin"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "c_doc", OleDbType = OleDbType.Numeric, Value = reader["c_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "s_doc", OleDbType = OleDbType.Char, Value = reader["s_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "n_doc", OleDbType = OleDbType.Char, Value = reader["n_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "d_doc", OleDbType = OleDbType.Date, Value = reader["d_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "podr_doc", OleDbType = OleDbType.VarChar, Value = reader["podr_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "ss", OleDbType = OleDbType.Char, Value = reader["ss"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "gr", OleDbType = OleDbType.Char, Value = reader["gr"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "mr", OleDbType = OleDbType.VarChar, Value = reader["mr"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "dat_reg", OleDbType = OleDbType.Date, Value = reader["dat_reg"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "form", OleDbType = OleDbType.Numeric, Value = reader["form"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "predst", OleDbType = OleDbType.Char, Value = reader["predst"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "spos", OleDbType = OleDbType.Numeric, Value = reader["spos"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "d_type4", OleDbType = OleDbType.Numeric, Value = reader["d_type4"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "comment", OleDbType = OleDbType.VarChar, Value = reader["comment"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "isrereg", OleDbType = OleDbType.Numeric, Value = reader["isrereg"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "p_doc", OleDbType = OleDbType.Numeric, Value = reader["p_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "vs_data", OleDbType = OleDbType.Date, Value = reader["vs_data"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "true_dr", OleDbType = OleDbType.Numeric, Value = reader["true_dr"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "tranz", OleDbType = OleDbType.Char, Value = reader["tranz"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "recid_chld", OleDbType = OleDbType.Integer, Value = reader["recid_chld"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "gz_data", OleDbType = OleDbType.Date, Value = reader["gz_data"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "pv", OleDbType = OleDbType.Char, Value = reader["pv"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "os_doc", OleDbType = OleDbType.Char, Value = reader["os_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "on_doc", OleDbType = OleDbType.Char, Value = reader["on_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "od_doc", OleDbType = OleDbType.Date, Value = reader["od_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "odr", OleDbType = OleDbType.Date, Value = reader["odr"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "ow", OleDbType = OleDbType.Numeric, Value = reader["ow"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "is2fio", OleDbType = OleDbType.Char, Value = reader["is2fio"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "is2doc", OleDbType = OleDbType.Char, Value = reader["is2doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "oper", OleDbType = OleDbType.Integer, Value = reader["oper"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "enp2", OleDbType = OleDbType.Char, Value = reader["enp2"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "ogrn_old2", OleDbType = OleDbType.Char, Value = reader["ogrn_old2"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "okato_old2", OleDbType = OleDbType.Char, Value = reader["okato_old2"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "dp_old2", OleDbType = OleDbType.Date, Value = reader["dp_old2"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "scn", OleDbType = OleDbType.Char, Value = reader["scn"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "gzk_flag", OleDbType = OleDbType.Numeric, Value = reader["gzk_flag"] });

                command.Parameters.Add(new OleDbParameter() { ParameterName = "doc_flag", OleDbType = OleDbType.Numeric, Value = reader["doc_flag"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "uec_flag", OleDbType = OleDbType.Char, Value = reader["uec_flag"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "e_doc", OleDbType = OleDbType.Date, Value = reader["e_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "ktg", OleDbType = OleDbType.Char, Value = reader["ktg"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "act", OleDbType = OleDbType.Boolean, Value = reader["act"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "perm2id", OleDbType = OleDbType.Integer, Value = reader["perm2id"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "x_doc", OleDbType = OleDbType.Numeric, Value = reader["x_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "p_doc2", OleDbType = OleDbType.Numeric, Value = reader["p_doc2"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "s_card2", OleDbType = OleDbType.Char, Value = reader["s_card2"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "n_card2", OleDbType = OleDbType.Char, Value = reader["n_card2"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "s_card", OleDbType = OleDbType.Char, Value = reader["s_card"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "n_card", OleDbType = OleDbType.Char, Value = reader["n_card"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "recid_pv", OleDbType = OleDbType.Integer, Value = reader["recid_pv"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "recid_smo", OleDbType = OleDbType.Integer, Value = reader["recid_smo"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "ul", OleDbType = OleDbType.Numeric, Value = reader["ul"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "d", OleDbType = OleDbType.Char, Value = reader["d"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "kor", OleDbType = OleDbType.Char, Value = reader["kor"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "str", OleDbType = OleDbType.Char, Value = reader["str"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "kv", OleDbType = OleDbType.Char, Value = reader["kv"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "c_okato", OleDbType = OleDbType.Char, Value = reader["c_okato"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "ra_name", OleDbType = OleDbType.Char, Value = reader["ra_name"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "np_c", OleDbType = OleDbType.Char, Value = reader["np_c"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "np_name", OleDbType = OleDbType.Char, Value = reader["np_name"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "ul_c", OleDbType = OleDbType.Char, Value = reader["ul_c"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "ul_name", OleDbType = OleDbType.Char, Value = reader["ul_name"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "dom2", OleDbType = OleDbType.Char, Value = reader["dom2"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "kor2", OleDbType = OleDbType.Char, Value = reader["kor2"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "str2", OleDbType = OleDbType.Char, Value = reader["str2"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "kv2", OleDbType = OleDbType.Char, Value = reader["kv2"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "c_perm", OleDbType = OleDbType.Numeric, Value = reader["c_perm"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "s_perm", OleDbType = OleDbType.Char, Value = reader["s_perm"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "n_perm", OleDbType = OleDbType.Char, Value = reader["n_perm"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "d_perm", OleDbType = OleDbType.Date, Value = reader["d_perm"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "ogrn_old", OleDbType = OleDbType.Char, Value = reader["ogrn_old"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "okato_old", OleDbType = OleDbType.Char, Value = reader["okato_old"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "dp_old", OleDbType = OleDbType.Date, Value = reader["dp_old"] });

                command.Parameters.Add(new OleDbParameter() { ParameterName = "ofam", OleDbType = OleDbType.Char, Value = reader["ofam"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "oim", OleDbType = OleDbType.Char, Value = reader["oim"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "oot", OleDbType = OleDbType.Char, Value = reader["oot"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "oc_doc", OleDbType = OleDbType.Numeric, Value = reader["oc_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "prfam", OleDbType = OleDbType.Char, Value = reader["prfam"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "prim", OleDbType = OleDbType.Char, Value = reader["prim"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "prot", OleDbType = OleDbType.Char, Value = reader["prot"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "prc_doc", OleDbType = OleDbType.Numeric, Value = reader["prc_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "prs_doc", OleDbType = OleDbType.Char, Value = reader["prs_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "prn_doc", OleDbType = OleDbType.Char, Value = reader["prn_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "prd_doc", OleDbType = OleDbType.Date, Value = reader["prd_doc"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "prpodr", OleDbType = OleDbType.Char, Value = reader["prpodr"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "prtel1", OleDbType = OleDbType.Char, Value = reader["prtel1"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "prtel2", OleDbType = OleDbType.Char, Value = reader["prtel2"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "prpinf", OleDbType = OleDbType.Char, Value = reader["prpinf"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "wrkcode", OleDbType = OleDbType.Char, Value = reader["wrkcode"] });
                command.Parameters.Add(new OleDbParameter() { ParameterName = "wrkname", OleDbType = OleDbType.Char, Value = reader["wrkname"] });
                #endregion
                command.ExecuteNonQuery();

                AddAdditionalFiles(reader, files, ids);
            }
        }

        /// <summary>
        /// Add additional files like photo, signature, etc.
        /// </summary>
        private static void AddAdditionalFiles(SqlDataReader reader, List<FileWrapper> files, List<long> ids)
        {
            long id = (long)reader["RECID_PV"];
            ids.Add(id);
            string path = string.Empty;
            if (reader["SCN"] != DBNull.Value && ((string)reader["SCN"]) != "POK")
            {
                if (reader["PhotoFileName"] != DBNull.Value)
                {
                    path = Path.Combine(ConfiguraionProvider.FileStorageFolder, (string)reader["PhotoFileName"]);
                    if (File.Exists(path))
                    {
                        files.Add(new FileWrapper() { Content = File.ReadAllBytes(path), Filename = string.Format("f" + new String('0', 6 - id.ToString().Length) + "{0}.jpg", id) });
                    }
                }
                if (reader["SignatureFileName"] != DBNull.Value)
                {
                    path = Path.Combine(ConfiguraionProvider.FileStorageFolder, (string)reader["SignatureFileName"]);
                    if (File.Exists(path))
                    {
                        files.Add(new FileWrapper() { Content = File.ReadAllBytes(path), Filename = string.Format("s" + new String('0', 6 - id.ToString().Length) + "{0}.jpg", id) });
                    }
                }
            }
        }

        public static DataTable GetDataTable(string filename, string query, OleDbParameter[] parameters = null)
        {
            DataTable result = new DataTable();
            OleDbCommand command = new OleDbCommand();
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            using (OleDbConnection conn = new OleDbConnection(string.Format("Provider=VFPOLEDB.1;Data Source={0};", filename)))
            {
                conn.Open();
                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        command.Parameters.Add(parameters[i]);
                    }
                }
                command.Connection = conn;
                command.CommandText = query;
                adapter.SelectCommand = command;
                try
                {
                    adapter.Fill(result);
                }
                catch (OleDbException ex)
                {
                    if (ex.Message.Contains("is not a table"))
                    {
                        return result;
                    }
                }
            }
            return result;
        }
    }
}
