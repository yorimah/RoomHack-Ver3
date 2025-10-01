using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffectController : MonoBehaviour
{
    [SerializeField]
    GameObject actionCamera;

    [SerializeField]
    GameObject hackCamera;

    private void Update()
    {
        if (UnitCore.Instance.statetype == UnitCore.StateType.Hack)
        {
            actionCamera.SetActive(false);
            hackCamera.SetActive(true);
        }
        else
        {
            actionCamera.SetActive(true);
            hackCamera.SetActive(false);
        }
    }
}
