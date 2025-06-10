using System.Collections.Generic;
using DataModel;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace Ui.MainMenu.MainMenuScreen
{
    public class MainMenuView : FullScreenView
    {

        [Header("Ui Elements")] 
        [SerializeField] private TMP_Text uid;
        [SerializeField] private TMP_Text tokensText;
        [SerializeField] private TMP_Text email;
        [SerializeField] private Button signOutButton;
        
        [SerializeField] private GameButtonObject gameButtonObjectPrefab;
        [SerializeField] private Transform gameButtonParent;
        private List<GameButtonObject> gameButtons = new List<GameButtonObject>();
        
        protected override void OnShow(Object obj)
        {
            UpdateUserData();
            UpdateTokens(DataManager.Instance.UserData.Tokens);
            signOutButton.onClick.AddListener(OnClickSignOut);

            gameButtons = new List<GameButtonObject>();
            foreach (var game in DataManager.Instance.games)
            {
                var gameButton = Instantiate(gameButtonObjectPrefab, gameButtonParent);
                gameButton.Init(game);
                gameButtons.Add(gameButton);
            }
            
            DataManager.Instance.UserData.OnUpdateToken += UpdateTokens;
            // DataManager.Instance.OnUserDataUpdated += UpdateUserData;
        }


        protected override void OnHide()
        {
            gameButtons.ForEach(button => Destroy(button.gameObject));
            gameButtons.Clear();

            DataManager.Instance.UserData.OnUpdateToken -= UpdateTokens;
            signOutButton.onClick.RemoveAllListeners();
        }


        private void OnClickSignOut()
        {
            FirebaseManager.Instance.SignOut();
            UiManager.Instance.ShowPanel(UiScreenName.LoginScreen, null);
        }

        private void UpdateUserData()
        {
            if (FirebaseManager.Instance.CurrentUser == null) return;
            
            uid.text = FirebaseManager.Instance.CurrentUser.UserId;
            email.text = FirebaseManager.Instance.CurrentUser.Email;
        }
        
        private void UpdateTokens(double tokens)
        {
            tokensText.text = $"Tokens {tokens:#.0#}";
        }

    }
}