using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackObject : MonoBehaviour
{
    [SerializeField, Header("�n�b�N���x��")]
    public int secLevele = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HackStart()
    {

        Debug.Log(this.gameObject.name + "�̃n�b�L���O�J�n");
    }
}
