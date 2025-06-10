using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MiniGames.ColorPuzzle
{
    public class GameController : MonoBehaviour
    {
        [Serializable]
        private class PuzzleColors
        {
            public Color color1; // Top-left corner
            public Color color2; // Top-right corner
            public Color color3; // Bottom-left corner
            public Color color4; // Bottom-right corner
        }
        
        [Serializable]
        private class PuzzleData
        {
            public Vector2 gridSize;
        }

        [SerializeField] private PuzzleObject puzzleElementPrefab;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private List<PuzzleColors> puzzleColors;
        [SerializeField] private List<PuzzleData> puzzles;
        [Range(0, 100)]
        [SerializeField] private float nonMovablePercentageMin, nonMovablePercentageMax;

        private PuzzleData _selectedData;
        private PuzzleColors _selectedColors;
        
        
        private PuzzleObject[,] _puzzleGrid;
        private PuzzleObject _selectedElement; // Currently selected puzzle element

        public static GameController Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            CreateGrid();
        }
        private void CreateGrid()
        {
            _selectedData = puzzles[Random.Range(0, puzzles.Count)];
            _selectedColors = puzzleColors[Random.Range(0, puzzleColors.Count)];

            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            gridLayoutGroup.constraintCount = (int)_selectedData.gridSize.x;
            Debug.Log(_selectedData.gridSize);

            var gridParentRect = gridLayoutGroup.GetComponent<RectTransform>();
            var gridParentSize = gridParentRect.rect.size;

            var cellWidth = gridParentSize.x / _selectedData.gridSize.x;
            var cellHeight = gridParentSize.y / _selectedData.gridSize.y;
            var min = Mathf.Min(cellWidth, cellHeight);

            gridLayoutGroup.cellSize = new Vector2(min, min);
            
            if (_puzzleGrid != null)
            {
                foreach (Transform child in gridLayoutGroup.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            _puzzleGrid = new PuzzleObject[(int)_selectedData.gridSize.x, (int)_selectedData.gridSize.y];

            _puzzleGrid = new PuzzleObject[(int)_selectedData.gridSize.x, (int)_selectedData.gridSize.y];

            var totalCells = (int)_selectedData.gridSize.x * (int)_selectedData.gridSize.y;

            var immovablePercentage = Random.Range(nonMovablePercentageMin, nonMovablePercentageMax) / 100f;
            var immovableCount = Mathf.RoundToInt(totalCells * immovablePercentage);

            var cellPositions = new List<Vector2Int>();
            for (var x = 0; x < _selectedData.gridSize.x; x++)
            {
                for (var y = 0; y < _selectedData.gridSize.y; y++)
                {
                    cellPositions.Add(new Vector2Int(x, y));
                }
            }

            Shuffle(cellPositions);

            var immovableCells = new HashSet<Vector2Int>();
            for (var i = 0; i < immovableCount; i++)
            {
                immovableCells.Add(cellPositions[i]);
            }

            for (var x = 0; x < _selectedData.gridSize.x; x++)
            {
                for (var y = 0; y < _selectedData.gridSize.y; y++)
                {
                    var puzzleElement = Instantiate(puzzleElementPrefab, gridLayoutGroup.transform);
                    var elementColor = CalculateGradientColor(x, y, _selectedColors, _selectedData.gridSize);
                    var isMovable = !immovableCells.Contains(new Vector2Int(x, y));
                    puzzleElement.Init(new Vector2(x, y), elementColor, !isMovable);
                    _puzzleGrid[x, y] = puzzleElement;
                }
            }
            
            // Step 1: Collect all movable elements
            List<PuzzleObject> movableElements = new List<PuzzleObject>();

            for (var x = 0; x < _selectedData.gridSize.x; x++)
            {
                for (var y = 0; y < _selectedData.gridSize.y; y++)
                {
                    if (!_puzzleGrid[x, y].IsDisabled) // Ensure the object is movable
                    {
                        movableElements.Add(_puzzleGrid[x, y]);
                    }
                }
            }

            Shuffle(movableElements);

            int index = 0;
            for (var x = 0; x < _selectedData.gridSize.x; x++)
            {
                for (var y = 0; y < _selectedData.gridSize.y; y++)
                {
                    if (!_puzzleGrid[x, y].IsDisabled)
                    {
                        SwapElements(_puzzleGrid[x, y], movableElements[index]);
                        index++;
                    }
                }
            }

        }

        private void Shuffle<T>(List<T> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var randomIndex = Random.Range(i, list.Count);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }

        private Color CalculateGradientColor(int x, int y, PuzzleColors colors, Vector2 gridSize)
        {
            var normalizedX = x / (gridSize.x - 1);
            var normalizedY = y / (gridSize.y - 1);

            var topColor = Color.Lerp(colors.color1, colors.color2, normalizedX);

            var bottomColor = Color.Lerp(colors.color3, colors.color4, normalizedX);

            var finalColor = Color.Lerp(topColor, bottomColor, normalizedY);

            return finalColor;
        }

        public void OnPuzzleElementClicked(int x, int y)
        {
            var clickedElement = _puzzleGrid[x, y];

            if (_selectedElement == null)
            {
                _selectedElement = clickedElement;
                _selectedElement.transform.localScale *= 1.1f;
            }
            else
            {
                SwapElements(_selectedElement, clickedElement);
                _selectedElement.transform.localScale /= 1.1f;
                _selectedElement = null; // Deselect after swapping
            }
        }

        private void SwapElements(PuzzleObject element1, PuzzleObject element2)
        {
            var element1Index = element1.transform.GetSiblingIndex();
            var element2Index = element2.transform.GetSiblingIndex();
            element1.transform.SetSiblingIndex(element2Index);
            element2.transform.SetSiblingIndex(element1Index);

            if (IsPuzzleSolved())
            {
                Debug.Log("Puzzle Solved");
                CreateGrid();
            }
        }
        
        
        private bool IsPuzzleSolved()
        {
            Dictionary<int, PuzzleObject> visualOrder = new Dictionary<int, PuzzleObject>();

            // Store elements in dictionary using their visual order
            for (int i = 0; i < gridLayoutGroup.transform.childCount; i++)
            {
                visualOrder[i] = gridLayoutGroup.transform.GetChild(i).GetComponent<PuzzleObject>();
            }

            int index = 0;
            for (var x = 0; x < _selectedData.gridSize.x; x++)
            {
                for (var y = 0; y < _selectedData.gridSize.y; y++)
                {
                    if (visualOrder[index] != _puzzleGrid[x, y])
                    {
                        return false;
                    }
                    index++;
                }
            }

            return true;
        }

    }
}