using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Tutorial1_save : MonoBehaviour
{
    [SerializeField]
    List<int> loadDeck = new List<int>();

    [SerializeField]
    GunName loadGun;

    [SerializeField]
    string loadScene;

    void Start()
    {
        PlayerSaveData playerSave = SaveManager.Instance.Load();

        playerSave.deckList = loadDeck;
        playerSave.gunName = loadGun;

        SaveManager.Instance.Save(playerSave);

        SceneManager.LoadScene(loadScene);
    }
}
