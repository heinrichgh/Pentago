using System;

namespace Pentago
{
    public class GridState
    {
        private int[,] _grid = new int[6, 6]
        {
            {0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0}
        };

//        private int[,] _grid = new int[6,6]
//        {
//            {0, 2, 3, 4, 5, 6},
//            {7, 8, 9, 10, 11, 12},
//            {13, 14, 15, 16, 17, 18},
//            {0, 0, 0, 0, 0, 0},
//            {0, 0, 0, 0, 0, 0},
//            {0, 0, 0, 0, 0, 0}
//        };

        public bool PlacePiece(int player, int row, int column)
        {
            if (!IsPlaceEmpty(row, column))
            {
                return false;
            }

            _grid[row, column] = player;

            return true;
        }

        public bool IsPlaceEmpty(int row, int column)
        {
            return _grid[row, column] == 0;
        }

        public override string ToString()
        {
            var output = "";
            for (int row = 0; row < 6; row++)
            {
                output += "[";
                for (int col = 0; col < 6; col++)
                {
                    output += $" {_grid[row, col]} ";
                }

                output += "]" + Environment.NewLine;
            }

            return output;
        }

        public void Rotate(Rotation rotation, int row, int column)
        {
            if (row <= 2 && column <= 2)
            {
                RotateTopLeft(rotation);
                return;
            }

            if (row <= 2 && column > 2)
            {
                RotateTopRight(rotation);
                return;
            }

            if (row > 2 && column <= 2)
            {
                RotateBottomLeft(rotation);
                return;
            }

            if (row > 2 && column > 2)
            {
                RotateBottomRight(rotation);
                return;
            }
        }

        // TODO: Look at optimizing these checks. Though they are running on a very small set.
        public int CheckWinConditions()
        {
            var playerWin = CheckWinHorizontal();
            if (playerWin != 0)
            {
                return playerWin;
            }

            playerWin = CheckWinVertical();
            if (playerWin != 0)
            {
                return playerWin;
            }

            playerWin = CheckWinDiagonalDown();
            if (playerWin != 0)
            {
                return playerWin;
            }

            playerWin = CheckWinDiagonalUp();
            if (playerWin != 0)
            {
                return playerWin;
            }

            playerWin = CheckDraw();
            if (playerWin != 0)
            {
                return playerWin;
            }

            return 0;
        }

        private int CheckWinHorizontal()
        {
            var count = 0;
            var player = 0;

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    var piece = _grid[row, col];
                    if (CountForWin(piece, ref count, ref player))
                    {
                        return player;
                    }
                }

                count = 0;
                player = 0;
            }

            return 0;
        }

        private int CheckWinVertical()
        {
            var count = 0;
            var player = 0;

            for (int col = 0; col < 6; col++)
            {
                for (int row = 0; row < 6; row++)
                {
                    var piece = _grid[row, col];
                    if (CountForWin(piece, ref count, ref player))
                    {
                        return player;
                    }
                }

                count = 0;
                player = 0;
            }

            return 0;
        }

        private int CheckWinDiagonalDown()
        {
            var count = 0;
            var player = 0;

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var rowI = row + i;
                        var colI = col + i;

                        if (rowI > 5 || colI > 5)
                        {
                            break;
                        }
                        
                        var piece = _grid[rowI, colI];
                        if (CountForWin(piece, ref count, ref player))
                        {
                            return player;
                        }
                    }

                    count = 0;
                    player = 0;
                }
            }

            return 0;
        }

        private int CheckWinDiagonalUp()
        {
            var count = 0;
            var player = 0;

            for (int row = 5; row >= 0; row--)
            {
                for (int col = 0; col < 6; col++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var rowI = row - i;
                        var colI = col + i;

                        if (rowI < 0 || colI > 5)
                        {
                            break;
                        }
                        
                        var piece = _grid[rowI, colI];
                        if (CountForWin(piece, ref count, ref player))
                        {
                            return player;
                        }
                    }

                    count = 0;
                    player = 0;
                }
            }

            return 0;
        }

        private bool CountForWin(int piece, ref int count, ref int player)
        {
            if (piece == 0)
            {
                count = 0;
                return false;
            }
            
            if (count == 0)
            {
                count = 1;
                player = piece;
            }
            else if (piece == player)
            {
                count++;

                if (count == 5)
                {
                    return true;
                }
            }
            else if (piece != player)
            {
                player = piece;
                count = 1;
            }

            return false;
        }

        private int CheckDraw()
        {
            var count = 0;

            for (int col = 0; col < 6; col++)
            {
                for (int row = 0; row < 6; row++)
                {
                    var piece = _grid[row, col];
                    count += piece != 0 ? 1 : 0;
                }
            }

            if (count == 36)
            {
                return -1;
            }
            
            return 0;
        }

        private void RotateTopLeft(Rotation rotation)
        {
            GridSectionRotation(rotation, 0, 0);
        }

        private void RotateTopRight(Rotation rotation)
        {
            GridSectionRotation(rotation, 0, 3);
        }

        private void RotateBottomLeft(Rotation rotation)
        {
            GridSectionRotation(rotation, 3, 0);
        }

        private void RotateBottomRight(Rotation rotation)
        {
            GridSectionRotation(rotation, 3, 3);
        }

        private void GridSectionRotation(Rotation rotation, int rowStart, int colStart)
        {
            if (rotation == Rotation.Clockwise)
            {
                RotateClockwise(rowStart, colStart);
            }
            else if (rotation == Rotation.AntiClockwise)
            {
                RotateAntiClockwise(rowStart, colStart);
            }
        }

        private void RotateAntiClockwise(int rowStart, int colStart)
        {
            for (var i = 0; i < 2; i++)
            {
                var temp = _grid[rowStart, colStart + i];
                _grid[rowStart, colStart + i] = _grid[rowStart + i, colStart + 2];
                _grid[rowStart + i, colStart + 2] = _grid[rowStart + 2, colStart + 2 - i];
                _grid[rowStart + 2, colStart + 2 - i] = _grid[rowStart + 2 - i, colStart];
                _grid[rowStart + 2 - i, colStart] = temp;
            }
        }

        private void RotateClockwise(int rowStart, int colStart)
        {
            for (var i = 0; i < 2; i++)
            {
                var temp = _grid[rowStart, colStart + i];
                _grid[rowStart, colStart + i] = _grid[rowStart + 2 - i, colStart];
                _grid[rowStart + 2 - i, colStart] = _grid[rowStart + 2, colStart + 2 - i];
                _grid[rowStart + 2, colStart + 2 - i] = _grid[rowStart + i, colStart + 2];
                _grid[rowStart + i, colStart + 2] = temp;
            }
        }
    }
}