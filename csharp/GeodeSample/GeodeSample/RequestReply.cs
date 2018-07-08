using Apache.Geode.Client;
using System;
using System.Collections.Concurrent;

namespace GeodeSample
{
    public class RequestReply
    {
        private Cache cache;
        private BlockingCollection<object> queue = new BlockingCollection<object>();

        static RequestReply()
        {
            Serializable.RegisterTypeGeneric(Response.CreateDeserializable);
        }

        public RequestReply(Cache cache)
        {
            this.cache = cache;
            if (this.cache.GetRegion<int, object>("request") == null)
            {
                RegionFactory regionFactory1 = cache.CreateRegionFactory(RegionShortcut.CACHING_PROXY);
                IRegion<int, object> regionRequest = regionFactory1.Create<int, object>("request");
            }

            if (this.cache.GetRegion<int, object>("reply") == null)
            {
                RegionFactory regionFactory2 = cache.CreateRegionFactory(RegionShortcut.CACHING_PROXY);
                IRegion<int, object> regionReply = regionFactory2.Create<int, object>("reply");

                regionReply.GetSubscriptionService().RegisterAllKeys();
                regionReply.AttributesMutator.SetCacheListener(new MyCacheListener(ref queue));
            }
        }

        public Response Put<T>(int key, T value)
        {
            cache.GetRegion<int, object>("request")[key] = value;
            object res = new Response();
            if (queue.TryTake(out res, 10000))
            {
                return (Response)res;
            }
            return null;
        }

        private class MyCacheListener : ICacheListener<int, object>
        {
            private BlockingCollection<object> queue;

            public MyCacheListener(ref BlockingCollection<object> queue)
            {
                this.queue = queue;
            }

            void ICacheListener<int, object>.AfterCreate(EntryEvent<int, object> ev)
            {
                this.queue.Add(ev.NewValue);
                Console.WriteLine("Create :" + ev.Key + "=" + ev.NewValue);
            }

            void ICacheListener<int, object>.AfterUpdate(EntryEvent<int, object> ev)
            {
                this.queue.Add(ev.NewValue);
                Console.WriteLine("Update :" + ev.Key + "=" + ev.NewValue);
            }

            void ICacheListener<int, object>.AfterDestroy(EntryEvent<int, object> ev)
            {
            }

            void ICacheListener<int, object>.AfterInvalidate(EntryEvent<int, object> ev)
            {
            }

            void ICacheListener<int, object>.AfterRegionClear(RegionEvent<int, object> ev)
            {
            }

            void ICacheListener<int, object>.AfterRegionDestroy(RegionEvent<int, object> ev)
            {
            }

            void ICacheListener<int, object>.AfterRegionDisconnected(IRegion<int, object> region)
            {
            }

            void ICacheListener<int, object>.AfterRegionInvalidate(RegionEvent<int, object> ev)
            {
            }

            void ICacheListener<int, object>.AfterRegionLive(RegionEvent<int, object> ev)
            {
            }

            void ICacheListener<int, object>.Close(IRegion<int, object> region)
            {
            }
        }
    }
}
