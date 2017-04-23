using LibPlug.Model;
using System;
using System.Windows.Forms;
using ZonyLrcTools.Untils;

namespace ZonyLrcTools.UI
{
    public partial class UI_Main : Form
    {
        /// <summary>
        /// 加载菜单图标数据
        /// </summary>
        private void loadMenuIcon()
        {
            button_SetWorkDirectory.Image = Properties.Resources.directory;
            button_DownLoadAlbumImage.Image = button_DownLoadLyric.Image = Properties.Resources.download;
            button_FeedBack.Image = Properties.Resources.feedback;
            button_DonateAuthor.Image = Properties.Resources.donate;
            button_AboutSoftware.Image = Properties.Resources.about;
            button_PluginsMrg.Image = Properties.Resources.plugins;
            button_Setting.Image = Properties.Resources.setting;
            Icon = Properties.Resources.App;
        }

        /// <summary>
        /// UI点击事件绑定
        /// </summary>
        private void bindUIMthod()
        {
            button_AboutSoftware.Click += delegate { new UI_About().ShowDialog(); };
            button_DonateAuthor.Click += delegate { new UI_Donate().ShowDialog(); };
            button_PluginsMrg.Click += delegate { new UI_PluginsManager().ShowDialog(); };
            button_FeedBack.Click += delegate { new UI_FeedBack().ShowDialog(); };
            button_Setting.Click += delegate { new UI_Settings().ShowDialog(); };
            this.FormClosed += delegate { Environment.Exit(0); };
        }

        /// <summary>
        /// 初始化插件共享资源
        /// </summary>
        private ResourceModel resourceInit()
        {
            ResourceModel _res = new ResourceModel();
            _res.MusicInfos = GlobalMember.AllMusics;
            _res.UI_Main_BottomLabel = statusLabel_StateText;
            _res.UI_Main_ListView = listView_MusicInfos;
            _res.UI_Main_ListView_RightClickMenu = contextMenuStrip_FileListView;
            _res.UI_Main_TopButtonMenu = toolStrip_TopMenus;
            return _res;
        }
    }
}
