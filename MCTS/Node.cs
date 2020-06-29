using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cards;

// the nodes class is the representation of nodes used to create the search tree in MCTS
// it has a gamestate in the form of a trick and saves other information MCTS needs of the nodes.
namespace HeartsGame
{
    public class Node
    {
        public Node myParent;
        public List<Node> myChildren;
        public Trick myTrick;
        public int myExplorations;
        public int[] myRewards;
        public Card myAction;
        public Node(Trick trick)
        {
            myTrick = trick;
            myChildren = new List<Node>();
            myExplorations = 0;
            myRewards = new int[trick.myRound.myNumberPlayers];
            for (int i = 0; i < myRewards.Length; i++)
            {
                myRewards[i] = 0;
            }
        }
        public Node(Node parent, Trick trick, Card Action)
        {
            myTrick = trick;
            myAction = Action;
            myParent = parent;
            myChildren = new List<Node>();
            myExplorations = 0;
            myRewards = new int[trick.myRound.myNumberPlayers];
            for (int i = 0; i < myRewards.Length; i++)
            {
                myRewards[i] = 0;
            }
        }
    }
}