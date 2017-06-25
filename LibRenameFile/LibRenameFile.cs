using LibPlug;
using LibPlug.Interface;
using LibPlug.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LibRenameFile
{
    [Plugins("重命名插件", "Zony", "提供歌曲文件名重命名功能", 1000, PluginTypesEnum.DIY)]
    public class LibRenameFile : IPlug_DIY
    {
        public PluginsAttribute PlugInfo { get; set; }

        private ResourceModel m_resourceModel;

        public void Init(ResourceModel shareResModel)
        {
            m_resourceModel = shareResModel;
            buildMenuItem();
        }

        /// <summary>
        /// 构建菜单条目
        /// </summary>
        private void buildMenuItem()
        {
            var _item = m_resourceModel.UI_Main_TopButtonMenu.Items.Add("重命名歌曲文件名称", Icon.meun_icon.ToBitmap());
            _item.Click += itemClickEvent;
        }

        /// <summary>
        /// 构建新文件名称
        /// </summary>
        /// <returns></returns>
        private string buildNewFileName(string sourceFileName)
        {
            string _newFileName = $"{ Path.GetDirectoryName(sourceFileName)}";
            return null;
        }

        /// <summary>
        /// 重置UI的进度条
        /// </summary>
        /// <param name="progressMaximun">进度条终值</param>
        private void resetUICounter(int progressMaximun = 0)
        {
            m_resourceModel.UI_Main_BottomProgressBar.Maximum = progressMaximun;
        }

        /// <summary>
        /// 启用顶部按钮
        /// </summary>
        private void enableTopMenu()
        {

        }

        /// <summary>
        /// 禁用顶部按钮
        /// </summary>
        private void disEnableTopMeun()
        {

        }

        /// <summary>
        /// 菜单点击事件
        /// </summary>
        private void itemClickEvent(object sender, EventArgs e)
        {
            if (MessageBox.Show(text: "是否重命名列表当中的歌曲文件？", caption: "提示", icon: MessageBoxIcon.Information, buttons: MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (m_resourceModel.MusicInfos.Count == 0)
                {
                    MessageBox.Show(text: "列表中没有歌曲!", caption: "提示", icon: MessageBoxIcon.Information, buttons: MessageBoxButtons.OK);
                    return;
                }

                List<string> _newFileNames = new List<string>(m_resourceModel.MusicInfos.Count);
                // 构建新的文件路径
                foreach (var file in m_resourceModel.MusicInfos)
                {
                    FileInfo _info = new FileInfo(file.Value.Path);
                    string _newFileName = buildNewFileName(_info.FullName);
                    if (File.Exists(_newFileName)) return;
                }
            }
            else
            {
                var _folderDlg = new FolderBrowserDialog();
                _folderDlg.ShowDialog();
                if (!string.IsNullOrEmpty(_folderDlg.SelectedPath))
                {

                }
            }
        }
    }
}
