using System;
using DataModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ui.MainMenu.MainMenuScreen
{
    public class GameButtonObject : MonoBehaviour
    {
        public TMP_Text gameName;
        public TMP_Text tokenPrice;
        public Button gameButton;

        private Game game;
        
        public void Init(Game gameData)
        {
            game = gameData;
            gameName.text = gameData.gameName;
            tokenPrice.text = gameData.tokenPrice.ToString("#.##");
        }

        private void OnEnable()
        {
            gameButton.onClick.AddListener(OnClickGame);
        }

        private void OnDisable()
        {
            gameButton.onClick.RemoveAllListeners();
        }

        private void OnClickGame()
        {
            if (DataManager.Instance.UserData.Tokens >= game.tokenPrice)
            {
                DataManager.Instance.UserData.Tokens -= game.tokenPrice;
                SceneManager.LoadScene(game.sceneName.ToString());

            }
            else
            {
                UiManager.Instance.ShowToast("Not Enough Tokens");
            }
        }
    }
}
