using UnityEngine;

namespace GeminiCore
{
    /// <summary>
    /// Manages the overall game state, orchestrates core components,
    /// and handles game initialization.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Core Managers")]
        [SerializeField] private GridManager gridManager;
        [SerializeField] private PhysicsHandler physicsHandler;
        [SerializeField] private StateScheduler stateScheduler;
        [SerializeField] private InputController inputController;
        [SerializeField] private ProceduralAnomalyGenerator pag; // New PAG reference

        [Header("Player Settings")]
        [SerializeField] private GameObject voidDiverPrefab; // Prefab for the Void Diver
        [SerializeField] private Vector2Int playerSpawnPosition = new Vector2Int(1, 1);

        private VoidDiver currentVoidDiver;

        [Header("Game State")]
        [SerializeField] private int totalAetherShardsInLevel = 1; // How many shards to collect to win
        private int collectedAetherShards = 0;

        void Awake()
        {
            // Ensure all managers are assigned
            if (gridManager == null) Debug.LogError("GameManager: GridManager not assigned!");
            if (physicsHandler == null) Debug.LogError("GameManager: PhysicsHandler not assigned!");
            if (stateScheduler == null) Debug.LogError("GameManager: StateScheduler not assigned!");
            if (inputController == null) Debug.LogError("GameManager: InputController not assigned!");
        }

        void Start()
        {
            InitializeGame();
        }

        /// <summary>
        /// Initializes the game state, spawns the player, and sets up initial conditions.
        /// </summary>
        [Header("Enemy Settings")]
        [SerializeField] private GameObject plasmaWaspPrefab; // Prefab for the Plasma Wasp
        [SerializeField] private Vector2Int[] plasmaWaspSpawnPositions;

        private List<PlasmaWaspController> activePlasmaWasps = new List<PlasmaWaspController>();

        void Awake()
        {
            // Ensure all managers are assigned
            if (gridManager == null) Debug.LogError("GameManager: GridManager not assigned!");
            if (physicsHandler == null) Debug.LogError("GameManager: PhysicsHandler not assigned!");
            if (stateScheduler == null) Debug.LogError("GameManager: StateScheduler not assigned!");
            if (inputController == null) Debug.LogError("GameManager: InputController not assigned!");
        }

        void Start()
        {
            // Show the start menu at the beginning of the game
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowStartMenu();
            }
            else
            {
                Debug.LogError("UIManager not found. Cannot show start menu.");
                InitializeGame(); // If no UI manager, just start the game directly
            }
        }

        /// <summary>
        /// Initializes the game state, spawns the player, and sets up initial conditions.
        /// This is now called by UIManager when the "Start Game" button is clicked.
        /// </summary>
        public void InitializeGame()
        {
            Debug.Log("GameManager: Initializing game...");

            // Generate the level using the PAG
            if (pag != null)
            {
                pag.GenerateLevel();
            }
            else
            {
                Debug.LogError("GameManager: ProceduralAnomalyGenerator not assigned! Cannot generate level.");
                // Fallback to a simple hardcoded level if PAG is missing
                InitializeHardcodedLevel();
            }

            // Spawn the Void Diver
            if (voidDiverPrefab != null)
            {
                GameObject playerGO = Instantiate(voidDiverPrefab, new Vector3(playerSpawnPosition.x, playerSpawnPosition.y, 0), Quaternion.identity);
                currentVoidDiver = playerGO.GetComponent<VoidDiver>();

                if (currentVoidDiver != null)
                {
                    currentVoidDiver.x = playerSpawnPosition.x;
                    currentVoidDiver.y = playerSpawnPosition.y;
                    currentVoidDiver.quantumBombs = 3; // Starting quantum bombs
                    Debug.Log($"GameManager: Void Diver spawned at {playerSpawnPosition.x},{playerSpawnPosition.y}");

                    // Link the spawned player to the InputController
                    if (inputController != null)
                    {
                        inputController.voidDiver = currentVoidDiver;
                    }
                }
                else
                {
                    Debug.LogError("GameManager: VoidDiver prefab does not have a VoidDiver component!");
                }
            }
            else
            {
                Debug.LogError("GameManager: VoidDiver Prefab is not assigned!");
            }

            // Ensure player spawn is empty and draw the grid
            if (gridManager != null)
            {
                gridManager.SetTileAt(playerSpawnPosition.x, playerSpawnPosition.y, TileType.Empty);

                GridVisualizer gridVisualizer = FindObjectOfType<GridVisualizer>();
                if (gridVisualizer != null)
                {
                    gridVisualizer.DrawFullGrid();
                }
                else
                {
                    Debug.LogWarning("GameManager: GridVisualizer not found in scene. Grid will not be drawn.");
                }
            }
            else
            {
                Debug.LogError("GameManager: GridManager is null, cannot initialize grid data.");
            }

            // Spawn Plasma Wasps
            if (plasmaWaspPrefab != null && plasmaWaspSpawnPositions != null)
            {
                foreach (Vector2Int spawnPos in plasmaWaspSpawnPositions)
                {
                    GameObject waspGO = Instantiate(plasmaWaspPrefab, new Vector3(spawnPos.x + 0.5f, spawnPos.y + 0.5f, 0), Quaternion.identity);
                    PlasmaWaspController waspController = waspGO.GetComponent<PlasmaWaspController>();
                    if (waspController != null)
                    {
                        waspController.x = spawnPos.x;
                        waspController.y = spawnPos.y;
                        waspController.gridManager = gridManager; // Link GridManager to wasp
                        activePlasmaWasps.Add(waspController);
                        Debug.Log($"GameManager: Plasma Wasp spawned at {spawnPos.x},{spawnPos.y}");
                    }
                    else
                    {
                        Debug.LogError("GameManager: PlasmaWasp prefab does not have a PlasmaWaspController component!");
                    }
                }
            }
            else
            {
                Debug.LogWarning("GameManager: PlasmaWasp Prefab or spawn positions not assigned. No wasps will spawn.");
            }
        }

        // New fallback method
        private void InitializeHardcodedLevel()
        {
            if (gridManager == null) return;
            for (int x = 0; x < gridManager.GridWidth; x++)
            {
                gridManager.SetTileAt(x, 0, TileType.SolidRock);
                gridManager.SetTileAt(x, 1, TileType.SoftEarth);
            }
            gridManager.SetTileAt(5, 2, TileType.AetherShard);
        }
        /// <summary>
        /// Called by InputController when an Aether Shard is collected.
        /// </summary>
        public void CollectAetherShard()
        {
            collectedAetherShards++;
            Debug.Log($"Aether Shard collected! Total: {collectedAetherShards}/{totalAetherShardsInLevel}");

            if (collectedAetherShards >= totalAetherShardsInLevel)
            {
                Debug.Log("All Aether Shards collected! You win!");
                // Show the win screen
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ShowWinScreen();
                }

                // Spawn the Exit Portal
                gridManager.SetTileAt(1, 2, TileType.ExitPortal); // Example position
                GridVisualizer gridVisualizer = FindObjectOfType<GridVisualizer>();
                gridVisualizer?.DrawTile(1, 2);
            }
        }
    }
}
