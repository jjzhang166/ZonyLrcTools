using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Linq;
using ZonyLrcTools.EnumDefine;
using ZonyLrcTools.Untils;
using LibPlug.Model;
using LibPlug;
using LibNet;
using LibPlug.Interface;

namespace ZonyLrcTools.UI
{
    public partial class UI_Main : Form
    {
        public UI_Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置工作目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SetWorkDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog _folderDlg = new FolderBrowserDialog();
            _folderDlg.Description = "请选择程序的工作目录:";
            _folderDlg.ShowDialog();

            if (!string.IsNullOrEmpty(_folderDlg.SelectedPath))
            {
                disenabledButton();
                clearContainer();
                setBottomStatusText(StatusHeadEnum.NORMAL, "开始扫描目录...");
                progress_DownLoad.Value = 0;

                string[] _files = FileUtils.SearchFiles(_folderDlg.SelectedPath, SettingManager.SetValue.FileSuffixs.Split(';'));
                for (int i = 0; i < _files.Length; i++) GlobalMember.AllMusics.Add(i, new MusicInfoModel() { Path = _files[i] });

                if (_files.Length > 0)
                {
                    progress_DownLoad.Value = 0; progress_DownLoad.Maximum = GlobalMember.AllMusics.Count;
                    getMusicInfoAndFillList(GlobalMember.AllMusics);
                }
                else
                {
                    setBottomStatusText(StatusHeadEnum.NORMAL, "并没有搜索到文件...");
                    enabledButton();
                }
            }
        }

        /// <summary>
        /// 快捷键检测
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (listView_MusicInfos.SelectedItems.Count != 0)
            {
                if (keyData == (Keys.Control | Keys.S))
                {
                    int _selectCount = listView_MusicInfos.Items.IndexOf(listView_MusicInfos.FocusedItem);
                    MusicInfoModel _info = GlobalMember.AllMusics[_selectCount];
                    _info.Artist = textBox_Aritst.Text;
                    _info.SongName = textBox_MusicTitle.Text;
                    _info.Album = textBox_Album.Text;
                    GlobalMember.MusicTagPluginsManager.Plugins[0].SaveTag(_info, null, textBox_Lryic.Text);
                    MessageBox.Show("已经保存歌曲标签信息!", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            return false;
        }

        private void UI_Main_Load(object sender, EventArgs e)
        {
            setBottomStatusText(StatusHeadEnum.WAIT, "等待用户操作...");
            CheckForIllegalCrossThreadCalls = false;
            var _res = resourceInit();

            if (GlobalMember.MusicTagPluginsManager.LoadPlugins() == 0) setBottomStatusText(StatusHeadEnum.ERROR, "加载MusicTag插件管理器失败...");
            if (GlobalMember.LrcPluginsManager.LoadPlugins() == 0) setBottomStatusText(StatusHeadEnum.ERROR, "加载歌词下载插件失败...");
            if (GlobalMember.DIYPluginsManager.LoadPlugins(_res) == 0) setBottomStatusText(StatusHeadEnum.ERROR, "自定义高级插件加载失败...");

            SettingManager.Load();
            GlobalMember.DIYPluginsManager.InitPlugins(); //高级插件延迟加载
            if (!SettingManager.SetValue.IsAgree) new UI_About().ShowDialog();
            if (SettingManager.SetValue.IsCheckUpdate)
            {
                if (VersionManager.CheckUpdate())
                {
                    if (MessageBox.Show(string.Format("检测到新版本，是否下载?\r\n更新内容:\r\n{0}", VersionManager.Info.UpdateInfo), "检测到更新", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        Process.Start(VersionManager.Info.DownLoadUrl);
                    }
                }
            }
            loadMenuIcon();
            bindUIMthod();
        }

        /// <summary>
        /// 动态加载歌曲信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_MusicInfos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_MusicInfos.SelectedItems.Count > 0)
            {
                int _selectIndex = listView_MusicInfos.SelectedItems[0].Index;
                MusicInfoModel _info = GlobalMember.AllMusics[_selectIndex];

                textBox_Aritst.Text = _info.Artist;
                textBox_MusicTitle.Text = _info.SongName;
                textBox_Album.Text = _info.Album;
                Stream _imgStream = GlobalMember.MusicTagPluginsManager.Plugins[0].LoadAlbumImg(_info.Path);
                if (_imgStream == null) pictureBox_AlbumImage.Image = null;
                else pictureBox_AlbumImage.Image = Image.FromStream(_imgStream);
                if (_info.IsBuildInLyric) textBox_Lryic.Text = GlobalMember.MusicTagPluginsManager.Plugins[0].LoadLyricText(_info.Path);
            }
        }

        /// <summary>
        /// 下载单首歌曲的歌词，支持多插件
        /// </summary>
        private void ToolStripMenuItem_DownLoadSelectMusic_Click(object sender, EventArgs e)
        {
            if (listView_MusicInfos.SelectedItems.Count == 0) return;

            var _downloadList = new Dictionary<int, MusicInfoModel>();
            foreach (ListViewItem item in listView_MusicInfos.SelectedItems)
            {
                _downloadList.Add(item.Index, GlobalMember.AllMusics[item.Index]);
            }

            // 选择下载插件
            var _dlg = new UI_PluginSelect();
            _dlg.ShowDialog();
            if (!string.IsNullOrEmpty(_dlg.SelectPluginName))
            {
                var _plug = GlobalMember.LrcPluginsManager.BaseOnNameGetPlugin(_dlg.SelectPluginName);
                parallelDownLoadLryic(_downloadList, _plug);
            }
        }

        /// <summary>
        /// 歌词下载按钮点击事件
        /// </summary>
        private void button_DownLoadLyric_Click(object sender, EventArgs e)
        {
            if (listView_MusicInfos.Items.Count == 0) setBottomStatusText(StatusHeadEnum.ERROR, "请选择歌曲目录再尝试下载歌词！");

            List<IPlug_Lrc> _openPlugins = GlobalMember.LrcPluginsManager.BaseOnTypeGetPlugins(PluginTypesEnum.LrcSource);
            foreach (var item in _openPlugins)
            {
                parallelDownLoadLryic(GlobalMember.AllMusics, item);
            }
        }

        /// <summary>
        /// 下载列表当中所有的专辑图像
        /// </summary>
        private void button_DownLoadAlbumImage_Click(object sender, EventArgs e)
        {
            if (listView_MusicInfos.Items.Count != 0)
            {
                parallelDownLoadAlbumImg(GlobalMember.AllMusics);
            }
            else setBottomStatusText(StatusHeadEnum.ERROR, "请选择歌曲目录再尝试下载专辑图像！");
        }

        /// <summary>
        /// 下载单首歌曲的专辑图像
        /// </summary>
        private void ToolStripMenuItem_DownLoadSelectedAlbumImg_Click(object sender, EventArgs e)
        {
            if (listView_MusicInfos.SelectedItems.Count != 0)
            {
                #region > 获得选中条目的歌曲信息并且加入容器 <
                var _tempDic = new Dictionary<int, MusicInfoModel>();
                foreach (ListViewItem item in listView_MusicInfos.SelectedItems)
                {
                    _tempDic.Add(item.Index, GlobalMember.AllMusics[item.Index]);
                }
                #endregion
                parallelDownLoadAlbumImg(_tempDic);
            }
        }

        /// <summary>
        /// 打开歌曲所在文件夹
        /// </summary>
        private void ToolStripMenuItem_OpenFileFolder_Click(object sender, EventArgs e)
        {
            if (listView_MusicInfos.SelectedItems.Count != 0)
            {
                int _selectCount = listView_MusicInfos.Items.IndexOf(listView_MusicInfos.FocusedItem);
                string _path = GlobalMember.AllMusics[_selectCount].Path;
                FileUtils.OpenFilePos(_path);
            }
        }

        /// <summary>
        /// 添加歌曲文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_AddDirectory_Click(object sender, EventArgs e)
        {
            disenabledButton(); progress_DownLoad.Value = 0;
            FolderBrowserDialog _dlg = new FolderBrowserDialog();
            _dlg.Description = "请选择歌曲文件夹";
            if (_dlg.ShowDialog() == DialogResult.OK)
            {
                if (Directory.Exists(_dlg.SelectedPath))
                {
                    setBottomStatusText(StatusHeadEnum.NORMAL, "开始扫描目录...");
                    progress_DownLoad.Value = 0;
                    string[] _files = FileUtils.SearchFiles(_dlg.SelectedPath, SettingManager.SetValue.FileSuffixs.Split(';'));
                    if (_files.Length < 0)
                    {
                        setBottomStatusText(StatusHeadEnum.NORMAL, "没有搜索到任何支持的文件！");
                        return;
                    }
                    Dictionary<int, MusicInfoModel> _tmpDic = new Dictionary<int, MusicInfoModel>();
                    for (int i = 0; i < _files.Length; i++) _tmpDic.Add(GlobalMember.AllMusics.Count == 0 ? i : GlobalMember.AllMusics.Count + i, new MusicInfoModel { Path = _files[i] });
                    progress_DownLoad.Value = 0; progress_DownLoad.Maximum = _tmpDic.Count;
                    getMusicInfoAndFillList(_tmpDic);
                    GlobalMember.AllMusics.Concat(_tmpDic);
                }
            }
            else enabledButton();
        }

        /// <summary>
        /// 保存专辑图像
        /// </summary>
        private void ToolStripMenuItem_SaveAlbumImage_Click(object sender, EventArgs e)
        {
            if (pictureBox_AlbumImage.Image != null)
            {
                SaveFileDialog _dlg = new SaveFileDialog();
                _dlg.Title = "保存专辑图像";
                _dlg.Filter = "*.png|*.png|*.bmp|*.bmp";
                _dlg.ShowDialog();
                if (!string.IsNullOrEmpty(_dlg.FileName))
                {
                    switch (Path.GetExtension(_dlg.FileName))
                    {
                        case ".png":
                            pictureBox_AlbumImage.Image.Save(_dlg.FileName, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                        case ".bmp":
                            pictureBox_AlbumImage.Image.Save(_dlg.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                    }
                    setBottomStatusText(StatusHeadEnum.SUCCESS, "保存图像成功!");
                }
            }
            else setBottomStatusText(StatusHeadEnum.ERROR, "并没有图片让你保存哦!");
        }

        /// <summary>
        /// 批量改名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_RenameFile_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 批量更改文件名
        /// </summary>
        private void batchChangeSongName()
        {
            if (listView_MusicInfos.Items.Count > 0)
            {
                setBottomStatusText(StatusHeadEnum.NORMAL, "正在批量更名...");
                progress_DownLoad.Value = 0;
                progress_DownLoad.Maximum = GlobalMember.AllMusics.Count;
                Task.Run(() =>
                {
                    foreach (var item in GlobalMember.AllMusics)
                    {
                        string _newFileName = item.Value.SongName + "(" + item.Value.Artist + ")" + Path.GetExtension(item.Value.Path);
                        string _newPath = Path.GetDirectoryName(item.Value.Path) + @"\" + _newFileName;
                        try
                        {
                            File.Move(item.Value.Path, _newPath);
                            item.Value.Path = _newPath;
                        }
                        catch { }
                    }
                    setBottomStatusText(StatusHeadEnum.COMPLETE, "更改文件名成功!");
                });
            }
        }

        /// <summary>
        /// 获得拖拽目录并进行扫描
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_MusicInfos_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
        }

        private void listView_MusicInfos_DragOver(object sender, DragEventArgs e)
        {
            var _path = ((string[])e.Data.GetData(DataFormats.FileDrop));
            if (_path.Length > 0)
            {
                var _result = searchFolderFiles(_path);
            }
        }
    }
}