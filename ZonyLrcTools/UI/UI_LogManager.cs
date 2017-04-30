using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ZonyLrcTools.Untils;
using System.Text.RegularExpressions;

namespace ZonyLrcTools.UI
{
    public partial class UI_LogManager : Form
    {
        public UI_LogManager()
        {
            InitializeComponent();
        }

        private void UI_LogManager_Load(object sender, EventArgs e)
        {
            getLogInfo(@"D:\Git\ZonyLrcTools\ZonyLrcTools\bin\Debug\LogFiles\20170430154908.log");
        }

        private List<LogModel> getLogInfo(string filePath)
        {
            FileStream _fs = File.Open(filePath, FileMode.Open);
            StreamReader _reader = new StreamReader(_fs);
            string _spilt = new string('=', 97);
            string _content = _reader.ReadToEnd().Replace("\r\n", string.Empty);
            string[] _logs = Regex.Split(_content, _spilt, RegexOptions.IgnoreCase).Where(x => x != string.Empty).ToArray();

            List<LogModel> _result = new List<LogModel>();
            foreach (var item in _logs)
            {
                _result.Add(new LogModel(item));
            }
            return _result;
        }
    }

    public sealed class LogModel
    {
        public LogModel(string source)
        {
            //Status = 
        }
        public string Status { get; set; }
        public string Information { get; set; }
        public string ErrorInfo { get; set; }
        public string ErrorStack { get; set; }
    }
}
