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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace Neat.Dennis.Common.LoggerManager
{
    internal class TricolorLoggerImp : INeatLoggerImp
    {
        private static string defaultConfigurationFile = string.Empty;

        Type loggingType;
        ILog log4NetLog;

        static object _lockObj = new object();

        static TricolorLoggerImp()
        {
            if (log4net.GlobalContext.Properties["RelatedPath"] == null)
            {
                log4net.GlobalContext.Properties["RelatedPath"] = string.Empty;
            }
            if (log4net.GlobalContext.Properties["ProcessName"] == null)
            {
                log4net.GlobalContext.Properties["ProcessName"] = Process.GetCurrentProcess().ProcessName + ".exe";
            }
            SetDefaultConfigFile();
        }

        public TricolorLoggerImp(Type type)
        {
            this.loggingType = type;
            this.log4NetLog = LogManager.GetLogger(type);
        }

        public TricolorLoggerImp(Type type, string loggerName)
        {
            this.loggingType = type;
            this.log4NetLog = LogManager.GetLogger(loggerName);
        }

        public static string DefaultConfigurationFile { get { return defaultConfigurationFile; } }
        public NeatLogLevel CurrentLogLevel { get { return (NeatLogLevel)((Hierarchy)LogManager.GetRepository()).Root.Level.Value; } }
        public bool IsErrorEnabled { get { return log4NetLog.IsErrorEnabled; } }
        public bool IsWarnEnabled { get { return log4NetLog.IsWarnEnabled; } }
        public bool IsInfoEnabled { get { return log4NetLog.IsInfoEnabled; } }
        public bool IsDebugEnabled { get { return log4NetLog.IsDebugEnabled; } }

        public void WriteEntry(string msg, NeatLogLevel level, int eventId, ushort taskCategory, string eventSource)
        {

            ILoggerRepository repository = log4NetLog.Logger.Repository;
            string loggerName = log4NetLog.Logger.Name;
            Level log4netLevel = new Level((int)level, level.ToString());
            LoggingEvent loggingEntry = new LoggingEvent(loggingType, repository, loggerName, log4netLevel, msg, null);
            loggingEntry.Properties["EventID"] = eventId;
            loggingEntry.Properties["TaskCategory"] = taskCategory;
            loggingEntry.Properties["EventSource"] = eventSource;
            log4NetLog.Logger.Log(loggingEntry);
        }

        private static void SetDefaultConfigFile()
        {
            var log4netConfigurationFile = GetConfigFileName();
            ApplyConfigFile(log4netConfigurationFile);
        }

        private static string GetConfigFileName()
        {
            var log4netConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.config");
            return log4netConfigurationFile;
        }

        private static void ApplyConfigFile(string log4netConfigurationFile)
        {
            if (!string.IsNullOrEmpty(log4netConfigurationFile))
            {
                defaultConfigurationFile = log4netConfigurationFile;
                XmlConfigurator.ConfigureAndWatch(new FileInfo(log4netConfigurationFile));
            }
            else
            {
            }
        }

    }

    public class Log4NetGlobalConfig
    {
        public static string ConfigFile;
        public static string LogPath { set { log4net.GlobalContext.Properties["RelatedPath"] = value; } }
        public static string ProcessName { set { log4net.GlobalContext.Properties["ProcessName"] = value; } }
    };
}
