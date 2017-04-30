using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibLyricBaiDu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibLyricBaiDu.Tests
{
    [TestClass()]
    public class LibLyricBaiDuMusicTests
    {
        [TestMethod()]
        public void DownLoadTest()
        {
            new LibLyricBaiDuMusic().DownLoad("周杰伦","黄金甲",out byte[] lrcData,false);
            Assert.Fail();
        }
    }
}