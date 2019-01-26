using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    /// <summary>
    /// Manages the UI for the game.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        #region Private Declarations


        #endregion

        #region Protected Declarations

        protected IEnumerator menuSwitchCoroutine;
        protected bool runningSwitchCoroutine;

        #endregion

        #region Public Declarations

        /// <summary> Denotes the instance of the UI manager to make calls from. </summary>
        public static UIManager Instance { get; private set; }

        /// <summary> A list of UI menus to use for the game. Not to be called outside of class. </summary>
        [Tooltip("A list of UI menus to use for the game")]
        public List<CanvasGroup> MenuList; // TODO: possibly make it a list of a different class to control more aspects

        /// <summary> The active UI menu in the game. Not to be set outside of class. </summary>
        [Tooltip("The active UI menu in the game")]
        public int ActiveMenuIndex;

        #endregion

        #region MonoBehaviour

        private void Awake () {
            if (Instance != null && Instance != this) {
                Debug.LogWarning("An instance of the UIManager already exisits!");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        #region Private Methods

        private IEnumerator SwitchActiveMenu (CanvasGroup previousMenu, CanvasGroup nextMenu) {
            runningSwitchCoroutine = true;
            if (previousMenu != nextMenu) {
                // Disable previous menu
                previousMenu.interactable = false;
                previousMenu.blocksRaycasts = false;
                previousMenu.alpha = 0;
                previousMenu.gameObject.SetActive(false);

                // Enable next menu
                nextMenu.gameObject.SetActive(true);
                nextMenu.blocksRaycasts = true;
                nextMenu.alpha = 1;
                nextMenu.interactable = true;
            }

            runningSwitchCoroutine = false;
            yield return null;
        }

        #endregion

        #region Protected Methods


        #endregion

        #region Public Methods

        /// <summary>
        /// Switches the active menu to the given menu.
        /// </summary>
        public void SwitchToMenu (CanvasGroup newMenu) {
            // Check if not running the switch coroutine
            if (!runningSwitchCoroutine) {
                // Check if given menu is in list
                if (MenuList.Contains(newMenu)) {
                    menuSwitchCoroutine = SwitchActiveMenu(MenuList[ActiveMenuIndex], newMenu);

                    ActiveMenuIndex = MenuList.FindIndex(menu => menu == newMenu);

                    StartCoroutine(menuSwitchCoroutine);
                }
            }
        }

        /// <summary>
        /// Switches the active menu to the given menu index.
        /// </summary>
        public void SwitchToMenuByIndex (int newMenuIndex) {
            // Check if not running the switch coroutine
            if (!runningSwitchCoroutine) {
                // Check if given menu index is within the list count
                if (newMenuIndex > -1 && newMenuIndex < MenuList.Count) {
                    menuSwitchCoroutine = SwitchActiveMenu(MenuList[ActiveMenuIndex], MenuList[newMenuIndex]);

                    ActiveMenuIndex = newMenuIndex;

                    StartCoroutine(menuSwitchCoroutine);
                }
            }
        }

        #endregion
    }
}
