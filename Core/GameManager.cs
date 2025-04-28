using QuickJam.Input;
using UnityEngine;

namespace QuickJam.Core
{
    public enum GameState
    {
        Title,
        Playing,
        Paused,
        GameOver
    }

    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private string titleSceneName = "Frontend";
        [SerializeField] private string gameplaySceneName = "InGame";
        public GameState CurrentState { get; private set; }

        protected override bool ShouldDontDestroyOnLoad => true;

        private void Start()
        {
            ChangeState(GameState.Title);

            InputManager.Instance.OnPausePressed += HandlePausePressed;
        }

        private void OnDestroy()
        {
            if (InputManager.Instance != null) InputManager.Instance.OnPausePressed -= HandlePausePressed;
        }

        private void HandlePausePressed()
        {
            if (CurrentState == GameState.Playing)
                PauseGame();
            else if (CurrentState == GameState.Paused) ResumeGame();
        }

        public void ChangeState(GameState newState)
        {
            if (CurrentState == newState)
                return;

            switch (CurrentState)
            {
                case GameState.Title:
                // Intentional fallthrough
                case GameState.GameOver:
                    if (newState == GameState.Playing)
                        StartGame();
                    if (newState == GameState.Title)
                        ReturnToTitle();
                    break;
                case GameState.Playing:
                    if (newState == GameState.Paused)
                        PauseGame();
                    else if (newState == GameState.GameOver)
                        GameOver();
                    break;
                case GameState.Paused:
                    if (newState == GameState.Playing)
                        ResumeGame();
                    break;
                default:
                    Debug.LogError($"Unhandled Game State: {newState}");
                    return;
            }

            Debug.Log($"Game State changed to {newState}");
            Time.timeScale = CurrentState == GameState.Paused ? 0f : 1f;
        }

        private void StartGame()
        {
            SceneLoader.Instance.LoadSceneAsync(gameplaySceneName);
            CurrentState = GameState.Playing;
        }

        private void PauseGame()
        {
            CurrentState = GameState.Paused;
        }

        private void ResumeGame()
        {
            CurrentState = GameState.Playing;
        }

        private void GameOver()
        {
            CurrentState = GameState.GameOver;
        }

        private void ReturnToTitle()
        {
            SceneLoader.Instance.LoadSceneAsync(titleSceneName);
            CurrentState = GameState.Title;
        }
    }
}