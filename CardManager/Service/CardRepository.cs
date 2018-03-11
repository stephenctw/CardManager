using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CardManager.Models;
using System.Configuration;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Text;

//reference here https://docs.microsoft.com/en-us/azure/redis-cache/cache-web-app-howto

namespace CardManager.Service
{
    public static class CardRepository
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly int ExpirationTime = Convert.ToInt32(ConfigurationManager.AppSettings["ExpirationTime"]);
        private static readonly string CacheConnection = ConfigurationManager.AppSettings["CacheConnection"].ToString();

        // Redis Connection string info
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = CacheConnection;
            return ConnectionMultiplexer.Connect(cacheConnection);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
        public static Card GetCard(string id)//get card from cache by id
        {
            Card res = null;
            if (Connection.IsConnected)
            {
                IDatabase cache = Connection.GetDatabase();
                if (cache.KeyExists(id.ToString()))
                {
                    var data = cache.StringGet(id.ToString());
                    res = JsonConvert.DeserializeObject<Card>(data);
                }
            }
            else
            {
                logger.Error("Redis not connected.");
                logger.Debug($"Connectionstring is {ConfigurationManager.AppSettings["CacheConnection"]}");
            }
            return res;
        }
        public static Card GetCardAndUpdateLastUse()//generate new card and id
        {
            Card card = null;
            string id = Guid.NewGuid().ToString();

            while (GetCard(id) != null)
            {
                id = Guid.NewGuid().ToString();
            }
            card = new Card(id);
            SaveCardAndUpdateLastUse(id, card);

            return card;
        }
        public static Card GetCardAndUpdateLastUse(string id)//get card from cache by id and update last use
        {
            Card res = GetCard(id);
            if (res != null)
            {
                SaveCardAndUpdateLastUse(id, res);
            }

            return res;
        }
        public static void SaveCardAndUpdateLastUse(string id, Card card)//save card to cache with id and update access time
        {
            if (Connection.IsConnected)
            {
                IDatabase cache = Connection.GetDatabase();
                string data = JsonConvert.SerializeObject(card);

                cache.StringSet(id, data, TimeSpan.FromMinutes(ExpirationTime));
            }
            else
            {
                logger.Error("Redis not connected.");
                logger.Debug($"Connectionstring is {ConfigurationManager.AppSettings["CacheConnection"]}");
            }
        }
    }
}