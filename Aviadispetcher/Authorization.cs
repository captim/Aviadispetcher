using System;
using System.Collections.Generic;
using System.Text;

namespace Aviadispetcher
{
    public class Authorization
    {
        public static int logUser
        {
            get;
            set;
        }
        public int LogCheck(string logText, string pswText)
        {
            logUser = 0;
            if ((logText == "Редактор") && (pswText == "222"))
                logUser = 2;
            return logUser;
        }
    }
}
