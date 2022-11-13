using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShedulerBotSgk.App
{
    internal class Application
    {
        static public readonly double Version = 3.00;
        static public bool Debug = false;
        static public bool AIMode = false;

        public void Verify(string[] args)
        {
            foreach (var item in args)
            {
                if (item == "-debug")
                    Debug = true;

                if (item == "-ai")
                    AIMode = true;
            }
        }
    }
}
