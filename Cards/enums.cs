// based on code from: https://github.com/andrewphoy/Hearts
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// enumeration of the Suits and Ranks
    public enum Suit {
        Clubs = 1,
        Diamonds = 2,
        Spades = 3,
        Hearts = 4
    }

    public enum Rank {
        /*
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        */
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    }

    public enum Direction {
        Ascending = 1,
        Descending = -1
    }