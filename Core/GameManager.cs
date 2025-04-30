using System;
using System.Collections.Generic;
using UnityEngine;
using QuickJam.Input;
using QuickJam.Save;
using QuickJam.UI;

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
        [Header("Scene Names")]
        [SerializeField] private string titleSceneName = "Frontend";
        [SerializeField] private string gameplaySceneName = "InGame";

        public GameState CurrentState { get; private set; }

        public event Action<GameState> OnStateChanged;

        protected override bool ShouldDontDestroyOnLoad => true;

        private bool _initialized = false;

        private readonly Dictionary<GameState, string> _sceneMap = new();

        private void Start()
        {
            if (_initialized) return;
            _initialized = true;

            _sceneMap[GameState.Title] = titleSceneName;
            _sceneMap[GameState.Playing] = gameplaySceneName;

            InputManager.Instance.OnPausePressed += HandlePausePressed;

            ChangeState(GameState.Title);
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
                PauseGame();
            else if (CurrentState == GameState.Paused)
                ResumeGame();
        }

        public bool ChangeState(GameState newState)
        {
            if (CurrentState == newState)
                return false;

            HandleExitState(CurrentState);
            HandleEnterState(newState);

            CurrentState = newState;
            Debug.Log($"[GameManager] Game State changed to {newState}");

            OnStateChanged?.Invoke(CurrentState);
            Time.timeScale = (CurrentState == GameState.Paused) ? 0f : 1f;

            return true;
        }

        private void HandleExitState(GameState currentState)
        {
            // TODO
        }

        private void HandleEnterState(GameState newState)
        {
            switch (newState)
            {
                case GameState.Playing:
                case GameState.Title:
                    if (_sceneMap.TryGetValue(newState, out var sceneName))
                        SceneLoader.Instance.LoadSceneAsync(sceneName);
                    break;

                case GameState.GameOver:
                    break;

                case GameState.Paused:
                    break;
            }
        }

        // public methods for changing game state

        public void StartGame()
        {
            if (!ChangeState(GameState.Playing)) return;
            UIManager.Instance.CloseAllUI();

            // TODO: move this to somewhere else
            var saveName = "player";
            if (SaveSystem.Instance.Exists(saveName))
            {
                GameData data = SaveSystem.Instance.Load<GameData>(saveName);
                Debug.Log($"ID: {data.playerID}, PlayTime: {data.playTime}");
            }
            else
            {
                Debug.Log("No save file found, creating new game");
                // temp test
                var data = new GameData
                {
                    playerID = 1,
                    playTime = 87.3f,
                    isFirstTime = true
                };
                
                SaveSystem.Instance.Save("player", data);
            }
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
            ChangeState(GameState.Title);
        }
    }
}
