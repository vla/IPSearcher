using System;
using System.Linq;
using System.Net;

namespace IPSearcher
{
    /// <summary>
    /// IpLocationHelper
    /// </summary>
    public static class IpLocationHelper
    {
        //0            -   16777215     0.0.0.0        -    0.255.255.255
        //167772160    -   184549375    10.0.0.0       -    10.255.255.255
        //1681915904   -   1686110207   100.64.0.0     -    100.127.255.255
        //2130706432   -   2147483647   127.0.0.0      -    127.255.255.255
        //2851995648   -   2852061183   169.254.0.0    -    169.254.255.255
        //2886729728   -   2887778303   172.16.0.0     -    172.31.255.255
        //3221225472   -   3221225727   192.0.0.0      -    192.0.0.255
        //3221225984   -   3221226239   192.0.2.0      -    192.0.2.255
        //3227017984   -   3227018239   192.88.99.0    -    192.88.99.255
        //3232235520   -   3232301055   192.168.0.0    -    192.168.255.255
        //3323068416   -   3323199487   198.18.0.0     -    198.19.255.255
        //3325256704   -   3325256959   198.51.100.0   -    198.51.100.255
        //3405803776   -   3405804031   203.0.113.0    -    203.0.113.255
        //3758096384   -   4294967295   224.0.0.0      -    255.255.255.255
        private static readonly uint[,] ReserveIP = new uint[,]
        {
                { 0u, 16777215u },
                { 167772160u, 184549375u },
                { 1681915904u, 1686110207u },
                { 2130706432u, 2147483647u },
                { 2851995648u, 2852061183u },
                { 2886729728u, 2887778303u },
                { 3221225472u, 3221225727u },
                { 3221225984u, 3221226239u },
                { 3227017984u, 3227018239u },
                { 3232235520u, 3232301055u },
                { 3323068416u, 3323199487u },
                { 3325256704u, 3325256959u },
                { 3405803776u, 3405804031u },
                { 3758096384u, 4294967295u }
        };

        /// <summary>
        /// IP是否保留地址
        /// </summary>
        /// <param name="address">ipv4地址</param>
        /// <returns>
        /// </returns>
        public static bool IsReserved(uint address)
        {
            for (int i = 0; i < ReserveIP.GetLength(0); i++)
            {
                if (address >= ReserveIP[i, 0] && address <= ReserveIP[i, 1])
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// IP位置查询
        /// </summary>
        /// <param name="searcher"><see cref="IIpSearcher"/></param>
        /// <param name="address">ipv4地址</param>
        /// <returns>
        /// IP位置信息
        /// </returns>
        public static IpLocation Search(this IIpSearcher searcher, string address)
        {
            if (IPAddress.TryParse(address, out IPAddress ip))
            {
                var octets = ip.GetAddressBytes();

                if (octets.Length == 4)
                {
                    return searcher.Search(IPv4ToInteger(ip.GetAddressBytes()));
                }
            }

            return null;
        }

        /// <summary>
        /// 转换为无符号整型字节
        /// </summary>
        /// <param name="address">无符号整型</param>
        /// <returns>返回无符号整型字节</returns>
        public static byte[] IPv4ToBytes(uint address)
        {
            var bytes = new byte[4];
            bytes[0] = (byte)(address >> 24);
            bytes[1] = (byte)(address >> 16);
            bytes[2] = (byte)(address >> 8);
            bytes[3] = (byte)address;
            return bytes;
        }

        /// <summary>
        /// 转换为整数
        /// </summary>
        /// <param name="address">IP地址（127.0.0.1）</param>
        /// <returns></returns>
        public static uint IPv4ToInteger(string address)
        {
            return IPv4ToInteger(IPv4ToBytes(address));
        }

        /// <summary>
        /// 转换为无符号整型字节
        /// </summary>
        /// <param name="address">IP地址（127.0.0.1）</param>
        /// <returns></returns>
        public static byte[] IPv4ToBytes(string address)
        {
            return address.Split('.').Select(s => byte.Parse(s)).ToArray();
        }

        /// <summary>
        /// 转换为无符号整数
        /// </summary>
        /// <param name="address">IP字节</param>
        /// <returns>返回无符号整数</returns>
        public static uint IPv4ToInteger(byte[] address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            if (address.Length != 4)
            {
                throw new ArgumentException("IPv4 字节长度必须是4个字节", nameof(address));
            }

            return (uint)((address[0] << 24 | address[1] << 16 | address[2] << 8 | address[3]) & 0x0FFFFFFFF);
        }
    }
}