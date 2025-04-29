using UnityEngine;
using UnityEngine.UI;
using QuickJam.UI;

public class TestUI : UIBase
{
    [SerializeField] private Button _startButton;
    
    public System.Action OnStartButtonClicked;

    private void Awake()
    {
        _startButton.onClick.AddListener(() =>
        {
            OnStartButtonClicked?.Invoke();
            // todo: close
        });
        
    }
}