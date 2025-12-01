using UnityEngine;
using System.Collections.Generic;

namespace GeminiCore
{
    /// <summary>
    /// Controls the movement and AI of a Plasma Wasp enemy.
    /// Follows open tunnels, avoiding solid objects.
    /// </summary>
    public class PlasmaWaspController : MonoBehaviour
    {
        [SerializeField] private GridManager gridManager;
        [SerializeField] private float moveDelay = 0.5f; // How often the wasp moves
        private float lastMoveTime;

        public int x;
        public int y;

        private Vector2Int currentDirection = Vector2Int.right; // Start moving right

        void Start()
        {
            // Initialize position (GameManager will set this)
            transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
        }

        void Update()
        {
            if (Time.time - lastMoveTime > moveDelay)
            {
                MoveWasp();
                lastMoveTime = Time.time;
            }
        }

        private void MoveWasp()
        {
            if (gridManager == null) return;

            Vector2Int nextPosition = new Vector2Int(x + currentDirection.x, y + currentDirection.y);

            // Check if next position is valid (within bounds and empty/soft earth)
            if (!gridManager.IsOutOfBounds(nextPosition.x, nextPosition.y) &&
                (gridManager.GetTileAt(nextPosition.x, nextPosition.y) == TileType.Empty ||
                 gridManager.GetTileAt(nextPosition.x, nextPosition.y) == TileType.SoftEarth ||
                 gridManager.GetTileAt(nextPosition.x, nextPosition.y) == TileType.AetherShard)) // Wasps can fly over shards
            {
                // Move to next position
                x = nextPosition.x;
                y = nextPosition.y;
                transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
            }
            else
            {
                // Hit an obstacle, try to change direction
                ChangeDirection();
            }
        }

        private void ChangeDirection()
        {
            // Simple maze-following: try turning right, then left, then reverse
            Vector2Int[] possibleDirections = new Vector2Int[]
            {
                RotateVector(currentDirection, 90),  // Turn right
                RotateVector(currentDirection, -90), // Turn left
                RotateVector(currentDirection, 180)  // Reverse
            };

            foreach (var dir in possibleDirections)
            {
                Vector2Int testPosition = new Vector2Int(x + dir.x, y + dir.y);
                if (!gridManager.IsOutOfBounds(testPosition.x, testPosition.y) &&
                    (gridManager.GetTileAt(testPosition.x, testPosition.y) == TileType.Empty ||
                     gridManager.GetTileAt(testPosition.x, testPosition.y) == TileType.SoftEarth ||
                     gridManager.GetTileAt(testPosition.x, testPosition.y) == TileType.AetherShard))
                {
                    currentDirection = dir;
                    return;
                }
            }
            // If all directions are blocked, stay put (shouldn't happen in open maze)
        }

        private Vector2Int RotateVector(Vector2Int vec, float angle)
        {
            float rad = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            return new Vector2Int(Mathf.RoundToInt(vec.x * cos - vec.y * sin), Mathf.RoundToInt(vec.x * sin + vec.y * cos));
        }
    }
}
