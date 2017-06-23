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
using Newtonsoft.Json;

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
            //getLogInfo(@"E:\代码\Git\ZonyLrcTools\ZonyLrcTools\bin\Debug\LogFiles\20170615193555.log");
        }

        private List<LogModel> getLogInfo(string filePath)
        {
            FileStream _fs = File.Open(filePath, FileMode.Open);
            StreamReader _reader = new StreamReader(_fs);
            return JsonConvert.DeserializeObject<List<LogModel>>(_reader.ReadToEnd());
        }
    }
}
