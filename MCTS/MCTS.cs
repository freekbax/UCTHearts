using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cards;
using CloneExtensions;

// MCTS plays the perfect information variant of hearts
// it does this according to the standard variant of UTC
// it uses nodes in which information of the gamestate is saved and rewards are saved.
namespace HeartsGame
{
    public static class MCTS
    {
        public static Random rnd = new Random();
        public static Card MCTSSample(Trick startingtrick, int budget, float c)
        {
            Node node = new Node(startingtrick);
            int i = 0;
            if (startingtrick.AllActions(startingtrick.myRound.myHands[startingtrick.myPlayerTurn]).Count == 1)
            {
                return startingtrick.AllActions(startingtrick.myRound.myHands[startingtrick.myPlayerTurn]).First();
            }
            while (i < budget)
            {
                node = Treepolicy(node, c);
                int[] rewards = Simulate(node);
                node = UpdateReward(node, rewards);
                i++;
            }
            float highUCB = 0;
            Node highChild = node;
            foreach (Node child in node.myChildren)
            {
                if (highUCB < UCB1(child.myRewards[node.myTrick.myPlayerTurn], child.myParent.myExplorations, child.myExplorations, c))
                {
                    highUCB = UCB1(child.myRewards[node.myTrick.myPlayerTurn], child.myParent.myExplorations, child.myExplorations, c);
                    highChild = child;
                }
            }
            return highChild.myAction;
        }
        public static void Expand(Node node)
        {
            Hand actions = node.myTrick.AllActions(node.myTrick.myRound.myHands[node.myTrick.myPlayerTurn]);
            foreach (Card card in actions)
            {
                Trick newtrick = DoAction(card, node.myTrick);
                Node child = new Node(node, newtrick, card);
                node.myChildren.Add(child);
            }
        }
        public static Node Treepolicy(Node node, float c)
        {
            while (!node.myTrick.IsTerminal())
            {
                if (node.myChildren.Any())
                {
                    float highUCB = 0;
                    Node highChild = node;
                    foreach (Node child in node.myChildren)
                    {
                        if (child.myExplorations == 0)
                        {
                            return child;
                        }
                        else
                        {
                            if (highUCB < UCB1(child.myRewards[node.myTrick.myPlayerTurn], child.myParent.myExplorations, child.myExplorations, c))
                            {
                                highUCB = UCB1(child.myRewards[node.myTrick.myPlayerTurn], child.myParent.myExplorations, child.myExplorations, c);
                                highChild = child;
                            }

                        }
                    }
                    node = highChild;
                }
                else
                {
                    Expand(node);
                    return node.myChildren[rnd.Next(node.myChildren.Count)];
                }
            }
            return node;
        }
        public static float UCB1(int reward, int parentexplorations, int explorations, float c)
        {
            float ucb = (float)(Math.Abs(((reward / explorations) / 26f) - 1) + (2 * c * Math.Sqrt(2 * Math.Log(parentexplorations) / explorations)));
            return ucb;
        }
        public static Node UpdateReward(Node node, int[] rewards)
        {
            while (node.myParent != null)
            {
                for (int i = 0; i < rewards.Length; i++)
                {
                    node.myRewards[i] += rewards[i];
                }
                node.myExplorations++;
                node = node.myParent;
            }
            node.myExplorations++;
            return node;
        }
        static Trick DoAction(Card card, Trick trick)
        {
            // Function doing an card as an action used by the MCTS algoritm
            Trick t = CloneFactory.GetClone(trick);
            t.myRound.myHands[trick.myPlayerTurn].Remove(card);
            // if the first player plays a card
            if (t.myRound.myTrickStarter == t.myPlayerTurn)
            {
                // update Suit and highcard
                t.mySuit = card.mySuit;
                t.myHighCard = card;
                t.myTrickleader = t.myPlayerTurn;
                t.myCards.Add(card);
            }
            else
            {
                // if not starter player plays a new highcard
                if (t.mySuit == card.mySuit && t.myHighCard.myRank < card.myRank)
                {
                    t.myHighCard = card;
                    t.myTrickleader = t.myPlayerTurn;
                    t.myCards.Add(card);
                }
                else
                {
                    t.myCards.Add(card);
                }
            }
            //if the next player is the trickstarter, we stop the trick
            if (t.Nextplayer() == t.myRound.myTrickStarter)
            {
                t.myRound.myTakenCards[t.myTrickleader].AddRange(t.myCards);
                t.myRound.myTrickStarter = t.myTrickleader;
                t = new Trick(t.myRound, false);
                t.myPlayerTurn = t.myRound.myTrickStarter;
            }
            else
            {
                t.myPlayerTurn = trick.Nextplayer();
            }
            return t;
        }
        public static int[] Simulate(Node n)
        {
            Trick sim = CloneFactory.GetClone(n.myTrick);
            while (!sim.IsTerminal())
            {
                Hand actions = sim.AllActions(sim.myRound.myHands[sim.myPlayerTurn]);
                sim = DoAction(actions[rnd.Next(actions.Count)], sim);
            }
            sim.myRound.GivePionts();
            return sim.myRound.myPoints;
        }
    }
}