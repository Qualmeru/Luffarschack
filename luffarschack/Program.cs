using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luffarschack
{
    class Program
    {
        /*
          * Detta är ett Luffarshack spel som fungerar som det gör i vanliga fall, Två spelare som spelar mot varandra och ingen dator.
          * Observera att du inte kan flytta dina pjäser efteråt 
          * Du använder dig av piltangenter för att navigera runt på spelplanen och sedan tryck enter för att placera pjäsen.
          * Samtliga konsol positioner är hårdkodade i sitt ursprung ( vinnaren visas alltid). Resten beräknas genom förskjutning från ursprunget till "(0,0)" på nätet, vilket är mer likt (2, 4).
          */

        // Origin marker Position(ursprungs position på markören)
        const int xOrig = 2;
        const int yOrig = 4;

        //  steps to move
        const int xStep = 4;
        const int yStep = 2;
        static int move = 0;
        //so that player can not remove more than one player piece at second stage
        static bool moved = true;

        static int[] pos = new int[2];
        //The board 
        static Player[,] board = new Player[3, 3];
        //set current winner to none at beginning
        static Player winner = Player.None;

        enum Player
        {
            None = 0,
            P1,
            P2
        }

        static void Main(string[] args)
        {
            Console.CursorSize = 100;
            ConsoleKey playAgain = ConsoleKey.Y;
            while (playAgain == ConsoleKey.Y)
            {
                //Player 1 Starts
                Player currentPlayer = Player.P1;
                while (!GameOver())
                {
                    GetUserMove(currentPlayer);
                    currentPlayer = currentPlayer == Player.P1 ? Player.P2 : Player.P1;

                }

                DisplayBoard(Player.None);
                Console.SetCursorPosition(0, 13);

                if (winner == Player.None)
                {
                    Console.WriteLine("Oavgjort!...");
                }
                else
                {
                    Console.WriteLine("Vinnaren är: Spelare {0}", winner == Player.P1 ? 'X' : 'O');
                }

                ResetGame();
                Console.WriteLine("Vill du spela igen? [Y/N]\n");
                playAgain = Console.ReadKey().Key;
            }
        }

        static void GetUserMove(Player currentPlayer)
        {
            move++;
            bool validMove = false;
            while (!validMove)
            {
                DisplayBoard(currentPlayer);
                //place the marker att the top left spot
                Console.CursorLeft += 2;
                Console.CursorTop -= 8;

                pos[0] = Console.CursorLeft;
                pos[1] = Console.CursorTop;

                ConsoleKey conKey = ConsoleKey.NoName;
                while ((conKey = Console.ReadKey().Key) != ConsoleKey.Enter)
                {
                    DisplayBoard(currentPlayer);
                    //implementation for moving player piece with arrow keys
                    switch (conKey)
                    {
                        case ConsoleKey.UpArrow:
                            if (pos[1] == 8 || pos[1] == 6) pos[1] -= 2;
                            break;
                        case ConsoleKey.DownArrow:
                            if (pos[1] < 8) pos[1] += 2;
                            break;
                        case ConsoleKey.LeftArrow:
                            if (pos[0] > 2) pos[0] -= 4;
                            break;
                        case ConsoleKey.RightArrow:
                            if (pos[0] < 10) pos[0] += 4;
                            break;
                    }

                    Console.SetCursorPosition(pos[0], pos[1]);
                }

                //gets the x and y position on the board where the player piece was set 
                int y = (pos[1] - yOrig) / yStep;
                int x = (pos[0] - xOrig) / xStep;
                //current playing player
                var current = currentPlayer == Player.P1 ? 1 : 2;
                //selected position player
               var position = board[y, x] == Player.P1 ? 1 : 2;

                //Implementation for moving player piece at second stage

                if (move > 6 && position == current || board[y, x] == Player.None && move > 6)
                {


                    if (moved)
                    {//position == current &&

                        if (position == current && board[y, x] == currentPlayer)
                        {
                            moved = false;
                            
                            //sets seleced position to none (' ')
                            board[y, x] = Player.None;




                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine(" Tryck på en av dina pjäser och sedan på platsen du vill pjäsen flytta till.");
                            Console.WriteLine("(Tryck valfri tangent för att fortsätta...)");
                            Console.ReadKey();
                        }

                    }
                    else if (board[y, x] == Player.None && !moved)
                    {

                        board[y, x] = currentPlayer;
                        moved = true;
                        validMove = true;


                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("något fel hände, kontrollera ditt drag.  Tryck på en av dina pjäser och sedan på platsen du vill pjäsen flytta till. ");
                        Console.WriteLine("(Tryck valfri tangent för att fortsätta...)");
                        Console.ReadKey();
                    }






                }
                else if (board[y, x] == Player.None && move > 6 || move > 6 && !(position == current))
                {
                    Console.Clear();
                    Console.WriteLine("Tryck på en av dina pjäser och sedan på platsen du vill pjäsen flytta till. ");
                    Console.WriteLine("(Tryck valfri tangent för att fortsätta...)");
                    Console.ReadKey();
                }

                // first stage

                if (move <= 6)
                {

                    if (board[y, x] == Player.None)
                    {
                        board[y, x] = currentPlayer;
                        validMove = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Platsen du försöker flytta till är upptagen, var vänlig att flytta markören någon annanstans...");
                        Console.WriteLine("(Tryck valfri tangent för att fortsätta...)");
                        Console.ReadKey();
                    }


                }



            }
        }




        static void DisplayBoard(Player currentPlayer)
        {
            //Draw the player Board 
            Console.Clear();
            DisplayInstructions();
            Console.WriteLine("-------------");
            for (int row = 0; row < 3; row++)
            {
                char[] arr = new char[3];
                for (int col = 0; col < 3; col++)
                {
                    //space at every avalable place at the beginning
                    if (board[row, col] == Player.None)
                    {

                        arr[col] = ' ';
                    }
                    else
                    {
                        arr[col] = board[row, col] == Player.P1 ? 'X' : 'O';
                    }
                }
                Console.WriteLine("| {0} |", string.Join(" | ", arr));
                Console.WriteLine("-------------");
            }
            Console.WriteLine("\nSpelare {0}'s tur....", currentPlayer == Player.P1 ? 'X' : 'O');
        }

        static bool GameOver()
        {
            // Reading order
            bool boardComplete = true;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Player player = Player.None;
                    if ((player = board[row, col]) != Player.None)
                    {
                        // Check row
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (board[row, i] != player)
                                {
                                    break;
                                }
                                else if (i == 2)
                                {
                                    winner = player;
                                    return true;
                                }
                            }
                        }

                        // Check column
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (board[i, col] != player)
                                {
                                    break;
                                }
                                else if (i == 2)
                                {

                                    winner = player;
                                    return true;
                                }
                            }
                        }

                        // Determines whether we should check the diagonals
                        if ((row == 1 && col == 1) || (col != 1 && row != 1))
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (board[i, i] != player)
                                {
                                    break;
                                }
                                else if (i == 2)
                                {
                                    winner = player;
                                    return true;
                                }
                            }

                            for (int i = 2, j = 0; j < 3; i--, j++)
                            {
                                if (board[i, j] != player)
                                {
                                    break;
                                }
                                else if (j == 2)
                                {
                                    winner = player;
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        boardComplete = false;
                    }
                }
            }

            if (boardComplete)
            {
                return true;
            }
            return false;
        }

        static void ResetGame()
        {
            board = new Player[3, 3];
            winner = Player.None;
            move = 0;
            moved = true;

        }

        static void DisplayInstructions()
        {

            if (move > 6)
            {
                Console.WriteLine("Tänk på att du inte ska ta up en pjäs och sätta den på samma ställe,\ndå går turen över.\n");
            }
            else
            {
                Console.WriteLine("Tips: Använd Piltangenter för att navigera. \n (Tryck enter när du vill placera markören)\n");
            }
        }


    }

}
