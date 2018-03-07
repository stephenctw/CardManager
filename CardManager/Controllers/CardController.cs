using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CardManager.Models;
using CardManager.Service;

namespace CardManager.Controllers
{
    public class CardController : ApiController
    {
        public String Get()
        {
            return "Welcome using Card Manager";
        }
        [HttpGet]
        public int Pop(int id)
        {
            Card card = CardRepository.GetCard(id);
            int res = -1;
            if (card != null && card.Idx >= 0 && card.Idx < 52)
            {
                res = card.Deck[card.Idx];
                card.Idx++;
                CardRepository.SaveCardAndUpdateLastUse(id, card);
            }
            return res;
        }

        [HttpGet]
        public Card Shuffle(int id)
        {
            Card card = CardRepository.GetCardAndUpdateLastUse(id);
            if (card != null)
            {
                card.Shuffle();
                CardRepository.SaveCardAndUpdateLastUse(id, card);
            }
            return card;
        }
        [HttpGet]
        public Card Cut(int id)
        {
            Card card = CardRepository.GetCardAndUpdateLastUse(id);
            if (card != null)
            {
                card.Cut();
                CardRepository.SaveCardAndUpdateLastUse(id, card);
            }
            return card;
        }
        [HttpGet]
        public Card Create()
        {
            return CardRepository.GetCardAndUpdateLastUse();
        }
        [HttpGet]
        public Card Create(int id)
        {
            Card card = new Card(id);
            if (card != null)
            {
                CardRepository.SaveCardAndUpdateLastUse(id, card);
            }
            return card;
        }
        [HttpGet]
        public Card ReturnDeck(int id)
        {
            return CardRepository.GetCardAndUpdateLastUse(id);
        }
    }
}
