using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework
{
    /// <summary>
    /// Defines the game mode being used.
    /// </summary>
    public enum GameModes
    {
        Demo,
        Mobile,
        Release
    }

    /// <summary>
    /// Manages the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Private Declarations

        /// <summary> Denotes the game mode that is to be used. </summary>
        [SerializeField][Tooltip("Denotes the game mode that is to be used")]
        private GameModes _gameMode = GameModes.Demo;

        #endregion

        #region Public Declarations

        /// <summary> Denotes the instance of the game manager to make calls from. </summary>
        public static GameManager Instance { get; private set; }

        /// <summary> Denotes the current game mode that is being used. </summary>
        public GameModes GameMode { get { return _gameMode; } }

        /// <summary> Denotes if the game is paused or not. </summary>
        public bool GamePaused { get; private set; }

        /// <summary> Denotes if the game is in the main scene. </summary>
        public bool InMainScene { get; private set; }

        /// <summary> Denotes if the game is on a mobile device. </summary>
        public bool IsMobile { get; private set; }

        #endregion

        #region MonoBehaviour

        private void Awake () {
            if (Instance != null && Instance != this) {
                Debug.LogWarning("An instance of the GameManager already exisits!");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Check if game is on a mobile device
#if UNITY_ANDROID || UNITY_IOS
            IsMobile = true;
#else
            IsMobile = false;
#endif

            // Register to when a scene has been loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Is called when a scene has loaded.
        /// </summary>
        private void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
            // If scene was additively load, return
            if (mode == LoadSceneMode.Additive) {
                return;
            }

            // Check if in the title scene
            if (scene.buildIndex == 0) {
                InMainScene = true;
            }
        }

        /// <summary>
        /// Is called when the game in Demo mode ends.
        /// </summary>
        private void EndGameDemo () {

        }

        /// <summary>
        /// Is called when the game in Mobile mode ends.
        /// </summary>
        private void EndGameMobile () {

        }

        /// <summary>
        /// Is called when the game in Release mode ends.
        /// </summary>
        private void EndGameRelease () {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Pauses the game.
        /// </summary>
        public void Pause () {
            // Check is game is already paused
            if (!GamePaused) {
                // Set isPaused to true
                GamePaused = true;

                // Set time.timescale to 0, this will cause animations and physics to stop updating
                Time.timeScale = 0;
            }
        }

        /// <summary>
        /// Unpauses the game.
        /// </summary>
        public void UnPause () {
            // Check is game is paused
            if (GamePaused) {
                // Set isPaused to false
                GamePaused = false;

                // Set time.timescale to 1, this will cause animations and physics to continue updating at regular speed
                Time.timeScale = 1;
            }
        }

        /// <summary>
        /// Toggles the pause state of the game.
        /// </summary>
        public void TogglePause () {
            // Check is game is paused or not
            if (GamePaused) {
                UnPause();
            }
            else {
                Pause();
            }
        }

        /// <summary>
        /// Call the "Quit" function after a delay.
        /// </summary>
        public void DelayQuit (float delay) {
            Invoke("Quit", delay);
        }

        /// <summary>
        /// Quit the game.
        /// </summary>
        public void Quit () {
            // Check if we are running in the editor or not
#if UNITY_EDITOR
        // Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // Quit the application
            Application.Quit();
#endif
        }

        /// <summary>
        /// Is called when the game has ended.
        /// </summary>
        public void GameHasEnded () {
            switch (GameMode) {
                case GameModes.Demo:
                    EndGameDemo();
                    break;
                case GameModes.Mobile:
                    EndGameMobile();
                    break;
                case GameModes.Release:
                    EndGameRelease();
                    break;
            }
        }

        #endregion
    }
}
