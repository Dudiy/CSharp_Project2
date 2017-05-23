﻿using System;
using System.Collections.Generic;
using System.Text;

namespace B17_Ex02
{
    public class Round
    {
        private LetterSequence m_Sequence;
        private string m_Result = string.Empty;
        private bool m_WinRound = false;

        public Round(string i_SequenceStr)
        {
            m_Sequence = new LetterSequence(i_SequenceStr);
        }

        public string Sequence
        {
            get { return m_Sequence.SequenceStr; }
        }

        public string Result
        {
            get
            {
                if (m_Result.Equals(string.Empty))
                {
                    // TODO throw error??                    
                }
                 
                return m_Result;
            }
        }
        
        // updates the value of result, returns true if the guess is correct
        public void PlayRound(LetterSequence i_ComputerSequence)
        {
            m_Result = m_Sequence.Compare(i_ComputerSequence);

            // win round if: length of result is the same as the sequence and there are no 'X' chars
            m_WinRound = m_Result.Length == LetterSequence.LengthOfSequence && !m_Result.Contains("X");     
        }

        public bool IsWinRound()
        {
            return m_WinRound;          // TODO change to m_playerWon and use getter?
        }
    }
}
