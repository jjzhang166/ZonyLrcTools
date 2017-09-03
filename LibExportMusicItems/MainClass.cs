using LibPlug.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibPlug;
using LibPlug.Model;

namespace LibExportMusicItems
{
    public class MainClass : IPlug_DIY
    {
        public PluginsAttribute PlugInfo { get; set; }

        public void Init(ResourceModel shareResModel)
        {
            throw new NotImplementedException();
        }

        private bool ExportFile()
        {
            return false;
        }
    }
}
