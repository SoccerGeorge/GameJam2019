using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameFramework
{
    /// <summary>
    /// Manages the scene loading for the game.
    /// </summary>
    public class SceneLoadManager : MonoBehaviour
    {
        #region Private Declarations

        /// <summary> The progress bar to update while a scene is loading. </summary>
        [SerializeField][Tooltip("The progress bar to update while a scene is loading")]
        private Slider _progressBar = null;

        /// <summary> The asynchronous operation for loading a scene. </summary>
        private AsyncOperation _asyncOp;

        #endregion

        #region Public Declarations

        /// <summary> Denotes the instance of the level manager to make calls from. </summary>
        public static SceneLoadManager Instance = null;

        /// <summary> Denotes the current scene index. </summary>
        public int CurrentSceneIdx { get; private set; }

        /// <summary> Denotes the current scene name. </summary>
        public string CurrentSceneName { get; private set; }

        #endregion

        #region MonoBehaviour

        private void Awake () {
            if (Instance != null && Instance != this) {
                Debug.LogWarning("An instance of the SceneLoadManager already exisits!");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Register to when the active scene has changed
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            // Register to when a scene has been loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Is called when the active scene has changed.
        /// </summary>
        private void OnActiveSceneChanged (Scene currentScene, Scene nextScene) {

        }

        /// <summary>
        /// Is called when a scene has loaded.
        /// </summary>
        private void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
            // If scene was additively load, return
            if (mode == LoadSceneMode.Additive) {
                return;
            }

            // Update the current scene index and name
            CurrentSceneIdx = scene.buildIndex;
            CurrentSceneName = scene.name;
        }

        /// <summary>
        /// Updates the progress bar as the scene loads.
        /// </summary>
        private IEnumerator UpdateProgressBar () {
            // Wait until the async operation fully loads
            while (!_asyncOp.isDone) {
                // Update the progress bar
                _progressBar.value = _asyncOp.progress;
                yield return null;
            }

            // Reset the the async operation
            _asyncOp = null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the scene asynchronous by name.
        /// </summary>
        public void LoadSceneAsync (string sceneName) {
            _asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            if (_progressBar != null) {
                StartCoroutine(UpdateProgressBar());
            }
        }

        /// <summary>
        /// Loads the scene asynchronous by index.
        /// </summary>
        public void LoadSceneAsync (int sceneBuildIndex) {
            _asyncOp = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Single);

            if (_progressBar != null) {
                StartCoroutine(UpdateProgressBar());
            }
        }

        /// <summary>
        /// Loads the scene additive asynchronous by name.
        /// </summary>
        public void LoadSceneAdditiveAsync (string sceneName) {
            _asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Loads the scene additive asynchronous by index.
        /// </summary>
        public void LoadSceneAdditiveAsync (int sceneBuildIndex) {
            _asyncOp = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Unloads the scene asynchronous by name.
        /// </summary>
        public void UnloadScene (string sceneName) {
            SceneManager.UnloadSceneAsync(sceneName);
        }

        /// <summary>
        /// Unloads the scene asynchronous by index.
        /// </summary>
        public void UnloadScene (int sceneBuildIndex) {
            SceneManager.UnloadSceneAsync(sceneBuildIndex);
        }

        #endregion
    }
}
