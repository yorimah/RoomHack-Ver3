using UnityEngine;
using UnityEngine.SceneManagement;
public class Accepting : MonoBehaviour
{
    [SerializeField]
    
   public void Accept()
    {
        SceneManager.LoadScene("Yori_Stage_Timer");
    }
}
