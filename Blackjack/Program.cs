using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Blackjack
{
    class Program
    {
        public static Deck deck = new Deck();

        static void Main(string[] args)
        {
            Console.Title = "Blackjack by Kyle Smith";
            GameStart();
        }

        private static void GameStart()
        {
            deck.Shuffle();
            Console.CursorVisible = false;


            Player mainPlayer = new Player("Player", false);
            Player dealer = new Player("Dealer", true);
            List<Player> players = new List<Player> { mainPlayer, dealer };

            foreach (var player in players)
            {
                player.GenerateHand();

            }

            Console.WriteLine(string.Format("Dealer's hand: #HIDDEN#, {0}", dealer.hand[1].Number.ToString()));
            Console.WriteLine(string.Format("Your hand: {0}, {1} ({2})", mainPlayer.hand[0].Number.ToString(), mainPlayer.hand[1].Number.ToString(), mainPlayer.Total()));

            foreach (Player player in players)
            {
                ProcessTurn(player, player.hand);
            }

            EndTotals(mainPlayer, dealer);

            Console.ReadLine();
        }

        private static void ProcessTurn(Player player, List<Card> hand)
        {
            if(!player.IsDealer)
            {
                Console.WriteLine("\n" + player.Name.ToUpper() + ": Hit (ENTER), Stand (SPACE)");
                
                ConsoleKeyInfo response = Console.ReadKey(true);

                List<string> newHand = new List<string>();

                if (response.Key == ConsoleKey.Spacebar)
                {
                    return;
                }
                else if (response.Key == ConsoleKey.Enter)
                {
                    Card randomCard = deck.RandomCard();
                    player.hand.Add(deck.RandomCard());

                    foreach (var card in player.hand)
                    {
                        newHand.Add(card.Number.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("You did not enter a valid key.");
                    ProcessTurn(player, player.hand);
                }
                
                Console.WriteLine("Your hand: " + string.Join(", ", newHand) + " (" + player.Total() + ")");
                

                if(player.Total() > 21)
                {
                    return; //player busts, exit method and proceed to next player or Program.EndTotals()
                }
                else
                {
                    ProcessTurn(player, hand);
                }
            }
            else
            {
                Console.WriteLine("Dealer's hand: " + player.hand[0].Number.ToString() + ", " + player.hand[1].Number.ToString() + " (" + player.Total() + ")");
                if (player.Total() == 21 && player.hand.Count == 2)
                {
                    return; //dealer has blackjack at start, exit method and go to Program.EndTotals()
                }
                else
                {
                    List<string> newHand = new List<string>();
                    foreach (Card card in player.hand)
                    {
                            newHand.Add(card.Number.ToString());
                    }
                    while (player.Total() < 17)
                    {
                        Thread.Sleep(1500);
                        Move.Hit(player, deck, newHand);
                        Console.WriteLine("Dealer's hand after hit: " + string.Join(", ", newHand) + " (" + player.Total() + ")\n");
                    }
                    if (player.Total() > 21) //dealer busts, exit method and go to Program.EndTotals()
                    {
                        return;
                    }
                }
            }
        }

        private static void EndTotals(Player player, Player dealer)
        {
            if (dealer.Total() > 21)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Dealer busts! Everybody wins!");
                return;
            }
            else if (dealer.Total() > 21 && player.Total() > 21)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You and the dealer both lose! Nobody wins.");
            }
            else if (dealer.Total() == 21 && dealer.hand.Count == 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Dealer has Blackjack. You lose!");
            }
            else
            {
                if (player.Total() > 21)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You busted. Sorry, you lose!");
                }
                else if (player.Total() <= 21)
                {
                    if (player.Total() < dealer.Total())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You had less than the dealer. Sorry, you lose!");
                    }
                    else if (player.Total() == dealer.Total())
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("You had the same as the dealer. Push, take your money back.");
                    }
                    else if (player.Total() > dealer.Total())
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You had more than the dealer. You won!");
                    }
                }
            }
            Console.ResetColor();
            Console.WriteLine("Go again? Y/N");
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Y)
            {
                GameStart();
            }
            else if (key.Key == ConsoleKey.N)
            {
                Console.WriteLine("Thanks for playing Blackjack by Kyle Smith. Press any key to exit.");
            }
        }
    }
}
