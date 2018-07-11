using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IPSearcher;

namespace PerformanceTest
{
    public class IPSearcherPerformance
    {
        [Test("内置数据源性能测试")]
        public void InnerDataSourceSearch()
        {
            Helper.PrintMem();

            //预热
            IpLocationSearch.Find("14.123.238.167");

            uint ip = IpLocationHelper.IPv4ToInteger("116.19.114.15");

            Helper.Time("单条查询", () =>
            {
                IpLocationSearch.Find(ip);
            });

            int count = 20000000;

            Helper.Time("批量查询", () =>
            {
                IpLocationSearch.Find(ip);
            }, count);

            Helper.TimeWithParallel("并发查询", () =>
            {
                IpLocationSearch.Find(ip);
            }, count);

            Helper.TimeWithThread("多线程查询", () =>
            {
                IpLocationSearch.Find(ip);
            }, 4, count);

        }

        [Test("内存数据源性能测试")]
        public void MemoryDataSourceSearch()
        {
            Helper.PrintMem();

            var dest = Path.Combine(AppContext.BaseDirectory, "ip.dat");

            if (!File.Exists(dest))
            {
                var filename = Path.Combine(AppContext.BaseDirectory, "ip.txt");

                using (var fs = File.Create(dest))
                {
                    IpSearcherFactory.Generate(Helper.GetIPList(filename).ToArray(), fs);
                }
            }

            var dataSource = new MemoryDataSource(dest);

            var searcher = IpSearcherFactory.GetSearcher(dataSource);

            //预热
            searcher.Search(IpLocationHelper.IPv4ToInteger("14.123.238.167"));

            uint ip = IpLocationHelper.IPv4ToInteger("116.19.114.15");

            Helper.Time("单条查询", () =>
            {
                searcher.Search(ip);
            });

            int count = 20000000;

            Helper.Time("批量查询", () =>
            {
                searcher.Search(ip);
            }, count);

            Helper.TimeWithParallel("并发查询", () =>
            {
                searcher.Search(ip);
            }, count);

            Helper.TimeWithThread("多线程查询", () =>
            {
                searcher.Search(ip);
            }, 4, count);

        }


        [Test("文件IO数据源性能测试")]
        public void StreamDataSourceSearch()
        {
            Helper.PrintMem();

            var dest = Path.Combine(AppContext.BaseDirectory, "ip.dat");

            if (!File.Exists(dest))
            {
                var filename = Path.Combine(AppContext.BaseDirectory, "ip.txt");

                using (var fs = File.Create(dest))
                {
                    IpSearcherFactory.Generate(Helper.GetIPList(filename).ToArray(), fs);
                }
            }

            var dataSource = new StreamDataSource(dest);

            var searcher = IpSearcherFactory.GetSearcher(dataSource);

            //预热
            searcher.Search(IpLocationHelper.IPv4ToInteger("14.123.238.167"));

            uint ip = IpLocationHelper.IPv4ToInteger("116.19.114.15");

            Helper.Time("单条查询", () =>
            {
                searcher.Search(ip);
            });

            int count = 20000;

            Helper.Time("批量查询", () =>
            {
                searcher.Search(ip);
            }, count);

            Helper.TimeWithParallel("并发查询", () =>
            {
                searcher.Search(ip);
            }, count);

            Helper.TimeWithThread("多线程查询", () =>
            {
                searcher.Search(ip);
            }, 4, count);
        }
    }
}
