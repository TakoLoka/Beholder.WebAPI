using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Services.Cache.Redis
{
    public class RedisManager : IRedisService
    {
        public List<string> GetAll(string searchKey)
        {
            searchKey = searchKey + "*";
            using (IRedisClient client = new RedisClient())
            {
                List<string> dataList = new List<string>();
                List<string> allKeys = client.SearchKeys(searchKey);
                foreach (string key in allKeys)
                {
                    dataList.Add(client.Get<string>(key));
                }
                return dataList;
            }
        }

        public string GetOneWithKey(string cachekey)
        {
            using (IRedisClient client = new RedisClient())
            {
                var redisdata = client.Get<string>(cachekey);

                return redisdata;
            }
        }

        public void SetKey(string key, string value, TimeSpan timeSpan)
        {
            using (IRedisClient client = new RedisClient())
            {
                client.SetValue(key, value, timeSpan);
            }
        }
    }
}
