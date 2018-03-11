using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardManager;
using CardManager.Controllers;
using CardManager.Models;
using System.Configuration;

namespace CardManager.Tests.Controllers
{
    [TestClass]
    public class CardControllerTest
    {
        private static readonly string validId = "validId";
        private static readonly string invalidId = "invalidId";
        private static readonly string nullId = null;
        private static readonly int validOffset = 20;
        private static readonly int invalidOffset = 52;
        public void InitAppSettings()
        {
            ConfigurationManager.AppSettings["ExpirationTime"] = 30.ToString();
            ConfigurationManager.AppSettings["CacheConnection"] = "StephenRedis.redis.cache.windows.net:6380,password=ktpj/aHoFFRBVrQeKnfya0UGbuCMpXbiwwiemdYGfN0=,ssl=True,abortConnect=False";
        }
        [TestMethod]
        public void CreateNewCardTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create();
            Assert.IsNotNull(card);
        }
        [TestMethod]
        public void CreateNewCardWithValidIdTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);
            Assert.IsNotNull(card);
        }
        [TestMethod]
        public void CreateNewCardWithNullIdTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(nullId);
            Assert.IsNotNull(card);
        }
        [TestMethod]
        public void ReturnDeckWithValidIdTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.ReturnDeck(validId);
            Assert.IsNotNull(ret);
        }
        [TestMethod]
        public void ReturnDeckWithInValidIdTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.ReturnDeck(invalidId);
            Assert.IsNull(ret);
        }
        [TestMethod]
        public void ReturnDeckWithNullIdTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.ReturnDeck(nullId);
            Assert.IsNull(ret);
        }
        [TestMethod]
        public void CutDeckWithValidIdTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.Cut(validId);
            Assert.IsNotNull(ret);
        }
        [TestMethod]
        public void CutDeckWithInValidIdTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.Cut(invalidId);
            Assert.IsNull(ret);
        }
        [TestMethod]
        public void CutDeckWithNullIdTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.Cut(nullId);
            Assert.IsNull(ret);
        }
        [TestMethod]
        public void CutDeckWithValidIdValidOffsetTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.Cut(validId, validOffset);
            Assert.IsNotNull(ret);
        }
        [TestMethod]
        public void CutDeckWithInValidIdValidOffsetTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.Cut(invalidId, validOffset);
            Assert.IsNull(ret);
        }
        [TestMethod]
        public void CutDeckWithNullIdValidOffsetTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.Cut(nullId, validOffset);
            Assert.IsNull(ret);
        }
        [TestMethod]
        public void CutDeckWithValidIdInValidOffsetTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.Cut(validId, invalidOffset);
            Assert.IsNull(ret);
        }
        [TestMethod]
        public void CutDeckWithInValidIdInValidOffsetTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.Cut(invalidId, invalidOffset);
            Assert.IsNull(ret);
        }
        [TestMethod]
        public void CutDeckWithNullIdInValidOffsetTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.Cut(nullId, invalidOffset);
            Assert.IsNull(ret);
        }
        [TestMethod]
        public void ShuffleDeckWithValidIdTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.Shuffle(validId);
            Assert.IsNotNull(ret);
        }
        [TestMethod]
        public void ShuffleDeckWithInValidIdTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.Shuffle(invalidId);
            Assert.IsNull(ret);
        }
        [TestMethod]
        public void ShuffleDeckWithNullIdTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.Shuffle(nullId);
            Assert.IsNull(ret);
        }
        [TestMethod]
        public void PopDeckWithValidIdTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.Pop(validId);
            Assert.IsTrue(ret>=0 && ret < Card.NumberOfCardsInDeck);
        }
        [TestMethod]
        public void PopDeckWithInValidIdTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.Pop(invalidId);
            Assert.IsTrue(ret == -1);
        }
        [TestMethod]
        public void PopDeckWithNullIdTest()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = card_controller.Pop(nullId);
            Assert.IsTrue(ret == -1);
        }
        [TestMethod]
        public void PopDeckWithValidIdTest2()
        {
            InitAppSettings();
            var card_controller = new CardController();
            var card = card_controller.Create(validId);

            var ret = -1;
            for (int i = 0; i <= Card.NumberOfCardsInDeck; i++)
            {
                ret = card_controller.Pop(validId);
            }
            Assert.IsTrue(ret == -1);
        }
    }
}
