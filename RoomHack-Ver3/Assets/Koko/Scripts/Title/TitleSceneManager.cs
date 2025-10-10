
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] TitleText;

    bool sequenceEnd = false;

    float timer = 0;
    [SerializeField]
    private string[] nextScene;
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 2)
        {
            TitleText[0].SetActive(true);
        }

        if (timer >= 3)
        {
            TitleText[1].SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (sequenceEnd == true)
            {
                int rand = Random.Range(0, nextScene.Length);
                Debug.Log(nextScene[rand].ToString());
                SceneManager.LoadScene(nextScene[rand]);
            }
            else
            {
                TitleText[0].SetActive(true);
                TitleText[1].SetActive(true);
                sequenceEnd = true;
            }
        }
    }
}
