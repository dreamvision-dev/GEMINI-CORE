using UnityEngine;

namespace GeminiCore
{
    /// <summary>
    /// Represents the player character, the Void Diver.
    /// </summary>
    public class VoidDiver : MonoBehaviour
    {
        [Header("Diver State")]
        public int x;
        public int y;
        public int quantumBombs;

        public Animator animator; // Reference to the Animator component

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            // Initialize position or other state if needed
            Debug.Log($"Void Diver spawned at ({x}, {y}) with {quantumBombs} Quantum Bombs.");
        }

        /// <summary>
        /// Sets the diver's grid position.
        /// </summary>
        /// <param name="newX">New X coordinate.</param>
        /// <param name="newY">New Y coordinate.</param>
        public void SetGridPosition(int newX, int newY)
        {
            x = newX;
            y = newY;
            // Potentially update transform.position here based on grid coordinates
            transform.position = new Vector3(x + 0.5f, y + 0.5f, 0); // Add 0.5f to center on tile
        }
    }
}
