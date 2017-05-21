﻿
using System;
using System.Collections.Generic;
using System.Text;

namespace B17_Ex02
{
    class GameManager
    {
        private byte m_MaxNumOfGuesses = 4;                 //initialized to a valid number
        private byte m_currRoundNum = 1;
        private byte m_maxWordLenWithSpaces = (byte)(2 * LetterSequence.LengthOfSequence - 1);
        private bool m_runGameFlag = true;
        private LetterSequence m_ComputerSequence = new LetterSequence();   //empty ctor generates a random sequence
        private List<Round> m_RoundsPlayed = new List<Round>();

        public GameManager()                                 //TODO changed from "start" to ctor
        {
            getMaxNumOfGuessesFromUser();
            Ex02.ConsoleUtils.Screen.Clear();
            printBoard();
            while (m_runGameFlag)
            {
                run();
            }
        }

        private void getMaxNumOfGuessesFromUser()
        {
            string userInput;
            bool isValidInput;

            Console.WriteLine("Please input max number of guesses:");
            userInput = Console.ReadLine();
            isValidInput = byte.TryParse(userInput, out m_MaxNumOfGuesses);
            while (!isValidInput || m_MaxNumOfGuesses < 4 || m_MaxNumOfGuesses > 10)
            {
                if (!isValidInput)
                {
                    Console.Write("Invalid input, please input a number between 4 and 10: ");
                }
                else
                {
                    Console.Write("The number is out of range, please input a number between 4 and 10: ");
                }
                userInput = Console.ReadLine();
                isValidInput = byte.TryParse(userInput, out m_MaxNumOfGuesses);
            }
        }

        private void printBoard()
        {
            // main title
            Console.WriteLine("Current board status:");
            //print the title row and the first row
            printFirstTwoRows();
            // continue of table
            printRounds();
        }

        private void printFirstTwoRows()
        {
            StringBuilder rowTitleString = new StringBuilder();
            string hiddenComputerSequenceStr = new string('#', LetterSequence.LengthOfSequence);

            // first line in table
            //rowTitleString.Append(m_verticalBorderChar);
            rowTitleString.Append('║');
            rowTitleString.Append("Pins:".PadRight(m_maxWordLenWithSpaces + 2)); // 2 for space near border
            rowTitleString.Append('║');
            rowTitleString.Append("Result:".PadRight(m_maxWordLenWithSpaces));
            rowTitleString.Append('║');
            Console.WriteLine(rowTitleString);
            printRowSeperator();
            // second line in table
            printRow(hiddenComputerSequenceStr, string.Empty);
            printRowSeperator();
        }

        private void printRowSeperator()
        {
            StringBuilder rowSeperator = new StringBuilder();

            // Pins column
            rowSeperator.Append('╠');
            rowSeperator.Append(new string('═', m_maxWordLenWithSpaces + 2)); // 2 for space near border
            // Result column
            rowSeperator.Append('╬');
            rowSeperator.Append(new string('═', m_maxWordLenWithSpaces));
            rowSeperator.Append('╣');
            Console.WriteLine(rowSeperator);
        }

        private void printRounds()
        {
            string pinsString = string.Empty;
            string resultString = string.Empty;

            for (int i = 0; i < m_MaxNumOfGuesses; i++)
            {
                // the (i+1) round has already occurred
                if (i < m_RoundsPlayed.Count)
                {
                    pinsString = m_RoundsPlayed[i].Sequence;
                    resultString = m_RoundsPlayed[i].Result;
                }
                else
                {
                    pinsString = resultString = string.Empty;
                }

                printRow(pinsString, resultString);
                printRowSeperator();
            }
        }

        private void printRow(string i_PinsString, string i_ResultString)
        {
            StringBuilder rowString = new StringBuilder();
            string formattedPinsStr = separateLettersStringWithSpaces(i_PinsString);
            string formattedResultStr = separateLettersStringWithSpaces(i_ResultString);

            rowString.Append('║');
            rowString.Append(' ');
            rowString.Append(formattedPinsStr.PadRight(2 * LetterSequence.LengthOfSequence));
            rowString.Append('║');
            rowString.Append(formattedResultStr.PadRight(2 * LetterSequence.LengthOfSequence - 1));
            rowString.Append('║');
            Console.WriteLine(rowString);
        }

        //gets a string, and adds a space between every two chars
        private string separateLettersStringWithSpaces(string i_Str)
        {
            StringBuilder outputStr = new StringBuilder();

            foreach (char letter in i_Str)
            {
                outputStr.Append(letter);
                outputStr.Append(' ');
            }

            // remove the last space in outputStr
            if (outputStr.Length != 0)
            {
                outputStr.Remove(outputStr.Length - 1, 1);
            }

            return outputStr.ToString();
        }

        private void run()
        {
            Round currentRound;
            string userInput;

            while (m_runGameFlag && m_currRoundNum <= m_MaxNumOfGuesses)           //TODO why not while(m_runGameFlag && m_currRoundNum <= m_maxRoundNum)
            {
                userInput = getInputFromUser();
                // sequence input
                if (!userInput.ToUpper().Equals("Q"))
                {
                    currentRound = new Round(userInput);
                    currentRound.PlayRound(m_ComputerSequence);
                    m_RoundsPlayed.Add(currentRound);
                    m_currRoundNum++;
                    Ex02.ConsoleUtils.Screen.Clear();
                    printBoard();
                    if (currentRound.IsWinRound())
                    {
                        winGame();
                    }
                }
                // quit input
                else
                {
                    endGame();
                }

            }
            loseGame();
        }

        // return valid input: valid sequence or "Q"
        private string getInputFromUser()
        {
            string userInput = string.Empty;
            string validationResult;
            bool endOfInput = false;

            while (!endOfInput)
            {
                Console.WriteLine("Please type your next guess <A B C D> or 'Q' to quit");
                userInput = Console.ReadLine();
                // valid input: valid sequence or "Q"
                if (LetterSequence.IsValidSequence(userInput, out validationResult) ||
                    userInput.ToUpper().Equals("Q"))
                {
                    endOfInput = true;
                }
                // invalid input, print the kind of error
                else
                {
                    Console.WriteLine("{0} Try again.", validationResult);
                }
            }

            return userInput;
        }

        private void winGame()
        {
            Console.WriteLine("You guessed after {0} steps!", m_currRoundNum);
            promptUserForRestart();
        }

        private void loseGame()
        {
            Console.WriteLine("Game Over - No more guesses allowed.");
            Console.WriteLine("The correct sequence is: {0}", m_ComputerSequence.SequenceStr);
            promptUserForRestart();
        }

        private void promptUserForRestart()
        {
            string userInput = string.Empty;
            bool endOfInput = false;

            Console.WriteLine("Would you like to start a new game? <Y/N>");
            userInput = Console.ReadLine();
            while (!endOfInput)
            {
                if (userInput.ToUpper().Equals("Y"))
                {
                    startNewGame();
                    endOfInput = true;
                }
                else if (userInput.ToUpper().Equals("N"))
                {
                    endGame();
                    endOfInput = true;
                }
                else
                {
                    Console.WriteLine("Error - please insert Y/N");
                    userInput = Console.ReadLine();               
                }
            }
        }

        private void startNewGame()
        {
            Ex02.ConsoleUtils.Screen.Clear();
            getMaxNumOfGuessesFromUser();
            m_ComputerSequence = new LetterSequence();
            m_RoundsPlayed = new List<Round>();       //TODO why not m_RoundsOfGame.Clear()
            m_currRoundNum = 1;
            Ex02.ConsoleUtils.Screen.Clear();
            printBoard();
            m_runGameFlag = true;
        }

        private void endGame()
        {
            Console.WriteLine("The game ended.");
            m_runGameFlag = false;
        }
    }
}
