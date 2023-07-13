using System;
using System.Collections.Generic;

namespace YahtzeeCSNet5
{
    class Game
    {
        public Game(int numPlayers)
        {
            numPlayers = numPlayers < 2 ? 2 : numPlayers;
            players = new List<Player>();

            for (int i = 0; i < numPlayers; i++)
            {
                Console.Write($"Enter Name for Player {i + 1}:");
                players.Add(new Player(Console.ReadLine()));
            }
            playerTurn = players[0];
        }
        public List<Player> players { get; set; }
        public Player playerTurn { get; set; }

        public bool hasGameEnded { get; set; }
        public void nextPlayerTurn()
        {
            try
            {
                playerTurn = players[players.IndexOf(playerTurn) + 1];
            }
            catch
            {
                playerTurn = players[players.IndexOf(playerTurn) - 1];
            }
        }
    }
}
