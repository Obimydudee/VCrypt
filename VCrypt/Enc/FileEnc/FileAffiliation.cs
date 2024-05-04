using System;
using Microsoft.Win32;

namespace VCrypt.Enc.FileEnc
{
    internal class FileAffiliation
    {
        public static string ExecutablePath = $"{Environment.CurrentDirectory}\\VCrypt.exe";
        public static string fileExtensionKeyVCry = @"HKEY_CURRENT_USER\Software\Classes\.VCry";
        public static string fileExtensionKeyVPass = @"HKEY_CURRENT_USER\Software\Classes\.VPass";
        public static string shellOpenCommandKey = @"HKEY_CURRENT_USER\Software\Classes\VCrypt\shell\open\command";

        public static void doDahTing()
        {
            if (!RegistryKeyExists(fileExtensionKeyVCry))
            {
                using (RegistryKey fileExtensionKeyVCryObj = Registry.CurrentUser.CreateSubKey(fileExtensionKeyVCry))
                {
                    fileExtensionKeyVCryObj.SetValue("", "VCrypt");
                }
            }

            if (!RegistryKeyExists(fileExtensionKeyVPass))
            {
                using (RegistryKey fileExtensionKeyVPassObj = Registry.CurrentUser.CreateSubKey(fileExtensionKeyVPass))
                {
                    fileExtensionKeyVPassObj.SetValue("", "VCrypt");
                }
            }

            if (!RegistryKeyExists(shellOpenCommandKey))
            {
                using (RegistryKey shellOpenCommandKeyObj = Registry.CurrentUser.CreateSubKey(shellOpenCommandKey))
                {
                    shellOpenCommandKeyObj.SetValue("", ExecutablePath + " \"%1\"");
                }
            }
        }

        public static bool RegistryKeyExists(string keyPath)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath, false);
                return key != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
