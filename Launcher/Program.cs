using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sonic_CD;

namespace Launcher
{
    class Program
    {
        static void Main( string[] args )
        {
            using ( var game = new Game())
            {
                game.Run();
            }
        }
    }
}
