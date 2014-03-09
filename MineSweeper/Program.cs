using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
	class Program
	{
		static void Main(string[] args)
		{

			Console.Title = "Console Minesweeper";

			ConsoleInput.ResetColours();
			Console.Clear();

			//  Game Width
			int width = ConsoleInput.GetNumberValue("Please input the game grid width: ", 5, 50);
			int height = ConsoleInput.GetNumberValue("Please input the game grid height: ", 5, 50);

			//  Go!
			Game game = new Game(width, height);
			ConsoleInput.SetGameReference(game);
			game.Play();

		}
	}
}
