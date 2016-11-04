#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace KillTheSnowman
{
    public static class GridManager
    {
        static int horizonalTiles;
        static int verticalTiles;
        static float tileWidth;
        static float tileHeight;

        static List<List<List<Player>>> playerGrid;

        public static void Initialize(int x, int y)
        {
            horizonalTiles = x;
            verticalTiles = y;
            playerGrid = new List<List<List<Player>>>(horizonalTiles);
            for (int i = 0; i < playerGrid.Capacity; i++)
            {
                List<List<Player>> list = new List<List<Player>>(verticalTiles);
                for (int j = 0; j < list.Capacity; j++)
                {
                    list.Add(new List<Player>());
                }
                playerGrid.Add(list);
            }
            tileWidth = Game1.WINDOW_WIDTH / horizonalTiles;
            tileHeight = Game1.WINDOW_HEIGHT / verticalTiles;
        }

        private static Vector2 getGridCoordinates(Vector2 atLocation)
        {
            if (atLocation.X < 0 ||
                atLocation.X > Game1.WINDOW_WIDTH ||
                atLocation.Y < 0 ||
                atLocation.Y > Game1.WINDOW_HEIGHT)
            {
                return new Vector2(2 * horizonalTiles, 2 * verticalTiles);
            }
            
            Vector2 result = Vector2.Zero;

            while (atLocation.X > result.X * tileWidth)
            {
                result.X++;
            }
            while (atLocation.Y > result.Y * tileHeight)
            {
                result.Y++;
            }

            result.X -= result.X > 0 ? 1 : 0;
            result.Y -= result.Y > 0 ? 1 : 0;

            return result;
        }

        public static List<Player> GridTile(Vector2 atLocation)
        {
            Vector2 coordinates = getGridCoordinates(atLocation);
            if (coordinates.X == 2 * horizonalTiles && coordinates.Y == 2 * verticalTiles)
            {
                return new List<Player>();
            }
            int x = (int)coordinates.X;
            int y = (int)coordinates.Y;
            return playerGrid[x][y];
        }

        public static List<Player> AreaAroundGrid(Vector2 atLocation)
        {
            Vector2 coordinates = getGridCoordinates(atLocation);
            if (coordinates.X == 2 * horizonalTiles && coordinates.Y == 2 * verticalTiles)
            {
                return new List<Player>();
            }
            int x = (int)coordinates.X;
            int y = (int)coordinates.Y;

            List<Player> result = new List<Player>();
            result.AddRange(playerGrid[x][y]);
            if (y - 1 >= 0)
            {
                result.AddRange(playerGrid[x][y - 1]);
            }
            if (y + 1 < verticalTiles)
            {
                result.AddRange(playerGrid[x][y + 1]);
            }
            if (x - 1 >= 0)
            {
                result.AddRange(playerGrid[x - 1][y]);
                if (y - 1 >= 0)
                {
                    result.AddRange(playerGrid[x - 1][y - 1]);
                }
                if (y + 1 < verticalTiles)
                {
                    result.AddRange(playerGrid[x - 1][y + 1]);
                }
            }
            if (x + 1 < horizonalTiles)
            {
                result.AddRange(playerGrid[x + 1][y]);
                if (y - 1 >= 0)
                {
                    result.AddRange(playerGrid[x + 1][y - 1]);
                }
                if (y + 1 < verticalTiles)
                {
                    result.AddRange(playerGrid[x + 1][y + 1]);
                }
            }

            return result;
        }

        /*
        public void UpdateTile(Player withPlayer, Vector2 atLocation)
        {

        }
         * */
    }
}
