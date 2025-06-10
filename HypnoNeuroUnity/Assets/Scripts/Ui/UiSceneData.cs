using System.Collections.Generic;
using UnityEngine;

namespace Ui
{
    
    [System.Serializable]
    public class UiSceneData
    {
        public SceneName sceneName;
        public Canvas canvasPrefab;
        public UiScreenName startingScreen;
        public List<Views> viewsData;
    }
    
}

public enum UiScreenName
{
    MainScreen,
    LoginScreen,
    SignUpScreen,
    LoadingScreen,
    GameScreen,
}

public enum SceneName
{
    MainMenu,
    
    // Mini Games
    ZenSlider,
    DoNotFall,
    ColorPuzzle,
    Orbit,
    
}