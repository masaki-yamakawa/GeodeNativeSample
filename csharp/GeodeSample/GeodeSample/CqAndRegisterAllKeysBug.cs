using Apache.Geode.Client;
using System;

namespace GeodeSample
{
    public class MyCqListener<TKey, TResult> : ICqListener<TKey, TResult>
    {
        public virtual void OnEvent(CqEvent<TKey, TResult> ev)
        {
            string val = ev.getNewValue() as string;
            string key = ev.getKey() as string;
            Console.WriteLine("MyCqListener: OnEvent key={0}, value={1}, op={2}", key, val, ev.getQueryOperation().ToString());
        }
        public virtual void OnError(CqEvent<TKey, TResult> ev)
        {
            Console.WriteLine("MyCqListener: OnError");
        }
        public virtual void Close()
        {
            Console.WriteLine("MyCqListener: close");
        }
    }

    class CqAndRegisterAllKeysBug
    {
        static void Main(string[] args)
        {
            try
            {
                CacheFactory cacheFactory = CacheFactory.CreateCacheFactory().SetSubscriptionEnabled(true);
                Cache cache = cacheFactory.Create();
                Console.WriteLine("Created the Geode Cache");

                RegionFactory regionFactory = cache.CreateRegionFactory(RegionShortcut.CACHING_PROXY);
                IRegion<string, string> region = regionFactory.Create<string, string>("exampleRegion");
                region.GetSubscriptionService().RegisterAllKeys();
                Console.WriteLine("RegisterAllKeys to exampleRegion");

                QueryService<string, string> qrySvc = cache.GetQueryService<string, string>();
                CqAttributesFactory<string, string> cqFac = new CqAttributesFactory<string, string>();
                ICqListener<string, string> cqLstner = new MyCqListener<string, string>();
                cqFac.AddCqListener(cqLstner);
                CqAttributes<string, string> cqAttr = cqFac.Create();
                CqQuery<string, string> qry = qrySvc.NewCq("MyCq", "select * from /exampleRegion p where p.toString = '1'", cqAttr, false);
                ICqResults<string> results = qry.ExecuteWithInitialResults();
                Console.WriteLine("Execute CQ: Initial ResultSet returned {0} rows", results.Size);

                while (true)
                {
                    System.Threading.Thread.Sleep(5000);
                    string val1 = null;
                    string val2 = null;
                    region.TryGetValue("1", ref val1);
                    region.TryGetValue("2", ref val2);
                    Console.WriteLine("exampleRegion['1']: {0}", val1);
                    Console.WriteLine("exampleRegion['2']: {0}", val2);
                }
            }
            catch (GeodeException gfex)
            {
                Console.WriteLine("Geode Exception: {0}", gfex.Message);
            }
        }
    }
}

