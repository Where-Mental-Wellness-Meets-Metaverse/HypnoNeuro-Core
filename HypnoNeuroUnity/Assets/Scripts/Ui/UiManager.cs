using System;
using System.Collections.Generic;
using System.Linq;
using DebugClasses;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace Ui
{
    public class UiManager : MonoBehaviour
    {
        public static UiManager Instance;

        public ToastPrefab toastPrefab;

        public List<UiSceneData> sceneData;

        private readonly Dictionary<UiScreenName ,Views> _activeScreens = new Dictionary<UiScreenName ,Views>();
        private readonly Stack<PanelData> _panelStack = new Stack<PanelData>();

        public Canvas ActiveCanvas { get; private set; }
        public Views ActivePanel { get; private set; }

        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneName sceneName = Enum.Parse<SceneName>(scene.name);
            LoadSceneUi(sceneName);
        }

        public void LoadSceneUi(SceneName sceneName)
        {
            var uiSceneData = GetSceneData(sceneName);

            if (uiSceneData is not null)
            {
                _activeScreens.Clear();
                _panelStack.Clear();

                ActiveCanvas = Instantiate(uiSceneData.canvasPrefab);
                foreach (var views in uiSceneData.viewsData)
                {
                    var screen = Instantiate(views, ActiveCanvas.transform);
                    _activeScreens.Add(screen.screenName, screen);

                    screen.SetDefault();

                    if (views.screenName != uiSceneData.startingScreen)
                        continue;

                    ActivePanel = screen;
                    ActivePanel.Show(null);

                    // Push the starting screen with null data onto the stack
                    _panelStack.Push(new PanelData(ActivePanel, null));
                }
            }
            else
            {
                Debug.LogWarning($"Unhandled scene: {sceneName}");
            }
        }

        private UiSceneData GetSceneData(SceneName sceneName)
        {
            return sceneData.FirstOrDefault(data => data.sceneName == sceneName);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public Views ShowPanel(UiScreenName uiScreenName, Object data)
        {
            Views view = GetUiView(uiScreenName);
            if (!view.showLastPanel)
            {
                ActivePanel.Hide();
            }
            ActivePanel = view;
            ActivePanel.Show(data);

            _panelStack.Push(new PanelData(ActivePanel, data));
            Debugger.ShowLogMessage("Panel shown: " + uiScreenName);
            return ActivePanel;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void HidePanel()
        {
            if (_panelStack.Count > 1)
            {
                PanelData currentPanelData = _panelStack.Pop();
                currentPanelData.Panel.Hide();

                PanelData previousPanelData = _panelStack.Peek();

                ActivePanel = previousPanelData.Panel;
                if (!currentPanelData.Panel.showLastPanel)
                {
                    ActivePanel.Show(previousPanelData.Data);
                }
            }
            else
            {
                Debugger.ShowWarningMessage("No more panels to hide.");
            }
        }

        public Views GetUiView(UiScreenName uiScreenName)
        {
            if (_activeScreens.TryGetValue(uiScreenName, out var view))
            {
                return view;
            }

            Debugger.ShowErrorMessage($"No view found with the name {uiScreenName}");
            return null;
        }

        // Helper class to store panel and associated data
        private class PanelData
        {
            public Views Panel { get; }
            public Object Data { get; }

            public PanelData(Views panel, Object data)
            {
                Panel = panel;
                Data = data;
            }
        }
        
        public void ShowToast(string message)
        {
            var toast = Instantiate(toastPrefab, ActiveCanvas.transform);
            toast.Init(message);
        }
    }
}