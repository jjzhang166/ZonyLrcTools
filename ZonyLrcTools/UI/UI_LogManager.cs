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
        private readonly List<string> m_filePaths;
        private readonly List<LogModel> m_bindLogModel;

        public UI_LogManager()
        {
            m_filePaths = new List<string>();
            m_bindLogModel = new List<LogModel>();
            InitializeComponent();
        }

        private void UI_LogManager_Load(object sender, EventArgs e)
        {
            fillListBox();
        }

        private void listBox_FileViewer_DoubleClick(object sender, EventArgs e)
        {
            int _index = listBox_FileViewer.SelectedIndex;
            if (_index >= 0)
            {
                if (File.Exists(m_filePaths[_index]))
                {
                    var _logItems = LogManager.ReadLogFile(m_filePaths[_index]);
                    if (_logItems == null) return;
                    m_bindLogModel.Clear();
                    foreach (var item in _logItems)
                    {
                        listBox_LogItem.Items.Add(item.Information);
                        m_bindLogModel.Add(item);
                    }
                }
            }
        }

        private void fillListBox()
        {
            string[] _files = Directory.GetFiles(Environment.CurrentDirectory + @"\LogFiles", "*.log");
            foreach (var item in _files)
            {
                m_filePaths.Add(item);
                listBox_FileViewer.Items.Add(Path.GetFileName(item));
            }
        }

        private void listBox_LogItem_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}
