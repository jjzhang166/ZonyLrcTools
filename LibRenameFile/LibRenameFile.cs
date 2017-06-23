using LibPlug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibPlug.Interface;
using LibPlug.Model;
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

        private void itemClickEvent(object sender, EventArgs e)
        {
            if (MessageBox.Show(text: "是否重命名列表当中的歌曲文件？", caption: "提示", icon: MessageBoxIcon.Information, buttons: MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (m_resourceModel.MusicInfos.Count == 0)
                {
                    MessageBox.Show(text: "列表中没有歌曲!", caption: "提示", icon: MessageBoxIcon.Information, buttons: MessageBoxButtons.OK);
                    return;
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
