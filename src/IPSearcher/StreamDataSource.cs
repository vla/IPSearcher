using System;
using System.IO;
using System.Text;

namespace IPSearcher
{
    /// <summary>
    /// Data source based on IO stream
    /// </summary>
    /// <seealso cref="IDataSource" />
    public class StreamDataSource : IDataSource
    {
        private readonly Stream DataSource;
        private readonly BinaryReader br;
        private readonly object sync = new object();

        public StreamDataSource(string filename)
            : this(new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
        }

        public StreamDataSource(Stream stream)
        {
            DataSource = stream;
            br = new BinaryReader(stream);
            Length = stream.Length;
        }

        public long Length { get; }

        public byte ReadByte(int position)
        {
            if (position < 0 || position > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            lock (sync)
            {
                DataSource.Seek(position, SeekOrigin.Begin);
                return br.ReadByte();
            }
        }

        public int ReadData(int position, ref byte[] data)
        {
            if (position < 0 || position > DataSource.Length || data.Length > DataSource.Length - position)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            lock (sync)
            {
                DataSource.Seek(position, SeekOrigin.Begin);
                return DataSource.Read(data, 0, data.Length);
            }
        }

        public string ReadString(int position, int count)
        {
            if (position < 0 || position > DataSource.Length || count > DataSource.Length - position)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            var data = new byte[count];
            ReadData(position, ref data);
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        public ushort ReadUInt16(int position)
        {
            if (position < 0 || position > DataSource.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            lock (sync)
            {
                DataSource.Seek(position, SeekOrigin.Begin);
                return br.ReadUInt16();
            }
        }

        public short ReadInt16(int position)
        {
            if (position < 0 || position > DataSource.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            lock (sync)
            {
                DataSource.Seek(position, SeekOrigin.Begin);
                return br.ReadInt16();
            }
        }

        public uint ReadUInt32(int position)
        {
            if (position < 0 || position > DataSource.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            lock (sync)
            {
                DataSource.Seek(position, SeekOrigin.Begin);
                return br.ReadUInt32();
            }
        }

        public int ReadInt32(int position)
        {
            if (position < 0 || position > DataSource.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            lock (sync)
            {
                DataSource.Seek(position, SeekOrigin.Begin);
                return br.ReadInt32();
            }
        }

        public int ReadInt32In3Bit(int position)
        {
            if (position < 0 || position > DataSource.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            var data = new byte[3];

            ReadData(position, ref data);

            return data[0] | data[1] << 8 | data[2] << 16;
        }

        public ulong ReadUInt64(int position)
        {
            if (position < 0 || position > DataSource.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            lock (sync)
            {
                DataSource.Seek(position, SeekOrigin.Begin);
                return br.ReadUInt64();
            }
        }

        public long ReadInt64(int position)
        {
            if (position < 0 || position > DataSource.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            lock (sync)
            {
                DataSource.Seek(position, SeekOrigin.Begin);
                return br.ReadInt64();
            }
        }

        public void Dispose()
        {
            br?.Dispose();
            DataSource?.Dispose();
        }
    }
}