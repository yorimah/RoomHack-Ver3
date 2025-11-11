using UnityEngine;

public class AfterImageEffecter : MonoBehaviour
{
    private Sprite affterImageSprite;
    private SpriteRenderer affterImageSpriteRender;
    private Color initColor;
    private float timer = 0;
    private float generateEffectTime = 0.2f;
    private GameObject affterImageObject;

    private GameObject affterImageObjects;
    void Start()
    {
        affterImageSprite = this.GetComponent<SpriteRenderer>().sprite;
        initColor = this.GetComponent<SpriteRenderer>().color;
        initColor.a = 0.5f;
        affterImageObjects = new GameObject(gameObject.name + "AfferImages");
    }

    void Update()
    {
        if (GameTimer.Instance.IsHackTime)
        {
            if (timer >= generateEffectTime)
            {
                GenerateAffterImage();
            }
            else
            {
                timer += GameTimer.Instance.GetScaledDeltaTime();
            }
        }
    }

    void GenerateAffterImage()
    {
        affterImageObject = new GameObject(gameObject.name + "AfferImage");
        affterImageObject.transform.parent = this.transform;
        affterImageObject.transform.position = this.transform.position;
        affterImageObject.transform.rotation = this.transform.rotation;
        affterImageObject.transform.localScale = new Vector3(1, 1, 1);
        affterImageSpriteRender = affterImageObject.AddComponent<SpriteRenderer>();
        affterImageObject.AddComponent<AffterImageFade>();
        affterImageSpriteRender.sprite = affterImageSprite;
        affterImageSpriteRender.color = initColor;
        timer = 0;
    }
}
