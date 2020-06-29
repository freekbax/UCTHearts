using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cards;
using CloneExtensions;

// determinization takes care of the determinization process for MCTS
// it samples hands in differen ways and makes new round instances of those sampled hands
// it solves the new round hands made and takes the best actions over all the rounds.
namespace HeartsGame
{
    class Determinization
    {
        public static Card UseDeterminization(Trick startingTrick, int iterations, int determinizations, float c)
        {
            Dictionary<Card, int> bestcard = new Dictionary<Card, int>();
            foreach (Card action in startingTrick.AllActions(startingTrick.myRound.myHands[startingTrick.myPlayerTurn]))
            {
                bestcard[action] = 0;
            }
            for (int i = 0; i < determinizations; i++)
            {
                Trick t = CloneFactory.GetClone(startingTrick);
                SampleRadomHands(t);
                bestcard[MCTS.MCTSSample(t, iterations, c)]++;
            }
            // returning action with the highest count.
            return bestcard.Aggregate((x, y) => x.Value > y.Value ? x : y).Key; ;
        }
        public static Card UseBetterDeterminization(Trick startingTrick, int iterations, int determinizations, float c)
        {
            Dictionary<Card, int> bestcard = new Dictionary<Card, int>();
            foreach (Card action in startingTrick.AllActions(startingTrick.myRound.myHands[startingTrick.myPlayerTurn]))
            {
                bestcard[action] = 0;
            }
            for (int i = 0; i < determinizations; i++)
            {
                Trick t = CloneFactory.GetClone(startingTrick);
                SampleHands(t);
                bestcard[MCTS.MCTSSample(t, iterations, c)]++;
            }
            // returning action with the highest count.
            return bestcard.Aggregate((x, y) => x.Value > y.Value ? x : y).Key; ;
        }
        public static Card UseBestDeterminization(Trick startingTrick, int iterations, int determinizations, float c)
        {
            Dictionary<Card, int> bestcard = new Dictionary<Card, int>();
            foreach (Card action in startingTrick.AllActions(startingTrick.myRound.myHands[startingTrick.myPlayerTurn]))
            {
                bestcard[action] = 0;
            }
            for (int i = 0; i < determinizations; i++)
            {
                Trick t = CloneFactory.GetClone(startingTrick);
                SampleBetterHands(t);
                bestcard[MCTS.MCTSSample(t, iterations, c)]++;
            }
            // returning action with the highest count.
            return bestcard.Aggregate((x, y) => x.Value > y.Value ? x : y).Key; ;
        }
        public static void SampleRadomHands(Trick startingTrick)
        {
            List<Card> newCards = new List<Card>();
            foreach (Suit s in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank r in Enum.GetValues(typeof(Rank)))
                {
                    if (!startingTrick.myRound.myHands[startingTrick.myPlayerTurn].Contains(new Card(s, r)))
                    {
                        newCards.Add(new Card(s, r));
                    }
                }
            }
            for (int j = 0; j < startingTrick.myRound.myHands.Length; j++)
            {
                if (j != startingTrick.myPlayerTurn)
                {
                    for (int i = 0; i < startingTrick.myRound.myHands[j].Count; i++)
                    {
                        startingTrick.myRound.myHands[j][i] = newCards.First();
                        newCards.RemoveAt(0);
                    }
                }
            }
        }
        public static void SampleHands(Trick startingTrick)
        {
            List<Card> newCards = new List<Card>();
            for (int j = 0; j < startingTrick.myRound.myHands.Length; j++)
            {
                if (j != startingTrick.myPlayerTurn)
                {
                    for (int i = 0; i < startingTrick.myRound.myHands[j].Count; i++)
                    {
                        newCards.Add(startingTrick.myRound.myHands[j][i]);

                    }
                }
            }
            Deck newCardDeck = new Deck(newCards);
            newCardDeck.Shuffle();
            for (int j = 0; j < startingTrick.myRound.myHands.Length; j++)
            {
                if (j != startingTrick.myPlayerTurn)
                {
                    for (int i = 0; i < startingTrick.myRound.myHands[j].Count; i++)
                    {
                        startingTrick.myRound.myHands[j][i] = newCardDeck.myCards.First();
                        newCardDeck.myCards.RemoveAt(0);
                    }
                }
            }
        }
        public static void SampleBetterHands(Trick startingTrick)
        {
            List<Card> newCards = new List<Card>();
            for (int j = 0; j < startingTrick.myRound.myHands.Length; j++)
            {
                if (j != startingTrick.myPlayerTurn)
                {
                    for (int i = 1; i < startingTrick.myRound.myHands[j].Count; i++)
                    {
                        newCards.Add(startingTrick.myRound.myHands[j][i]);
                    }
                }
            }
            Deck newCardDeck = new Deck(newCards);
            newCardDeck.Shuffle();
            for (int j = 0; j < startingTrick.myRound.myHands.Length; j++)
            {
                if (j != startingTrick.myPlayerTurn)
                {
                    for (int i = 1; i < startingTrick.myRound.myHands[j].Count; i++)
                    {
                        if (startingTrick.myRound.myPlayers[startingTrick.myPlayerTurn].hasSuit[(int)newCardDeck.myCards.First().mySuit + 1])
                        {
                            startingTrick.myRound.myHands[j][i] = newCardDeck.myCards.First();
                            newCardDeck.myCards.RemoveAt(0);
                        }
                    }
                }
            }
        }
    }
}