using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using LibNet;
using Newtonsoft.Json.Linq;
using System.IO;
using LibPlug.Model;

namespace LibSingleLyricSearch
{
    public partial class UI_SearchWindow : Form
    {
        private readonly NetUtils m_netUtils;
        private readonly ResourceModel m_resourceModel;

        public UI_SearchWindow()
        {
            InitializeComponent();
        }

        public UI_SearchWindow(ResourceModel resource) : this()
        {
            m_netUtils = new NetUtils();
            m_resourceModel = resource;
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            listView_LyricList.Items.Clear();

            string _artist, _songName;
            const string _requestUrl = @"http://music.163.com/api/search/get/web?csrf_token=";
            const string _referer = @"http://music.163.com";

            _artist = m_netUtils.URL_Encoding(textBox_Artist.Text, Encoding.UTF8);
            _songName = m_netUtils.URL_Encoding(textBox_SongName.Text, Encoding.UTF8);

            string _buildKey = $"{_artist}+{_songName}";
            string _requestData = $"&s={_buildKey}&type=1&offset=0&total=true&limit=5";
            string _result = m_netUtils.HttpPost(_requestUrl, Encoding.UTF8, _requestData, _referer);

            var _list = decodeSongList(_result);
            renderListView(_list);
        }

        private void ToolStripMenuItem_DownLoad_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView_LyricList.SelectedItems)
            {
                string _lyricText = getLyricText(item.SubItems[2].Text);
                string _lyricPath = Environment.CurrentDirectory + @"\Lyric";
                string _fileName = $"{item.SubItems[0].Text}{item.SubItems[1].Text}.lrc";

                if (!Directory.Exists(_lyricPath)) Directory.CreateDirectory(_lyricPath);
                try
                {
                    using (FileStream _fs = new FileStream($@"{_lyricPath}\{_fileName}", FileMode.OpenOrCreate))
                    {
                        byte[] _data = Encoding.UTF8.GetBytes(_lyricText);
                        _fs.Write(_data, 0, _data.Length);
                    }
                    MessageBox.Show("下载成功,歌词文件保存在工具的'Lyric'目录当中!", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    if (MessageBox.Show(text: "歌曲IDv3有不符合规定的文件名称，无法保存，是否重命名lrc文件名称?", icon: MessageBoxIcon.Information, caption: "错误", buttons: MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        SaveFileDialog _saveDialog = new SaveFileDialog() { Filter = "*.lrc|*.lrc", Title = "lrc另存为" };
                        if (_saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            if (!string.IsNullOrEmpty(_saveDialog.FileName))
                            {
                                using (FileStream _saveFs = new FileStream($@"{_lyricPath}\{_saveDialog.FileName}", FileMode.OpenOrCreate))
                                {
                                    byte[] _data = Encoding.UTF8.GetBytes(_lyricText);
                                    _saveFs.Write(_data, 0, _data.Length);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void listView_LyricList_ItemActivate(object sender, EventArgs e)
        {
            if (listView_LyricList.SelectedItems != null)
            {
                var _window = new UI_LyricView();
                _window.LyricText = getLyricText(listView_LyricList.SelectedItems[0].SubItems[2].Text);
                _window.Show();
            }
        }

        /// <summary>
        /// 解析歌词列表
        /// </summary>
        /// <returns></returns>
        private List<SongListItem> decodeSongList(string jsonList)
        {
            List<SongListItem> _resultList = new List<SongListItem>();

            if (!string.IsNullOrEmpty(jsonList) && jsonList.IndexOf("songs") != -1)
            {
                JObject _result = JObject.Parse(jsonList);
                JArray _jArray = (JArray)_result["result"]["songs"];
                foreach (var item in _jArray)
                {
                    _resultList.Add(new SongListItem()
                    {
                        SongName = item.Value<string>("name"),
                        SongID = item.Value<string>("id"),
                        Artist = ((JArray)item["artists"])[0].Value<string>("name")
                    });
                }
            }
            return _resultList;
        }

        /// <summary>
        /// 渲染列表
        /// </summary>
        /// <param name="list">歌词列表</param>
        private void renderListView(List<SongListItem> list)
        {
            foreach (var item in list)
            {
                listView_LyricList.Items.Add(new ListViewItem(new string[] { item.SongName, item.Artist, item.SongID }));
            }
        }

        /// <summary>
        /// 获得指定SID的歌词数据
        /// </summary>
        /// <param name="sid"></param>
        private string getLyricText(string sid)
        {
            string _lrcUrl = "http://music.163.com/api/song/lyric?os=osx&id=" + sid + "&lv=-1&kv=-1&tv=-1";
            string _lyricJson = m_netUtils.HttpGet(_lrcUrl, Encoding.UTF8, @"http://music.163.com");

            JObject _jObj = JObject.Parse(_lyricJson);
            return _jObj["lrc"] != null ? _jObj["lrc"]["lyric"].ToString() : "暂时没有歌词";
        }

        private void listView_LyricList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Link;
        }

        private void listView_LyricList_DragOver(object sender, DragEventArgs e)
        {
            var _path = ((string[])e.Data.GetData(DataFormats.FileDrop));
            if (_path.Length > 0)
            {
                MusicInfoModel _model = new MusicInfoModel
                {
                    Path = _path[0],
                };
                m_resourceModel.MusicTagUtils.LoadTag(_model.Path, _model);
                textBox_Artist.Text = _model.Artist;
                textBox_SongName.Text = _model.SongName;
                button_Search_Click(sender, e);
            }
        }

        /// <summary>
        /// 歌词结果模型
        /// </summary>
        private class SongListItem
        {
            /// <summary>
            /// 歌曲名称
            /// </summary>
            public string SongName { get; set; }
            /// <summary>
            /// 艺术家/歌手
            /// </summary>
            public string Artist { get; set; }
            /// <summary>
            /// 歌曲SID
            /// </summary>
            public string SongID { get; set; }
        }
    }
}
