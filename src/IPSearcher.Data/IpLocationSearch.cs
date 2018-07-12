using System;
using System.Net;

namespace IPSearcher
{
    /// <summary>
    /// IpLocationSearch
    /// </summary>
    public class IpLocationSearch
    {
        private static Lazy<IDataSource> LoadInnerDataSource = new Lazy<IDataSource>(() =>
        {
            var t = typeof(IpLocationSearch);

            var name = "IPSearcher.Data.ip.dat";

            using (var stream = t.Assembly.GetManifestResourceStream(name))
            {
                return new MemoryDataSource(stream);
            }
        });

        private static Lazy<IIpSearcher> Searcher = new Lazy<IIpSearcher>(() =>
        {
            return GetSearcher();
        });

        /// <summary>
        /// 获取内部数据源
        /// </summary>
        /// <returns>IDataSource</returns>
        public static IDataSource GetInnerDataSource()
        {
            return LoadInnerDataSource.Value;
        }

        /// <summary>
        /// 获取IP搜索器
        /// </summary>
        /// <returns>
        ///   <see cref="IIpSearcher" />
        /// </returns>
        public static IIpSearcher GetSearcher()
        {
            return IpSearcherFactory.GetSearcher(GetInnerDataSource());
        }

        /// <summary>
        /// IP位置查询
        /// </summary>
        /// <param name="address">ipv4地址</param>
        /// <returns>
        /// IP位置信息
        /// </returns>
        public static IpLocation Find(string address)
        {
            if (IPAddress.TryParse(address, out IPAddress ip))
            {
                var octets = ip.GetAddressBytes();

                if (octets.Length == 4)
                {
                    return Searcher.Value.Search(IpLocationHelper.IPv4ToInteger(ip.GetAddressBytes()));
                }
            }

            return null;
        }

        /// <summary>
        /// IP位置查询
        /// </summary>
        /// <param name="address">ipv4地址</param>
        /// <returns>
        /// IP位置信息
        /// </returns>
        public static IpLocation Find(uint address)
        {
            return Searcher.Value.Search(address);
        }
    }
}