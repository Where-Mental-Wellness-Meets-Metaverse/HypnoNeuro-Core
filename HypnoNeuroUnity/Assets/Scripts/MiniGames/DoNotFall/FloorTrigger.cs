using System;
using UnityEngine;

namespace MiniGames.DoNotFall
{
    public class FloorTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Floor Hit");
                GameController.Instance.currentHeight -= 20f;
                GameController.Instance.InitializeGrid();
            }
        }
    }
}
