using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class CIDRDivider
    {
        public static List<string> Divide(string raw)
        {
            IPNetwork wholeInternet = IPNetwork.Parse(raw);
            IPNetworkCollection ips = wholeInternet.Subnet(28);
            List<IPNetwork> iPNetworks = ips.ToList();
            return iPNetworks.ConvertAll(x => x.ToString());
        }
    }
}
