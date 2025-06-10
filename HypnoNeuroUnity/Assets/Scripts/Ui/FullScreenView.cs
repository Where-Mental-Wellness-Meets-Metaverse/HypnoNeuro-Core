using UnityEngine;
using UnityEngine.Serialization;

namespace Ui
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class FullScreenView : Views
    {
        [Header("Animation Variables")] 
        public float animationTime = 0.2f;
        
        private CanvasGroup canvasGroup;
        protected void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        public override void Show(object obj)
        {
            // Animation
            gameObject.SetActive(true);
            base.Show(obj);
            FadeCanvas(0, 1, animationTime);
        }

        public override void Hide()
        {
            // Animation
            gameObject.SetActive(false);
            base.Hide();
        }

        public override void SetDefault()
        {
            gameObject.SetActive(false);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void FadeCanvas(float fromAlpha, float toAlpha, float duration)
        {
            canvasGroup.alpha = fromAlpha; // Set the initial alpha value

            iTween.ValueTo(gameObject, iTween.Hash(
                "from", fromAlpha,
                "to", toAlpha,
                "time", duration,
                "easetype", iTween.EaseType.easeInOutQuad,
                "onupdate", "UpdateAlpha",
                "onupdatetarget", gameObject
            ));
        }

        private void UpdateAlpha(float newAlpha)
        {
            canvasGroup.alpha = newAlpha;
        }
        
    }
}
