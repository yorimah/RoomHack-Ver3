using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LoadManager : MonoBehaviour
{
    [Inject]
    IGetPlayerDie onDead;
    public void Start()
    {
        onDead.PlayerDie += () => { SceneManager.LoadScene("GameOverDemoScene"); };
    }
}
