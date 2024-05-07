﻿using System;
using System.IO;

namespace VCrypt.Enc.FileEnc
{
    internal class log
    {
        public log() { }
        public void FileLog(string Message, string FileName)
        {
            try
            {
                using (TextWriter tw = new StreamWriter(FileName, true))
                {
                    tw.WriteLine(Message);
                }
            }
            catch (Exception ex)  //Writing to log has failed, send message to trace in case anyone is listening.
            {
                System.Diagnostics.Trace.Write(ex.ToString());
            }
        }
    }
}
