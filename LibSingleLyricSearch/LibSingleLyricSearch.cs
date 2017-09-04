using LibPlug;
using LibPlug.Interface;
using LibPlug.Model;
using System;

namespace LibSingleLyricSearch
{
    [Plugins("单歌词搜索插件", "Zony", "可以进行单首歌曲的歌词进行搜索。", 1000, PluginTypesEnum.DIY)]
    public class LibSingleLyricSearch : IPlug_DIY
    {
        public PluginsAttribute PlugInfo { get; set; }

        public void Init(ResourceModel shareResModel)
        {
            var _buttonRef = shareResModel.UI_Main_TopButtonMenu.Items.Add("歌词搜索");
            _buttonRef.Click += (object sender, EventArgs e) => new UI_SearchWindow(shareResModel).Show();
        }
    }
}
