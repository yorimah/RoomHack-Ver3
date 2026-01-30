using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class RelicUIManager : MonoBehaviour
{
    [SerializeField, Header("UIPrefabアタッチ")]
    RelicIconUI relicIconPrefab;

    [SerializeField]
    List<RelicIconUI> relicIconUIList = new List<RelicIconUI>();

    [SerializeField] Vector2 startPos = new Vector2(-900, -450);
    [SerializeField] float space = 100;

    [Inject]
    IGetRelicList relicEvent;

    List<IRelicEvent> relicEventList = new List<IRelicEvent>();

    private void Start()
    {
        relicEventList = relicEvent.relicEvents;
    }

    public void Update()
    {

        // 足りなければ生成
        while(relicEventList.Count > relicIconUIList.Count)
        {
            relicIconUIList.Add(Instantiate(relicIconPrefab, Vector2.zero, Quaternion.identity, this.transform));
        }


        for (int i = 0; i < relicIconUIList.Count; i++)
        {
            if (i < relicEventList.Count)
            {
                relicIconUIList[i].gameObject.SetActive(true);

                // 表示情報設定
                relicIconUIList[i].thisRelic = relicEventList[i].relicName;
                relicIconUIList[i].isActive = relicEvent.relicEvents[i].IsEventTrigger;

                // 座標指定
                Vector2 UIPos = startPos;
                UIPos.y += space * i;
                relicIconUIList[i].GetComponent<RectTransform>().localPosition = UIPos;
            }
            else
            {
                relicIconUIList[i].gameObject.SetActive(false);
            }
        }
    }
}
