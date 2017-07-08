using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZonyLrcTools.Untils
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public static class LogManager
    {
        private static FileStream m_logFile;
        private static string m_logName;
        private static string m_logPath = Environment.CurrentDirectory + @"\LogFiles\";
        private static object m_lockObj = new object();

        private static readonly List<LogModel> m_logList;

        static LogManager()
        {
            if (!Directory.Exists(m_logPath)) Directory.CreateDirectory(m_logPath);
            m_logName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";
            m_logFile = new FileStream(Path.Combine(m_logPath, m_logName), FileMode.OpenOrCreate);
            m_logList = new List<LogModel>();
        }

        /// <summary>
        /// 写入日志记录
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="text">具体信息</param>
        /// <param name="e">错误堆栈</param>
        public static void WriteLogRecord(string status, string text, Exception e = null)
        {
            lock (m_lockObj)
            {
                m_logList.Add(new LogModel()
                {
                    Status = status,
                    Information = text,
                    ErrorInfo = e?.Message ?? e?.InnerException?.Message ?? "无",
                    ErrorStack = e?.StackTrace ?? e?.InnerException?.StackTrace ?? "无"
                });
            }
        }

        /// <summary>
        /// 保存日志信息
        /// </summary>
        public static void FlushLogData()
        {
            StreamWriter _sw = new StreamWriter(m_logFile);
            _sw.Write(JsonConvert.SerializeObject(m_logList));
            _sw.Flush();
            m_logFile.Flush();
            _sw.Close();
            m_logFile.Close();
        }

        /// <summary>
        /// 读取日志文件信息
        /// </summary>
        /// <param name="filePath">日志文件路径</param>
        /// <returns></returns>
        public static List<LogModel> ReadLogFile(string filePath)
        {
            try
            {
                FileStream _fs = File.Open(filePath, FileMode.Open);
                StreamReader _reader = new StreamReader(_fs);
                return JsonConvert.DeserializeObject<List<LogModel>>(_reader.ReadToEnd());
            }
            catch (IOException) { return new List<LogModel>(); }
        }
    }

    /// <summary>
    /// 日志模型
    /// </summary>
    public sealed class LogModel
    {
        public string Status { get; set; }
        public string Information { get; set; }
        public string ErrorInfo { get; set; }
        public string ErrorStack { get; set; }
    }
}
