using System;

namespace IPSearcher
{
    internal class DefaultSearcher : IIpSearcher
    {
        private const int head_size = 4; //头部长度
        private const int ip_store_size = 12; //IP数据占用长度

        private readonly int ip_start_position; //IP起始位置

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSearcher"/> class.
        /// </summary>
        /// <param name="dataSource">数据源</param>
        public DefaultSearcher(IDataSource dataSource)
        {
            DataSource = dataSource;

            ip_start_position = dataSource.ReadInt32(0);

            Count = (int)((dataSource.Length - ip_start_position) / ip_store_size);
        }

        /// <summary>
        /// <see cref="IDataSource"/>
        /// </summary>
        public IDataSource DataSource { get; }

        /// <summary>
        /// IP总数
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// IP查询
        /// </summary>
        /// <param name="address">IP地址</param>
        /// <returns>IP位置信息</returns>
        public IpLocation Search(uint address)
        {
            if (IpLocationHelper.IsReserved(address))
            {
                return new IpLocation
                {
                    City = "内网IP",
                    Isp = "保留地址"
                };
            }

            //IP索引分区前缀
            var prefix = (byte)(address >> 24);

            FindPartition(prefix, out int start, out int end);

            //如果不在分区索引上则从整个IP数据储存进行搜索
            if (start < 0 || end < 0)
            {
                start = ip_start_position;
                end = (int)DataSource.Length - 1;
            }

            string data;

            if (DataSource is MemoryDataSource)
            {
                data = FindDataByMemory(start, end, address);
            }
            else
            {
                data = FindData(start, end, address);
            }

            if (!string.IsNullOrWhiteSpace(data))
            {
                var seg = data.Split('|');

                return new IpLocation
                {
                    Country = seg[0],
                    Province = seg[1],
                    City = seg[2],
                    Isp = seg[3]
                };
            }

            return null;
        }

        /// <summary>
        /// 查询IP分区范围
        /// </summary>
        /// <param name="prefix">IP前缀0~255</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="endIndex">结束位置</param>
        private void FindPartition(byte prefix, out int startIndex, out int endIndex)
        {
            startIndex = endIndex = -1;

            int pos = head_size + (prefix * 9);

            if (DataSource is MemoryDataSource)
            {
                if (prefix != DataSource.ReadByte(pos))
                {
                    return;
                }

                startIndex = DataSource.ReadInt32(pos + 1);
                endIndex = DataSource.ReadInt32(pos + 5);
            }
            else
            {
                var data = new byte[9];

                DataSource.ReadData(pos, ref data);

                if (prefix != data[0])
                {
                    return;
                }

                startIndex = BitConverter.ToInt32(data, 1);
                endIndex = BitConverter.ToInt32(data, 5);
            }
        }

        private string FindDataByMemory(int startIndex, int endIndex, uint number)
        {
            int position = 0;
            int left = 0;
            int right = (endIndex - startIndex) / ip_store_size;//计算高位索引

            while (left <= right)
            {
                int middle = (left + right) / 2;

                position = startIndex + middle * ip_store_size;

                var minIP = DataSource.ReadUInt32(position);
                var maxIP = DataSource.ReadUInt32(position + 4);

                if (minIP >= number && maxIP <= number)
                {
                    return GetData(DataSource.ReadInt32(position + 8));
                }

                if (number < minIP)
                {
                    right = middle - 1;
                }
                else
                {
                    if (number > maxIP)
                    {
                        left = middle + 1;
                    }
                    else
                    {
                        return GetData(DataSource.ReadInt32(position + 8));
                    }
                }
            }
            return null;
        }

        private string FindData(int startIndex, int endIndex, uint number)
        {
            int position = 0;
            int left = 0;
            int right = (endIndex - startIndex) / ip_store_size;//计算高位索引

            var data = new byte[ip_store_size];

            while (left <= right)
            {
                int middle = (left + right) / 2;

                position = startIndex + middle * ip_store_size;

                DataSource.ReadData(position, ref data);

                var minIP = BitConverter.ToUInt32(data, 0);
                var maxIP = BitConverter.ToUInt32(data, 4);

                if (minIP >= number && maxIP <= number)
                {
                    return GetData(BitConverter.ToInt32(data, 8));
                }

                if (number < minIP)
                {
                    right = middle - 1;
                }
                else
                {
                    if (number > maxIP)
                    {
                        left = middle + 1;
                    }
                    else
                    {
                        return GetData(BitConverter.ToInt32(data, 8));
                    }
                }
            }
            return null;
        }

        private string GetData(int offset)
        {
            var len = DataSource.ReadByte(offset);
            var data = DataSource.ReadString(offset + 1, len);
            return data;
        }
    }
}