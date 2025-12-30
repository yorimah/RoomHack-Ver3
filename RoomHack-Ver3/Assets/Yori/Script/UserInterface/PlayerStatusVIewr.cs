using UnityEngine;
using Zenject;
using UnityEngine.UI;
public class PlayerStatusVIewr : MonoBehaviour
{
    [Inject]
    IGetSaveData getSaveData;

    private Text viewText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
