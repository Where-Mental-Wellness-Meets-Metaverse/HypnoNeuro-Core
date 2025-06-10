using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = System.Object;

namespace Ui.Games
{
    public class GameScreen : FullScreenView
    {
        public Button backButton;
        
        
        protected override void OnShow(Object obj)
        {
            backButton.onClick.AddListener(OnClickBack);
            
        
        
        }

        private void OnClickBack()
        {
            SceneManager.LoadScene(SceneName.MainMenu.ToString());
        }

        protected override void OnHide()
        {
            backButton.onClick.RemoveListener(OnClickBack);
        }
    }
}
