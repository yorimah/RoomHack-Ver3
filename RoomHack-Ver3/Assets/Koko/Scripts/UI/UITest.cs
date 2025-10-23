using UnityEngine;

public class UITest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            EffectManager.Instance.ActEffect(-10, this.transform.position, 1);
        }
    }
}
