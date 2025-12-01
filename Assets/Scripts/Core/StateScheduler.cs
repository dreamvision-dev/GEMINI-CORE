using UnityEngine;
using System.Collections;

namespace GeminiCore
{
    /// <summary>
    /// Handles all time-based dynamic elements, such as the state changes
    /// for Phase Gravel and the duration of Quantum Bomb effects.
    /// </summary>
    public class StateScheduler : MonoBehaviour
    {
        [Header("Phase Gravel Timing")]
        [SerializeField] private float phaseInterval = 5.0f;

        public bool IsPhaseGravelSolid { get; private set; } = true; // Expose the state

        void Start()
        {
            // Start the timer for Phase Gravel state changes
            StartCoroutine(PhaseGravelCycle());
        }

        /// <summary>
        /// Coroutine that toggles the state of Phase Gravel on a fixed interval.
        /// </summary>
        private IEnumerator PhaseGravelCycle()
        {
            while (true)
            {
                yield return new WaitForSeconds(phaseInterval);

                IsPhaseGravelSolid = !IsPhaseGravelSolid;
                // No direct grid update here. PhysicsHandler and GridVisualizer will react to IsPhaseGravelSolid.
                Debug.Log("Phase Gravel state changed. Is Solid: " + IsPhaseGravelSolid);

                // Trigger a full grid redraw to update PhaseGravel visuals
                GridVisualizer gridVisualizer = FindObjectOfType<GridVisualizer>();
                gridVisualizer?.DrawFullGrid();
            }
        }

        [Header("Quantum Bomb Timing")]
        [SerializeField] private float quantumBombDuration = 3.0f; // Default duration

        public bool IsGravityReversed { get; private set; } = false; // Expose gravity reversal state

        /// <summary>
        /// Activates the local gravity reversal effect of a Quantum Bomb.
        /// </summary>
        /// <param name="duration">How long the effect should last.</param>
        public void ActivateQuantumBomb(float duration)
        {
            // Stop any existing bomb effect to prevent overlapping coroutines
            StopCoroutine("QuantumBombEffect");
            StartCoroutine(QuantumBombEffect(duration));
        }

        private IEnumerator QuantumBombEffect(float duration)
        {
            IsGravityReversed = true;
            Debug.Log("Quantum Bomb: Gravity Reversed!");
            // TODO: Potentially notify PhysicsHandler or other systems immediately

            yield return new WaitForSeconds(duration);

            IsGravityReversed = false;
            Debug.Log("Quantum Bomb: Gravity Normal!");
            // TODO: Potentially notify PhysicsHandler or other systems immediately
        }
    }
}
