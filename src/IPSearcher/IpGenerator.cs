using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IPSearcher
{
    /**
    文件存储结构

    头部区
    +------------- ---+
    | 4bytes          |
    +------------ ----+
     IP存储区起始位置

    IP索引256个分区
    +------------+----------+----------+
    |   1bytes   |  4bytes  |  4bytes  |
    +------------+----------+----------+
        0~255       起始流      结束流
        0~255.x.x.x

    数据存储区
    +------------+-------------+
    | 1bytes     | len bytes   |
    +------------+-------------+
      长度          数据内容
                    Country|Province|City|Isp

    IP存储区
    +------------+----------+----------+
    | 4bytes     |  4bytes  |  4bytes  |
    +------------+----------+----------+
        起始IP      结束IP    数据流位置

*/

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
                PrepareHead(bw);
                PrepareIndex(bw);
                WriteIpData(bw, PrepareData(bw));
            }
        }

        private void PrepareHead(BinaryWriter bw)
        {
            bw.Seek(0, SeekOrigin.Begin);
            bw.Write(0);//IP存储区起始位置
        }

        private void PrepareIndex(BinaryWriter bw)
        {
            //分配索引分区
            var data = new byte[256 * 9];
            bw.Write(data);
        }

        private void WriteIpData(BinaryWriter bw, IDictionary<string, int> dictData)
        {
            var group = Data.OrderBy(o => o.Prefix).GroupBy(p => p.Prefix);

            int startIdx = (int)bw.BaseStream.Position;
            int endIdx = 0;

            //记录IP数据存储的起始位置
            Write(bw, 0, startIdx);

            foreach (var list in group)
            {
                bw.Seek(0, SeekOrigin.End);

                foreach (var info in list)
                {
                    dictData.TryGetValue(info.GetText(), out int offset);
                    bw.Write(info.MinIP);
                    bw.Write(info.MaxIP);
                    bw.Write(offset);
                }

                endIdx = (int)bw.BaseStream.Position;

                //分区记录
                var data = new byte[9];

                //分区索引0~255
                data[0] = list.Key;

                //分区文件流的起始位置
                data[1] = (byte)startIdx;
                data[2] = (byte)(startIdx >> 8);
                data[3] = (byte)(startIdx >> 16);
                data[4] = (byte)(startIdx >> 24);

                //分区文件流结束为止
                data[5] = (byte)endIdx;
                data[6] = (byte)(endIdx >> 8);
                data[7] = (byte)(endIdx >> 16);
                data[8] = (byte)(endIdx >> 24);

                WritePartition(bw, list.Key, data);
            }
        }

        private IDictionary<string, int> PrepareData(BinaryWriter bw)
        {
            var dict = new Dictionary<string, int>();

            var list = Data.Select(w => w.GetText()).Distinct();

            bw.Seek(0, SeekOrigin.End);

            foreach (var text in list)
            {
                var data = Encoding.UTF8.GetBytes(text);
                dict[text] = (int)bw.BaseStream.Position;
                bw.Write((byte)data.Length);
                bw.Write(data);
            }

            return dict;
        }

        private void WritePartition(BinaryWriter bw, int partition, byte[] data)
        {
            //填充每个IP索引区范围
            var offset = 4 + partition * data.Length;
            bw.Seek(offset, SeekOrigin.Begin);
            bw.Write(data);
        }

        private void Write(BinaryWriter bw, int offset, int val)
        {
            bw.Seek(offset, SeekOrigin.Begin);
            bw.Write(val);
        }
    }
}