using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCrypt.Enc.FileEnc;

namespace VCrypt
{
    internal class Program
    {
        internal static string[] Asciilogo = new string[] {
            @"$$\    $$\  $$$$$$\                                 $$\     ",
            @"$$ |   $$ |$$  __$$\                                $$ |    ",
            @"$$ |   $$ |$$ /  \__| $$$$$$\  $$\   $$\  $$$$$$\ $$$$$$\   ",
            @"\$$\  $$  |$$ |      $$  __$$\ $$ |  $$ |$$  __$$\\_$$  _|  ",
            @" \$$\$$  / $$ |      $$ |  \__|$$ |  $$ |$$ /  $$ | $$ |    ",
            @"  \$$$  /  $$ |  $$\ $$ |      $$ |  $$ |$$ |  $$ | $$ |$$\ ",
            @"   \$  /   \$$$$$$  |$$ |      \$$$$$$$ |$$$$$$$  | \$$$$  |",
            @"    \_/     \______/ \__|       \____$$ |$$  ____/   \____/ ",
            @"                               $$\   $$ |$$ |               ",
            @"                               \$$$$$$  |$$ |               ",
            @"                                \______/ \__|               ",
        };
        static void Main(string[] args)
        {

            string[] array = new string[]
            {
            "Encrypt Drive", "Decrypt Drive", "Exit"
            };

            int num = 0;
            while (true)
            {
                System.Console.Title = "| MortisTool | Made with love~<3 | proxsec |";
                System.Console.Clear();
                for (int i = 0; i < 11; i++)
                {
                    Console.WriteLine(Asciilogo[i]);
                }
                for (int j = 0; j < array.Length; j++)
                {
                    if (j == num)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("> ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                    Console.WriteLine(array[j]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                switch (System.Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (num > 0)
                        {
                            num--;
                            System.Console.WriteLine(num);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (num < array.Length - 1)
                        {
                            num++;
                            System.Console.WriteLine(num);
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (num == array.Length - 1)
                        {
                            System.Console.WriteLine("Exiting program.");
                            return;
                        }
                        switch (num)
                        {
                            case 0:
                                EncryptionTime();
                                break;
                            case 1:
                                //RC4Encc();
                                break;
                            default:
                                System.Console.WriteLine("You selected " + array[num] + ". Which is option: " + num);
                                System.Console.WriteLine("Press any key to continue.");
                                System.Console.ReadKey();
                                break;
                        }
                        break;
                }
            }
            
        }

        public static void EncryptionTime()
        {
            Console.Clear();
            log _log = new log();
            var removableDrives = DriveInfo.GetDrives()
                                       .Where(d => d.DriveType == DriveType.Removable)
                                       .ToList();

            Console.WriteLine("Choose a Removable Drive to encrypt:");
            for (int i = 0; i < removableDrives.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {removableDrives[i].Name} - {removableDrives[i].VolumeLabel}");
            }

            Console.WriteLine("Select a drive by entering its number:");
            int selectedDriveIndex = Convert.ToInt32(Console.ReadLine()) - 1;

            if (selectedDriveIndex >= 0 && selectedDriveIndex < removableDrives.Count)
            {
                DriveInfo selectedDrive = removableDrives[selectedDriveIndex];
                string selectedDriveLocation = selectedDrive.Name;
                string password = PasswdGen.Generate();
                //string passpath = ;
                _log.FileLog(password, Environment.CurrentDirectory + $"\\VCryptDrive-{selectedDrive.VolumeLabel}-Password.VPass");
                Console.Clear();
                Console.WriteLine($"Generated password has beens stored at: {Environment.CurrentDirectory + $"\\VCryptDrive-{selectedDrive.VolumeLabel}-Password.VPass"}");

                FileEncrypter.FileEncryption(password, selectedDriveLocation);
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
            Console.ReadLine();
        }
    }
}
