using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class GameSquare
    {

        public int X { get; private set; }

        public int Y { get; private set; }

		public bool IsMine { get; private set; }

		public bool Cleared { get; private set; }

		public bool Flagged { get; private set; }

        public bool ShowBombNeighbours { get; private set; }

        public int BombNeighbours { get; private set; }

        public GameSquare[] Neighbours { get; private set; }

        /// <summary>
        /// Create a new Game Square
        /// </summary>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        public GameSquare(int x, int y, bool IsMine): this(x,y)
        {
            //  Is Mine
            this.IsMine = IsMine;

        }

        /// <summary>
        /// Create a new Game Square
        /// </summary>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        public GameSquare(int x, int y)
        {

            this.Cleared = false;
            this.ShowBombNeighbours = false;
            this.X = x;
            this.Y = y;

        }

        /// <summary>
        /// Calculates which other squares are neighbours for this square
        /// </summary>
        /// <param name="game"></param>
        public void CalculateNeightbours(Game game)
        {

            this.BombNeighbours = 0;
            List<GameSquare> neighbours = new List<GameSquare>();
            for (int x = this.X - 1; x <= this.X + 1; x++)
            {
                if (x < 0 || x > game.Width-1) { continue; }
                for (int y = this.Y - 1; y <= this.Y + 1; y++)
                {
                    if (y < 0 || y > game.Height - 1) { continue; }
                    if (x == this.X && y == this.Y) { continue; }
                    neighbours.Add(game.GameSquares[x, y]);
                }
            }

            foreach (GameSquare square in neighbours)
            { if (square.IsMine) { this.BombNeighbours++; } }
            this.Neighbours = neighbours.ToArray();

        }

        /// <summary>
        /// Steps on this square
        /// </summary>
        public void StepOn()
        {

            if (!this.BombNeighbours.Equals(0))
            { this.ShowBombNeighbours = true; }
            else
            {

				this.Cleared = true;

                //  Iterate Neightbours
                foreach (GameSquare square in this.Neighbours)
                {
                    if (!square.Cleared)
                    {
                        if (square.BombNeighbours.Equals(0))
                        { square.StepOn(); }
                        else
                        {
                            square.ShowBombNeighbours = true;
                        }
                    }
                }

            }
		}

		/// <summary>
		/// Sets the flagged status.
		/// </summary>
		/// <param name="Flagged">If set to <c>true</c> flagged.</param>
		public void SetFlaggedStatus(bool Flagged)
		{
			this.Flagged = Flagged;
		}

        /// <summary>
        /// Gets the character that should be rendered for this square
        /// </summary>
        /// <returns>A character that renders the state of the square</returns>
        public string getRenderCharacter()
        {

            //if (this.IsMine) { return "*"; }

			if (this.Flagged) { return "?"; }
            if (!this.Cleared && !this.ShowBombNeighbours)
            { return "."; }
            else
            {
                if (this.BombNeighbours.Equals(0))
                { return " "; }
                else
                { return this.ShowBombNeighbours ?  this.BombNeighbours.ToString() : " "; }
            }

         }

    }
}
