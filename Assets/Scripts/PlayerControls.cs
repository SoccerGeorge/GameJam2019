using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace GameFramework.Controls
{
    public enum ControlKey
    {
        LeftTurn,
        RightTurn,
        Forward,
        Reverse,
        HandBrake,
        Grab,
        ShowList,
        Pause,
        Confirm,
        Cancel,
        //PowerUp,
        //Respawn
    }

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

        #endregion

        #region Public Declarations

        public string controlPrefix;
        public int playerIndex;
        public string leftTurn;
        public string rightTurn;
        public string forward;
        public string reverse;
        public string handBrake;
        public string grab;
        public string showList;
        public string pause;
        public string confirm;
        public string cancel;
        //public string powerUp;
        //public string respawn;

        #endregion

        #region Private Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Public Methods

        public PlayerControls () { }

        public virtual bool UpdateControl (ControlKey controlKey, string newControl) {
            if (newControl.IsInList(leftTurn, rightTurn, forward, reverse, handBrake, grab, showList, pause, confirm, cancel)) {
                return false;
            }

            switch (controlKey) {
                case ControlKey.LeftTurn:
                    leftTurn = newControl;
                    break;
                case ControlKey.RightTurn:
                    rightTurn = newControl;
                    break;
                case ControlKey.Forward:
                    forward = newControl;
                    break;
                case ControlKey.Reverse:
                    reverse = newControl;
                    break;
                case ControlKey.HandBrake:
                    handBrake = newControl;
                    break;
                case ControlKey.Grab:
                    grab = newControl;
                    break;
                case ControlKey.ShowList:
                    showList = newControl;
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

            leftTurn = "";
            rightTurn = "";
            forward = "";
            reverse = "";
            handBrake = "";
            grab = "";
            showList = "";
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

        public KeyCode LeftTurnKeyCode { get { return _stringToKeyCode[leftTurn]; } }
        public KeyCode RightTurnKeyCode { get { return _stringToKeyCode[rightTurn]; } }
        public KeyCode ForwardKeyCode { get { return _stringToKeyCode[forward]; } }
        public KeyCode ReverseKeyCode { get { return _stringToKeyCode[reverse]; } }
        public KeyCode HandBrakeKeyCode { get { return _stringToKeyCode[handBrake]; } }
        public KeyCode GrabKeyCode { get { return _stringToKeyCode[grab]; } }
        public KeyCode ShowListKeyCode { get { return _stringToKeyCode[showList]; } }
        public KeyCode PauseKeyCode { get { return _stringToKeyCode[pause]; } }
        public KeyCode ConfirmKeyCode { get { return _stringToKeyCode[confirm]; } }
        public KeyCode CancelKeyCode { get { return _stringToKeyCode[cancel]; } }

        #endregion

        #region Private Methods

        private KeyCode FindKeyCodeFromString (string keyCodeStr) {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode))) {
                if (key.ToString() == keyCodeStr) {
                    return key;
                }
            }
            return KeyCode.None;
        }

        private void UpdateStringToKeyCodeDic () {
            _stringToKeyCode.Clear();

            string[] controls = new string[] { leftTurn, rightTurn, forward, reverse, handBrake, grab, showList, pause, confirm, cancel };

            foreach (string str in controls) {
                _stringToKeyCode[str] = FindKeyCodeFromString(str);
            }
        }

        #endregion

        #region Protected Methods

        #endregion

        #region Public Methods

        public KeyboardControls (int playerId) {
            _stringToKeyCode = new Dictionary<string, KeyCode>();

            playerIndex = playerId;
            controlPrefix = "Keyboard_{1}_".FormatStr(playerIndex);

            leftTurn = "LeftArrow";
            rightTurn = "RightArrow";
            forward = "UpArrow";
            reverse = "DownArrow";
            handBrake = "LeftControl";
            grab = "LefShift";
            showList = "Tab";
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
            leftTurn = "LeftArrow";
            rightTurn = "RightArrow";
            forward = "UpArrow";
            reverse = "DownArrow";
            handBrake = "LeftControl";
            grab = "LefShift";
            showList = "Tab";
            pause = "Escape";
            confirm = "Enter";
            cancel = "Backspace";

            UpdateStringToKeyCodeDic();
        }

        #endregion
    }
}
