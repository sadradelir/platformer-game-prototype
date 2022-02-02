using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayFromSplash : MonoBehaviour
{
    [MenuItem("Edit/Play-Stop, But From Prelaunch Scene %0")]
    public static void PlaySplash()
    {
        if (EditorApplication.isPlaying == true)
        {
            EditorApplication.isPlaying = false;
            return;
        }

        EditorApplication.SaveCurrentSceneIfUserWantsTo();
        EditorApplication.OpenScene("Assets/Scenes/GameScene.unity");
        EditorApplication.isPlaying = true;
    }
}

 
