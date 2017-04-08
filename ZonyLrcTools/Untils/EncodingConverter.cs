using System;
using System.Text;

namespace ZonyLrcTools.Untils
{
    /// <summary>
    /// 编码转换器
    /// </summary>
    public class EncodingConverter
    {
        /// <summary>
        /// 将源字节集转换为目标编码
        /// </summary>
        /// <param name="_sourceBytes">字节集</param>
        /// <param name="encodingName">编码名称</param>
        /// <returns></returns>
        public virtual byte[] ConvertBytes(byte[] _sourceBytes, string encodingName)
        {
            return Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(encodingName), _sourceBytes);
        }
    }

    public sealed class EncodingUTF8_Bom : EncodingConverter
    {
        public override byte[] ConvertBytes(byte[] _sourceBytes, string encodingName)
        {
            byte[] _tmpData = new byte[_sourceBytes.Length + 3];
            _tmpData[0] = 0xef;
            _tmpData[1] = 0xbb;
            _tmpData[2] = 0xbf;

            Array.Copy(_sourceBytes, 0, _tmpData, 3, _sourceBytes.Length);
            return _tmpData;
        }
    }

    public sealed class EncodingANSI : EncodingConverter
    {
        public override byte[] ConvertBytes(byte[] _sourceBytes, string encodingName)
        {
            return Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(encodingName), _sourceBytes);
        }
    }
}