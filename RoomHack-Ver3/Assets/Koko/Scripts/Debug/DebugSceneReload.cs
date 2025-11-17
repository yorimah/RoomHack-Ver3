using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSceneReload : MonoBehaviour
{
    // エディター上でしか動かないスクリプト

    void Update()
    {
#if UNITY_EDITOR

        // シーンリロード
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }



}
