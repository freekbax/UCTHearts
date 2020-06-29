// based on code from: https://github.com/andrewphoy/Hearts
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cards;
using CloneExtensions;

// the trick function takes care of a trick
// in the play function is takes care of all the actions from all the players
// it changes the round it is part of
namespace HeartsGame
{
    public class Trick
    {
        public Round myRound;
        public bool myIsFirstTrick;
        public List<Card> myCards;
        public Card myHighCard;
        public Suit mySuit;
        public int myTrickleader;
        public int myPlayerTurn;
        public Trick()
        {

        }
        public Trick(Round round, bool isFirstTrick)
        {
            myRound = round;
            myIsFirstTrick = isFirstTrick;
            myCards = new List<Card>();
        }
        public void Play(int trickStarter)
        {
            myPlayerTurn = trickStarter;
            // loop wich gives every player a turn
            for (int i = 0; i < myRound.myNumberPlayers; i++)
            {
                // players plays a card
                Card played = myRound.myPlayers[myPlayerTurn].GetCard(this);
                System.Console.WriteLine(played.ToString());
                myRound.myHands[myPlayerTurn].Remove(played);
                // if starting player plays a card change the suit and initiate highcard
                if (trickStarter == myPlayerTurn)
                {
                    mySuit = played.mySuit;
                    myHighCard = played;
                    myTrickleader = myPlayerTurn;
                    myCards.Add(played);
                }
                else
                {
                    // if player plays a new highcard
                    if (mySuit == played.mySuit && myHighCard.myRank < played.myRank)
                    {
                        myHighCard = played;
                        myTrickleader = myPlayerTurn;
                        myCards.Add(played);
                    }
                    else if(mySuit != played.mySuit)
                    {
                        myCards.Add(played);
                        myRound.myPlayers[myPlayerTurn].hasSuit[(int)(mySuit) - 1] = false;
                    }
                    else
                    {
                        myCards.Add(played);
                    }
                }
                //changing player turn
                myPlayerTurn = this.Nextplayer();
            }
        }
        public int Nextplayer()
        {
            int next = (myPlayerTurn + 1) % myRound.myNumberPlayers;
            return next;
        }
        public string PrintTrick()
        {
            string str = string.Empty;
            this.myCards.ForEach(c => str += c.Tag + " ");
            return str;

        }
        public bool IsLegalCard(Card card)
        {
            if (mySuit != 0 && mySuit != card.mySuit && myRound.myHands[myPlayerTurn].ContainsSuit(mySuit))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public Hand AllActions(Hand hand)
        {
            Hand h = new Hand();
            foreach (Card c in hand)
            {
                if (this.IsLegalCard(c))
                {
                    h.Add(c);
                }
            }
            return h;
        }
        public void Askplayer(int player)
        {
            System.Console.WriteLine("Trick: " + this.PrintTrick());
            //System.Console.WriteLine("Player " + player);
            System.Console.WriteLine("Cards in hand: " + myRound.myHands[player].Printhand());
        }
        public bool IsTerminal()
        {
            if (!myRound.myHands[myPlayerTurn].Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}