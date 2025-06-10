using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MiniGames.ColorPuzzle
{
    public class PuzzleObject : MonoBehaviour
    {
        public Image image;
        public Image disabledImage;
        public Button button;
        
        public bool IsDisabled { get; private set; }
        public Color Color { get; private set; }
        public Vector2 GridPosition { get; private set; }

        public void Init(Vector2 gridPosition, Color color, bool isDisabled)
        {
            GridPosition = gridPosition;
            image.color = color;
            
            IsDisabled = isDisabled;
            Color = color;
            disabledImage.gameObject.SetActive(isDisabled);
            button.interactable = !isDisabled;
            
            button.onClick.AddListener(OnClickGridElement);
        }

        private void OnClickGridElement()
        {
            var xPos = (int)GridPosition.x;
            var yPos = (int)GridPosition.y;

            GameController.Instance.OnPuzzleElementClicked(xPos, yPos);
        }
    }
}