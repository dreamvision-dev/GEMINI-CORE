using UnityEngine;

namespace GeminiCore
{
    /// <summary>
    /// Handles button clicks and other events from the main menu UI.
    /// </summary>
    public class MenuController : MonoBehaviour
    {
        /// <summary>
        /// Called when the "Start Game" button is clicked.
        /// </summary>
        public void OnStartGameClicked()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.StartGame();
            }
            else
            {
                Debug.LogError("UIManager not found in scene!");
            }
        }

        /// <summary>
        /// Called when the "Restart Game" button is clicked (on Game Over or Win screens).
        /// </summary>
        public void OnRestartGameClicked()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.RestartGame();
            }
            else
            {
                Debug.LogError("UIManager not found in scene!");
            }
        }

        /// <summary>
        /// Called when the "Quit Game" button is clicked.
        /// </summary>
        public void OnQuitGameClicked()
        {
            Debug.Log("Quitting game...");
            Application.Quit();
        }
    }
}
