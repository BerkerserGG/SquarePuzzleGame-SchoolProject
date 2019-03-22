using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquarePuzzleGame_SchoolProject
{
    public class PlayerScore
    {
        public string PlayerName { get; set; }
        public double Score { get; set; }
        public PlayerScore(string playerName, double score)
        {
            PlayerName = playerName;
            Score = score;
        }
    }
}
