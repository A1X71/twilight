/**************************************************************************
*
*  PROPRIETARY and CONFIDENTIAL
*
*  This file is licensed from, and is a trade secret of:
*
*                   Neat, Inc.
*                   No. 66, Xigang North Road
*                   Qinhuangdao City, Hebei Province, China
*                   Telephone: 0335-3660312
*                   WWW: www.neat.com.cn
*
*  Refer to your License Agreement for restrictions on use,
*  duplication, or disclosure.
*
*  Copyright © 2017-2018 Neat® Inc. All Rights Reserved. 
*
*  Unpublished - All rights reserved under the copyright laws of the China.
*  $Revision: 159 $
*  $Author: dennis_zhang $        
*  $Date: 2017-07-25 10:23:50 +0800 (周二, 25 七月 2017) $
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neat.Dennis.Common.LoggerManager
{
    public class NeatLogger : INeatLogger
    {
        static string loggingPrefix = string.Empty;
        static string loggingPostfix = string.Empty;

        INeatLoggerImp loggerImp;

        public NeatLogger(INeatLoggerImp imp, bool checkSensitiveKeyword)
        {
            this.loggerImp = imp;
        }

        public NeatLogger(Type type, bool checkSensitiveKeyword)
            : this(new TricolorLoggerImp(type), checkSensitiveKeyword)
        {
        }

        public NeatLogger(INeatLoggerImp imp)
            : this(imp, true)
        {
        }

        public NeatLogger(Type type)
            : this(type, true)
        {
        }

        public static NeatLogger GetInstance(Type type)
        {
            return GetInstance(type, true);
        }

        public static NeatLogger GetInstance(Type type, bool checkSensitiveKeyword)
        {
            return new NeatLogger(type);
        }

        #region --public properties--
        public NeatLogLevel CurrentLogLevel { get { return loggerImp.CurrentLogLevel; } }
        public bool IsErrorEnabled { get { return loggerImp.IsErrorEnabled; } }
        public bool IsWarnEnabled { get { return loggerImp.IsWarnEnabled; } }
        public bool IsInfoEnabled { get { return loggerImp.IsInfoEnabled; } }
        public bool IsDebugEnabled { get { return loggerImp.IsDebugEnabled; } }

        #endregion

        #region --public log method--

        #region --Debug methods--
        public void Debug(string formatStr, params object[] args)
        {
            try
            {
                if (!IsDebugEnabled) return;

                string finalMsg = GetFinalMessage(formatStr, args);
                WriteEntry(finalMsg, NeatLogLevel.DEBUG, 0, 0, string.Empty);
            }
            catch (Exception e)
            {
                string skipFxCop = e.Message;
            }
        }
        #endregion

        #region --Info methods--
        public void Info(string formatStr, params object[] args)
        {
            try
            {
                if (!IsInfoEnabled) return;

                string finalMsg = GetFinalMessage(formatStr, args);
                WriteEntry(finalMsg, NeatLogLevel.INFO, 0, 0, string.Empty);
            }
            catch (Exception e)
            {
                string skipFxCop = e.Message;
            }
        }
        #endregion

        #region --Warn methods--
        public void Warn(string formatStr, params object[] args)
        {
            try
            {
                if (!IsWarnEnabled) return;

                string finalMsg = GetFinalMessage(formatStr, args);
                WriteEntry(finalMsg, NeatLogLevel.WARN, 0, 0, string.Empty);
            }
            catch (Exception e)
            {
                string skipFxCop = e.Message;
            }
        }
        #endregion

        #region --Error methods--

        public void Error(string formatStr, params object[] args)
        {
            try
            {
                if (!IsErrorEnabled) return;

                string finalMsg = GetFinalMessage(formatStr, args);
                WriteEntry(finalMsg, NeatLogLevel.ERROR, 0, 0, string.Empty);
            }
            catch (Exception e)
            {
                string skipFxCop = e.Message;
            }
        }
        #endregion

        #region --Log methods--

        public void Log(NeatLogLevel loglevel, string formatStr, params object[] args)
        {
            try
            {
                if (CurrentLogLevel > loglevel) return;

                string finalMsg = GetFinalMessage(formatStr, args);
                WriteEntry(finalMsg, loglevel, 0, 0, string.Empty);
            }
            catch (Exception e)
            {
                string skipFxCop = e.Message;
            }
        }

        #endregion

        #endregion

        private string GetFinalMessage(string formatStr, params object[] args)
        {
            string finalMsg = string.Empty;
            if (args.Length == 0)
            {
                finalMsg = formatStr;
            }
            else if (args.Length == 1 && formatStr.IndexOf("{0}", StringComparison.OrdinalIgnoreCase) == -1)
            {
                finalMsg = string.Format("{0}\t{1}", formatStr, args[0]);
            }
            else
            {
                finalMsg = string.Format(formatStr, args);
            }

            if (!string.IsNullOrEmpty(loggingPrefix))
            {
                finalMsg = loggingPrefix + "    " + finalMsg;
            }
            if (!string.IsNullOrEmpty(loggingPostfix))
            {
                finalMsg = finalMsg + "    " + loggingPostfix;
            }
            return finalMsg;
        }

        private void WriteEntry(string msg, NeatLogLevel level, int eventId, ushort taskCategory, string eventSource)
        {
            loggerImp.WriteEntry(msg, level, eventId, taskCategory, eventSource);
        }
    }
}
