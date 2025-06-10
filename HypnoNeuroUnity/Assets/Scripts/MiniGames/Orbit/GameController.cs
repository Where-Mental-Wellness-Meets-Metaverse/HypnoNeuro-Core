using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace MiniGames.Orbit
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;

        public Player player;

        public void Awake()
        {
            Time.timeScale = 1f;
            Instance = this;
        }

        public void GameLost()
        {
            Time.timeScale = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}
