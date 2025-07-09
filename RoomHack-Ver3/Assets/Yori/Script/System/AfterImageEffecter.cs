using System.Collections.Generic;
using UnityEngine;

public class AfterImageEffecter : MonoBehaviour
{
    private Sprite affterImageSprite;
    private SpriteRenderer affterImageSpriteRender;
    private Color initColor;


    private float timer = 0;
    private float generateEffectTime = 0.2f;

    private GameObject affterImageObject;


    void Start()
    {
        affterImageSprite = this.GetComponent<SpriteRenderer>().sprite;
        initColor = this.GetComponent<SpriteRenderer>().color;
        initColor.a = 0.5f;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameTimer.Instance.customTimeScale < 1)
        {
            timer += GameTimer.Instance.ScaledDeltaTime;
            if (timer >= generateEffectTime)
            {
                GenerateAffterImage();
            }
        }
    }

    void GenerateAffterImage()
    {
        affterImageObject = new GameObject(gameObject.name + "AfferImage");
        affterImageObject.transform.position = this.transform.position;
        affterImageObject.transform.rotation = this.transform.rotation;
        affterImageSpriteRender = affterImageObject.AddComponent<SpriteRenderer>();
        affterImageObject.AddComponent<AffterImageFade>();
        affterImageSpriteRender.sprite = affterImageSprite;
        affterImageSpriteRender.color = initColor;
        timer = 0;
    }
}
