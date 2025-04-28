
using UnityEngine;
using QuickJam.Input;

namespace QuickJam.Core
{
    public enum GameState
    {
        Title,
        Playing,
        Paused,
        GameOver
    }


    namespace QuickJam.Core
    {
        public class GameManager : MonoSingleton<GameManager>
        {
            public GameState CurrentState { get; private set; }
        
            protected override bool ShouldDontDestroyOnLoad => true;

            private void Start()
            {
                ChangeState(GameState.Title);

                InputManager.Instance.OnPausePressed += HandlePausePressed;
            }

            private void OnDestroy()
            {
                if (InputManager.Instance != null)
                {
                    InputManager.Instance.OnPausePressed -= HandlePausePressed;
                }
            }

            private void HandlePausePressed()
            {
                if (CurrentState == GameState.Playing)
                {
                    PauseGame();
                }
                else if (CurrentState == GameState.Paused)
                {
                    ResumeGame();
                }
            }

            public void ChangeState(GameState newState)
            {
                CurrentState = newState;
                Debug.Log($"Game State changed to {newState}");
                Time.timeScale = (CurrentState == GameState.Paused) ? 0f : 1f;
            }

            public void StartGame()
            {
                ChangeState(GameState.Playing);
            }

            public void PauseGame()
            {
                ChangeState(GameState.Paused);
            }

            public void ResumeGame()
            {
                ChangeState(GameState.Playing);
            }

            public void GameOver()
            {
                ChangeState(GameState.GameOver);
            }

            public void ReturnToTitle()
            {
                SceneLoader.Instance.LoadSceneAsync("Frontend");
                ChangeState(GameState.Title);
            }
        }
    }

}
