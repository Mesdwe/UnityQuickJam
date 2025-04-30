using QuickJam.Core;
using QuickJam.Save;
using QuickJam.UI;
using UnityEngine;

namespace QuickJam.Samples.TestScripts
{
    public class Test : MonoBehaviour
    {
        public TestUI testUI;
        void Start()
        {
           
            TestUI popup =  UIManager.Instance.OpenUI(testUI, UILayer.Overlay);

            popup.OnStartButtonClicked = () => { GameManager.Instance.StartGame(); };
            popup.OnDeleteSaveButtonClicked = () => { SaveSystem.Instance.Delete("player"); };
            

        }


    }
}
