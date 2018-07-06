using System;
using System.Collections.Generic;
using System.Text;

namespace IPSearcher
{
    internal class DefaultSearcher : IIpSearcher
    {
        public DefaultSearcher(IDataSource dataSource)
        {
            DataSource = dataSource;
        }

        public int Count => throw new NotImplementedException();

        public IDataSource DataSource { get; }

        public IpLocation Search(uint address)
        {
            //TODO Parse ipv4 goto search

            throw new NotImplementedException();
        }
    }
}
