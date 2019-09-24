using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Stopify.Web.Controllers
{
    public class TestCacheController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        public TestCacheController(IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;

        }

        public IActionResult Test()
        {


            //note: for distributedCaching (it is a separate dictionary-like object, located in the controller!
            //works with byte[] or strings!
            var stringValue = distributedCache.GetString("Now");
            if (stringValue == null)
            {
                stringValue = DateTime.UtcNow.ToString();
                distributedCache.SetString("Now", stringValue);
            }




            //note: cache -> really efficient with dateTime , because it changes  and  you can check the difference!
            if (!this.memoryCache.TryGetValue("sum", out string time))
            {
                time = DateTime.UtcNow.ToString();
                //NOT CREATE ENTRY, RATHER SET!
                this.memoryCache.Set<string>("sum", time, new MemoryCacheEntryOptions()
                {
                    //note: cache this is cool too! if yo don`t do any action within 4 seconds, it will expire and 
                    //it will reset itself!
                    SlidingExpiration = new TimeSpan(0, 0, 4)
                }
                );
            }

            //return this.Content($"this value {time}");

            return this.View();

        }


        //note: cache-related -> response location tells us where it should add the headers!
        //this means, that the location given from below should be stored in cache? 
        [ResponseCache(Duration = 120,Location =ResponseCacheLocation.Client)]
        public IActionResult ResponseCacheTest()
        {
            return Content(DateTime.UtcNow.ToString());
        }
    }
}
