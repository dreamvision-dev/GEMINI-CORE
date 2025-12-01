using UnityEngine;

namespace GeminiCore
{
    /// <summary>
    /// The central authority for the game map.
    /// Manages the 2D array of cells and handles tile property lookups.
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Dimensions")]
        [SerializeField] private int gridWidth = 32;
        [SerializeField] private int gridHeight = 32;

        public int GridWidth => gridWidth;
        public int GridHeight => gridHeight;

        private TileType[,] grid;

        void Awake()
        {
            InitializeGrid();
        }

        /// <summary>
        /// Creates the initial grid and populates it with empty tiles.
        /// </summary>
        private void InitializeGrid()
        {
            grid = new TileType[gridWidth, gridHeight];

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    grid[x, y] = TileType.Empty;
                }
            }

            Debug.Log("Grid initialized with dimensions: " + gridWidth + "x" + gridHeight);
        }

        /// <summary>
        /// Gets the type of tile at a specific grid coordinate.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns>The TileType at the given coordinates.</returns>
        public TileType GetTileAt(int x, int y)
        {
            if (IsOutOfBounds(x, y))
            {
                Debug.LogWarning($"Coordinates ({x},{y}) are out of bounds.");
                return TileType.SolidRock; // Treat out of bounds as a solid wall
            }
            return grid[x, y];
        }

        /// <summary>
        /// Sets the type of tile at a specific grid coordinate.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="tileType">The new TileType to set.</param>
        public void SetTileAt(int x, int y, TileType tileType)
        {
            if (IsOutOfBounds(x, y))
            {
                Debug.LogWarning($"Cannot set tile. Coordinates ({x},{y}) are out of bounds.");
                return;
            }
            grid[x, y] = tileType;
        }

        /// <summary>
        /// Checks if a given coordinate is outside the grid boundaries.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns>True if the coordinates are out of bounds, false otherwise.</returns>
        public bool IsOutOfBounds(int x, int y)
        {
            return x < 0 || x >= gridWidth || y < 0 || y >= gridHeight;
        }
    }
}
