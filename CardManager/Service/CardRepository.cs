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
        /// <summary>
        /// Try to get a deck from cache with user given id
        /// </summary>
        /// <param name="id">a valid deck id</param>
        /// <returns>Card object if successful, null if fail</returns>
        public static Card GetCard(string id)
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
        /// <summary>
        /// Try to generate a new deck and save it to cache, also refresh its expiration time
        /// </summary>
        /// <returns>Card object if successful, null if fail</returns>
        public static Card GetCardAndUpdateLastUse()
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
        /// <summary>
        /// Try to get a deck from cache with user given id, also refresh its expiration time
        /// </summary>
        /// <param name="id">a valid deck id</param>
        /// <returns>Card object if successful, null if fail</returns>
        public static Card GetCardAndUpdateLastUse(string id)
        {
            Card res = GetCard(id);
            if (res != null)
            {
                SaveCardAndUpdateLastUse(id, res);
            }

            return res;
        }
        /// <summary>
        /// Try to save a deck to cache with user given id, also refresh its expiration time
        /// </summary>
        /// <param name="id">a valid deck id</param>
        /// <returns></returns>
        public static void SaveCardAndUpdateLastUse(string id, Card card)
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