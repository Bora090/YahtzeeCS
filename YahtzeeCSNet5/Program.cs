using System;
using System.Collections.Generic;
using System.Linq;

namespace YahtzeeCSNet5
{
    internal class Program
    {
        static void Main(string[] args)
        {


            Turn turn = new Turn(); 
            Console.Write("How many players do you want (Default 2 Minimum 2):");
            int numPlayers;
            try
            {
                numPlayers = int.Parse(Console.ReadLine());
            }
            catch { numPlayers = 2; }

            Game game = new Game(numPlayers);
            Console.Clear();
            while (!game.hasGameEnded)
            {
                if (turn.throwsLeft < 3 && game.players.Where(player => player.getPossibleMoves(turn).Count == 0).ToList().Count == numPlayers)
                {
                    game.hasGameEnded = true;
                    continue;
                }
                if (turn.throwsLeft < 3 && game.playerTurn.getPossibleMoves(turn).Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"{game.playerTurn.name} You have no possible moves left your turn has been skipped");
                    game.nextPlayerTurn();
                    continue;
                }
                Console.WriteLine($"It\'s your turn {game.playerTurn.name}!");
                Console.WriteLine($"\nCommands:\nroll - To roll dices you aren't holding ({turn.diceLeft} dice, {turn.throwsLeft} throws Left)\nhold:args - hold dices from your current roll ex, hold:1,5\ninfo - view your possible moves/more info\nmove:args - pick a move from the possible moves ex, move:chance\nclear - Clear the console screen\nscore - View the score of both players");
                Console.Write("Enter Command:");
                string inputStr = Console.ReadLine();
                string cmdStr = inputStr.ToLower();
                string cmdArgs = "";
                try
                {
                    cmdStr = inputStr.Split(':')[0].ToLower();
                    cmdArgs = inputStr.Split(':')[1];
                }
                catch { }

                Console.WriteLine("\n");
                switch (cmdStr)
                {
                    case "roll":
                        Console.Clear();
                        if (!turn.canRoll())
                        {
                            Console.WriteLine("Cant roll perhaps you have no throws or dice left, pick a option");
                            break;
                        }
                        Console.WriteLine("You rolled {0}", String.Join(",", turn.roll()));
                        break;
                    case "info":
                        Console.Clear();
                        List<Move> moves = game.playerTurn.getPossibleMoves(turn);
                        Console.WriteLine("Current Roll: {0}", String.Join(",", turn.currentRoll));
                        if (turn.holding.Count > 0)
                        {
                            Console.WriteLine("Holding: {0}", String.Join(",", turn.holding));
                        }
                        Console.WriteLine("Possible Moves:");
                        foreach (Move move in moves)
                        {
                            Console.WriteLine($"{move.name} | {move.score} ({Utility.beautifyName(move.name)})");
                        }
                        break;
                    case "move":
                        if (turn.isMovePossible(cmdArgs, game.playerTurn))
                        {
                            Move selectedMove = turn.getMove(cmdArgs, game.playerTurn);
                            if (!game.playerTurn.isMoveAvailable(selectedMove))
                            {
                                Console.WriteLine("You have already done that move you are unable to do it again.\nSee available moves by typing \"info\" or check \"score\" for which moves you have typed");
                                break;
                            }
                            game.playerTurn.scoreCard[selectedMove.name] = true;
                            game.playerTurn.score.Add(selectedMove);
                            game.playerTurn.totalScore += selectedMove.score;
                            turn = new Turn();
                            game.nextPlayerTurn();
                            Console.Clear();
                            break;
                        }
                        Console.WriteLine("You are unable to do that move.\nSee available moves by typing \"info\"");
                        break;
                    case "hold":
                        try
                        {
                            turn.hold(cmdArgs.Split(",").Select(int.Parse).ToList());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message.ToString());
                        }
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    case "score":
                        Console.Clear();
                        foreach (Player player in game.players)
                        {
                            Console.WriteLine($"{player.name} score:");
                            foreach (Move move in player.score)
                            {
                                Console.WriteLine($"{Utility.beautifyName(move.name)} | {move.score}");
                            }
                            Console.WriteLine($"TOTAL | {player.totalScore}");
                        }
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("That command does not exist");
                        break;
                }
            }
            foreach (Player player in game.players)
            {
                Console.WriteLine($"{player.name} score:");
                foreach (Move move in player.score)
                {
                    Console.WriteLine($"{Utility.beautifyName(move.name)} | {move.score}");
                }
                Console.WriteLine($"TOTAL | {player.totalScore}");
            }
        }
    }
}
