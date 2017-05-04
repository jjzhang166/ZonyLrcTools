using LibPlug;
using LibPlug.Interface;
using LibPlug.Model;
using System;

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
            m_resource.UI_Main_TopButtonMenu.Items[1].Enabled = false;
            throw new NotImplementedException();
        }
    }
}