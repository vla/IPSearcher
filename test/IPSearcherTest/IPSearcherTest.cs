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
            Assert.Equal("�㶫ʡ", result.Province);

            dataSource.Dispose();
        }
    }
}
