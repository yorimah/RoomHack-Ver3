using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
public class Door : MonoBehaviour, IDamageable, IHackObject
{
    public int armorInt { get; set; }

    [SerializeField, Header("装甲")]
    private int armorSerialze = 0;

    public string HackObjectName { get; protected set; }
    public List<ToolType> cantHackToolType { get; set; } = new List<ToolType>();
    public List<ToolEventBase> nowHackEvent { get; set; } = new List<ToolEventBase>();

    public bool CanHack { get; set; } = false;

    // ダメージ関連
    public float MaxHitPoint { get; private set; } = 5;
    public float NowHitPoint { get; set; }
    public int hitDamegeLayer { get; set; } = 2;

    [SerializeField, Header("ハックダメージ倍率")]
    private int hackMag = 10;

    [SerializeField, Header("HP")]
    private int serializeHitPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private BoxCollider2D boxCollider;

    // 長さ
    private float viewDistance = 1;
    // 分割数
    private int segment = 360;

    private float viewerAngle = 360;

    [SerializeField]
    LayerMask targetLayerMask;

    bool isHit;

    [SerializeField, Header("ドアのスプライト")]
    List<SpriteRenderer> spriteRendererList;

    [SerializeField]
    GameObject LeftDoor;

    [SerializeField]
    GameObject RightDoor;

    private Vector2 moveVec = new Vector2(1, 0);

    [SerializeField]
    Sprite deadSprite;

    [SerializeField]
    Sprite liveSprite;

    void Start()
    {
        MaxHitPoint = serializeHitPoint;
        NowHitPoint = MaxHitPoint;
        boxCollider = GetComponent<BoxCollider2D>();
        isHit = false;
        HackObjectName = GetType().Name;
        armorInt = armorSerialze;
    }

    // Update is called once per frame
    void Update()
    {
        if (NowHitPoint >= 0)
        {
            float halhAngle = viewerAngle * 0.5f;

            for (int i = 0; i <= segment; i++)
            {
                float diffusionAngle = -halhAngle + (viewerAngle / segment) * i;

                Quaternion rot = Quaternion.AngleAxis(diffusionAngle, Vector3.forward);
                Vector3 dir = rot * Vector2.one;

                RaycastHit2D wallHit = Physics2D.Raycast(gameObject.transform.position, dir,
                    viewDistance, targetLayerMask);
                if (wallHit.collider != null)
                {
                    if (wallHit.collider.TryGetComponent<EnemyBase>(out var _))
                    {
                        isHit = true;
                        break;
                    }

                }
                if (i == segment)
                {
                    isHit = false;
                }
            }
            // 何かしらあたってたら
            if (isHit)
            {
                OpenDoor(1);
            }
            else
            {
                CloseDoor();
            }
        }

        if (CanHack)
        {
            foreach (var spriteRenderer in spriteRendererList)
            {
                spriteRenderer.sortingLayerName = "playerView";
            }
        }
        else
        {
            foreach (var spriteRenderer in spriteRendererList)
            {
                spriteRenderer.sortingLayerName = "Default";
            }
        }
    }


    Vector2 rightClamp = new Vector2(0.25f, 0.75f);
    Vector2 leftClamp = new Vector2(-0.75f, -0.25f);
    public void OpenDoor(int moveSpeed)
    {
        boxCollider.enabled = false;
        MoveDoor(LeftDoor, -moveSpeed, leftClamp);
        MoveDoor(RightDoor, moveSpeed, rightClamp);
    }

    public void MoveDoor(GameObject moveDoor, int moveDire, Vector2 clamp)
    {
        Vector2 moveTransform = moveDoor.transform.localPosition;
        moveTransform += moveDire * GameTimer.Instance.GetScaledDeltaTime() * this.moveVec;
        moveTransform.x = Mathf.Clamp(moveTransform.x, clamp.x, clamp.y);
        moveDoor.transform.localPosition = moveTransform;
    }

    public void CloseDoor()
    {
        boxCollider.enabled = true;
        MoveDoor(LeftDoor, 1, leftClamp);
        MoveDoor(RightDoor, -1, rightClamp);
    }
    public void HitDmg(int dmg, float hitStop)
    {
        // 防護点上回ってHP回復したわバカタレ
        int damage = dmg - armorInt;
        if (damage < 0) damage = 0;
        NowHitPoint -= damage;
        EffectManager.Instance.ActEffect_Num(damage, this.transform.position, 1);
        EffectManager.Instance.ActEffect_Num(-armorInt, this.transform.position + Vector3.up * 0.2f, 1, new Color(0.8f, 0.8f, 0.8f, 1), 0.5f);

        if (NowHitPoint <= 0)
        {
            Die();
        }
        else
        {
            if (hitDamegeLayer == 2)
            {
                SeManager.Instance.Play("Hit");
            }
            HitStopper.Instance.StopTime(hitStop);
        }
    }

    public void HackDmg(int dmg, float hitStop)
    {
        // ハックダメージに防護点計算してたら意味ないやんけ！おい！
        NowHitPoint -= dmg * hackMag;
        EffectManager.Instance.ActEffect_Num(dmg, this.transform.position, 1, new Color32(195, 4, 197, 255), 1);

        if (NowHitPoint <= 0)
        {
            Die();
        }
        else
        {
            if (hitDamegeLayer == 2)
            {
                SeManager.Instance.Play("Hit");
            }
            HitStopper.Instance.StopTime(hitStop);
        }
    }
    public void Die()
    {
        _ = DieOpenDoor();
    }
    public async UniTask DieOpenDoor()
    {
        foreach (var spriteRender in spriteRendererList)
        {
            if (spriteRender.sprite == liveSprite)
            {
                Debug.Log("sprite変更！");
                spriteRender.sprite = deadSprite;
            }
        }
        while (LeftDoor.transform.localPosition.x > leftClamp.x)
        {
            OpenDoor(2);
            await UniTask.Yield();
        }

        boxCollider.enabled = false;
    }
}
