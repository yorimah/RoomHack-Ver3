using UnityEngine;

public class AffterImageFade : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private float fadeColorAlpha = 0.7f;
    Color fadeColor = new Color(0, 0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fadeColor.a = fadeColorAlpha;
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color -= fadeColor * GameTimer.Instance.ScaledDeltaTime;
        if (spriteRenderer.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
