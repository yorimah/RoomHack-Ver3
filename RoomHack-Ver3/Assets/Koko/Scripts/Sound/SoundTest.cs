using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundTest : MonoBehaviour
{
    [SerializeField]
    float volume = 1;

    void Start()
    {
        BgmManager.Instance.Play("Baccano_-_Baccano_No_Theme");
    }

    void Update()
    {
        BgmManager.Instance.CurrentAudioSource.volume = volume;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("YoriTestScene");
        }
    }
}
