using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Cards;

// the game class represents a game of hearts
// it plays round untill a player reaches 75 points
// it has a play function wich plays the games and returns the results in a file.
namespace HeartsGame
{
    class Game
    {
        int[] points;
        Round myRound;
        public Game(int playernumber)
        {
            myRound = new Round(playernumber);
            points = new int[playernumber];
            for (int i = 0; i < playernumber; i++)
            {
                points[i] = 0;
            }
        }
        public void Play(string path)
        {
            while (points.Max() < 75)
            {
                int[] roundreward = myRound.Play();
                for (int i = 0; i < points.Length; i++)
                {
                    points[i] += roundreward[i];
                }
                myRound = new Round(myRound.myNumberPlayers);
            }
            this.Report(path);
        }
        public void Report(string path)
        {
            StringBuilder csv = new StringBuilder();
            var newLine = string.Format("{0}, {1}, {2},{3}", points[0], points[1], points[2], points[3]);
            csv.AppendLine(newLine);
            File.AppendAllText(path, csv.ToString());
        }
    }
}