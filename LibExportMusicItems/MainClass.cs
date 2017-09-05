using LibPlug;
using LibPlug.Interface;
using LibPlug.Model;
using System;

namespace LibExportMusicItems
{
    public class MainClass : IPlug_DIY
    {
        public PluginsAttribute PlugInfo { get; set; }

        private ResourceModel m_shareResourceModel;

        public void Init(ResourceModel shareResModel)
        {
            m_shareResourceModel = shareResModel;
            var _menu = m_shareResourceModel.UI_Main_ListView_RightClickMenu.Items.Add("导出当前音乐列表");
            _menu.Click += (object sender, EventArgs e) => exportFile();
        }

        private bool exportFile()
        {
            return false;
        }
    }
}
