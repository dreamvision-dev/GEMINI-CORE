using UnityEngine;

namespace GeminiCore
{
    /// <summary>
    /// Handles all physics-based calculations, including gravity,
    /// rockfalls, and rolling mechanics.
    /// </summary>
    public class PhysicsHandler : MonoBehaviour
    {
        [SerializeField] private GridManager gridManager;

        void FixedUpdate()
        {
            // Physics calculations should run in FixedUpdate for consistency
            ProcessPhysicsTick();
        }

        /// <summary>
        /// Iterates over the grid and applies physics rules for one tick.
        /// This is where rock falling, rolling, and other movements are calculated.
        /// </summary>
        private void ProcessPhysicsTick()
        {
            if (gridManager == null)
            {
                Debug.LogError("GridManager not assigned to PhysicsHandler.");
                return;
            }

            // Get grid dimensions
            int gridWidth = gridManager.GridWidth;
            int gridHeight = gridManager.GridHeight;

            // Get reference to StateScheduler
            StateScheduler stateScheduler = FindObjectOfType<StateScheduler>();
            bool isPhaseGravelSolid = (stateScheduler != null) ? stateScheduler.IsPhaseGravelSolid : true; // Default to solid if not found
            bool isGravityReversed = (stateScheduler != null) ? stateScheduler.IsGravityReversed : false; // Default to normal gravity

            // Determine the direction of gravity
            int gravityDirection = isGravityReversed ? 1 : -1; // 1 for up, -1 for down

            // Iterate from bottom-up (or top-down if gravity reversed)
            // This iteration order needs to be dynamic based on gravity direction.
            // If gravity is normal (-1), iterate from y=0 upwards.
            // If gravity is reversed (1), iterate from y=gridHeight-1 downwards.
            int startY = isGravityReversed ? gridHeight - 1 : 0;
            int endY = isGravityReversed ? -1 : gridHeight;
            int stepY = isGravityReversed ? -1 : 1;

            for (int y = startY; y != endY; y += stepY)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    TileType currentTile = gridManager.GetTileAt(x, y);

                    // Check for CorruptedMineral
                    if (currentTile == TileType.CorruptedMineral)
                    {
                        // Check if the tile in gravity direction is empty or intangible PhaseGravel (falling)
                        TileType tileInGravityDirection = gridManager.GetTileAt(x, y + gravityDirection);
                        bool canFall = (tileInGravityDirection == TileType.Empty || (tileInGravityDirection == TileType.PhaseGravel && !isPhaseGravelSolid));

                        if (canFall)
                        {
                            // Make it fall
                            gridManager.SetTileAt(x, y + gravityDirection, TileType.CorruptedMineral);
                            gridManager.SetTileAt(x, y, TileType.Empty);

                            // Update visualizer
                            GridVisualizer gridVisualizer = FindObjectOfType<GridVisualizer>();
                            if (gridVisualizer != null)
                            {
                                gridVisualizer.DrawTile(x, y);                 // Clear old position
                                gridVisualizer.DrawTile(x, y + gravityDirection); // Draw new position
                            }
                        }
                        else // Cannot fall straight down, check for rolling
                        {
                            bool canRollLeft = false;
                            bool canRollRight = false;

                            // Check if tile to the left/right and diagonally in gravity direction are empty or intangible PhaseGravel
                            // Rolling direction is relative to gravity.
                            // If gravity is down (-1), roll into (x-1, y-1) or (x+1, y-1)
                            // If gravity is up (1), roll into (x-1, y+1) or (x+1, y+1)

                            // Check left roll
                            TileType tileSideLeft = gridManager.GetTileAt(x - 1, y);
                            TileType tileDiagonalLeft = gridManager.GetTileAt(x - 1, y + gravityDirection);

                            bool canRollLeftThroughPhaseGravel = (tileSideLeft == TileType.PhaseGravel && !isPhaseGravelSolid) &&
                                                                 (tileDiagonalLeft == TileType.PhaseGravel && !isPhaseGravelSolid);

                            if (!gridManager.IsOutOfBounds(x - 1, y) && (tileSideLeft == TileType.Empty || canRollLeftThroughPhaseGravel) &&
                                !gridManager.IsOutOfBounds(x - 1, y + gravityDirection) && (tileDiagonalLeft == TileType.Empty || canRollLeftThroughPhaseGravel))
                            {
                                canRollLeft = true;
                            }

                            // Check right roll
                            TileType tileSideRight = gridManager.GetTileAt(x + 1, y);
                            TileType tileDiagonalRight = gridManager.GetTileAt(x + 1, y + gravityDirection);

                            bool canRollRightThroughPhaseGravel = (tileSideRight == TileType.PhaseGravel && !isPhaseGravelSolid) &&
                                                                  (tileDiagonalRight == TileType.PhaseGravel && !isPhaseGravelSolid);

                            if (!gridManager.IsOutOfBounds(x + 1, y) && (tileSideRight == TileType.Empty || canRollRightThroughPhaseGravel) &&
                                !gridManager.IsOutOfBounds(x + 1, y + gravityDirection) && (tileDiagonalRight == TileType.Empty || canRollRightThroughPhaseGravel))
                            {
                                canRollRight = true;
                            }

                            // Prioritize a random direction if both are possible, or just pick one
                            if (canRollLeft && canRollRight)
                            {
                                if (Random.Range(0, 2) == 0)
                                {
                                    canRollRight = false;
                                }
                                else
                                {
                                    canRollLeft = false;
                                }
                            }

                            if (canRollLeft)
                            {
                                gridManager.SetTileAt(x - 1, y + gravityDirection, TileType.CorruptedMineral);
                                gridManager.SetTileAt(x, y, TileType.Empty);

                                GridVisualizer gridVisualizer = FindObjectOfType<GridVisualizer>();
                                if (gridVisualizer != null)
                                {
                                    gridVisualizer.DrawTile(x, y);                     // Clear old position
                                    gridVisualizer.DrawTile(x - 1, y + gravityDirection); // Draw new position
                                }
                            }
                            else if (canRollRight)
                            {
                                gridManager.SetTileAt(x + 1, y + gravityDirection, TileType.CorruptedMineral);
                                gridManager.SetTileAt(x, y, TileType.Empty);

                                GridVisualizer gridVisualizer = FindObjectOfType<GridVisualizer>();
                                if (gridVisualizer != null)
                                {
                                    gridVisualizer.DrawTile(x, y);                     // Clear old position
                                    gridVisualizer.DrawTile(x + 1, y + gravityDirection); // Draw new position
                                }
                            }
                        }
                    }
                }
            }

            // After all physics movements, check for player-enemy collisions
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null && gameManager.currentVoidDiver != null)
            {
                foreach (PlasmaWaspController wasp in gameManager.activePlasmaWasps)
                {
                    if (wasp != null && wasp.x == gameManager.currentVoidDiver.x && wasp.y == gameManager.currentVoidDiver.y)
                    {
                        Debug.LogError("Player collided with Plasma Wasp! Game Over!");
                        // Show the game over screen
                        if (UIManager.Instance != null)
                        {
                            UIManager.Instance.ShowGameOverScreen();
                        }
                        // Disable the player
                        gameManager.currentVoidDiver.gameObject.SetActive(false);
                        break; // Only need to detect one collision
                    }
                }
            }
        }
    }
}
