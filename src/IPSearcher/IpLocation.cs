namespace IPSearcher
{
    /// <summary>
    /// IP位置信息
    /// </summary>
    public class IpLocation
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        ///  城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 运营商
        /// </summary>
        public string Isp { get; set; }

        public override string ToString()
        {
            return $"{Country}|{Province}|{City}|{Isp}";
        }
    }
}