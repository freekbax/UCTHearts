using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cards;

// the player class defines a player.
// it can be a human player, playing with console input
// or a npc player either random or with an play function from MCTS.
namespace HeartsGame
{

    public class Player
    {
        public static Random rnd = new Random();
        public string myName;
        public bool myNpc;
        public bool myRandom;
        public int myNr;
        public bool[] hasSuit;
        public Player(string name, bool npc, bool rnd, int nr)
        {
            myName = name;
            myNpc = npc;
            myRandom = rnd;
            myNr = nr;
            hasSuit = Enumerable.Repeat(true, 4).ToArray(); 

        }
        public Player()
        {

        }
        public Card GetCard(Trick trick)
        {
            //TODO: bepalen welke kaart te spelen
            if (myNpc)
            {
                if (myRandom)
                {
                    // random player
                    Hand actions = trick.AllActions(trick.myRound.myHands[myNr]);
                    return actions[rnd.Next(actions.Count)];
                }
                else
                {
                    // calling the function to compute the action
                    return MCTS.MCTSSample(trick, 125, 0.1f);
                    //return Determinization.UseBestDeterminization(trick, 250,25, 0.1f);
                }
            }
            else
            {
                // handling player input
                trick.Askplayer(myNr);
                string tag = Console.ReadLine();
                Card card = trick.myRound.myHands[myNr].CardByTag(tag);
                // checking if tag is card in hand
                if (card != null)
                {
                    if (trick.IsLegalCard(card))
                    {
                        return card;
                    }
                    else
                    {
                        return this.GetCard(trick);
                    }
                }
                else
                {
                    return this.GetCard(trick);
                }
            }
        }
    }
}