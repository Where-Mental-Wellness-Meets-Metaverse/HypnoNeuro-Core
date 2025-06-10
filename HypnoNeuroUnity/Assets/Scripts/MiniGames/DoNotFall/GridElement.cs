using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MiniGames.DoNotFall
{
    public class GridElement : MonoBehaviour
    {
        public float maxHp = 5f; // Initial HP of the tile

        public float damageRate = 1f; // HP reduction per second when the player stands on it

        public Renderer spriterRenderer; // Reference to the tile's renderer
        public Color originalColor; // Original color of the tile
        private bool _isPlayerOnTile; // Whether the player is standing on this tile
        private float _hp = 5f; // Initial HP of the tile

        private bool _init = false;
        private void Start()
        {
            _hp = maxHp;
        }

        public void Init(Color color)
        {
            spriterRenderer.material.color = color;
            originalColor = color;
            _init = true;
        }

        private void Update()
        {
            if (!_init) return;
                
            if (!_isPlayerOnTile) return;
            
            _hp -= damageRate * Time.deltaTime;

            UpdateTileColor();

            if (_hp <= 0)
            {
                DestroyTile();
            }
        }

        private void UpdateTileColor()
        {
            float t = 1f - Mathf.Clamp01(_hp / maxHp);
            Color newColor = Color.Lerp(originalColor, Color.white, t);

            spriterRenderer.material.color = newColor;
        }

        private void DestroyTile()
        {
            gameObject.SetActive(false);
        }
        public void SetPlayerOnTile()
        {
            _isPlayerOnTile = true;
        }
    }
}