using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HackInformationDisp : MonoBehaviour
{
    [SerializeField, Header("要アタッチ")]
    CameraPositionController cameraPositionController;

    [SerializeField, Header("要アタッチ")]
    GeneralUpdateText nameText;

    [SerializeField, Header("要アタッチ")]
    GeneralUpdateText hpText;

    [SerializeField, Header("要アタッチ")]
    GeneralUpdateText nowHackText;

    [SerializeField]
    GeneralUpdateText enemyNumText;

    GameObject target;

    [Inject]
    IGetEnemyList getEnemyList;

    private void Update()
    {
        List<Enemy> enemies = new List<Enemy>();
        enemies.AddRange(getEnemyList.GetEnemies());
        int enemyNum = enemies.Count;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].isDead)
            {
                enemyNum--;
            }
        }
        enemyNumText.inputText = "Enemy:" + enemyNum.ToString();

        if (cameraPositionController.targetObject != null)
        {
            nameText.gameObject.SetActive(true);
            hpText.gameObject.SetActive(true);
            nowHackText.gameObject.SetActive(true);

            if (target == null)
            {
                target = cameraPositionController.targetObject;
            }

            if (cameraPositionController.targetObject != target)
            {
                target = cameraPositionController.targetObject;
                nameText.DispTextReset();
                hpText.DispTextReset();
                nowHackText.DispTextReset();
            }

            // HP表記
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                hpText.inputText = damageable.NowHitPoint.ToString() + " / " + damageable.MaxHitPoint.ToString();
            }
            else
            {
                hpText.inputText = "UnBroken";
            }

            // ハッキング対象の名前表記
            // ハッキング内容表記
            if (target.TryGetComponent<IHackObject>(out var hackObject))
            {
                nameText.inputText = hackObject.HackObjectName;

                string inputText = null;
                foreach (var item in hackObject.nowHackEvent)
                {
                    inputText += item.thisToolTag.ToString();
                    inputText += "\n";
                }
                inputText += " ";
                nowHackText.inputText = inputText;
            }

        }
        else
        {
            nameText.gameObject.SetActive(false);
            hpText.gameObject.SetActive(false);
            nowHackText.gameObject.SetActive(false);
        }
    }
}
