using System.Collections.Generic;
using UnityEngine;
using GameFramework.Controls;

namespace GameFramework
{
    /// <summary>
    /// Manages the players for the game.
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        #region Private Declarations

        /// <summary> A list of player prefabs to use for the game. </summary>
        [SerializeField][Tooltip("A list of player prefabs to use for the game")]
        private List<GameObject> _playerPrefabs = new List<GameObject>();

        /// <summary> A list of active players in the game. </summary>
        private List<Player> _players = new List<Player>();

        /// <summary> The default controls for players in the game. </summary>
        private PlayerControls _defaultPlayerControl = null;

        #endregion

        #region Protected Declarations


        #endregion

        #region Public Declarations

        /// <summary> Denotes the instance of the player manager to make calls from. </summary>
        public static PlayerManager Instance { get; private set; }

        #endregion

        #region MonoBehaviour

        private void Awake () {
            if (Instance != null && Instance != this) {
                Debug.LogWarning("An instance of the PlayerManager already exisits!");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start () {
            CreatePlayerForLevel(0);
        }

        #endregion

        #region Private Methods


        #endregion

        #region Protected Methods


        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new player for the game.
        /// </summary>
        public Player CreatePlayerForLevel (int playerPrefabId, Vector3? startPosition = null, Quaternion? startRotation = null) {
            GameObject newPlayerGO = Instantiate(_playerPrefabs[playerPrefabId], startPosition ?? Vector3.zero, startRotation ?? Quaternion.identity);
            Player newPlayer = newPlayerGO.GetComponent<Player>();
            newPlayer.PlayerId = _players.Count;
            _players.Add(newPlayer);
            return newPlayer;
        }

        /// <summary>
        /// Gets a player in the game.
        /// </summary>
        public Player GetPlayer (int playerId) {
            return _players[playerId];
        }

        /// <summary>
        /// Gets the default controls for players.
        /// </summary>
        public PlayerControls GetDefaultPlayerControl () {
            return _defaultPlayerControl;
        }

        #endregion
    }
}
