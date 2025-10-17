using UnityEngine;
using System.Collections.Generic;

public class ToolEventManager : MonoBehaviour
{
    List<ToolEvent> eventPool = new List<ToolEvent>();

    [SerializeField]
    ToolEvent playEvent;

    public ToolEvent EventPlay(ToolEvent _event, GameObject _target)
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
        playEvent.transform.position = _target.transform.position;
        playEvent.hackTargetObject = _target;
        playEvent.isEventAct = true;

        EffectManager.Instance.ActEffect(EffectManager.EffectType.Success, _target.transform.position, 0, false);

        return playEvent;
    }


}
