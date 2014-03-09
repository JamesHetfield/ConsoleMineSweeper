using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public static class ConsoleInput
    {

		public static Game GameReference { get; private set; }
		public static void SetGameReference(Game GameReference)
		{ ConsoleInput.GameReference = GameReference; }

		/// <summary>
		/// Resets the colours.
		/// </summary>
		public static void ResetColours()
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
		}

        /// <summary>
        /// Renders an error message, either to the Game or directly to the console
        /// </summary>
        /// <param name="Message"></param>
        private static void renderErrorMessage(string Message)
        {
            if (ConsoleInput.GameReference != null)
            {
                ConsoleInput.GameReference.CurrentErrorMessage = Message;
                ConsoleInput.GameReference.DrawGrid();
            }
            else
            { Console.WriteLine(Message); }
         }

        /// <summary>
        /// Gets a string input from the console
        /// </summary>
        /// <param name="InputMessage">The input message to display to the user</param>
        public static string GetStringInput(string InputMessage)
        {

			while (true) 
			{

				Console.Write (InputMessage);
				string input = Console.ReadLine ();

				//  Stop?
				if (input.ToLowerInvariant ().Equals ("quit") || input.ToLowerInvariant ().Equals ("exit") || input.ToLowerInvariant ().Equals ("stop")) {
					System.Environment.Exit (0);
				}

				//  Help?
				if (input.ToLowerInvariant ().Equals ("help") || input.ToLowerInvariant ().Equals ("h") || input.ToLowerInvariant ().Equals ("?")) {
					ConsoleInput.renderHelp ();
					continue;
				}

				return input;

			}
        }

        /// <summary>
        /// Gets a number input from the console
        /// </summary>
        /// <param name="InputMessage">The input message to display to the user</param>
        public static int GetNumberValue(string InputMessage)
        {

            int value = 0;
            //  Get Width
            while (true)
            {
                string input = ConsoleInput.GetStringInput(InputMessage);

                if (!int.TryParse(input, out value))
                {
                    ConsoleInput.renderErrorMessage("Invalid entry, expected a number.  Please try again.");
                    continue;
                }

                break;

            }

            return value;

        }

        /// <summary>
        /// Gets a number input from the console
        /// </summary>
        /// <param name="InputMessage">The input message to display to the user</param>
        /// <param name="MaxValue">The maximum acceptable value</param>
        public static int GetNumberValue(string InputMessage, int MaxValue)
        {

            int value = 0;
            //  Get Width
            while (true)
            {
                string input = ConsoleInput.GetStringInput(InputMessage);

                if (!int.TryParse(input, out value))
                {
                    ConsoleInput.renderErrorMessage("Invalid entry, expected a number.  Please try again.");
                    continue;
                }

                //  Validate
                if (value > MaxValue)
                {
                    ConsoleInput.renderErrorMessage(string.Format("Invalid entry, expected a number less than {0}.  Please try again.", MaxValue));
                    break;
                }

                break;

            }

            return value;

        }

        /// <summary>
        /// Gets a number input from the console
        /// </summary>
        /// <param name="InputMessage">The input message to display to the user</param>
        /// <param name="MinValue">The minimum acceptable value</param>
        /// <param name="MaxValue">The maximum acceptable value</param>
        public static int GetNumberValue(string InputMessage, int MinValue, int MaxValue)
        {

            int value = 0;
            //  Get Width
            while (true)
            {
                string input = ConsoleInput.GetStringInput(InputMessage);

                if (!int.TryParse(input, out value))
                {
                    ConsoleInput.renderErrorMessage("Invalid entry, expected a number.  Please try again.");
                    continue;
                }

                //  Validate
                if (value < MinValue || value > MaxValue)
                {
                    ConsoleInput.renderErrorMessage(string.Format("Invalid entry, expected a number between {0} and {1}.  Please try again.", MinValue, MaxValue));
                    continue;
                }

                break;

            }

            return value;

        }

        /// <summary>
        /// Gets a coordinate input from the console
        /// </summary>
        /// <param name="InputMessage">The input message to display to the user</param>
        public static Coordinates GetCoordinateInput(string InputMessage, int MaxXValue, int MaxYValue)
        {

            int x = 0;
            int y = 0;

            //  Input
            while (true)
            {

                string input = ConsoleInput.GetStringInput(InputMessage);

                //  Read Line
                string[] inputSplit = input.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (!inputSplit.Length.Equals(2))
                {
                    ConsoleInput.renderErrorMessage("Invalid entry, expected a coordinate (x,y).  Please try again.");
                    continue;
                }

                //  Parse x
                if (!int.TryParse(inputSplit[0].Trim(), out x))
                {
                    ConsoleInput.renderErrorMessage("Invalid x value entered.  Please try again.");
                    continue;
                }

                //  Parse Y
                if (!int.TryParse(inputSplit[1].Trim(), out y))
                {
                    ConsoleInput.renderErrorMessage("Invalid y value entered.  Please try again.");
                    continue;
                }

                //  Validate X
                if (x < 0 || x > MaxXValue)
                {
                    ConsoleInput.renderErrorMessage(string.Format("Invalid entry for X, expected a number between 0 and {0}.  Please try again.", MaxXValue));
                    continue;
                }

                //  Validate Y
                if (y < 0 || y > MaxYValue)
                {
                    ConsoleInput.renderErrorMessage(string.Format("Invalid entry for Y, expected a number between 0 and {0}.  Please try again.", MaxYValue));
                    continue;
                }

                //  Break!
                break;

            }

            //  Return
            return new Coordinates(x, y);

        }

		/// <summary>
		/// Renders the help.
		/// </summary>
		private static void renderHelp()
		{

			Console.Clear();

			Console.WriteLine("Console Mine Sweeper");
			Console.WriteLine("(c) 2014 James Hetfield");
			Console.WriteLine();
			Console.WriteLine("Try to find the mines without getting blown up!");
			Console.WriteLine();

			Console.WriteLine("  Help");
			Console.WriteLine("--------");
			Console.WriteLine();
			Console.WriteLine();

			//---------------------------
			//	Coordinates
			Console.WriteLine("  Coordinates");
			Console.WriteLine("---------------");
			Console.WriteLine("Coordinates are entered in the format of \"x,y\".  For example \"10,10\"");
			Console.WriteLine();
			Console.WriteLine();

			//---------------------------
			//	Commands
			Console.WriteLine("  Commands");
			Console.WriteLine("-----------");

			Console.WriteLine("\"step on\", \"step\", \"s\"");
			Console.WriteLine("Steps on the current square");
			Console.WriteLine();

			Console.WriteLine("\"flag\", \"f\"");
			Console.WriteLine("Flags the current square as being a suspected mine");
			Console.WriteLine();

			Console.WriteLine("\"unflag\", \"u\"");
			Console.WriteLine("Unflags the current square as being a suspected mine");
			Console.WriteLine();

			Console.WriteLine("\"move\", \"m\"");
			Console.WriteLine("Moves to a new square");
			Console.WriteLine();
			Console.WriteLine();

			//---------------------------
			//	Commands
			Console.WriteLine("  General");
			Console.WriteLine("----------");

			Console.WriteLine("\"help\", \"h\", \"?\"");
			Console.WriteLine("Renders this help information");
			Console.WriteLine();

			Console.WriteLine("\"quit\", \"exit\", \"stop\"");
			Console.WriteLine("Quits the game");
			Console.WriteLine();
			Console.WriteLine();



			//---------------------------
			//	Press Any Key...
			Console.Write("Press any key to return to the game...");
			Console.ReadKey();


			//---------------------------
			//	Return to Game
			ConsoleInput.GameReference.DrawGrid();
			return;

		}

    }
}
