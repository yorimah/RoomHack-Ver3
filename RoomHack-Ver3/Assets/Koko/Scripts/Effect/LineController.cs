using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public Vector2 startPosition;
    public Vector2 endPosition;

    public float lineSpace = 1;

    [SerializeField]
    GameObject startObject;
    [SerializeField]
    GameObject endObject;

    [SerializeField]
    GameObject dispNumberObject;

    float lineDistance;
    Vector2 lineDirection;

    private void Update()
    {
        if (startObject != null)
        {
            startPosition = startObject.transform.position;
        }

        if (endObject != null)
        {
            endPosition = endObject.transform.position;
        }

        lineDistance = Vector2.Distance(startPosition, endPosition);
        lineDirection = endPosition - startPosition;

        transform.localScale = new Vector3(lineDistance - lineSpace, 0.1f, 1);
        transform.localPosition = startPosition + lineDirection / 2;
        transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(lineDirection.y, lineDirection.x)*Mathf.Rad2Deg);
    }

}
