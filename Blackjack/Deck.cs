using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack
{
    public class Deck
    {
        public List<Card> cards = new List<Card>();

        public Deck()
        {
            foreach (Number currentNumber in Enum.GetValues(typeof(Number)))
            {
                foreach (Suit currentSuit in Enum.GetValues(typeof(Suit)))
                {
                    cards.Add(new Card(currentNumber, currentSuit));
                }
            }
        }

        public void Shuffle()
        {
            int y = 0;
            foreach (Card card in cards)
            {
                y++;
            }
            for (int i = 0; i < 100; i++)
            {
                Random rand = new Random();
                int x = rand.Next(0, y);
                Card card = cards[x];
                cards.Remove(card);
                cards.Add(card);
            }
        }

        public Card RandomCard()
        {
            Random rand = new Random();
            int x = rand.Next(cards.Count);
            return cards[x];
        }
    }
}
