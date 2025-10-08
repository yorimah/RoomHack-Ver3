using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWallCheck
{
    // 2点間にwallを持つものがないかをboolで返答
    public bool WallHit(Transform _from, Transform _for, LayerMask _layerMask)
    {
        bool isHit = false;
        RaycastHit2D hit = Physics2D.Raycast(_from.position, _for.position - _from.position, _layerMask);
        Debug.DrawRay(_from.position, _for.position - _from.position);

        if (hit.collider != null)
        {
            isHit = true;
        }

        return isHit;
    }


}
