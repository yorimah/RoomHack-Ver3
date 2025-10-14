using UnityEngine;

public class ToolEvent_Bind : ToolEvent
{
    public override toolTag thisToolTag { get; set; } = toolTag.Bind;

    bool isSet = false;
    float timer = 0;
    float lifeTime = 5;

    Enemy targetData;
    float startSpeed;

    GameObject effect;

    private void Update()
    {
        if (isEventAct)
        {
            // 初期設定
            if (!isSet)
            {
                HackSetup();
                isSet = true;
            }

            // 稼働処理
            targetData.moveSpeed = 0;
            Tracking();

            // 終了設定
            timer -= GameTimer.Instance.ScaledDeltaTime;
            if (timer <= 0 || (hackTargetObject.TryGetComponent<Enemy>(out var enemy) && enemy.died))
            {
                targetData.moveSpeed = startSpeed;
                EventRemove();
                effect.GetComponent<ParticleSystem>().Stop();
                isSet = false;
                isEventAct = false;
            }
        }
    }

    // 初期設定
    void HackSetup()
    {
        EventAdd();

        effect = EffectManager.Instance.ActEffect(EffectManager.EffectType.Bad, this.gameObject);

        targetData = hackTargetObject.GetComponent<Enemy>();
        startSpeed = targetData.moveSpeed;

        timer = lifeTime;
    }

    public override void ToolAction()
    {
    }
}
