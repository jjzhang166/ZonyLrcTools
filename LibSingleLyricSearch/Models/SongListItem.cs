namespace LibSingleLyricSearch.Models
{
    /// <summary>
    /// 歌词结果模型
    /// </summary>
    public class SongListItem
    {
        /// <summary>
        /// 歌曲名称
        /// </summary>
        public string SongName { get; set; }
        /// <summary>
        /// 艺术家/歌手
        /// </summary>
        public string Artist { get; set; }
        /// <summary>
        /// 歌曲SID
        /// </summary>
        public string SongID { get; set; }
    }
}
