using System;

namespace NIM2.Tools
{
    public class Game
    {
        private readonly ChainOfPearls _chain1 = new ChainOfPearls();
        private readonly ChainOfPearls _chain2 = new ChainOfPearls();

        private int Gamemode { get; set; } = 1;
        private int CurrentPlayer { get; set; } = 1;

        private void InitializeChainsize(int chainSize)
        {
            _chain1.SizeOfChain = chainSize;
            _chain2.SizeOfChain = chainSize + 1;
        }

        private void DrawChains()
        {
            if (_chain1.SizeOfChain > 0 && _chain2.SizeOfChain > 0)
            {
                Console.Write("Kette 1: ");
                _chain1.DrawChain();
                Console.Write("Kette 2: ");
                _chain2.DrawChain();
            }else if (_chain1.SizeOfChain == 0)
            {
                Console.Write("Kette 2: ");
                _chain2.DrawChain();
            }else if (_chain2.SizeOfChain == 0)
            {
                Console.Write("Kette 1: ");
                _chain1.DrawChain();
            }
            else
            {
                Console.Write("Fehler");
            }
        }
        
        public void Start()
        {
            Console.WriteLine("Willkommen zu Perlen vor die Säue!\n");
            Console.WriteLine("Regeln:");
            Console.WriteLine(
                "Es gibt zwei Reihen Perlen. Eine Reihe mit fünf Perlen\ndie andere Reihe mit sechs Perlen:");
            Console.WriteLine("OOOOO");
            Console.WriteLine("OOOOOO");
            Console.WriteLine("Die Spieler nehmen abwechselnd Perlen weg. ");
            Console.WriteLine("Dabei gilt: In einem Spielzug darf ein Spieler nur aus einer Reihe\nPerlen entnehmen.");
            Console.WriteLine("Er darf dabei so viele Perlen wie er möchte aus der Reihe nehmen. ");
            Console.WriteLine("Der Spieler, der die letzte Perle nimmt, verliert.");
            Console.ReadKey();
            Console.Clear();
            this.GetGameMode();
        }

        private void GetGameMode()
        {
            Console.Write("Wähle deinen Spielmodus: (1/2)\n");
            Console.Write("1. Einzelspieler\n");
            Console.Write("2. Mehrspieler\n");
            
            bool isInt = Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out int selection);

            if (isInt && (selection.Equals(1) || selection.Equals(2)))
            {
                this.Gamemode = selection;

                if (Gamemode == 2)
                {
                    this.InitializeChainsize(5);
                    Console.Clear();
                    this.MultiplayerGameRound();
                }
                else
                {
                    this.InitializeChainsize(5);
                    Console.Clear();
                    this.SingleplayerGameRound();
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Falsche Eingabe, bitte nur die Zahl 1 oder 2 eingeben.");
                this.GetGameMode();
            }
        }

        private void GetInput(int playerNumber)
        {
            bool isInt = false;
            int chain = 1;
            
            if (_chain1.SizeOfChain != 0 && _chain2.SizeOfChain != 0)
            {
                Console.WriteLine("Spieler Nummer " + playerNumber + ", aus welcher Kette sollen Perlen genommen werden? (1/2)");
                isInt = Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out chain);
            }
            else
            {
                isInt = true;
                chain = (_chain1.SizeOfChain == 0) ? 2 : 1;
            }
            
            if (isInt  && 
                ((chain.Equals(1) && _chain1.SizeOfChain > 0) || (chain.Equals(2) && _chain2.SizeOfChain > 0)))
            {
                ChainOfPearls selectedChain = chain.Equals(1) ?  _chain1 :  _chain2;

                Console.WriteLine("\nSpieler Nummer " + playerNumber + ", wie viele Perlen sollen entnommen werden? (1-" + selectedChain.SizeOfChain + ")");
                isInt = Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out int numberOfPearls);
                Console.Clear();
                
                if (isInt && numberOfPearls <= selectedChain.SizeOfChain)
                {
                    Console.WriteLine("Nehme " + numberOfPearls + " Perlen aus Kette " + chain);
                    selectedChain.SizeOfChain -= numberOfPearls;
                    this.CurrentPlayer = (this.CurrentPlayer == 1) ?  2 :  1;
                }
                else
                {
                    Console.WriteLine("\nFalsche Eingabe");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("\nFalsche Eingabe, bitte nur die Zahl 1 oder 2 eingeben.");
            }
        }

        private bool HasGameEnded()
        {
            return (_chain1.SizeOfChain == 0 && _chain2.SizeOfChain == 1) 
                   || (_chain1.SizeOfChain == 1 && _chain2.SizeOfChain == 0);
        }

        private void MultiplayerGameRound()
        {           
            this.DrawChains();
            this.GetInput(this.CurrentPlayer);

            if (this.HasGameEnded())
            {
                Console.WriteLine("Spieler " + this.CurrentPlayer + " gewinnt!");
            }
            else
            {
                this.MultiplayerGameRound();
            }
        }

        private void SingleplayerGameRound()
        {
            while (!this.HasGameEnded())
            {            
                this.DrawChains();
                this.GetInput(this.CurrentPlayer);
                this.AiRound();
            }

            if (this.CurrentPlayer == 2)
            {
                this.DrawChains();
                Console.WriteLine("Du gewinnst!");
            }
            else
            {
                this.DrawChains();
                Console.WriteLine("Computer gewinnt!");
            }
        }

        private void AiRound()
        {
            if (HasGameEnded()) return;
            
            this.DrawChains();
            
            #region Ansatz 1
            
//            if (_chain1.SizeOfChain == 1 && !_chain2.IsEmpty)
//            {
//                _chain2.SizeOfChain = 0;
//            }else if (_chain2.SizeOfChain == 1 && !_chain1.IsEmpty)
//            {
//                _chain1.SizeOfChain = 0;
//            }else if (!_chain1.IsEmpty && !_chain2.IsEmpty)
//            {
//                if ((_chain1.SizeOfChain % 2 == 0 && _chain2.SizeOfChain % 2 != 0) && _chain1.SizeOfChain != 2)
//                {
//                    _chain2.SizeOfChain--;
//                }
//                else if ((_chain2.SizeOfChain % 2 == 0 && _chain1.SizeOfChain % 2 != 0) && _chain2.SizeOfChain != 2)
//                {
//                    _chain1.SizeOfChain--;
//                }
//                else
//                {
//                    if (_chain1.SizeOfChain != 2)
//                    {
//                        _chain1.SizeOfChain = 2;
//                    }
//                    else
//                    {
//                        _chain2.SizeOfChain = 2;
//                    }
//                }
//            }else if (_chain1.IsEmpty)
//            {
//                _chain2.SizeOfChain = 1;
//            }else if (_chain2.IsEmpty)
//            {
//                _chain1.SizeOfChain = 1;
//            }
            
            #endregion Ansatz 1
            
            #region Ansatz 2
            
//            
//            if (!_chain1.IsEmpty && !_chain2.IsEmpty && (_chain1.SizeOfChain > 2 && _chain2.SizeOfChain > 2))
//            {
//                if (_chain1.SizeOfChain > _chain2.SizeOfChain)
//                {
//                    _chain1.SizeOfChain = _chain2.SizeOfChain;
//                }
//                else if (_chain2.SizeOfChain > _chain1.SizeOfChain)
//                {
//                    _chain2.SizeOfChain = _chain1.SizeOfChain;
//                }
//                else
//                {
//                    if (_chain1.SizeOfChain % 2 != 0)
//                    {
//                        --_chain1.SizeOfChain;
//                    }
//                    else
//                    {
//                        --_chain2.SizeOfChain;
//                    }
//                }
//            }
//            else if (_chain1.IsEmpty)
//            {
//                _chain2.SizeOfChain = 1;
//            }
//            else if (_chain2.IsEmpty)
//            {
//                _chain1.SizeOfChain = 1;
//            }
//            else
//            {
//                if (_chain1.SizeOfChain > _chain2.SizeOfChain)
//                {
//                    _chain1.SizeOfChain--;
//                }
//                else
//                {
//                    _chain2.SizeOfChain--;
//                }
//            }

            #endregion Ansatz 2
            
            #region Ansatz 3
//
//            int xor = _chain1.SizeOfChain ^ _chain2.SizeOfChain;
//           
//            if (_chain1.SizeOfChain == 0 || _chain2.SizeOfChain == 0)
//            {
//                Console.WriteLine("EZ Move 1");
//                _chain1.SizeOfChain = (_chain1.IsEmpty) ? 0 : 1;
//                _chain2.SizeOfChain = (_chain2.IsEmpty) ? 0 : 1;
//            }
//            else if (_chain1.SizeOfChain == 1 || _chain2.SizeOfChain == 1)
//            {
//                Console.WriteLine("EZ Move 2");
//                _chain1.SizeOfChain = (_chain1.SizeOfChain == 1) ? _chain1.SizeOfChain : 0;
//                _chain2.SizeOfChain = (_chain2.SizeOfChain == 1) ? _chain2.SizeOfChain : 0;
//            }else if (_chain1.SizeOfChain == 2 ^ _chain2.SizeOfChain == 2)
//            {
//                Console.WriteLine("EZ Move 3");
//                _chain1.SizeOfChain = (_chain1.SizeOfChain == 2) ? _chain1.SizeOfChain : 2;
//                _chain2.SizeOfChain = (_chain2.SizeOfChain == 2) ? _chain2.SizeOfChain : 2;
//            }
//            else if (_chain1.SizeOfChain >= 3 && _chain2.SizeOfChain >= 3)
//            {
//                Console.WriteLine("Bad move2");
//                if (_chain1.SizeOfChain > _chain2.SizeOfChain)
//                {
//                    _chain1.SizeOfChain--;
//                }
//                else
//                {
//                    _chain2.SizeOfChain--;
//                }
//            }            
//            else if ((_chain1.SizeOfChain == _chain2.SizeOfChain ) && _chain1.SizeOfChain != 2)
//            {
//                Console.WriteLine("Bad move1");
//                _chain1.SizeOfChain--;
//            }   
//            else if ((_chain1.SizeOfChain ^ xor) + 1 < (_chain1.SizeOfChain))
//            {
//                Console.WriteLine("XOR1");
//                _chain1.SizeOfChain -= _chain1.SizeOfChain ^ xor;
//            }
//            else if ((_chain2.SizeOfChain ^ xor) + 1 < (_chain2.SizeOfChain))
//            {
//                Console.WriteLine("XOR2");
//                _chain2.SizeOfChain -= _chain2.SizeOfChain ^ xor;
//            }
//            else
//            {
//                Console.WriteLine("I surrender!");
//                _chain1.SizeOfChain = 1;
//                _chain2.SizeOfChain = 0;
//            }

            //ToDo: Check if could be XORED
            #endregion Ansatz 3

            // Shortest Version:
            #region Ansatz 4

            if (_chain1.SizeOfChain == 1 || _chain2.SizeOfChain == 1)
            {
                _chain1.SizeOfChain = (_chain1.SizeOfChain == 1) ? _chain1.SizeOfChain : 0;
                _chain2.SizeOfChain = (_chain2.SizeOfChain == 1) ? _chain2.SizeOfChain : 0;
            }
            else if (_chain1.SizeOfChain > _chain2.SizeOfChain)
            {
                _chain1.SizeOfChain = _chain2.SizeOfChain;
            }
            else if (_chain2.SizeOfChain > _chain1.SizeOfChain)
            {
                _chain2.SizeOfChain = _chain1.SizeOfChain;
            }
            else
            {
                _chain1.SizeOfChain--;
            }

            #endregion Ansatz 4

            #region Ansatz 5

//            if (_chain1.SizeOfChain == 1 || _chain2.SizeOfChain == 1)
//            {
//                _chain1.SizeOfChain = (_chain1.SizeOfChain == 1) ? _chain1.SizeOfChain : 0;
//                _chain2.SizeOfChain = (_chain2.SizeOfChain == 1) ? _chain2.SizeOfChain : 0;
//            }
//            else
//            {
//                int xor = _chain1.SizeOfChain ^ _chain2.SizeOfChain;
//
//                if ((_chain1.SizeOfChain > _chain2.SizeOfChain) && (_chain1.SizeOfChain - xor > 0))
//                {
//                    _chain1.SizeOfChain -= xor;
//                }
//                else if (_chain2.SizeOfChain - xor > 0)
//                {
//                    _chain2.SizeOfChain -= xor;
//                }
//                else
//                {
//                    Console.WriteLine("Dindu nuffin!");
//                }
//            }

            #endregion Ansatz 5

            this.CurrentPlayer = 1;
        }
    }
}