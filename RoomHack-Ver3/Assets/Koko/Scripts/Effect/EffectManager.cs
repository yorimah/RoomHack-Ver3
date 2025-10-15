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
        Bomb,
        Success,
        MissileFire,
    }

    // エフェクトを登録
    [SerializeField]
    GameObject[] EffectPrefab;

    //[SerializeField]
    //int effectNum;

    // 各エフェクト毎にList管理
    [SerializeField]
    List<List<GameObject>> poolList = new List<List<GameObject>>();
    GameObject useableEffect;

    private void Start()
    {
        foreach (var item in EffectPrefab)
        {
            poolList.Add(new List<GameObject>());
        }
    }

    // エフェクト起動
    public GameObject ActEffect(EffectType _effectType, Vector2 _pos, float _rot, bool _isScaleTime)
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
            //Debug.Log("effe");
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

        if (_isScaleTime) StartCoroutine(EffectSpeedScaled(useableEffect));

        //StartCoroutine(EffectUpdate(useableEffect, _time, _isScaleTime));

        return useableEffect;
    }

    // エフェクト簡略版
    public GameObject ActEffect(EffectType _effectType, Vector2 _pos)
    {
        return ActEffect(_effectType, _pos, 0, true);
    }

    // エフェクト制限時間版
    public GameObject ActEffect(EffectType _effectType, Vector2 _pos, float _rot, bool _isScaleTime, float _lifeTime)
    {
        GameObject effect = ActEffect(_effectType, _pos, _rot, _isScaleTime);
        StartCoroutine(EffectLifeTime(effect, _lifeTime));
        return effect;
    }

    // エフェクト追従版
    public GameObject ActEffect(EffectType _effectType, GameObject _target)
    {
        GameObject effect = ActEffect(_effectType, _target.transform.position, 0, true);
        StartCoroutine(EffectPositionTrace(effect, _target));
        return effect;
    }


    // エフェクトの表示をオフに
    //IEnumerator EffectUpdate(GameObject _effect, float _time, bool _isScaleTime)
    //{
    //    float timer = 0;

    //    while (timer < _time)
    //    {

    //        timer += GameTimer.Instance.ScaledDeltaTime;

    //        // パーティクル再生速度変更
    //        if (_isScaleTime)
    //        {
    //            var main = _effect.GetComponent<ParticleSystem>().main;
    //            main.simulationSpeed = GameTimer.Instance.customTimeScale;
    //        }

    //        yield return null;
    //    }

    //    _effect.SetActive(false);
    //}

    // エフェクト再生速度を調整するか否か
    IEnumerator EffectSpeedScaled(GameObject _effect)
    {
        while (_effect.activeSelf)
        {
            //Debug.Log("追跡中！" + timer +" / "+ _effect +"  / "+ _target);
            var main = _effect.GetComponent<ParticleSystem>().main;
            main.simulationSpeed = GameTimer.Instance.customTimeScale;

            yield return null;
        }
    }

    // エフェクト制限時間
    IEnumerator EffectLifeTime(GameObject _effect, float _lifeTime)
    {
        yield return new WaitForSeconds(_lifeTime);
        _effect.SetActive(false);
    }

    // 対象追従
    IEnumerator EffectPositionTrace(GameObject _effect, GameObject _target)
    {
        while (_effect.activeSelf)
        {
            //Debug.Log("追跡中！" + timer +" / "+ _effect +"  / "+ _target);
            if (_target != null) _effect.transform.position = _target.transform.position;

            yield return null;
        }
    }
}
