using System;

namespace Pentago
{
    public class GridState
    {
        private int[,] _grid = new int[6,6]
        {
            {0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0}
        };

        public bool PlacePiece(int player, int row, int column)
        {
            if (_grid[row, column] != 0)
            {
                return false;
            }

            _grid[row, column] = player;

            return true;
        }

        public override string ToString()
        {
            var output = "";
            for (int x = 0; x < 6; x += 1)
            {
                output += "[";
                for (int y = 0; y < 6; y += 1) {
                    output += $" {_grid[x,y]} ";
                }
                output += "]" + Environment.NewLine;
            }

            return output;
        }
    }
}