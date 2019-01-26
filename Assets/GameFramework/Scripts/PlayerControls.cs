using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace GameFramework.Controls
{
    /// <summary>
    /// The list of controls for the player.
    /// Need to update this for every project.
    /// </summary>
    public enum ControlKey
    {
        Left,
        Right,
        Forward,
        Back,
        Pause,
        Confirm,
        Cancel
    }

    /// <summary>
    /// The list of gamepads supported for the player.
    /// Need to update this for every project.
    /// </summary>
    public enum GamePadType
    {
        XBox,
        Other
    }

    public class PlayerControls
    {
        #region Private Declarations


        #endregion

        #region Protected Declarations

        protected string controlPrefix;
        protected int playerIndex;
        protected string left;
        protected string right;
        protected string forward;
        protected string back;
        protected string pause;
        protected string confirm;
        protected string cancel;

        #endregion

        #region Public Declarations


        #endregion

        #region Private Methods


        #endregion

        #region Protected Methods


        #endregion

        #region Public Methods

        public PlayerControls () { }

        public virtual bool UpdateControl (ControlKey controlKey, string newControl) {
            if (newControl.IsInList(left, right, forward, back, pause, confirm, cancel)) {
                return false;
            }

            switch (controlKey) {
                case ControlKey.Left:
                    left = newControl;
                    break;
                case ControlKey.Right:
                    right = newControl;
                    break;
                case ControlKey.Forward:
                    forward = newControl;
                    break;
                case ControlKey.Back:
                    back = newControl;
                    break;
                case ControlKey.Pause:
                    pause = newControl;
                    break;
                case ControlKey.Confirm:
                    confirm = newControl;
                    break;
                case ControlKey.Cancel:
                    cancel = newControl;
                    break;
            }

            return true;
        }

        public virtual void ResetControls () { }

        #endregion
    }

    public class GamePadControls : PlayerControls
    {
        #region Private Declarations

        #endregion

        #region Protected Declarations

        #endregion

        #region Public Declarations

        public GamePadType gamePad;

        #endregion

        #region Private Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Public Methods

        public GamePadControls (int playerId, GamePadType padType) {
            playerIndex = playerId;
            gamePad = padType;
            controlPrefix = "{0}_GamePad_{1}_".FormatStr(gamePad, playerIndex);

            left = "";
            right = "";
            forward = "";
            back = "";
            pause = "";
            confirm = "";
            cancel = "";
        }

        public override void ResetControls () {
            switch (gamePad) {
                case GamePadType.XBox:
                    break;
                default:
                    break;
            }
        }

        #endregion
    }

    public class KeyboardControls : PlayerControls
    {
        #region Private Declarations

        private Dictionary<string, KeyCode> _stringToKeyCode;

        #endregion

        #region Protected Declarations


        #endregion

        #region Public Declarations

        public KeyCode LeftKeyCode { get { return _stringToKeyCode[left]; } }
        public KeyCode RightKeyCode { get { return _stringToKeyCode[right]; } }
        public KeyCode ForwardKeyCode { get { return _stringToKeyCode[forward]; } }
        public KeyCode BackKeyCode { get { return _stringToKeyCode[back]; } }
        public KeyCode PauseKeyCode { get { return _stringToKeyCode[pause]; } }
        public KeyCode ConfirmKeyCode { get { return _stringToKeyCode[confirm]; } }
        public KeyCode CancelKeyCode { get { return _stringToKeyCode[cancel]; } }

        #endregion

        #region Private Methods


        #endregion

        #region Protected Methods

        protected KeyCode FindKeyCodeFromString (string keyCodeStr) {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode))) {
                if (key.ToString() == keyCodeStr) {
                    return key;
                }
            }
            return KeyCode.None;
        }

        protected void UpdateStringToKeyCodeDic () {
            _stringToKeyCode.Clear();

            string[] controls = new string[] { left, right, forward, back, pause, confirm, cancel };

            foreach (string str in controls) {
                _stringToKeyCode[str] = FindKeyCodeFromString(str);
            }
        }

        #endregion

        #region Public Methods

        public KeyboardControls (int playerId) {
            _stringToKeyCode = new Dictionary<string, KeyCode>();

            playerIndex = playerId;
            controlPrefix = "Keyboard_{0}_".FormatStr(playerIndex);

            left = "LeftArrow";
            right = "RightArrow";
            forward = "UpArrow";
            back = "DownArrow";
            pause = "Escape";
            confirm = "Enter";
            cancel = "Backspace";

            UpdateStringToKeyCodeDic();
        }

        public override bool UpdateControl (ControlKey controlKey, string newControl) {
            bool controlsUpdated = base.UpdateControl(controlKey, newControl);

            if (controlsUpdated) {
                UpdateStringToKeyCodeDic();
            }

            return controlsUpdated;
        }

        public override void ResetControls () {
            left = "LeftArrow";
            right = "RightArrow";
            forward = "UpArrow";
            back = "DownArrow";
            pause = "Escape";
            confirm = "Enter";
            cancel = "Backspace";

            UpdateStringToKeyCodeDic();
        }

        #endregion
    }
}
