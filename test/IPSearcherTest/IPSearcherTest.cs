using System;
using System.IO;
using System.Linq;
using IPSearcher;
using Xunit;

namespace IPSearcherTest
{
    public class IPSearcherTest
    {
        [Fact]
        public void Writer_And_Reader()
        {
            var filename = Path.Combine(AppContext.BaseDirectory, "ip.txt");
            var dest = Path.Combine(AppContext.BaseDirectory, "ip.dat");

            using (var fs = File.Create(dest))
            {
                IpSearcherFactory.Generate(Helper.GetIPList(filename).ToArray(), fs);
            }

            var dataSource = new StreamDataSource(dest);

            var searcher = IpSearcherFactory.GetSearcher(dataSource);

            var result = searcher.Search(IpLocationHelper.IPv4ToInteger("14.123.238.167"));

            Assert.NotNull(result);
            Assert.Equal("广东省", result.Province);

            dataSource.Dispose();
        }

        [Fact]
        public void Check_ALL_Match()
        {
            var filename = Path.Combine(AppContext.BaseDirectory, "ip.txt");
            var dest = Path.Combine(AppContext.BaseDirectory, "ip2.dat");
            var data = Helper.GetIPList(filename).ToArray();

            using (var fs = File.Create(dest))
            {
                IpSearcherFactory.Generate(Helper.GetIPList(filename).ToArray(), fs);
            }

            var dataSource = new MemoryDataSource(dest);

            var searcher = IpSearcherFactory.GetSearcher(dataSource);

            IpLocation result;

            foreach (var info in data)
            {
                result = searcher.Search(info.MinIP);

                if(info.Country != result.Country)
                {
                    var a = 123;
                }

                Assert.Equal(info.Country, result.Country ?? string.Empty);
                //Assert.Equal(info.Province, result.Province ?? string.Empty);
                //Assert.Equal(info.City, result.City ?? string.Empty);

                //if (info.Isp == "内网IP")
                //{
                //    Assert.Equal("保留地址", result.Isp ?? string.Empty);
                //}
                //else
                //{
                //    Assert.Equal(info.Isp, result.Isp ?? string.Empty);
                //}

            }

            dataSource.Dispose();
        }
    }
}
