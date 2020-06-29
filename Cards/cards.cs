// based on code from: https://github.com/andrewphoy/Hearts
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Cards
{
    // card related classes
    public class Card
    {
        // class representing a single card
        public Rank myRank;
        public Suit mySuit;
        public Card()
        {

        }

        public Card(Suit suit, Rank rank)
        {
            mySuit = suit;
            myRank = rank;
        }

        public Card(Rank rank, Suit suit)
        {
            mySuit = suit;
            myRank = rank;
        }
        // function to check if cards are the same
        public override bool Equals(object obj)
        {
            try
            {
                Card b = (Card)obj;
            }
            catch(InvalidCastException e) 
            {
                string error = e.ToString();
                return false;
            }
            Card c = (Card)obj;
            if (c == null)
            {
                return false;
            }

            return c.myRank == this.myRank && c.mySuit == this.mySuit;
        }
        // creating a number for all the cards
        public override int GetHashCode()
        {
            return (int)myRank + 13 * (int)mySuit;
        }
        // writing what the cards is 
        public override string ToString()
        {
            return myRank.ToString() + " of " + mySuit.ToString();
        }
        public int Points()
        {
            if (this.mySuit == Suit.Hearts)
                return 1;
            else if (this.mySuit == Suit.Spades && this.myRank == Rank.Queen)
                return 8;
            else
                return 0;
        }
        // short tag of the card
        public string Tag
        {
            get { return ((int)myRank).ToString() + mySuit.ToString().First(); }
        }
    }
    public class Hand : List<Card>
    {
        // class of hand, which is a list of cards
        public bool ContainsSuit(Suit suit)
        {
            return this.Any(c => c.mySuit == suit);
        }
        public List<Card> CardsOfSuit(Suit suit)
        {
            List<Card> cards = new List<Card>();
            foreach (Card card in this)
            {
                if (card.mySuit == suit)
                {
                    cards.Add(card);
                }
            }
            return cards;
        }
        // searching card by tag in hand
        public Card CardByTag(string tag)
        {
            var cards = from c in this
                        where c.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase)
                        select c;
            if (cards.Count() == 0)
            {
                return null;
            }
            else
            {
                return (Card)cards.First();
            }
        }
        public string Printhand()
        {
            string str = string.Empty;
            this.ForEach(c => str += c.Tag + " ");
            return str;
        }

        #region Sorting
        public void SortBySuit()
        {
            this.SortBySuit(Direction.Ascending);
        }

        public void SortBySuit(Direction direction)
        {
            this.Sort(new SuitComparer(direction));
        }

        public void SortByRank()
        {
            this.SortByRank(Direction.Ascending);
        }

        public void SortByRank(Direction direction)
        {
            this.Sort(new RankComparer(direction));
        }
        private class SuitComparer : IComparer<Card>
        {

            private Direction Direction { get; set; }

            public SuitComparer(Direction direction)
            {
                this.Direction = direction;
            }

            public int Compare(Card x, Card y)
            {
                if ((int)x.mySuit > (int)y.mySuit)
                {
                    return 1 * (int)Direction;
                }
                else if ((int)x.mySuit < (int)y.mySuit)
                {
                    return -1 * (int)Direction;
                }
                else
                {
                    // same suit
                    if ((int)x.myRank > (int)y.myRank)
                    {
                        return 1 * (int)Direction;
                    }
                    else if ((int)x.myRank < (int)y.myRank)
                    {
                        return -1 * (int)Direction;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
        }

        private class RankComparer : IComparer<Card>
        {

            private Direction Direction { get; set; }

            public RankComparer(Direction direction)
            {
                this.Direction = direction;
            }

            public int Compare(Card x, Card y)
            {
                if ((int)x.myRank > (int)y.myRank)
                {
                    return 1 * (int)Direction;
                }
                else if ((int)x.myRank < (int)y.myRank)
                {
                    return -1 * (int)Direction;
                }
                else
                {
                    // same rank
                    if ((int)x.mySuit > (int)y.mySuit)
                    {
                        return 1 * (int)Direction;
                    }
                    else if ((int)x.mySuit < (int)y.mySuit)
                    {
                        return -1 * (int)Direction;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
        }
        #endregion
    }
    public class Deck
    {
        // Deck class
        public List<Card> myCards;

        public Deck()
        {
            // contructor of a deck, enumarating all the cards
            myCards = new List<Card>();
            foreach (Suit s in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank r in Enum.GetValues(typeof(Rank)))
                {
                    myCards.Add(new Card(s, r));
                }
            }
        }
        public Deck(List<Card> cards)
        {
            myCards = cards;
        }

        public Card[] Cards
        {
            // function making an array of all the cards
            get { return myCards.ToArray(); }
        }
        public void Shuffle()
        {
            // function suffeling the cards
            Random rnd = new Random();

            for (int i = myCards.Count - 1; i > 0; i--)
            {
                int swapIdx = rnd.Next(i + 1);
                Card temp = myCards[i];
                myCards[i] = myCards[swapIdx];
                myCards[swapIdx] = temp;
            }
        }

        public Hand[] Deal(int numPlayers)
        {
            // dealing the cards to the players
            Hand[] hands = new Hand[numPlayers];

            for (int i = 0; i < numPlayers; i++)
            {
                hands[i] = new Hand();
            }

            while (myCards.Count >= numPlayers)
            {
                for (int j = 0; j < numPlayers; j++)
                {
                    hands[j].Add(myCards[0]);
                    myCards.RemoveAt(0);
                }
            }
            return hands;
        }

    }
}