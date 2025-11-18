using UnityEngine;

public class HackObjectGetter : MonoBehaviour
{
    
    public GameObject targetObject;

    GameObject GetMousePositionObject()
    {
        GameObject obj = null;

        // レイ射出、四角
        RaycastHit2D[] hitsss = Physics2D.BoxCastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.5f, 0.5f), 0f, Vector2.down, 0.1f);

        foreach (RaycastHit2D hit in hitsss)
        {
            if (hit.collider.gameObject.TryGetComponent<IHackObject>(out var hackObject)
                //|| hit.collider.gameObject == playerObject)
                )
            {
                obj = hit.collider.gameObject;
            }
        }

        return obj;
    }

    private void Update()
    {
        
    }
}
