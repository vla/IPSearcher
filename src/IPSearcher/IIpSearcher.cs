using System;
using System.Collections.Generic;
using System.Text;

namespace IPSearcher
{
    /// <summary>
    /// IP搜索器
    /// </summary>
    public interface IIpSearcher
    {
        /// <summary>
        /// IP总数
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 数据源
        /// </summary>
        IDataSource DataSource { get; }

        /// <summary>
        /// IP查询
        /// </summary>
        /// <param name="address">IP地址</param>
        /// <returns>IP位置信息</returns>
        IpLocation Search(uint address);
    }
}