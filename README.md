# IP Searcher
基于文件存储的IP范围搜索

高性IP归属地查询库，文件压缩后只有4MB不到。

提供内存及IO加载方式查询，建议装载到内存上查询性能更高。

Packages & Status
---

Package  | NuGet         |
-------- | :------------ |
|**IPSearcher**|[![NuGet package](https://buildstats.info/nuget/IPSearcher)](https://www.nuget.org/packages/IPSearcher)
|**IPSearcher.Data**|[![NuGet package](https://buildstats.info/nuget/IPSearcher.Data)](https://www.nuget.org/packages/IPSearcher.Data)

Usage
---

```cs
var info = IpLocationSearch.Find("127.0.0.1");
```