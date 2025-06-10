using UnityEngine;

namespace DebugClasses
{
    public class Debugger : MonoBehaviour
    {
        private static Debugger _instance;

        public static void StartDebugger()
        {
            GameObject debugger = new GameObject("Debugger");
            debugger.AddComponent<Debugger>();
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                if (_instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }


        // ReSharper disable Unity.PerformanceAnalysis
        public static void ShowLogMessage(string msg)
        {
            if (_instance is null)
                return;
        
            Debug.Log(msg);
        }
    
        // ReSharper disable Unity.PerformanceAnalysis
        public static void ShowWarningMessage(string msg)
        {
            if (_instance is null)
                return;

            Debug.LogWarning(msg);
        }
    
        // ReSharper disable Unity.PerformanceAnalysis
        public static void ShowErrorMessage(string msg)
        {
            if (_instance is null)
                return;

            Debug.LogError(msg);
        }

    }
}
