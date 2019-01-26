using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Controls;

namespace GameFramework
{
    public abstract class Player : MonoBehaviour
    {
        #region Private Declarations


        #endregion

        #region Protected Declarations


        #endregion

        #region Public Declarations

        public bool UsingKeyboard { get; protected set; }
        public bool UsingGamePad { get; protected set; }
        public int PlayerId { get; set; }
        public PlayerControls Controls { get; protected set; }

        #endregion

        #region MonoBehaviour

        protected abstract void Start ();

        #endregion

        #region Private Methods


        #endregion

        #region Protected Methods

        protected abstract void Move ();

        #endregion

        #region Public Methods


        #endregion
    }
}
