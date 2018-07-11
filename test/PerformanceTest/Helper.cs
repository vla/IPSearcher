using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using IPSearcher;

namespace PerformanceTest
{
    class Helper
    {
        public static void Time(string name, Action action, int iteration = 1)
        {
            TimeExecute(name, (act) =>
            {
                for (int i = 0; i < iteration; i++) act();

            }, action, iteration);
        }


        public static void TimeWithThread(string name, Action action, int task = 1, int iteration = 1)
        {
            TimeExecute(name, (act) =>
            {
                var tasks = new Task[task];
                var taskCount = iteration / tasks.Length;

                for (int i = 0; i < tasks.Length; i++)
                {
                    tasks[i] = Task.Run(() =>
                    {
                        for (int x = 0; x < taskCount; x++)
                        {
                            act();
                        }
                    });
                };

                Task.WaitAll(tasks);

            }, action, iteration);
        }

        public static void TimeWithParallel(string name, Action action, int iteration = 1)
        {
            TimeExecute(name, (act) =>
            {
                Parallel.For(0, iteration, l =>
                {
                    act();
                });
            }, action, iteration);
        }

        private static void TimeExecute(string name, Action<Action> inner, Action action, int iteration = 1)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            int[] gcCounts = new int[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            Stopwatch watch = new Stopwatch();

            ulong cycleCount = GetCycleCount();
            watch.Start();

            inner(action);

            watch.Stop();
            ulong cpuCycles = GetCycleCount() - cycleCount;

            Console.ForegroundColor = currentForeColor;
            Console.WriteLine("\tIterations:\t" + iteration);
            Console.WriteLine("\tTime Elapsed:\t" + watch.Elapsed.TotalMilliseconds + "ms");
            Console.WriteLine("\tPer Second:\t" + (iteration / watch.Elapsed.TotalSeconds).ToString("N0"));
            Console.WriteLine("\tCPU Cycles:\t" + cpuCycles.ToString("N0"));
            Console.WriteLine("\tMemory:\t\t" + FormatBytesToString(Process.GetCurrentProcess().WorkingSet64));

            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                int count = GC.CollectionCount(i) - gcCounts[i];
                Console.WriteLine("\tGen " + i + ": \t\t" + count);
            }

            Console.WriteLine();
        }

        private static ulong GetCycleCount()
        {
            ulong cycleCount = 0;
            QueryThreadCycleTime(GetCurrentThread(), ref cycleCount);
            return cycleCount;
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentThread();

        public static void PrintMem()
        {
            Console.WriteLine("Memory:" + Helper.FormatBytesToString(Process.GetCurrentProcess().WorkingSet64));
        }

        public static string FormatBytesToString(double bytes)
        {
            const ulong
              KB = 1UL << 10,
              MB = 1UL << 20,
              GB = 1UL << 30,
              TB = 1UL << 40,
              PB = 1UL << 50,
              EB = 1UL << 60;

            if (bytes > EB)
                return string.Format("{0}EB", (bytes / EB).ToString("F2"));
            if (bytes > PB)
                return string.Format("{0}PB", (bytes / PB).ToString("F2"));
            if (bytes > TB)
                return string.Format("{0}TB", (bytes / TB).ToString("F2"));
            if (bytes > GB)
                return string.Format("{0}GB", (bytes / GB).ToString("F2"));
            if (bytes > MB)
                return string.Format("{0}MB", (bytes / MB).ToString("F2"));
            if (bytes > KB)
                return string.Format("{0}KB", (bytes / KB).ToString("F2"));
            return bytes + "Byte";
        }

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
