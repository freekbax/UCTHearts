// based on code from: https://github.com/andrewphoy/Hearts
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cards;

// the round class takes care of a round of hearts.
// it creates all the players and plays the tricks
// the play function plays a round and returns the results.
namespace HeartsGame
{
    public class Round
    {
        public Hand[] myHands;
        public Hand[] myTakenCards;
        public int[] myPoints;
        public bool myIsFirstTrick;
        public int myTrickStarter;
        public int myNumberPlayers;
        public List<Player> myPlayers;
        public Round()
        {

        }
        public Round(int numberPlayers)
        {
            myNumberPlayers = numberPlayers;
            myTakenCards = new Hand[numberPlayers];
            myPoints = new int[numberPlayers];
            myPlayers = new List<Player>();
            for (int i = 0; i < numberPlayers; i++)
            {
                myTakenCards[i] = new Hand();
                myPoints[i] = 0;
                Player p = new Player("player " + i.ToString(), true, true, i);
                myPlayers.Add(p);
            }
            myTakenCards[numberPlayers - 1] = new Hand();
            myPoints[numberPlayers - 1] = 0;
            myPlayers[0].myRandom = false;
            myPlayers[1].myRandom = false;

        }
        internal int[] Play()
        {
            PrepareHands();
            PlayTricks();
            GivePionts();
            return myPoints;
        }

        #region PreTrickPrep
        public void PrepareHands()
        {
            Deck deck = new Deck();
            deck.Shuffle();
            this.myHands = deck.Deal(myNumberPlayers);
            foreach (Hand h in myHands)
            {
                h.SortByRank();
                h.SortBySuit();
            }
        }
        #endregion

        #region Tricks
        private void PlayTricks()
        {
            myIsFirstTrick = true;
            myTrickStarter = GetStartingPlayer();

            while (myHands[0].Count > 0)
            {
                PlayTrick();
            }
        }

        private void PlayTrick()
        {
            Trick t = new Trick(this, myIsFirstTrick);
            t.Play(myTrickStarter);

            myTakenCards[t.myTrickleader].AddRange(t.myCards);

            myTrickStarter = t.myTrickleader;
            myIsFirstTrick = false;
        }
        public void GivePionts()
        {
            for (int i = 0; i < myNumberPlayers; i++)
            {
                myTakenCards[i].ForEach(c => myPoints[i] += c.Points());
            }
        }

        private int GetStartingPlayer()
        {
            Card startCard = GetStartCard();
            for (int i = 0; i < myNumberPlayers; i++)
            {
                if (myHands[i].Contains(startCard))
                {
                    return i;
                }
            }
            throw new Exception("Could not find the start player");
        }

        internal Card GetStartCard()
        {
            Card card = new Card(Suit.Clubs, Rank.Seven);
            return card;
        }

        #endregion

    }
}