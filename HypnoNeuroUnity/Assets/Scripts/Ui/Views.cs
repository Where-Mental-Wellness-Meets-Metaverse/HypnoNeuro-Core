using UnityEngine;
using Object = System.Object;

namespace Ui
{
    public abstract class Views : MonoBehaviour
    {
        public UiScreenName screenName;        
        public bool showLastPanel;

        protected abstract void OnShow(Object obj);

        protected abstract void OnHide();

        public abstract void SetDefault();

        public virtual void Show(Object obj)
        {
            OnShow(obj);
        }
        public virtual void Hide()
        {
            OnHide();
        }
    }
}