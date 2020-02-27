using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Services.Cache.Redis
{
    public interface IRedisService
    {
        List<string> GetAll(string cachekey);
        string GetOneWithKey(string cachekey);
        void SetKey(string key, string value, TimeSpan timeSpan);
    }
}
