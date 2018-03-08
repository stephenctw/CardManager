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
        [Route("api/card/{id}/pop")]
        public int Pop(string id)
        {
            int res = -1;
            try
            {
                Card card = CardRepository.GetCard(id);
                if (card != null && card.idx >= 0 && card.idx < Card.NumberOfCardsInDeck)
                {
                    res = card.deck[card.idx];
                    card.idx++;
                    CardRepository.SaveCardAndUpdateLastUse(id, card);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("error at Poping card");
                Console.WriteLine($"error message {e.Message}");
            }
            return res;
        }

        [HttpGet]
        [Route("api/card/{id}/shuffle")]
        public Card Shuffle(string id)
        {
            Card card = null;
            try
            {
                card = CardRepository.GetCardAndUpdateLastUse(id);
                if (card != null)
                {
                    card.Shuffle();
                    CardRepository.SaveCardAndUpdateLastUse(id, card);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error at Poping card");
                Console.WriteLine($"error message {e.Message}");
            }
            return card;
        }
        [HttpGet]
        [Route("api/card/{id}/cut")]
        public Card Cut(string id)
        {
            Card card = null;
            try
            {
                card = CardRepository.GetCardAndUpdateLastUse(id);
                if (card != null)
                {
                    card.Cut();
                    CardRepository.SaveCardAndUpdateLastUse(id, card);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error at Poping card");
                Console.WriteLine($"error message {e.Message}");
            }
            return card;
        }
        [HttpGet]
        [Route("api/card/{id}/cut")]
        public Card Cut(string id, int offset)
        {
            Card card = null;
            try
            {
                card = CardRepository.GetCardAndUpdateLastUse(id);
                if (card != null)
                {
                    card.Cut(offset);
                    CardRepository.SaveCardAndUpdateLastUse(id, card);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error at Poping card");
                Console.WriteLine($"error message {e.Message}");
            }
            return card;
        }
        [HttpGet]
        [Route("api/card/{id}/create")]
        public Card Create(string id)
        {
            Card card = new Card(id);
            try
            {
                if (card != null)
                {
                    CardRepository.SaveCardAndUpdateLastUse(id, card);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error at Poping card");
                Console.WriteLine($"error message {e.Message}");
            }
            return card;
        }
        [HttpGet]
        [Route("api/card/create")]
        public Card Create()
        {
            Card card = null;
            try
            {
                card = CardRepository.GetCardAndUpdateLastUse();
            }
            catch (Exception e)
            {
                Console.WriteLine("error at Poping card");
                Console.WriteLine($"error message {e.Message}");
            }
            return card;
        }
        [HttpGet]
        [Route("api/card/{id}")]
        [Route("api/card/{id}/returndeck")]
        public Card ReturnDeck(string id)
        {
            Card card = null;
            try
            {
                card = CardRepository.GetCardAndUpdateLastUse(id);
            }
            catch (Exception e)
            {
                Console.WriteLine("error at Poping card");
                Console.WriteLine($"error message {e.Message}");
            }
            return card;
        }
    }
}
