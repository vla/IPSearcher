using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IPSearcher
{
    /// <summary>
    /// IP数据存储生成器
    /// </summary>
    public class IpGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IpGenerator"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public IpGenerator(IpEntity[] data)
        {
            Data = data;
        }

        /// <summary>
        /// IP数据信息
        /// </summary>
        public IpEntity[] Data { get; }

        /// <summary>
        /// 将IP数据源写入目标流
        /// </summary>
        /// <param name="stream">目标流</param>
        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
            {
               //TODO build ip data
            }
        }
    }
}
