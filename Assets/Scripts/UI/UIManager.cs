using UnityEngine;
using UnityEngine.SceneManagement;

namespace GeminiCore
{
    /// <summary>
    /// Manages the game's UI, including menus and in-game HUD elements.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("UI Panels")]
        [SerializeField] private GameObject startMenuPanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private TMPro.TextMeshProUGUI storyText;
        // TODO: Add HUD panel for score, bombs, etc.

        [TextArea(5, 10)]
        [SerializeField] private string storyContent = "You are a Void Diver, tasked with extracting hyper-volatile Aether Shards from the cores of unstable, low-gravity celestial bodies. Success depends on mastering the unique, volatile environmental mechanics. Good luck, Diver.";

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Persist across scenes
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ShowStartMenu()
        {
            Time.timeScale = 0; // Pause the game
            startMenuPanel?.SetActive(true);
            gameOverPanel?.SetActive(false);
            winPanel?.SetActive(false);

            if (storyText != null)
            {
                storyText.text = storyContent;
            }
        }

        public void HideAllMenus()
        {
            Time.timeScale = 1; // Resume the game
            startMenuPanel?.SetActive(false);
            gameOverPanel?.SetActive(false);
            winPanel?.SetActive(false);
        }

        public void ShowGameOverScreen()
        {
            Time.timeScale = 0; // Pause the game
            gameOverPanel?.SetActive(true);
        }

        public void ShowWinScreen()
        {
            Time.timeScale = 0; // Pause the game
            winPanel?.SetActive(true);
        }

        public void RestartGame()
        {
            Time.timeScale = 1; // Resume time before reloading
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void StartGame()
        {
            HideAllMenus();
        }
    }
}
