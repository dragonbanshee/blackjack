using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Move
    {
        public static void Hit(Player player, Deck deck, List<string> newHand)
        {
            Card randomCard = deck.RandomCard();
            player.hand.Add(randomCard);
            newHand.Add(randomCard.Number.ToString());
        }
    }
}
