using System;

namespace NIM2.Tools
{
    public class ChainOfPearls
    {
        public int SizeOfChain { get; set; }
        public bool IsEmpty => this.SizeOfChain == 0;

        public void DrawChain()
        {
            string dummy = "";
            for (var i = 0; i < SizeOfChain; i++)
            {
                dummy += "O";
            }
            dummy += "\n";
            Console.Write(dummy);
        }
    }
}