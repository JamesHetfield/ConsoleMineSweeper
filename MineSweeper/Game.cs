using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class Game
    {

        public string CurrentErrorMessage { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public GameSquare[,] GameSquares { get; private set; }
		public GameSquare CurrentGameSquare { get; private set; }
		public Coordinates CurrentCoords { get; private set; }
		public GameSquare[] Mines { get; private set; }
		public List<GameSquare> FlaggedSquares { get; private set; }

		/// <summary>
		/// Gets a value indicating whether all mines are flagged
		/// </summary>
		/// <value><c>true</c> if all mines flagged; otherwise, <c>false</c>.</value>
		public bool AllMinesFlagged 
		{ 
			get 
			{
				foreach (GameSquare mine in this.Mines) 
				{ if (!mine.Flagged) { return false; } }
				return true;
			}
		}

        public Game(int Width, int Height)
        {

            //---------------------------
            //  Width/Height
            this.Width = Width;
            this.Height = Height;

            //---------------------------
            //  Mines
			List<GameSquare> mines = new List<GameSquare>();
            this.GameSquares = new GameSquare[Width, Height];
            Random rand = new Random();
            for (int i = 1; i <= Math.Round(((decimal)Width * (decimal)Height) / (decimal)10, MidpointRounding.AwayFromZero); i++)
            {

                int xRand;
                int yRand;

                while (true)
                {
                    xRand = rand.Next(0, this.Width - 1);
                    yRand = rand.Next(0, this.Height - 1);
                    if (this.GameSquares[xRand, yRand] != null)
                    { continue; }
                    this.GameSquares[xRand, yRand] = new GameSquare(xRand, yRand, true);
					mines.Add (this.GameSquares [xRand, yRand]);
                    break;

                }
            }
			this.Mines = mines.ToArray();
			mines = null;

            //---------------------------
            //  Generate Squares
			this.FlaggedSquares = new List<GameSquare>();
            for (int w = 0; w < this.Width; w++)
            {
                for (int h = 0; h < this.Height; h++)
                {
                    if (this.GameSquares[w, h] == null)
                    {this.GameSquares[w, h] = new GameSquare(w, h);}
                }
            }

			//---------------------------
			//  Calculate Neightbours
            foreach(GameSquare square in this.GameSquares)
            { square.CalculateNeightbours(this); }

        }

        public void Play()
        {

            while (true)
            {

                //---------------------------
                //  Draw Grid
                this.DrawGrid();

                //  Get Input Coords
				this.CurrentCoords = ConsoleInput.GetCoordinateInput("Input Co-ords (x,y): ", this.Width, this.Height);
				this.CurrentGameSquare = this.GameSquares[this.CurrentCoords.X - 1, this.CurrentCoords.Y - 1];

				//  Cleared Already?
				if (this.CurrentGameSquare.Cleared)
                {
					this.CurrentErrorMessage = string.Format("Square {0},{1} has already been cleared.", this.CurrentCoords.X, this.CurrentCoords.Y);
                    continue;
                }

				while (true) 
				{

					//---------------------------
					//  Draw Grid
					this.DrawGrid();

					//	Get Command
					string command = ConsoleInput.GetStringInput ("Enter Command: ");

					switch (command) 
					{
						
						case "s":
						case "step":
						case "step on":

							//  Is Mine?
							if (this.CurrentGameSquare.IsMine)
							{
								this.DrawGrid (true);
								Console.WriteLine("*KABOOOM!*");
								Console.WriteLine("You're dead!!");
								return;

							}

							this.CurrentGameSquare.StepOn();
							continue;

						case "f":
						case "flag":
							this.CurrentGameSquare.SetFlaggedStatus (true);
							this.FlaggedSquares.Add(this.CurrentGameSquare);

							//	All Flagged?
							if (this.AllMinesFlagged) 
							{ 
								this.Win (); 
								return;
							}

							continue;

						case "u":
						case "unflag":
							this.CurrentGameSquare.SetFlaggedStatus (false);
							this.FlaggedSquares.Remove(this.CurrentGameSquare);
							continue;

						case "m":
						case "move":
							//	Do Nothing
							break;

						default:
							this.CurrentErrorMessage = string.Format("Invalid Command: {0}", command);
							continue;

					}

					//	Break
					break;

				}

            }


        }

		/// <summary>
		/// Draws the game grid
		/// </summary>
		public void DrawGrid()
		{ 
			this.DrawGrid (false); 
		}

        /// <summary>
        /// Draws the game grid
        /// </summary>
		/// <param name="DrawMines">Specifies whether or not to draw the mine positions</param>
		public void DrawGrid(bool DrawMines)
        {

			int cx = 0;
			int cy = 0;

			//---------------------------
			//  Reset and Clear the Console
			ConsoleInput.ResetColours();
            Console.Clear();

			//---------------------------
            //  Get Console Lines
            List<string> consoleLine = new List<string>(this.Height);
            for (int y = 0; y < this.Height; y++)
            {
                string[] charArray = new string[this.Width];
                for (int x = 0; x < this.Width; x++)
                {
                    GameSquare square = this.GameSquares[x,y];
                    charArray[x] = square.getRenderCharacter();
                }
                consoleLine.Add(string.Join("", charArray));
            }

			//---------------------------
            //  Draw the grid
			Console.ForegroundColor = ConsoleColor.Gray;
            foreach (string line in consoleLine)
			{ Console.WriteLine(line); }

			//---------------------------
			//  Write a Line
            Console.WriteLine();

			//---------------------------
            //  Error Message
			if (!string.IsNullOrWhiteSpace (this.CurrentErrorMessage)) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine (this.CurrentErrorMessage);
				this.CurrentErrorMessage = string.Empty;
				ConsoleInput.ResetColours();
			} 
			else 
			{ Console.WriteLine(); }

			//---------------------------
			//  Current Game Square
			if (this.CurrentGameSquare != null)
			{ 

				//	Print Coords
				Console.WriteLine (string.Format("Current Coordinates: {0}, {1}", this.CurrentGameSquare.X + 1, this.CurrentGameSquare.Y + 1)); 

				//	Get Current Cursor Position
				cx = Console.CursorLeft;
				cy = Console.CursorTop;

				//	Render Current Square Char
				Console.SetCursorPosition(this.CurrentGameSquare.X, this.CurrentGameSquare.Y);
				Console.ForegroundColor = ConsoleColor.Red;

				string renderChar = this.CurrentGameSquare.getRenderCharacter();
				if (renderChar == " " || renderChar == ".") { renderChar = "+"; }

				Console.Write(renderChar);

				//	Reset Console
				ConsoleInput.ResetColours();
				Console.SetCursorPosition(cx, cy);

			}
			else 
			{ Console.WriteLine(); }

			//---------------------------
			//  Render Flagged
			//	Get Current Cursor Position
			cx = Console.CursorLeft;
			cy = Console.CursorTop;


			foreach (GameSquare flaggedSquare in this.FlaggedSquares) 
			{
				Console.ForegroundColor = (flaggedSquare == this.CurrentGameSquare ? ConsoleColor.Red : ConsoleColor.Green);
				Console.SetCursorPosition(flaggedSquare.X, flaggedSquare.Y);
				Console.Write("?");
			}

			//	Reset Console
			ConsoleInput.ResetColours();
			Console.SetCursorPosition(cx, cy);


			//---------------------------
			//  Render Mines?
			if (DrawMines) 
			{

				//	Get Current Cursor Position
				cx = Console.CursorLeft;
				cy = Console.CursorTop;

				Console.ForegroundColor = ConsoleColor.Red;
				foreach (GameSquare mine in this.Mines) 
				{
					Console.SetCursorPosition(mine.X, mine.Y);
					Console.Write("*");
				}

				//	Reset Console
				ConsoleInput.ResetColours();
				Console.SetCursorPosition(cx, cy);
			}

			//---------------------------
            //  Write a Line
            Console.WriteLine();

        }

		/// <summary>
		/// Executes when the player wins (all mines are flagged)
		/// </summary>
		public void Win()
		{
			this.DrawGrid (true);
			Console.WriteLine("Congratulations!");
			Console.WriteLine("You found all of the mines.");
		}

    }
}
