using MTSEntBlocks.LoggerBlock;
using System;

namespace EphesoftService
{
    public class CustomLogger
    {
        private string PrefIx = string.Empty;
        public CustomLogger()
        {

        }
        public CustomLogger(string _prefix)
        {
            if (!string.IsNullOrEmpty(_prefix))
                PrefIx = ":" + _prefix;
        }

        public void Debug(string msg)
        {
            Logger.WriteTraceLog(PrefIx + msg);
        }

        public void Error(string msg)
        {
            Logger.WriteTraceLog(PrefIx + msg);
        }

        public void Error(string msg, Exception ex)
        {
            string msgToWrite = PrefIx + msg;
            if (ex != null && ex.Message != null)
                msgToWrite += "\n" + "Exception" + ex.Message;
            if (ex != null && ex.StackTrace != null)
                msgToWrite += "\n" + ex.StackTrace;
            Logger.WriteTraceLog(msgToWrite);
        }

        public void Info(string msg)
        {
            Logger.WriteTraceLog(PrefIx + msg);
        }

    }
}