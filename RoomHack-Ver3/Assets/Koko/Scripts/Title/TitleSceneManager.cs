
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] TitleText;

    float skipValue = 1;
    bool sequenceEnd = false;

    private void Start()
    {
        TitleSequance().Forget();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (sequenceEnd == true)
            {
                SceneManager.LoadScene("UpgradeTest");
            }
            else
            {
                skipValue = 0;
            }
        }
    }

    async UniTask TitleSequance()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2f * skipValue));
        TitleText[0].SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(1f * skipValue));
        TitleText[1].SetActive(true);

        sequenceEnd = true;
    }

}
