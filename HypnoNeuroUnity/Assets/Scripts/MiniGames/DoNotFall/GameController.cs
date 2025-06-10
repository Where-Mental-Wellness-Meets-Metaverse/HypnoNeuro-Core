using System.Collections.Generic;
using UnityEngine;

namespace MiniGames.DoNotFall
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;
        
        [Header("Grid")]
        public List<Color> gridColors = new List<Color>();
        public Vector2 gridSize;
        public GridElement gridElementPrefab;
        public GameObject floorPrefab;
        public float spacing = 1.1f;
        public float currentHeight = 0;
        
        [Header("Player")]
        public Player playerPrefab;
        public Vector2Int startPosition;

        private Transform _gridParent;
        private GameObject _gridFloor;
        private GridElement[,] _gridElements;

        public Player Player {get; private set;}
        public Vector2 GridSize => gridSize;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            
            InitializeGrid();
            InitializePlayer();
        }

        private void InitializePlayer()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("Player prefab is not assigned!");
                return;
            }

            var playerPosition = GetGridElement(startPosition.x,startPosition.y).transform.position + Vector3.up;

            Player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        }
        public void InitializeGrid()
        {
            DestroyExistingGrid();

            _gridElements = new GridElement[(int)gridSize.x, (int)gridSize.y];
            _gridParent = new GameObject("GridParent").transform;

            _gridFloor = Instantiate(floorPrefab, new Vector3(0, currentHeight - 10, 0), Quaternion.identity);
            var color = gridColors[Random.Range(0, gridColors.Count)];

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int z = 0; z < gridSize.y; z++)
                {
                    var position = new Vector3(x * spacing, currentHeight, z * spacing);
                    var cube = Instantiate(gridElementPrefab, position, Quaternion.identity);
                    cube.transform.SetParent(_gridParent);
                    cube.Init(color);

                    _gridElements[x, z] = cube;
                }
            }
        }

        private void DestroyExistingGrid()
        {
            if (_gridElements != null)
            {
                for (int x = 0; x < _gridElements.GetLength(0); x++)
                {
                    for (int z = 0; z < _gridElements.GetLength(1); z++)
                    {
                        if (_gridElements[x, z] != null)
                        {
                            Destroy(_gridElements[x, z].gameObject);
                        }
                    }
                }
            }

            if (_gridParent != null)
            {
                Destroy(_gridParent.gameObject);
            }

            if (_gridFloor != null)
            {
                Destroy(_gridFloor);
            }
        }
        public GridElement GetGridElement(int x, int z)
        {
            if (x >= 0 && x < gridSize.x && z >= 0 && z < gridSize.y)
            {
                return _gridElements[x, z];
            }
            else
            {
                Debug.LogError($"Invalid grid coordinates: ({x}, {z})");
                return null;
            }
        }
    }
}