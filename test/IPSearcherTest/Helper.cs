using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IPSearcher;

namespace IPSearcherTest
{
    class Helper
    {
        public static IList<IpEntity> GetIPList(string filename)
        {
            var list = new List<IpEntity>();

            foreach (var line in File.ReadLines(filename))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var seg = line.Split('|');

                var start = seg[0].Split('.');
                var end = seg[1].Split('.');

                var startB = start.Select(s => byte.Parse(s)).ToArray();
                var endB = end.Select(s => byte.Parse(s)).ToArray();

                IpEntity info = new IpEntity();

                info.Prefix = startB[0];
                info.StartIP = seg[0];
                info.EndIP = seg[1];
                info.Country = seg[2] == "0" ? string.Empty : seg[2];
                info.Province = seg[3] == "0" ? string.Empty : seg[3];
                info.City = seg[4] == "0" ? string.Empty : seg[4];
                info.Isp = seg[5] == "0" ? string.Empty : seg[5];

                info.MinIP = IpLocationHelper.IPv4ToInteger(startB);
                info.MaxIP = IpLocationHelper.IPv4ToInteger(endB);

                list.Add(info);


            }

            return list;
        }
    }
}
