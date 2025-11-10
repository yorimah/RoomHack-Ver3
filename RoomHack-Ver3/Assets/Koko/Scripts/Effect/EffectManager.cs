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

        // リスト生成
        EffectManagerInit();
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
    [SerializeField, Header("要アタッチ、EffectTypeと対応するように")]
    GameObject[] effectPrefab;

    // 各エフェクト毎にList管理
    [SerializeField]
    List<List<GameObject>> poolList = new List<List<GameObject>>();
    GameObject useableEffect;

    private void EffectManagerInit()
    {
        foreach (var item in effectPrefab)
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
            useableEffect = Instantiate(effectPrefab[(int)_effectType], this.transform);
            pool.Add(useableEffect);
        }

        // エフェクト位置修正
        useableEffect.transform.position = _pos;

        // エフェクト角度修正
        useableEffect.transform.rotation = Quaternion.Euler(
                0,
                0,
                _rot
                );

        // エフェクト起動
        useableEffect.SetActive(true);
        useableEffect.GetComponent<ParticleSystem>().Play();

        // タイムスケールのオンオフ
        if (_isScaleTime) StartCoroutine(EffectSpeedScaled(useableEffect));

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
    public GameObject ActEffect(EffectType _effectType, GameObject _target, float _rotOffset)
    {
        GameObject effect = ActEffect(_effectType, _target.transform.position, _target.transform.localEulerAngles.z, true);
        StartCoroutine(EffectPositionTrace(effect, _target, _rotOffset));
        return effect;
    }

    // エフェクト再生速度を調整するか否か
    IEnumerator EffectSpeedScaled(GameObject _effect)
    {
        while (_effect.activeSelf)
        {
            //Debug.Log("追跡中！" + timer +" / "+ _effect +"  / "+ _target);
            var main = _effect.GetComponent<ParticleSystem>().main;
            main.simulationSpeed = GameTimer.Instance.GetCustomTimeScale();

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
    IEnumerator EffectPositionTrace(GameObject _effect, GameObject _target, float _rotOffset)
    {
        while (_effect.activeSelf)
        {
            //Debug.Log("追跡中！ / " + _effect + "  / " + _target);
            if (_target != null)
            {
                _effect.transform.position = _target.transform.position;

                //Vector3 rot = _effect.transform.localEulerAngles;
                //rot.x = _target.transform.localEulerAngles.z + _rotOffset;
                ////rot.x = _target.transform.localEulerAngles.z;
                //_effect.transform.localEulerAngles = rot;
                _effect.transform.rotation = Quaternion.Euler(
                0,
                0,
                _target.transform.eulerAngles.z + _rotOffset
                );
                //Debug.Log(rot);
            }

            yield return null;
        }
    }



    // 数値エフェクト
    [SerializeField, Header("要アタッチ")]
    GameObject numEffectPrefab;
    List<GameObject> numPoolList = new List<GameObject>();

    // 数値エフェクト起動
    public GameObject ActEffect(int _num, Vector2 _pos, float _lifeTime)
    {
        useableEffect = null;

        List<GameObject> pool = numPoolList;

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
            useableEffect = Instantiate(numEffectPrefab, this.transform);
            pool.Add(useableEffect);
        }

        // エフェクト移動、起動
        useableEffect.transform.position = _pos;
        useableEffect.SetActive(true);
        TextMesh textMesh = useableEffect.GetComponent<TextMesh>();
        textMesh.color = new Color(1, 0, 0, 1);
        textMesh.text = _num.ToString();
        StartCoroutine(NumEffectLifeTime(useableEffect, _lifeTime));

        return useableEffect;
    }

    IEnumerator NumEffectLifeTime(GameObject _numEffect, float _lifeTime)
    {
        float timer = 0;
        while (timer <= _lifeTime)
        {
            float alpha = 1 - (timer / _lifeTime);
            _numEffect.GetComponent<TextMesh>().color = new Color(1, 0, 0, alpha);
            yield return null;
            timer += Time.deltaTime;
        }
        _numEffect.SetActive(false);
    }
}
