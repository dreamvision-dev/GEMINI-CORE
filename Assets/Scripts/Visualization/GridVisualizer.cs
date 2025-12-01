using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace GeminiCore
{
    /// <summary>
    /// Renders the state of the GridManager to a Unity Tilemap.
    /// </summary>
    public class GridVisualizer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GridManager gridManager;
        [SerializeField] private Tilemap targetTilemap;

        [Header("Tile Mappings")]
        [SerializeField] private List<TileMapping> tileMappings;
        [SerializeField] private TileBase phaseGravelIntangibleTile; // New field for intangible state

        [System.Serializable]
        public class TileMapping
        {
            public TileType tileType;
            public TileBase tileBase;
        }

        private Dictionary<TileType, TileBase> tileDictionary;

        void Awake()
        {
            // Convert the list of mappings into a dictionary for fast lookups
            tileDictionary = new Dictionary<TileType, TileBase>();
            foreach (var mapping in tileMappings)
            {
                tileDictionary[mapping.tileType] = mapping.tileBase;
            }
        }

        void Start()
        {
            if (gridManager == null || targetTilemap == null)
            {
                Debug.LogError("GridVisualizer is missing references!");
                return;
            }

            DrawFullGrid();
        }

        /// <summary>
        /// Draws the entire grid based on the current state of the GridManager.
        /// </summary>
        public void DrawFullGrid()
        {
            targetTilemap.ClearAllTiles();
            
            int gridWidth = gridManager.GridWidth;
            int gridHeight = gridManager.GridHeight;

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    DrawTile(x, y);
                }
            }
        }

        /// <summary>
        /// Draws a single tile at the specified coordinates.
        /// </summary>
        public void DrawTile(int x, int y)
        {
            TileType tileType = gridManager.GetTileAt(x, y);
            TileBase tileToDraw = null;

            if (tileType == TileType.PhaseGravel)
            {
                StateScheduler stateScheduler = FindObjectOfType<StateScheduler>();
                if (stateScheduler != null && !stateScheduler.IsPhaseGravelSolid)
                {
                    tileToDraw = phaseGravelIntangibleTile; // Use intangible tile if not solid
                }
                else
                {
                    tileDictionary.TryGetValue(tileType, out tileToDraw); // Use solid tile
                }
            }
            else
            {
                tileDictionary.TryGetValue(tileType, out tileToDraw);
            }
            
            targetTilemap.SetTile(new Vector3Int(x, y, 0), tileToDraw);
        }
    }
}
