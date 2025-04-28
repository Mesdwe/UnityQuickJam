using System;
using QuickJam.Core;

namespace QuickJam.Input
{
    public class InputManager : MonoSingleton<InputManager>
    {
        private QuickJamInputActions _inputActions;

        protected override bool ShouldDontDestroyOnLoad => true;
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

        public event Action OnPausePressed;
        public event Action OnSubmitPressed;
    }
}