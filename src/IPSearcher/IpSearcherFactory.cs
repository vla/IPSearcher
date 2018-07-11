using System.IO;

namespace IPSearcher
{
    /// <summary>
    /// IpSearcherFactory
    /// </summary>
    public class IpSearcherFactory
    {
        /// <summary>
        /// 生成IP数据库
        /// </summary>
        /// <param name="data">IP数据源</param>
        /// <param name="stream">目标流</param>
        public static void Generate(IpEntity[] data, Stream stream)
        {
            new IpGenerator(data).Write(stream);
        }

        /// <summary>
        /// 获取IP搜索器
        /// </summary>
        /// <param name="dataSource"><see cref="IDataSource"/></param>
        /// <returns><see cref="IIpSearcher"/></returns>
        public static IIpSearcher GetSearcher(IDataSource dataSource)
        {
            return new DefaultSearcher(dataSource);
        }
    }
}