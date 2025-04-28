using UnityEngine;
using QuickJam.Core;
using System;

namespace QuickJam.Input
{
    public class InputManager : MonoSingleton<InputManager>
    {
        protected override bool ShouldDontDestroyOnLoad => true;

        private QuickJamInputActions _inputActions;

        public event Action OnPausePressed;
        public event Action OnSubmitPressed;
        // add more event here...
        
        protected override void Awake()
        {
            base.Awake();
            _inputActions = new QuickJamInputActions();
        }

        private void OnEnable()
        {
            _inputActions.Enable();

            _inputActions.Gameplay.Pause.performed += ctx => OnPausePressed?.Invoke();
            _inputActions.Gameplay.Submit.performed += ctx => OnSubmitPressed?.Invoke();
        }

        private void OnDisable()
        {
            _inputActions.Gameplay.Pause.performed -= ctx => OnPausePressed?.Invoke();
            _inputActions.Gameplay.Submit.performed -= ctx => OnSubmitPressed?.Invoke();

            _inputActions.Disable();
        }
    }
}
