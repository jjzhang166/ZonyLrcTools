using LibPlug;
using LibPlug.Interface;
using LibPlug.Model;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZonyLrcTools.EnumDefine;
using ZonyLrcTools.Untils;

namespace ZonyLrcTools.UI
{
    public partial class UI_Main : Form
    {
        /// <summary>
        /// 转换文本编码
        /// </summary>
        /// <returns></returns>
        private byte[] encodingConvert(byte[] sourceBytes)
        {
            EncodingConverter _convert = new EncodingConverter();
            switch (SettingManager.SetValue.EncodingName)
            {
                case "utf-8 bom":
                    _convert = new EncodingUTF8_Bom();
                    break;
                case "ANSI":
                    _convert = new EncodingANSI();
                    break;
                default:
                    _convert = new EncodingConverter();
                    break;
            }
            return _convert.ConvertBytes(sourceBytes, SettingManager.SetValue.EncodingName);
        }

        /// <summary>
        /// 搜索多路径的所有文件，并将其加入数据当中
        /// </summary>
        /// <param name="paths">多个路径的集合数组</param>
        /// <returns></returns>
        private string[] searchFolderFiles(string[] paths)
        {
            List<string> _pathList = new List<string>();
            foreach (var item in paths)
            {
                _pathList.AddRange(FileUtils.SearchFiles(item, SettingManager.SetValue.FileSuffixs.Split(';')));
            }

            return _pathList.ToArray();
        }

        /// <summary>
        /// 禁用按钮
        /// </summary>
        private void disenabledButton()
        {
            button_SetWorkDirectory.Enabled = button_DownLoadLyric.Enabled = button_DownLoadAlbumImage.Enabled = false;
        }

        /// <summary>
        /// 启用按钮
        /// </summary>
        private void enabledButton()
        {
            button_SetWorkDirectory.Enabled = button_DownLoadLyric.Enabled = button_DownLoadAlbumImage.Enabled = true;
        }

        /// <summary>
        /// 清空歌曲容器
        /// </summary>
        private void clearContainer()
        {
            GlobalMember.AllMusics.Clear();
            listView_MusicInfos.Items.Clear();
        }

        /// <summary>
        /// 并行下载歌词任务
        /// </summary>
        /// <param name="down">插件</param>
        /// <param name="list">待下载列表</param>
        private async void parallelDownLoadLryic(Dictionary<int, MusicInfoModel> list, IPlug_Lrc down)
        {
            setBottomStatusText(StatusHeadEnum.NORMAL, "正在下载歌词...");
            progress_DownLoad.Maximum = list.Count;
            progress_DownLoad.Value = 0;
            disenabledButton();

            await Task.Run(() =>
            {
                Parallel.ForEach(list, new ParallelOptions() { MaxDegreeOfParallelism = SettingManager.SetValue.DownloadThreadNum }, (item) =>
                {
                    string _path = FileUtils.BuildLrcPath(item.Value.Path);
                    if (SettingManager.SetValue.IsIgnoreExitsFile && File.Exists(_path))
                    {
                        listView_MusicInfos.Items[item.Key].SubItems[6].Text = "略过";
                        return;
                    }

                    bool _isSuccess = down.DownLoad(item.Value.Artist, item.Value.SongName, out byte[] _lrcData, SettingManager.SetValue.IsDownTranslate);
                    if (!_isSuccess)
                    {
                        listView_MusicInfos.Items[item.Key].SubItems[6].Text = "失败";
                        return;
                    }

                    string _lrcPath = FileUtils.BuildLrcPath(item.Value.Path, SettingManager.SetValue.UserDirectory);
                    _lrcData = encodingConvert(_lrcData);
                    FileUtils.WriteFile(_lrcPath, _lrcData);

                    listView_MusicInfos.Items[item.Key].SubItems[6].Text = "成功";
                    progress_DownLoad.Value += 1;
                });
                setBottomStatusText(StatusHeadEnum.SUCCESS, "歌词下载完成！");
                enabledButton();
            });
        }

        /// <summary>
        /// 并行下载专辑图像任务
        /// </summary>
        private async void parallelDownLoadAlbumImg(Dictionary<int, MusicInfoModel> list)
        {
            setBottomStatusText(StatusHeadEnum.NORMAL, "正在下载专辑图像...");
            progress_DownLoad.Maximum = list.Count;
            progress_DownLoad.Value = 0;
            disenabledButton();
            await Task.Run(() =>
            {
                Parallel.ForEach(list, new ParallelOptions() { MaxDegreeOfParallelism = SettingManager.SetValue.DownloadThreadNum }, (info) =>
                {
                    lock (info.Value)
                    {
                        if (info.Value.IsAlbumImg) listView_MusicInfos.Items[info.Key].SubItems[6].Text = "略过";
                        else
                        {
                            byte[] _imgBytes;
                            if (GlobalMember.LrcPluginsManager.BaseOnTypeGetPlugins(PluginTypesEnum.AlbumImg)[0].DownLoad(info.Value.Artist, info.Value.SongName, out _imgBytes, SettingManager.SetValue.IsDownTranslate))
                            {
                                GlobalMember.MusicTagPluginsManager.Plugins[0].SaveTag(info.Value, _imgBytes, string.Empty);
                                listView_MusicInfos.Items[info.Key].SubItems[6].Text = "成功";
                            }
                            else listView_MusicInfos.Items[info.Key].SubItems[6].Text = "失败";
                            progress_DownLoad.Value += 1;
                        }
                    }
                });
                setBottomStatusText(StatusHeadEnum.SUCCESS, "下载专辑图像完成...");
                enabledButton();
            });
        }

        /// <summary>
        /// 设置底部状态标识文本
        /// </summary>
        /// <param name="head">状态标识</param>
        /// <param name="content">状态内容</param>
        private void setBottomStatusText(string head, string content)
        {
            statusLabel_StateText.Text = string.Format("{0}:{1}", head, content);
            LogManager.WriteLogRecord(head, content);
        }

        /// <summary>
        /// 填充主界面ListView
        /// </summary>
        /// <param name="musics"></param>
        private void fillMusicListView(Dictionary<int, MusicInfoModel> music)
        {
            setBottomStatusText(StatusHeadEnum.NORMAL, "正在填充列表...");
            progress_DownLoad.Value = 0;
            foreach (var info in music)
            {
                listView_MusicInfos.Items.Insert(info.Key, new ListViewItem(new string[]
                {
                    Path.GetFileName(info.Value.Path),
                    Path.GetDirectoryName(info.Value.Path),
                    info.Value.TagType,
                    info.Value.SongName,
                    info.Value.Artist,
                    info.Value.Album,
                    ""
                }));
                progress_DownLoad.Value += 1;
            }
        }

        /// <summary>
        /// 获得歌曲信息并且填充列表
        /// </summary>
        /// <param name="musics"></param>
        private async void getMusicInfoAndFillList(Dictionary<int, MusicInfoModel> musics)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(musics, (item) =>
                {
                    GlobalMember.MusicTagPluginsManager.Plugins[0].LoadTag(item.Value.Path, item.Value);
                    progress_DownLoad.Value += 1;
                });
                fillMusicListView(musics);
                MessageBox.Show(string.Format("扫描成功，一共有{0}个音乐文件！", musics.Count), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                setBottomStatusText(StatusHeadEnum.SUCCESS, string.Format("扫描成功，一共有{0}个音乐文件！", musics.Count));
                enabledButton();
            });
        }
    }
}
