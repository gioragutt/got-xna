using Game_Of_Throws_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna;

namespace GameOfThrowsServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Game Of Throws Server";
            Console.WriteLine("Server version: Bow and Arrow");
            ConnectionHandler cntHandler = new ConnectionHandler();
            cntHandler.Listen();
        }
    }
}
