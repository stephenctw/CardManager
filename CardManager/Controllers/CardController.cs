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
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public String Get()
        {
            return "Welcome using Card Manager";
        }
        /// <summary>
        /// Pop one card off a deck with user given id
        /// </summary>
        /// <param name="id">a valid deck id</param>
        /// <returns>card number between 0-51 if successful, -1 if fail</returns>
        [HttpGet]
        [Route("api/card/{id}/pop")]
        public int Pop(string id)
        {
            int res = -1;
            if (id != null)
            {
                try
                {
                    Card card = CardRepository.GetCard(id);
                    if (card != null && card.idx >= 0 && card.idx < Card.NumberOfCardsInDeck)
                    {
                        res = card.deck[card.idx];
                        card.idx++;
                        CardRepository.SaveCardAndUpdateLastUse(id, card);
                    }
                    else
                    {
                        if (card == null)
                        {
                            logger.Debug("Card is null");
                        }
                        else
                        {
                            logger.Debug($"Card index: {card.idx}");
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.Error("Error at Poping card");
                    logger.Debug($"Error message {e.Message}");
                }
            }
            else
            {
                logger.Error("Invalid parameters.");
                logger.Debug($"Id: {id}");
            }
            return res;
        }

        /// <summary>
        /// Shuffle a deck with user given id
        /// </summary>
        /// <param name="id">a valid deck id</param>
        /// <returns>Card object if successful, null if fail</returns>
        [HttpGet]
        [Route("api/card/{id}/shuffle")]
        public Card Shuffle(string id)
        {
            Card card = null;
            if (id != null)
            {
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
                    logger.Error("Error at Shuffling deck");
                    logger.Debug($"Error message {e.Message}");
                }
            }
            else
            {
                logger.Error("Invalid parameters.");
                logger.Debug($"Id: {id}");
            }
            return card;
        }
        /// <summary>
        /// Cut a deck with user given id
        /// </summary>
        /// <param name="id">a valid deck id</param>
        /// <returns>Card object if successful, null if fail</returns>
        [HttpGet]
        [Route("api/card/{id}/cut")]
        public Card Cut(string id)
        {
            Card card = null;
            if (id != null)
            {
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
                    logger.Error("Error at Cutting deck");
                    logger.Debug($"Error message {e.Message}");
                }
            }
            else
            {
                logger.Error("Invalid parameters.");
                logger.Debug($"Id: {id}");
            }
            return card;
        }
        /// <summary>
        /// Cut a deck with user given id and user given offset
        /// </summary>
        /// <param name="id">a valid deck id</param>
        /// <param name="id">a valid offset between 0-51</param>
        /// <returns>Card object if successful, null if fail</returns>
        [HttpGet]
        [Route("api/card/{id}/cut")]
        public Card Cut(string id, int offset)
        {
            Card card = null;
            if (id != null && offset >= 0 && offset < Card.NumberOfCardsInDeck)
            {
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
                    logger.Error("Error at Cutting deck");
                    logger.Debug($"Error message {e.Message}");
                }
            }
            else
            {
                logger.Error("Invalid parameters.");
                logger.Debug($"Id: {id}, Offset: {offset}");
            }
            return card;
        }
        /// <summary>
        /// Create a deck with user given id
        /// </summary>
        /// <param name="id">a valid deck id</param>
        /// <returns>Card object if successful, null if fail</returns>
        [HttpGet]
        [Route("api/card/{id}/create")]
        public Card Create(string id)
        {
            Card card = new Card(id);
            if (id != null)
            {
                try
                {
                    if (card != null)
                    {
                        CardRepository.SaveCardAndUpdateLastUse(id, card);
                    }
                }
                catch (Exception e)
                {
                    logger.Error("Error at Creating deck");
                    logger.Debug($"Error message {e.Message}");
                }
            }
            else
            {
                logger.Error("Invalid parameters.");
                logger.Debug($"Id: {id}");
            }
            return card;
        }
        /// <summary>
        /// Create a deck
        /// </summary>
        /// <returns>Card object if successful, null if fail</returns>
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
                logger.Error("Error at Creating deck");
                logger.Debug($"Error message {e.Message}");
            }
            return card;
        }
        /// <summary>
        /// Return a deck with user given id
        /// </summary>
        /// <param name="id">a valid deck id</param>
        /// <returns>Card object if successful, null if fail</returns>
        [HttpGet]
        [Route("api/card/{id}")]
        [Route("api/card/{id}/returndeck")]
        public Card ReturnDeck(string id)
        {
            Card card = null;
            if (id != null)
            {
                try
                {
                    card = CardRepository.GetCardAndUpdateLastUse(id);
                }
                catch (Exception e)
                {
                    logger.Error("Error at Returning deck");
                    logger.Debug($"Error message {e.Message}");
                }
            }
            else
            {
                logger.Error("Invalid parameters.");
                logger.Debug($"Id: {id}");
            }
            return card;
        }
    }
}
