using UnityEngine;

namespace GeminiCore
{
    /// <summary>
    /// Maps keyboard/gamepad input to player movements and item usage.
    /// Ensures smooth, responsive 8-directional movement that snaps to the grid.
    /// </summary>
    public class InputController : MonoBehaviour
    {
        [SerializeField] private VoidDiver voidDiver; // Reference to the player character
        [SerializeField] private GridManager gridManager; // Reference to the GridManager

        [Header("Movement Settings")]
        [SerializeField] private float moveDelay = 0.15f; // Delay between movements
        private float lastMoveTime;

        void Update()
        {
            HandleMovementInput();
            HandleActionInput();
        }

        private void HandleMovementInput()
        {
            if (Time.time - lastMoveTime < moveDelay)
            {
                return; // Prevent rapid movement
            }

            int moveX = 0;
            int moveY = 0;

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                moveY = 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                moveY = -1;
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                moveX = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                moveX = 1;
            }

                        if (moveX != 0 || moveY != 0)

                        {

                            // Set animation parameters

                            if (voidDiver.animator != null)

                            {

                                voidDiver.animator.SetBool("isMoving", true);

                                voidDiver.animator.SetFloat("moveX", moveX);

                            }

            

                            // Calculate new position

                            int newX = voidDiver.x + moveX;

                            int newY = voidDiver.y + moveY;

            

                            // Check if the new position is within bounds and not a solid tile

                            if (!gridManager.IsOutOfBounds(newX, newY) && gridManager.GetTileAt(newX, newY) != TileType.SolidRock)

                            {

                                // If the tile is SoftEarth, dig it

                                if (gridManager.GetTileAt(newX, newY) == TileType.SoftEarth)

                                {

                                    gridManager.SetTileAt(newX, newY, TileType.Empty);

                                    // Update visualizer

                                    GridVisualizer gridVisualizer = FindObjectOfType<GridVisualizer>();

                                    gridVisualizer?.DrawTile(newX, newY);

                                }

                                

                                // If the tile is Empty, was dug, or is a collectible/portal, move the player

                                if (gridManager.GetTileAt(newX, newY) == TileType.Empty || 

                                    gridManager.GetTileAt(newX, newY) == TileType.AetherShard ||

                                    gridManager.GetTileAt(newX, newY) == TileType.ExitPortal)

                                {

                                    // Check for Aether Shard collection

                                    if (gridManager.GetTileAt(newX, newY) == TileType.AetherShard)

                                    {

                                        Debug.Log("Aether Shard collected!");

                                        GameManager gameManager = FindObjectOfType<GameManager>();

                                        gameManager?.CollectAetherShard();

                                    }

                                    // Check for entering Exit Portal

                                    else if (gridManager.GetTileAt(newX, newY) == TileType.ExitPortal)

                                    {

                                        Debug.Log("Entered Exit Portal! Loading next level...");

                                        // For now, just restart the game to simulate next level

                                        UIManager.Instance?.RestartGame();

                                        return; // Stop further processing this frame

                                    }

            

                                    // Clear the tile at the new position (if it was a collectible, it's now collected)

                                    gridManager.SetTileAt(newX, newY, TileType.Empty);

                                    GridVisualizer gridVisualizer = FindObjectOfType<GridVisualizer>();

                                    gridVisualizer?.DrawTile(newX, newY);

            

                                    // Update player's grid position

                                    voidDiver.SetGridPosition(newX, newY);

                                    Debug.Log($"Player moved to: ({voidDiver.x}, {voidDiver.y})");

                                }

                                else

                                {

                                    Debug.Log("Player blocked by: " + gridManager.GetTileAt(newX, newY));

                                }

                            }

                            else

                            {

                                Debug.Log("Player tried to move out of bounds or into SolidRock.");

                            }

            

                            lastMoveTime = Time.time;

                        }

                        else

                        {

                            // If no movement input, set isMoving to false

                            if (voidDiver != null && voidDiver.animator != null)

                            {

                                voidDiver.animator.SetBool("isMoving", false);

                            }

                        }
        }

        private void HandleActionInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // TODO: Implement action (e.g., use Quantum Bomb, dig)
                Debug.Log("Action button pressed (Space)");

                if (voidDiver != null && voidDiver.quantumBombs > 0)
                {
                    Debug.Log("Using Quantum Bomb!");
                    voidDiver.quantumBombs--; // Decrement bomb count

                    StateScheduler stateScheduler = FindObjectOfType<StateScheduler>();
                    if (stateScheduler != null)
                    {
                        stateScheduler.ActivateQuantumBomb(3.0f); // Activate for 3 seconds
                    }
                }
                else
                {
                    Debug.Log("No Quantum Bombs left or VoidDiver not found.");
                }
            }
        }
    }
}
