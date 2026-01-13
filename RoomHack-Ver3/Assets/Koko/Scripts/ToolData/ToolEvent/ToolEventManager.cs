using UnityEngine;
using System.Collections.Generic;

public class ToolEventManager : MonoBehaviour
{
    List<ToolEventBase> eventPool = new List<ToolEventBase>();

    [SerializeField]
    ToolEventBase playEvent;

    public ToolEventBase EventPlay(ToolEventBase _event, GameObject _target)
    {
        playEvent = null;

        // 未使用イベントを検索
        foreach (var item in eventPool)
        {
            if (item.thisToolTag == _event.thisToolTag && !item.isEventAct)
            {
                playEvent = item;
                break;
            }
        }

        // 未使用イベントがなければ新規作成
        if (playEvent == null)
        {
            playEvent = Instantiate(_event, this.transform);
            eventPool.Add(playEvent);
        }

        // イベント初期設定

        if(playEvent.TryGetComponent<IToolEvent_Target>(out var toolEventBase_Target))
        {
            toolEventBase_Target.hackTargetObject = _target;
            playEvent.transform.position = _target.transform.position;
            EffectManager.Instance.ActEffect(EffectManager.EffectType.Success, _target.transform.position, 0, false);
        }
        else
        {
            EffectManager.Instance.ActEffect(EffectManager.EffectType.Success, Vector2.zero, 0, false);
        }
        playEvent.EventStart();


        return playEvent;
    }


}
