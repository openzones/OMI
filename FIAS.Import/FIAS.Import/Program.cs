using System;
using System.IO;

namespace FIAS.Import
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = @"D:\Uralsib\OMInsurance\Database\FIAS.sql";
            string file1 = @"D:\Uralsib\OMInsurance\Database\FIAS1.sql";
            string insert = "INSERT [dbo].[ADDROBJ] ([ACTSTATUS], [AOGUID], [AOID], [AOLEVEL], [AREACODE], [AUTOCODE], [CENTSTATUS], [CITYCODE], [CODE], [CURRSTATUS], [ENDDATE], [FORMALNAME], [IFNSFL], [IFNSUL], [NEXTID], [OFFNAME], [OKATO], [OKTMO], [OPERSTATUS], [PARENTGUID], [PLACECODE], [PLAINCODE], [POSTALCODE], [PREVID], [REGIONCODE], [SHORTNAME], [STARTDATE], [STREETCODE], [TERRIFNSFL], [TERRIFNSUL], [UPDATEDATE], [CTARCODE], [EXTRCODE], [SEXTCODE], [LIVESTATUS], [NORMDOC]) VALUES ";
            int iterations = 500;
            int count = 0;
            using (TextReader reader = new StreamReader(file))
            {
                using (TextWriter writer = new StreamWriter(file1))
                {
                    string value;
                    while ((value = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (iterations == 500)
                            {
                                writer.WriteLine(insert);
                                Console.WriteLine(count++);
                            }
                            if (iterations == 0)
                            {
                                writer.Write(value);
                                writer.WriteLine(" GO ");
                                iterations = 500;
                                continue;
                            }
                            iterations--;
                            writer.Write(value);
                            writer.Write(",");
                            writer.WriteLine();
                        }
                    }
                    writer.WriteLine("GO");
                }
            }
            Console.ReadKey();
        }
    }
}
