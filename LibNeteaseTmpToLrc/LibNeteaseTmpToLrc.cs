using LibNet;
using LibPlug;
using LibPlug.Interface;
using LibPlug.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibNeteaseTmpToLrc
{
    [Plugins("网易云临时文件转LRC", "Zony", "可以将网易云临时歌词文件转换为lrc文件", 1000, PluginTypesEnum.DIY)]
    public class LibNeteaseTmpToLrc : IPlug_DIY
    {
        public PluginsAttribute PlugInfo { get; set; }
        private ResourceModel m_resource;

        public void Init(ResourceModel shareResModel)
        {
            m_resource = shareResModel;
            var _button = shareResModel.UI_Main_ListView_RightClickMenu.Items.Add("临时歌词文件转换");
            _button.Click += conversionToLrc;
        }

        /// <summary>
        /// 转换操作
        /// </summary>
        private void conversionToLrc(object sender, EventArgs e)
        {
            m_resource.UI_Main_TopButtonMenu.Items[0].Enabled = false;
            m_resource.UI_Main_TopButtonMenu.Items[2].Enabled = false;
            m_resource.UI_Main_BottomProgressBar.Maximum = 0;
            m_resource.UI_Main_BottomProgressBar.Value = 0;
            m_resource.MusicInfos.Clear();
            m_resource.UI_Main_ListView.Items.Clear();

            FolderBrowserDialog _dlg = new FolderBrowserDialog();
            _dlg.Description = "请选择网易云临时歌词文件路径";
            _dlg.ShowDialog();
            if (!string.IsNullOrEmpty(_dlg.SelectedPath))
            {
                string[] _files = Directory.GetFiles(_dlg.SelectedPath, "*.*");
                var _result = _files.Where(x => x.Split('.').Length == 1);

                int _successCount = 0;
                int _failedCount = 0;
                int _currentPos = 0;
                foreach (var file in _result)
                {
                    m_resource.UI_Main_ListView.Items.Add(new ListViewItem(new string[]
                    {
                        Path.GetFileName(file),file,"","","","",""
                    }));
                }

                new Task(() =>
                {
                    foreach (var item in _result)
                    {
                        if (tmpConvert(item))
                        {
                            m_resource.UI_Main_ListView.Items[_currentPos].SubItems[6].Text = "成功";
                            _successCount++;
                        }
                        else
                        {
                            m_resource.UI_Main_ListView.Items[_currentPos].SubItems[6].Text = "失败";
                            _failedCount++;
                        }
                        _currentPos++;
                    }
                    m_resource.UI_Main_BottomLabel.Text = $"转换成功{_successCount}个文件，失败{_failedCount}个文件.";
                }).Start();
            }

            m_resource.UI_Main_TopButtonMenu.Items[0].Enabled = true;
            m_resource.UI_Main_TopButtonMenu.Items[2].Enabled = true;
        }

        /// <summary>
        /// 临时文件转LRC
        /// </summary>
        /// <param name="filePath">临时文件路径</param>
        /// <returns></returns>
        private bool tmpConvert(string filePath)
        {
            try
            {
                using (FileStream _lrcFileStream = new FileStream(filePath, FileMode.Open))
                {
                    using (StreamReader _lrcFileStreamReader = new StreamReader(_lrcFileStream))
                    {
                        string _lrcData = _lrcFileStreamReader.ReadToEnd();
                        JObject _jsonLrc = JObject.Parse(_lrcData);
                        List<string> _lrcItem = new List<string>();

                        // 判断是否有翻译歌词
                        if (!_jsonLrc["translateLyric"].ToString().Equals(""))
                        {
                            string _orglrc = _jsonLrc["lyric"].ToString();
                            Regex _orgReg = new Regex(@"\[\d+:\d+.\d+\].+");
                            var _orgResult = _orgReg.Matches(_orglrc);
                            foreach (var item in _orgResult)
                            {
                                _lrcItem.Add(item.ToString());
                            }
                            // 获得翻译歌词
                            string _transLrc = _jsonLrc["translateLyric"].ToString();
                            Regex _transReg = new Regex(@"\[\d+:\d+.\d+\].+");
                            var _transResult = _transReg.Matches(_transLrc);
                            // 分割并且并入英文歌词当中
                            int _count = 0;
                            foreach (var item in _transResult)
                            {
                                var _tmp = item.ToString();
                                string[] _lrcItemArray = _tmp.Split(']');
                                _lrcItem[_count] = string.Format("{0} {1}", _lrcItem[_count], _lrcItemArray[1]);
                                _count++;
                            }
                        }
                        else
                        {
                            string _orglrc = _jsonLrc["lyric"].ToString();
                            Regex _orgReg = new Regex(@"\[\d+:\d+.\d+\].+");
                            var _orgResult = _orgReg.Matches(_orglrc);
                            foreach (var item in _orgResult)
                            {
                                _lrcItem.Add(item.ToString());
                            }
                        }
                        // 获得歌手信息与歌曲名称
                        NetUtils nt = new NetUtils();
                        string _musicID = _jsonLrc["musicId"].ToString();
                        string lrcresult = nt.HttpGet($"http://music.163.com/api/song/detail/?id={_musicID}&ids=%5B{_musicID}%5D", Encoding.UTF8);
                        JObject _jsonMusic = JObject.Parse(lrcresult);
                        JArray _jsonSongs = (JArray)_jsonMusic["songs"];
                        JArray _jsonArtists = (JArray)_jsonSongs[0]["artists"];
                        string _artist = _jsonArtists[0]["name"].ToString();
                        string _title = _jsonSongs[0]["name"].ToString();
                        // 构建文件名
                        string _fileName = string.Format("{0} - {1}", _artist, _title);

                        // 关闭文件流
                        _lrcFileStreamReader.Close();
                        _lrcFileStream.Close();

                        //构造歌词数据
                        StringBuilder _sb = new StringBuilder();
                        foreach (var item in _lrcItem)
                        {
                            _sb.Append(item + "\n");
                        }
                        // 输出到文件
                        string path = Path.GetDirectoryName(filePath) + @"\" + _fileName + ".lrc";
                        FileStream _wrfs = new FileStream(path, FileMode.Create);
                        byte[] _lrcbyte = Encoding.UTF8.GetBytes(_sb.ToString());
                        _wrfs.Write(_lrcbyte, 0, _lrcbyte.Length);
                        _wrfs.Close();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}