using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardManager.Models
{
    public class Card
    {
        private const int NumberOfCardsInDeck = 52;
        private static readonly int[] InitialDeck = new int [NumberOfCardsInDeck] {0,1,2,3,4,5,6,7,8,9,10,
                                                    11,12,13,14,15,16,17,18,19,20,
                                                    21,22,23,24,25,26,27,28,29,30,
                                                    31,32,33,34,35,36,37,38,39,40,
                                                    41,42,43,44,45,46,47,48,49,50,
                                                    51};
        private static Random rand = new Random();
        public int UserId;
        public int Idx;
        public DateTime LastUseDateTime;
        public int[] Deck;
        public Card(int id)
        {
            UserId = id;
            Idx = 0;
            Deck = new int [NumberOfCardsInDeck];
            Array.Copy(InitialDeck, Deck, NumberOfCardsInDeck);
        }
        public void Shuffle()
        {
            Idx = 0;
            for (int i = 0; i < NumberOfCardsInDeck; i++)
            {
                int target = rand.Next() % 52;
                int temp = Deck[i];
                Deck[i] = Deck[target];
                Deck[target] = temp;
            }
        }

        public void Cut()
        {
            Idx = 0;
            int offset = rand.Next() % 52;

            int [] cut_Deck = new int[NumberOfCardsInDeck];
            Array.Copy(Deck, offset, cut_Deck, 0, NumberOfCardsInDeck - offset);
            Array.Copy(Deck, 0, cut_Deck, NumberOfCardsInDeck - offset, offset);

            Deck = cut_Deck;
        }
    }
}