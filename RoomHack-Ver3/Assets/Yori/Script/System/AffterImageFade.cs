using UnityEngine;

public class AffterImageFade : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color fadeColor = new Color(0, 0, 0, 0.8f);
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
