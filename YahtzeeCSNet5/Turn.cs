using System;
using System.Collections.Generic;
using System.Linq;

namespace YahtzeeCSNet5
{
    class Turn
    {
        public Turn()
        {
            throwsLeft = 3;
            diceLeft = 5;
            holding = new List<int>(); //Line might be unneeded
            currentRoll = new List<int>(); //Line might be unneeded
        }
        public int throwsLeft { get; set; }
        public int diceLeft { get; set; }
        public List<int> holding { get; set; }
        public List<int> currentRoll { get; set; }

        public bool canRoll()
        {
            return throwsLeft > 0 && diceLeft > 0 && !(holding.Count == 5 && currentRoll.Count == 0);
        }

        public List<int> roll()
        {
            if (!canRoll())
            {
                throw new Exception("Cant roll perhaps you have no throws or dice left");
            }
            throwsLeft -= 1;
            List<int> roll = new List<int>();

            for (int i = 0; i < diceLeft; i++)
            {
                roll.Add(Utility.rnd.Next(1, 7));
            }
            currentRoll = roll;
            return currentRoll;

        }
        public void hold(List<int> dicesToHold) => hold(dicesToHold.ToArray());
        public void hold(params int[] dicesToHold)
        {
            if (!Utility.hasValues(currentRoll, dicesToHold))
            {
                throw new Exception("Cant hold dices you havent rolled");
            }

            foreach (int dice in dicesToHold)
            {
                currentRoll.Remove(dice);
                holding.Add(dice);
            }
            diceLeft -= dicesToHold.Length;
        }

        public bool isMovePossible(string moveName, Player player)
        {
            return player.getPossibleMoves(this).Count(move => move.name == moveName.ToLower()) > 0;
        }

        public Move getMove(string moveName, Player player)
        {
            return player.getPossibleMoves(this).Where(move => move.name == moveName.ToLower()).ToList()[0];
        }
        public List<Move> getPossibleMoves()
        {

            List<int> dicesToCheck = new List<int>();
            foreach (int dice in holding)
            {
                dicesToCheck.Add(dice);
            }
            foreach (int dice in currentRoll)
            {
                dicesToCheck.Add(dice);
            }

            List<Move> possibleMoves = new List<Move>();
            int diceSum = dicesToCheck.Sum();

            foreach (int dice in dicesToCheck.Distinct())
            {
                int count = dicesToCheck.Count(d => d == dice);
                if (count >= 4)
                {
                    possibleMoves.Add(new Move
                    {
                        name = "four-of-a-kind",
                        score = diceSum,
                    });
                }
                if (count >= 3)
                {
                    possibleMoves.Add(new Move
                    {
                        name = "three-of-a-kind",
                        score = diceSum,
                    });
                }
                if (count == 5)
                {
                    possibleMoves.Add(new Move
                    {
                        name = "yahtzee",
                        score = 50,
                    });
                }
            }

            if (dicesToCheck.Distinct().Count() == 2)
            {
                possibleMoves.Add(new Move
                {
                    name = "full-house",
                    score = 25,
                });
            }
            //maybe refactor singles code to be a list of Move objects
            Dictionary<string, int> singles = new Dictionary<string, int>() { { "ones", 1 }, { "twos", 2 }, { "threes", 3 }, { "fours", 4 }, { "fives", 5 }, { "sixes", 6 } };

            foreach (var single in singles) //these shenigans to avoid repeating code
            {
                int points = 0;
                int count = dicesToCheck.Count(s => s == single.Value);

                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        points += single.Value;
                    }

                    possibleMoves.Add(new Move
                    {
                        name = single.Key,
                        score = points,
                    });
                }
            }


            if (diceSum > 0)
            {
                possibleMoves.Add(new Move
                {
                    name = "chance",
                    score = diceSum,
                });
            }


            if (Utility.hasValues(dicesToCheck, 1, 2, 3, 4) || Utility.hasValues(dicesToCheck, 2, 3, 4, 5) || Utility.hasValues(dicesToCheck, 3, 4, 5, 6))
            {
                possibleMoves.Add(new Move
                {
                    name = "small-straight",
                    score = 30,
                });
            }

            if (Utility.hasValues(dicesToCheck, 1, 2, 3, 4, 5) || Utility.hasValues(dicesToCheck, 2, 3, 4, 5, 6))
            {
                possibleMoves.Add(new Move
                {
                    name = "large-straight",
                    score = 40,
                });
            }

            return possibleMoves;
        }
    }
}
