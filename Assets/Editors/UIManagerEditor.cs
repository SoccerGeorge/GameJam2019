using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GameFramework;

[CustomEditor(typeof(UIManager), true)][CanEditMultipleObjects]
public class UIManagerEditor : Editor
{
    #region Protected Declarations

    // Serialized properties
    protected SerializedProperty menuListProp;
    protected SerializedProperty activeMenuIndexProp;

    // Helper variables
    protected Texture2D textureDefault;
    protected Texture2D textureSelected;
    protected int canvasIdxToDelete = 0;

    #endregion

    #region Public Declarations

    /// <summary> Used to draw default inspector. </summary>
    public bool SeeDefaultInspector = false;

    /// <summary> Used to create a menu page. </summary>
    public bool CreatePage = false;

    /// <summary> Use to delete a menu page. </summary>
    public bool DeletePage = false;

    #endregion

    #region Editor

    void OnEnable () {
        // Setup the SerializedProperties
        menuListProp = serializedObject.FindProperty("MenuList");
        activeMenuIndexProp = serializedObject.FindProperty("ActiveMenuIndex");

        // Setup the highlighting textures
        textureDefault = MakeTexture(2, 2, new Color(0.5f, 0f, 1f, 0.2f));
        textureSelected = MakeTexture(2, 2, new Color(0.5f, 0f, 1f, 1f));
    }

    public override void OnInspectorGUI () {
        // Create a foldout dropdown of the default inspector
        SeeDefaultInspector = EditorGUILayout.Foldout(SeeDefaultInspector, "Default Inspector");

        // If true Default Inspector is drawn
        if (SeeDefaultInspector) {
            DrawDefaultInspector();
        }

        // Update the object in inspector
        serializedObject.Update();

        // Get the target script
        UIManager targetScript = (UIManager)target;

        // Create GUIStyles for highlighting
        GUIStyle styleDefault = new GUIStyle(GUI.skin.box);
        GUIStyle styleSelected = new GUIStyle(GUI.skin.box);
        styleDefault.normal.background = textureDefault;
        styleSelected.normal.background = textureSelected;

        // Add some spacing
        GUILayout.Space(10f);

        // Begin GUI layout for list
        EditorGUILayout.BeginVertical();

        // Create a label for the menu list
        GUIStyle labelStyle = new GUIStyle() { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
        EditorGUILayout.LabelField(new GUIContent("Menu List", "A list of UI menus to use for the game"), labelStyle);

        for (int i = 0; i < menuListProp.arraySize; ++i) {
            // Begin GUI layout for list element
            EditorGUILayout.BeginVertical(styleDefault);

            // Check if the menu page is active and begin selected GUI layout
            string curState = "Off";
            string curTooltip = "Select to make active menu page";
            if (targetScript.MenuList[i].alpha == 1) {
                curState = "On";
                curTooltip = "The active menu page";
                EditorGUILayout.BeginHorizontal(styleSelected);
            }
            else {
                EditorGUILayout.BeginHorizontal();
            }

            // Create the state toggle button
            if (GUILayout.Button(new GUIContent(curState, curTooltip), GUILayout.Width(40f))) {
                // Toggle the state of all the menu pages to the appropriate state
                for (int j = 0; j < targetScript.MenuList.Count; ++j) {
                    // Check if menu page is the one being selected
                    if (j == i) {
                        // Set the menu page to be shown and active
                        targetScript.MenuList[i].alpha = 1;
                        targetScript.MenuList[i].gameObject.SetActive(true);
                        activeMenuIndexProp.intValue = i;
                    }
                    else {
                        // Set the menu page to be hidden and inactive
                        targetScript.MenuList[j].alpha = 0;
                        targetScript.MenuList[j].gameObject.SetActive(false);
                    }
                }
            }

            // Create the property field for the menu page
            EditorGUILayout.PropertyField(menuListProp.GetArrayElementAtIndex(i), new GUIContent(""), GUILayout.Width(200));

            // Add some spacing
            GUILayout.Space(30f);

            // Create the delete menu page button
            if (GUILayout.Button("Delete page", GUILayout.Width(100f))) {
                DeletePage = true;
                canvasIdxToDelete = i;
                break;
            }

            // TODO: Look into adding arrows to change positioning in list

            // Add ending spacing
            GUILayout.Label("");

            // End GUI layouts for list elements
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        // End GUI layout for list
        EditorGUILayout.EndVertical();

        // Add some spacing
        GUILayout.Space(20f);

        // Create the create a new page button
        if (GUILayout.Button("Create a new Menu page")) {
            CreatePage = true;
        }

        // Add some spacing
        GUILayout.Space(10f);

        // Check if creating a new page
        if (CreatePage) {
            // Reset create page flag
            CreatePage = false;

            // Set all menu pages to be hidden and inactive
            for (int i = 0; i < menuListProp.arraySize; i++) {
                targetScript.MenuList[i].alpha = 0f;
                targetScript.MenuList[i].gameObject.SetActive(false);
            }

            // Create new menu page
            GameObject newMenuGO = new GameObject("NewMenuGO", typeof(RectTransform));
            newMenuGO.transform.SetParent(targetScript.gameObject.transform);

            // Setup new menu page
            newMenuGO.AddComponent<CanvasRenderer>();
            newMenuGO.AddComponent<CanvasGroup>();

            newMenuGO.name = "NewMenuPage_" + menuListProp.arraySize;

            newMenuGO.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            newMenuGO.GetComponent<RectTransform>().offsetMax = Vector2.zero;

            newMenuGO.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            newMenuGO.GetComponent<RectTransform>().anchorMax = Vector2.one;
            newMenuGO.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            newMenuGO.GetComponent<RectTransform>().localScale = Vector3.one;

            // Update menu page list
            menuListProp.arraySize = menuListProp.arraySize + 1;
            menuListProp.GetArrayElementAtIndex(menuListProp.arraySize - 1).objectReferenceValue = newMenuGO.GetComponent<CanvasGroup>();
        }

        // Check if deleting a page
        if (DeletePage) {
            // Reset delete page flag
            DeletePage = false;

            // Create a temporary menu page list and remove correct menu page
            List<CanvasGroup> tmpList = new List<CanvasGroup>();
            for (int i = 0; i < menuListProp.arraySize; i++) {
                if (i != canvasIdxToDelete) {
                    tmpList.Add((CanvasGroup)menuListProp.GetArrayElementAtIndex(i).objectReferenceValue);

                }
                else {
                    DestroyImmediate(targetScript.MenuList[i].gameObject);
                }
            }

            // Update menu page list
            menuListProp.arraySize = menuListProp.arraySize - 1;
            for (int i = 0; i < tmpList.Count; i++) {
                menuListProp.GetArrayElementAtIndex(i).objectReferenceValue = tmpList[i];
            }
        }

        // Apply changes to inspector
        serializedObject.ApplyModifiedProperties();
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Creates a texture for the GUIStyle.
    /// </summary>
    protected Texture2D MakeTexture (int width, int height, Color color) {
        // Create pixel colors
        Color[] pixelColors = new Color[width * height];
        for (int i = 0; i < pixelColors.Length; ++i) {
            pixelColors[i] = color;
        }

        // Create texture with color
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixelColors);
        texture.Apply();

        return texture;
    }

    #endregion
}
