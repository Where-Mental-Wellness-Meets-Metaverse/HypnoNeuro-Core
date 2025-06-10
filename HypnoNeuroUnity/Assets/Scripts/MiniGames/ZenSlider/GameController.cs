using UnityEngine;
using Newtonsoft.Json;

namespace MiniGames.ZenSlider
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;

        [SerializeField] private Player playerPrefab;
        [SerializeField] private MazeGenerator mazeGenerator;

        [SerializeField] private string jsonFilePath = "mazeData.json"; // Path to the JSON file

        public Player player;
        public AllMaze allMazeData; // Stores all maze data loaded from JSON

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            LoadMazeDataFromJson();
            mazeGenerator.Init(allMazeData);
        }

        private void LoadMazeDataFromJson()
        {
            var jsonFile = Resources.Load<TextAsset>("Json/MazeData");

            if (jsonFile != null)
            {
                var jsonData = jsonFile.text;

                allMazeData = JsonConvert.DeserializeObject<AllMaze>(jsonData);

                Debug.Log("Maze data loaded successfully from JSON.");
            }
            else
            {
                Debug.LogError("JSON file not found in Resources: " + jsonFilePath);
                allMazeData = new AllMaze();
            }
        }

        public void SpawnPlayer(Vector2Int worldPos)
        {
            if (player is not null)
                Destroy(player.gameObject);

            player = Instantiate(playerPrefab, new Vector3(worldPos.x, 0, worldPos.y), Quaternion.identity);
        }

        public void LevelCompleted()
        {
            mazeGenerator.Init(allMazeData);
        }
    }
}