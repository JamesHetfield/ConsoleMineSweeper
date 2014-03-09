using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{

    /// <summary>
    /// Represents coordinate in the game
    /// </summary>
    public class Coordinates
    {

        /// <summary>
        /// Intanciates a new Coordinate object with default position data: 0,0
        /// </summary>
        public Coordinates()
        {
            this.X = 0;
            this.Y = 0;
        }

        /// <summary>
        /// Instanciates a new Coordinate object with specified position data
        /// </summary>
        /// <param name="X">The X position</param>
        /// <param name="Y">The Y position</param>
        public Coordinates(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        /// <summary>
        /// X (horizontal) postition
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y (vertical) position
        /// </summary>
        public int Y { get; set; }

    }
}
