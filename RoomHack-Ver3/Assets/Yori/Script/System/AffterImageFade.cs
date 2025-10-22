using UnityEngine;

public class AffterImageFade : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private float fadeColorAlpha = 0.7f;
    Color fadeColor = new Color(0, 0, 0, 0);
    // Start is called before the first frame update
    private Vector2 initVector2;
    private Quaternion initQuaternion;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fadeColor.a = fadeColorAlpha;
        initVector2 = transform.TransformPoint(this.transform.localPosition);
        initQuaternion = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color -= fadeColor * GameTimer.Instance.GetScaledDeltaTime();
        if (spriteRenderer.color.a <= 0)
        {
            Destroy(gameObject);
        }
        transform.position = initVector2;
        transform.rotation = initQuaternion;
    }
}
