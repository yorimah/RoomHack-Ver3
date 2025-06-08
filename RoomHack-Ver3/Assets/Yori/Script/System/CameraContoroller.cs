using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraContoroller : MonoBehaviour
{
    [SerializeField, Header("プレイヤーの座標")]
    private Transform playerTransform;

    private float zCameraPosition;

    private Vector3 cameraMoveVector3;
    void Start()
    {
        zCameraPosition = this.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform!=null)
        {
            MoveCameraPosition(playerTransform);
        }       
    }

    void MoveCameraPosition(Transform targetPosition)
    {
        cameraMoveVector3 = new Vector3(targetPosition.position.x, targetPosition.position.y, zCameraPosition);
        this.transform.position = cameraMoveVector3;
    }

    public void ChangeTarget()
    {
        
    }

}
