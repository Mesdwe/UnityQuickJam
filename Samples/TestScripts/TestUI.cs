using UnityEngine;
using UnityEngine.UI;
using QuickJam.UI;

public class TestUI : UIBase
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _deleteSaveButton;
    
    public System.Action OnStartButtonClicked;
    public System.Action OnDeleteSaveButtonClicked;

    private void Awake()
    {
        _startButton.onClick.AddListener(() =>
        {
            OnStartButtonClicked?.Invoke();
            // todo: close
        });
        
        _deleteSaveButton.onClick.AddListener(() =>
        {
            OnDeleteSaveButtonClicked?.Invoke();
        });
    }
}