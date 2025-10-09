using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // Singletonパターン
    public static EffectManager Instance { get; private set; }
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

    public enum EffectType
    {
        HitDamage,
        HitDie,
        Bad,
        Fire,
    }

    [SerializeField]
    GameObject[] EffectPrefab;

    //[SerializeField]
    //int effectNum;

    [SerializeField]
    List<GameObject>[] poolList = new List<GameObject>[4]
    { new List<GameObject>(), new List<GameObject>(), new List<GameObject>(), new List<GameObject>(), };

    GameObject useableEffect;

    public GameObject EffectAct(EffectType _effectType, Vector2 _pos, float _rot, float _time)
    {
        useableEffect = null;

        List<GameObject> pool = poolList[(int)_effectType];

        // 使ってないオブジェクトを検索
        foreach (var item in pool)
        {
            if (item.activeSelf == false)
            {
                useableEffect = item;
                break;
            }
        }

        // 使ってないオブジェクトが見つからないなら新しく生成
        if (useableEffect == null)
        {
            useableEffect = Instantiate(EffectPrefab[(int)_effectType], this.transform);
            pool.Add(useableEffect);
        }

        // エフェクト移動、起動
        useableEffect.transform.position = _pos;

        Vector3 rot = useableEffect.transform.localEulerAngles;
        rot.x = _rot;
        useableEffect.transform.localEulerAngles = rot;

        useableEffect.SetActive(true);
        useableEffect.GetComponent<ParticleSystem>().Play();

        StartCoroutine(EffectUpdate(useableEffect, _time));

        return useableEffect;
    }

    public GameObject EffectAct(EffectType _effectType, Vector2 _pos)
    {
        return EffectAct(_effectType, _pos, 0, 1);
    }

    public GameObject EffectAct(EffectType _effectType, GameObject _target, float _time)
    {
        GameObject effect = EffectAct(_effectType, _target.transform.position, 0, _time);
        StartCoroutine(EffectPositionTrace(effect, _target, _time));
        return effect;
    }


    // エフェクトの表示をオフに
    IEnumerator EffectUpdate(GameObject _effect, float _time)
    {
        float timer = 0;

        while (timer < _time)
        {
            timer += GameTimer.Instance.ScaledDeltaTime;

            // パーティクル再生速度変更
            var main = _effect.GetComponent<ParticleSystem>().main;
            main.simulationSpeed = GameTimer.Instance.customTimeScale;

            yield return null;
        }

        _effect.SetActive(false);
    }

    IEnumerator EffectPositionTrace(GameObject _effect, GameObject _target, float _time)
    {
        float timer = 0;

        while (timer < _time)
        {
            //Debug.Log("追跡中！" + timer +" / "+ _effect +"  / "+ _target);
            if (_target != null) _effect.transform.position = _target.transform.position;
            timer += GameTimer.Instance.ScaledDeltaTime;

            yield return null;
        }
    }
}
