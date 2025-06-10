using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace MiniGames.ZenSlider
{
    public class MazeGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject obstaclePrefab; // Prefab for the obstacle
        [SerializeField] private GameObject finishLinePrefab; // Prefab for the Finish Line
        
        private AllMaze _mazeData;   // List of maze configurations

        private List<GameObject> _obstacles; // List to store spawned obstacles

        public MazeData SelectedMazeData { get; private set; } // Currently selected maze data

        public void Init(AllMaze mazeData)
        {
            _mazeData = mazeData;
            SelectedMazeData = mazeData.allMazeData[Random.Range(0, mazeData.allMazeData.Count)];
            GenerateMaze();
            SpawnPlayer();
        }

        private void GenerateMaze()
        {
            if(_obstacles is not null)
                ClearObstacles();

            _obstacles = new List<GameObject>();

            foreach (var obstacle in SelectedMazeData.obstaclesPositions.Select(position => Instantiate(obstaclePrefab, new Vector3(position.x, 0, position.y), Quaternion.identity, transform)))
            {
                _obstacles.Add(obstacle);
            }

            GameObject finishLine = Instantiate(finishLinePrefab, new Vector3(SelectedMazeData.finishLinePosition.x, 0, SelectedMazeData.finishLinePosition.y), Quaternion.identity, transform);
            _obstacles.Add(finishLine);
        }

        private void SpawnPlayer()
        {
            GameController.Instance.SpawnPlayer(SelectedMazeData.playerPosition);
        }
        
        private void ClearObstacles()
        {
            foreach (var obstacle in _obstacles)
            {
                Destroy(obstacle);
            }
            
            _obstacles.Clear();
        }
    }
    
    [Serializable]
    public class AllMaze
    {
        public List<MazeData> allMazeData = new List<MazeData>();
    }
    
    [Serializable]
    public class MazeData
    {
        public Vector2Int playerPosition;          // Player's starting position
        public Vector2Int finishLinePosition;      // Finish line position
        public List<Vector2Int> obstaclesPositions; // List of obstacle positions
    }
}