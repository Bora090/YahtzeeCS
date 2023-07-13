using System.Collections.Generic;
using System.Linq;

namespace YahtzeeCSNet5
{
    #pragma warning disable IDE1006 // Naming Styles
    class Player
    {
        public Player(string namestr)
        {
            name = namestr;
            score = new List<Move>();
            totalScore = 0;
            scoreCard = new Dictionary<string, bool>(){
                {"four-of-a-kind", false},
                {"three-of-a-kind", false},
                {"yahtzee", false},
                {"full-house", false},
                {"ones", false},
                {"twos", false},
                {"threes", false},
                {"fours", false},
                {"fives", false},
                {"sixes", false},
                {"chance", false},
                {"small-straight", false},
                {"large-straight", false}};
        }
        public string name { get; set; }
        public List<Move> score { get; set; }
        public int totalScore { get; set; }

        public Dictionary<string, bool> scoreCard { get; set; }

        public bool isMoveAvailable(string moveName) => !scoreCard[moveName.ToLower()];
        public bool isMoveAvailable(Move move) => !scoreCard[move.name];

        public List<Move> getPossibleMoves(Turn turn)
        {
            List<Move> possibleMoves = turn.getPossibleMoves().Where(move => isMoveAvailable(move)).ToList();
            if (possibleMoves.Count == 0 && turn.throwsLeft < 3)
            {
                foreach (KeyValuePair<string, bool> move in scoreCard.Where(move => move.Value == false))
                {
                    possibleMoves.Add(new Move
                    {
                        name = move.Key,
                        score = 0,
                    });
                }
            }
            return possibleMoves;
        }
    }
}
