using UnityEngine;

namespace QuickJam.UI
{
    public class UIBase : MonoBehaviour
    {

        public virtual void OnOpen()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnClose()
        {
            Destroy(gameObject);    // TODO: consider hiding it instead of destroying
        }
    }
}