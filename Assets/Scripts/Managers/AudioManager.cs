using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace GameFramework
{
    /// <summary>
    /// Manages the audio for the game.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        #region Private Declarations

        /// <summary> The audio mixer that controls the volume for the audio sources. </summary>
        [SerializeField][Tooltip("The audio mixer that controls the volume for the audio sources")]
        private AudioMixer _masterAudioMixer;

        /// <summary> A list of music audio clips to use for the game. </summary>
        [SerializeField] [Tooltip("A list of music audio clips to use for the game")]
        private List<AudioClipKeyValuePair> _musicAudioClips = new List<AudioClipKeyValuePair>();

        /// <summary> A list of sfx audio clips to use for the game. </summary>
        [SerializeField][Tooltip("A list of sfx audio clips to use for the game")]
        private List<AudioClipKeyValuePair> _sfxAudioClips = new List<AudioClipKeyValuePair>();

        #endregion

        #region Protected Declarations


        #endregion

        #region Public Declarations

        /// <summary> Denotes the instance of the audio manager to make calls from. </summary>
        public static AudioManager Instance { get; private set; }

        #endregion

        #region MonoBehaviour

        private void Awake () {
            if (Instance != null && Instance != this) {
                Debug.LogWarning("An instance of the AudioManager already exisits!");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        #region Private Methods


        #endregion

        #region Protected Methods


        #endregion

        #region Public Methods

        public AudioClip GetMusicByName (string musicName) {
            return _musicAudioClips.Find(x => x.Name == musicName).Clip;
        }

        public AudioClip GetMusicByIndex (int index) {
            return _musicAudioClips[index].Clip;
        }

        public AudioClip GetSfxByName (string sfxName) {
            return _sfxAudioClips.Find(x => x.Name == sfxName).Clip;
        }

        public AudioClip GetSfxByIndex (int index) {
            return _sfxAudioClips[index].Clip;
        }

        #endregion

        #region Internal Classes

        /// <summary>
        /// A scriptable object that holds the name and object reference for an AudioClip.
        /// </summary>
        public class AudioClipKeyValuePair : ScriptableObject
        {
            public string Name;
            public AudioClip Clip;
        }

        #endregion
    }
}
