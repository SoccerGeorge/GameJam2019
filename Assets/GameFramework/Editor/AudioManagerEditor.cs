using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Extensions;
using GameFramework;

#if UNITY_EDITOR
[CustomEditor(typeof(AudioManager), true)][CanEditMultipleObjects]
public class AudioManagerEditor : Editor
{
    #region Protected Declarations

    /// <summary> The serialized property for the AudioMixer reference. </summary>
    protected SerializedProperty audioMixerProp;

    /// <summary> The serialized property for the music AudioClip list reference. </summary>
    protected SerializedProperty musicAudioClipListProp;

    /// <summary> The serialized property for the sfx AudioClip list reference. </summary>
    protected SerializedProperty sfxAudioClipListProp;

    /// <summary> Used to draw default inspector. </summary>
    protected bool seeDefaultInspector = false;

    /// <summary> Denotes to create a music audio clip reference. </summary>
    protected bool createMusicAudioClip = false;

    /// <summary> Denotes to create a sfx audio clip reference. </summary>
    protected bool createSfxAudioClip = false;

    /// <summary> Denotes to delete a music audio clip reference. </summary>
    protected bool deleteMusicAudioClip = false;

    /// <summary> Denotes to delete a sfx audio clip reference. </summary>
    protected bool deleteSfxAudioClip = false;

    /// <summary> The index for the music audio clip reference to delete. </summary>
    protected int musicAudioClipIdxToDelete = 0;

    /// <summary> The index for the sfx audio clip reference to delete. </summary>
    protected int sfxAudioClipIdxToDelete = 0;

    #endregion

    #region Editor

    void OnEnable () {
        // Setup the SerializedProperties
        audioMixerProp = serializedObject.FindProperty("_masterAudioMixer");
        musicAudioClipListProp = serializedObject.FindProperty("_musicAudioClips");
        sfxAudioClipListProp = serializedObject.FindProperty("_sfxAudioClips");
    }

    public override void OnInspectorGUI () {
        // Create a foldout dropdown of the default inspector
        seeDefaultInspector = EditorGUILayout.Foldout(seeDefaultInspector, "Default Inspector");

        // If true Default Inspector is drawn
        if (seeDefaultInspector) {
            DrawDefaultInspector();
        }

        // Update the object in inspector
        serializedObject.Update();

        // Add some spacing
        GUILayout.Space(10f);

        // Create property field for audio mixer
        EditorGUILayout.PropertyField(audioMixerProp);

        // Add some spacing
        GUILayout.Space(5f);

        // Begin GUI layout for list
        EditorGUILayout.BeginVertical();

        // Create a label for the music audio clip list
        GUIStyle labelStyle = new GUIStyle() { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
        EditorGUILayout.LabelField(new GUIContent("Music Audio Clips", "A list of music audio clips to use for the game"), labelStyle);

        for (int i = 0; i < musicAudioClipListProp.arraySize; ++i) {
            // Begin vertical GUI layout for list element
            EditorGUILayout.BeginVertical();

            // Begin horizontal GUI layout for list element
            EditorGUILayout.BeginHorizontal();

            // Create the property field for the music audio clip key value pair
            AudioManager.AudioClipKeyValuePair audioClipKeyValuePair = musicAudioClipListProp.GetArrayElementAtIndex(i).objectReferenceValue as AudioManager.AudioClipKeyValuePair;
            GUIStyle indexLabelStyle = new GUIStyle() { alignment = TextAnchor.MiddleRight };
            EditorGUILayout.LabelField("{0} -".FormatStr(i), indexLabelStyle, GUILayout.Width(35f));
            audioClipKeyValuePair.Name = EditorGUILayout.TextField(audioClipKeyValuePair.Name, GUILayout.Width(100f));
            audioClipKeyValuePair.Clip = EditorGUILayout.ObjectField(audioClipKeyValuePair.Clip, typeof(AudioClip), false, GUILayout.Width(200f)) as AudioClip;

            // Add some spacing
            GUILayout.Space(10f);

            // Create the delete music audio clip button
            if (GUILayout.Button("Delete", GUILayout.Width(50f))) {
                deleteMusicAudioClip = true;
                musicAudioClipIdxToDelete = i;
                break;
            }

            // Add ending spacing
            GUILayout.Label("");

            // End horizontal GUI layout for list elements
            EditorGUILayout.EndHorizontal();

            // Add some spacing
            GUILayout.Space(5f);

            // End vertical GUI layout for list elements
            EditorGUILayout.EndVertical();
        }

        // End GUI layout for list
        EditorGUILayout.EndVertical();

        // Add some spacing
        GUILayout.Space(10f);

        // Create the create a new audio clip reference button
        if (GUILayout.Button("Create a new music AudioClip reference")) {
            createMusicAudioClip = true;
        }

        // Add some spacing
        GUILayout.Space(10f);

        // Begin GUI layout for list
        EditorGUILayout.BeginVertical();

        // Create a label for the sfx audio clip list
        EditorGUILayout.LabelField(new GUIContent("SFX Audio Clips", "A list of sfx audio clips to use for the game"), labelStyle);

        for (int i = 0; i < sfxAudioClipListProp.arraySize; ++i) {
            // Begin vertical GUI layout for list element
            EditorGUILayout.BeginVertical();

            // Begin horizontal GUI layout for list element
            EditorGUILayout.BeginHorizontal();

            // Create the property field for the sfx audio clip key value pair
            AudioManager.AudioClipKeyValuePair audioClipKeyValuePair = sfxAudioClipListProp.GetArrayElementAtIndex(i).objectReferenceValue as AudioManager.AudioClipKeyValuePair;
            GUIStyle indexLabelStyle = new GUIStyle() { alignment = TextAnchor.MiddleRight };
            EditorGUILayout.LabelField("{0} -".FormatStr(i), indexLabelStyle, GUILayout.Width(35f));
            audioClipKeyValuePair.Name = EditorGUILayout.TextField(audioClipKeyValuePair.Name, GUILayout.Width(100f));
            audioClipKeyValuePair.Clip = EditorGUILayout.ObjectField(audioClipKeyValuePair.Clip, typeof(AudioClip), false, GUILayout.Width(200f)) as AudioClip;
            sfxAudioClipListProp.GetArrayElementAtIndex(i).objectReferenceValue = audioClipKeyValuePair;

            // Add some spacing
            GUILayout.Space(10f);

            // Create the delete sfx audio clip button
            if (GUILayout.Button("Delete", GUILayout.Width(50f))) {
                deleteSfxAudioClip = true;
                sfxAudioClipIdxToDelete = i;
                break;
            }

            // Add ending spacing
            GUILayout.Label("");

            // End horizontal GUI layout for list elements
            EditorGUILayout.EndHorizontal();

            // Add some spacing
            GUILayout.Space(5f);

            // End vertical GUI layout for list elements
            EditorGUILayout.EndVertical();
        }

        // End GUI layout for list
        EditorGUILayout.EndVertical();

        // Add some spacing
        GUILayout.Space(10f);

        // Create the create a new sfx audio clip reference button
        if (GUILayout.Button("Create a new sfx AudioClip reference")) {
            createSfxAudioClip = true;
        }

        // Add some spacing
        GUILayout.Space(10f);

        // Check buttons
        CheckForButtonClicks();

        // Apply changes to inspector
        serializedObject.ApplyModifiedProperties();
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Creates a texture for the GUIStyle.
    /// </summary>
    protected void CheckForButtonClicks () {
        // Get the target script
        AudioManager targetScript = (AudioManager)target;

        // Check if creating a new music audio clip reference
        if (createMusicAudioClip) {
            // Reset create music audio clip reference flag
            createMusicAudioClip = false;

            // Create new audio clip reference
            AudioManager.AudioClipKeyValuePair newAudioClip = CreateInstance<AudioManager.AudioClipKeyValuePair>();

            // Update music audio clip list
            musicAudioClipListProp.arraySize = musicAudioClipListProp.arraySize + 1;
            musicAudioClipListProp.GetArrayElementAtIndex(musicAudioClipListProp.arraySize - 1).objectReferenceValue = newAudioClip;
        }

        // Check if deleting a music audio clip reference
        if (deleteMusicAudioClip) {
            // Reset delete music audio clip reference flag
            deleteMusicAudioClip = false;

            // Create a temporary music audio clip list and remove correct music audio clip
            List<AudioManager.AudioClipKeyValuePair> tmpList = new List<AudioManager.AudioClipKeyValuePair>();
            for (int i = 0; i < musicAudioClipListProp.arraySize; i++) {
                if (i != musicAudioClipIdxToDelete) {
                    tmpList.Add(musicAudioClipListProp.GetArrayElementAtIndex(i).objectReferenceValue as AudioManager.AudioClipKeyValuePair);

                }
                else {
                    DestroyImmediate(musicAudioClipListProp.GetArrayElementAtIndex(i).objectReferenceValue);
                }
            }

            // Update music audio clip list
            musicAudioClipListProp.arraySize = musicAudioClipListProp.arraySize - 1;
            for (int i = 0; i < tmpList.Count; i++) {
                musicAudioClipListProp.GetArrayElementAtIndex(i).objectReferenceValue = tmpList[i];
            }
        }

        // Check if creating a new sfx audio clip reference
        if (createSfxAudioClip) {
            // Reset create sfx audio clip reference flag
            createSfxAudioClip = false;

            // Create new audio clip reference
            AudioManager.AudioClipKeyValuePair newAudioClip = CreateInstance<AudioManager.AudioClipKeyValuePair>();

            // Update sfx audio clip list
            sfxAudioClipListProp.arraySize = sfxAudioClipListProp.arraySize + 1;
            sfxAudioClipListProp.GetArrayElementAtIndex(sfxAudioClipListProp.arraySize - 1).objectReferenceValue = newAudioClip;
        }

        // Check if deleting a sfx audio clip reference
        if (deleteSfxAudioClip) {
            // Reset delete sfx audio clip reference flag
            deleteSfxAudioClip = false;

            // Create a temporary sfx audio clip list and remove correct sfx audio clip
            List<AudioManager.AudioClipKeyValuePair> tmpList = new List<AudioManager.AudioClipKeyValuePair>();
            for (int i = 0; i < sfxAudioClipListProp.arraySize; i++) {
                if (i != sfxAudioClipIdxToDelete) {
                    tmpList.Add(sfxAudioClipListProp.GetArrayElementAtIndex(i).objectReferenceValue as AudioManager.AudioClipKeyValuePair);

                }
                else {
                    DestroyImmediate(sfxAudioClipListProp.GetArrayElementAtIndex(i).objectReferenceValue);
                }
            }

            // Update sfx audio clip list
            sfxAudioClipListProp.arraySize = sfxAudioClipListProp.arraySize - 1;
            for (int i = 0; i < tmpList.Count; i++) {
                sfxAudioClipListProp.GetArrayElementAtIndex(i).objectReferenceValue = tmpList[i];
            }
        }
    }

    #endregion
}
#endif
