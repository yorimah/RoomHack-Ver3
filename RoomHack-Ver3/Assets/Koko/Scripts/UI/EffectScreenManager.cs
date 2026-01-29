using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EffectScreenManager : MonoBehaviour
{
    // Singletonパターン
    public static EffectScreenManager Instance { get; private set; }
    private void Awake()
    {
        // 重複を防止
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public enum ScreenTag
    {
        blackScreen,
        whiteScreen,
    }

    [SerializeField, Header("アタッチしてね")]
    List<Image> screenList = new List<Image>();

    // 透過度セット
    // フェードとセットで使用すること
    public void SetScreenOpacity(int _index, float _opacity)
    {
        screenList[_index].color = new Color(screenList[_index].color.r, screenList[_index].color.g, screenList[_index].color.b, _opacity);
    }

    // フェード、ratioが1以上でフェードイン、1未満でフェードアウト
    // フェード前にSetScreenOpacityを起動すること
    public void ScreenFade(int _index, float _ratio)
    {
        StartCoroutine(FadeSequence(_index, _ratio));
    }

    IEnumerator FadeSequence(int _index, float _ratio)
    {
        // エラーチェック
        if (_ratio != 1)
        {
            // フェードインかアウトか判定
            bool isFadeOut = true;
            if (_ratio > 1) isFadeOut = false;

            bool isFadeNow = true;

            while (isFadeNow)
            {
                //Debug.Log("wawawa");
                // 数値変更
                float transparency = screenList[_index].color.a;
                transparency *= _ratio;
                SetScreenOpacity(_index, transparency);
                
                // ループ脱出判定
                if (isFadeOut)
                {
                    if (screenList[_index].color.a < 0.01f)
                    {
                        SetScreenOpacity(_index, 0);
                        isFadeNow = false;
                    }
                }
                else
                {
                    if (screenList[_index].color.a > 1)
                    {
                        SetScreenOpacity(_index, 1);
                        isFadeNow = false;
                    }
                }
                
                yield return null;
            }
        }
        else
        {
            Debug.LogError("_ratioに1いれるな、いみねーぞ");
        }
    }
}
