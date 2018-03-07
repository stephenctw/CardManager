using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CardManager.Models;

namespace CardManager.Service
{
    public class CardRepository
    {
        private const int DeckExpirationTime = 30;
        private const int MaxNumberOfDeck = 100;
        private const string CacheKey = "CardStore";
        public static Card GetCard(int id)//get card from cache by id
        {
            Card res = null;
            var ctx = HttpContext.Current;
            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    res = null;
                }
                else
                {
                    Dictionary<int, Card> cards = (Dictionary<int, Card>)ctx.Cache[CacheKey];
                    if (!cards.ContainsKey(id) || cards[id] == null || (DateTime.Now - cards[id].LastUseDateTime).TotalMinutes >= DeckExpirationTime)
                    {
                        res = null;
                    }
                    else
                    {
                        res = cards[id];
                    }
                }
            }
            return res;
        }
        public static Card GetCardAndUpdateLastUse()//generate new card and id
        {
            Card card = null;
            int target = 0;
            for (target = 0; target < MaxNumberOfDeck; target++)
            {
                if (GetCard(target) == null)
                {
                    break;
                }
            }
            if (target != MaxNumberOfDeck)
            {
                card = new Card(target);
                SaveCardAndUpdateLastUse(target, card);
            }
            return card;
        }
        public static Card GetCardAndUpdateLastUse(int id)//get card from cache by id and update last use
        {
            Card res = GetCard(id);
            if (res != null)
            {
                SaveCardAndUpdateLastUse(id, res);
            }
            return res;
        }
        public static void SaveCardAndUpdateLastUse(int id, Card card)//save card to cache with id and update access time
        {
            var ctx = HttpContext.Current;
            if (ctx != null)
            {
                card.LastUseDateTime = DateTime.Now;
                if (ctx.Cache[CacheKey] == null)
                {
                    Dictionary<int, Card> cards = new Dictionary<int, Card>
                    {
                        [id] = card
                    };
                    ctx.Cache[CacheKey] = cards;
                }
                else
                {
                    Dictionary<int, Card> cards = (Dictionary<int, Card>)ctx.Cache[CacheKey];
                    cards[id] = card;
                }
            }
        }
    }
}