namespace IPSearcher
{
    /// <summary>
    /// IP位置信息
    /// </summary>
    public class IpEntity
    {
        /// <summary>
        /// 起始IP
        /// </summary>
        public uint MinIP { get; set; }

        /// <summary>
        /// 结束IP
        /// </summary>
        public uint MaxIP { get; set; }

        /// <summary>
        /// 前綴
        /// </summary>
        public byte Prefix { get; set; }

        /// <summary>
        /// 起始IP-字符形式
        /// </summary>
        public string StartIP { get; set; } = string.Empty;

        /// <summary>
        /// 结束IP-字符形式
        /// </summary>
        public string EndIP { get; set; } = string.Empty;

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; } = string.Empty;

        /// <summary>
        ///  城市
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// 运营商
        /// </summary>
        public string Isp { get; set; } = string.Empty;

        public string GetText()
        {
            return $"{Country}|{Province}|{City}|{Isp}";
        }

        public override string ToString()
        {
            return $"{MinIP},{MaxIP},{StartIP},{EndIP},{Province},{City}";
        }
    }
}