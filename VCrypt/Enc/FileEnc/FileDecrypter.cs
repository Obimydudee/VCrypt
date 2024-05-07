using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCrypt.Enc.FileEnc
{
    internal class FileDecrypter
    {
        private static readonly string[] FileExtensions = new string[] //the things that has too be added lol
        {
        "VCry"
        };

        public static void mkf(string stf, string fnm, string pss)
        {
            log _log = new log();
            AES aes = new AES();
            //Console.WriteLine(stf);
            var step1 = aes.Decrypt(stf, pss);
            _log.FileLog(step1, Environment.CurrentDirectory + "\\AfterAESDecrypt.txt");
            string step2 = FromHex(step1);
            
            File.WriteAllText($"{fnm}", string.Join("\r\n", new[] { step2 }));
            //File.Delete($"{fnm}.VCry");
        }

        public static string FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            byte[] anon = raw;
            string slin = Encoding.ASCII.GetString(anon, 0, anon.Length);

            return slin;
        }

        public static string B64decrypt(string e)
        {
            byte[] enc = Convert.FromBase64String(e);
            string pt = Encoding.UTF8.GetString(enc);
            return pt;
        }

        public static void FileDecryption(string password, string path)
        {
            string[] directories = Directory.GetDirectories(path);

            foreach (string directory in directories)
            {
                try
                {
                    using (var progress = new progressBar())
                    {


                        string[] items = Directory.GetFiles(directory, "*.*");
                        int i = 0;
                        foreach (var itm in items)
                        {
                            if (itm.Contains(".VCry"))
                            {
                                progress.Report((double)i / items.Count());
                                string Stringedbytes = File.ReadAllText(itm);
                                mkf(Stringedbytes, itm.Replace(".VCry", ""), password);
                                i++;
                                Console.Title = $"{i} file(s) have been Decrypted in {directory}";
                                if (i == items.Count())
                                {
                                    Console.WriteLine("Done!");
                                    i = 0;
                                    GC.Collect();
                                    continue;
                                }
                            }

                            
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }



        }
    }
}
