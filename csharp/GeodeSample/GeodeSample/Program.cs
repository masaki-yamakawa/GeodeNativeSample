using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.Geode.Client;

namespace GeodeSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CacheFactory cacheFactory = CacheFactory.CreateCacheFactory().SetSubscriptionEnabled(true);
                Cache cache = cacheFactory.Create();
                Console.WriteLine("Created the Geode Cache");

                RequestReply requestReply = new RequestReply(cache);
                Response res = requestReply.Put(777, "Hello World");
                Console.Write("Response=" + res);
            } catch (GeodeException gfex)
            {
                Console.WriteLine("BasicOperations Geode Exception: {0}", gfex.Message);
            }
        }
    }
}
