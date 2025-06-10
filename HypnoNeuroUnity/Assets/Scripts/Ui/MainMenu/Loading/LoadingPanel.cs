using System.Threading.Tasks;
using DataModel;
using DebugClasses;
using Managers;
using TMPro;
using Object = System.Object;
using UnityEngine;

namespace Ui.MainMenu.Loading
{
    public class LoadingPanel : FullScreenView
    {
        public TMP_Text statusText;
        
        protected override async void OnShow(Object obj)
        {
            await Loading();
        }

        protected override void OnHide()
        {
            
        }
        
        private async Task Loading()
        {
            await Task.Delay(1000); // Wait for 1 second before Firebase initialization
            
            statusText.text = "Initializing Firebase";
            await FirebaseManager.Instance.InitializeFirebase(); // Initialize Firebase here

            if (FirebaseManager.Instance.CurrentUser != null)
            {

                statusText.text = "Loading User Data";
                await DataManager.Instance.LoadUserDataFromFirestore();
                
                statusText.text = "Almost Done, Logging in";
                await Task.Delay(2000); // Wait for additional 2 seconds

                UiManager.Instance.ShowPanel(UiScreenName.MainScreen, null);
            }
            else
            {
                UiManager.Instance.ShowPanel(UiScreenName.LoginScreen, null);
            }
        }

    }
}