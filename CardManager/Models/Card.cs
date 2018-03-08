using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardManager.Models
{
    public class Card
    {
        public const int NumberOfCardsInDeck = 52;
        private static readonly int[] InitialDeck = new int [NumberOfCardsInDeck] {0,1,2,3,4,5,6,7,8,9,10,
                                                    11,12,13,14,15,16,17,18,19,20,
                                                    21,22,23,24,25,26,27,28,29,30,
                                                    31,32,33,34,35,36,37,38,39,40,
                                                    41,42,43,44,45,46,47,48,49,50,
                                                    51};
        private static Random rand = new Random();
        public string userId;
        public int idx;
        public int[] deck;
        public Card(string id)
        {
            userId = id;
            idx = 0;
            deck = new int [NumberOfCardsInDeck];
            Array.Copy(InitialDeck, deck, NumberOfCardsInDeck);
        }
        public void Shuffle()
        {
            idx = 0;
            for (int i = 0; i < NumberOfCardsInDeck; i++)
            {
                int target = rand.Next(NumberOfCardsInDeck);
                int temp = deck[i];
                deck[i] = deck[target];
                deck[target] = temp;
            }
        }

        public void Cut()
        {
            int offset = rand.Next(NumberOfCardsInDeck);

            Cut(offset);
        }
        public void Cut(int offset)
        {
            idx = 0;

            int[] cut_Deck = new int[NumberOfCardsInDeck];
            Array.Copy(deck, offset, cut_Deck, 0, NumberOfCardsInDeck - offset);
            Array.Copy(deck, 0, cut_Deck, NumberOfCardsInDeck - offset, offset);

            deck = cut_Deck;
        }
    }
}