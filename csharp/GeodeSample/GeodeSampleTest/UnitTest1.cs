using Apache.Geode.Client;
using GeodeSample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GeodeSampleTest
{
    [TestClass]
    public class UnitTest1
    {
        private static Cache cache;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            CacheFactory cacheFactory = CacheFactory.CreateCacheFactory().SetSubscriptionEnabled(true);
            cache = cacheFactory.Create();
        }

        [TestMethod]
        public void TestMethod1()
        {
            RequestReply requestReply = new RequestReply(cache);
            Response res = requestReply.Put(777, "Hello World");
            Console.Write("Response=" + res);
            Assert.AreEqual("777, Hello World", res);
        }
    }
}
