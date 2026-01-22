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

    // エフェクト種類、下のリストと対応するように
    public enum EffectType
    {
        HitDamage,
        HitDie,
        Bad,
        Fire,
        Bomb,
        Success,
        MissileFire,
        Time,
        HitWall,
    }

    // エフェクトを登録
    [SerializeField, Header("要アタッチ、EffectTypeと対応するように")]
    GameObject[] effectPrefab;

    // 各エフェクト毎にList管理
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

    // エフェクト簡略版（角度不要）
    public GameObject ActEffect(EffectType _effectType, Vector2 _pos)
    {
        return ActEffect(_effectType, _pos, 0, true);
    }

    // エフェクト制限時間版
    public GameObject ActEffect_Time(EffectType _effectType, Vector2 _pos, float _rot, bool _isScaleTime, float _lifeTime)
    {
        GameObject effect = ActEffect(_effectType, _pos, _rot, _isScaleTime);
        StartCoroutine(EffectLifeTime(effect, _lifeTime));
        return effect;
    }

    // エフェクト追従版
    public GameObject ActEffect_PositionTrace(EffectType _effectType, GameObject _target, Vector2 _posOffset)
    {
        GameObject effect = ActEffect(_effectType, _target.transform.position, _target.transform.localEulerAngles.z, true);
        StartCoroutine(EffectPositionTrace(effect, _target, _posOffset));
        StartCoroutine(EffectTraceStoper(effect, _target));
        return effect;
    }

    // エフェクト追従（角度込み）
    public GameObject ActEffect_Trace(EffectType _effectType, GameObject _target, Vector2 _posOffset, float _rotOffset)
    {
        GameObject effect = ActEffect(_effectType, _target.transform.position, _target.transform.localEulerAngles.z, true);
        StartCoroutine(EffectPositionTrace(effect, _target, _posOffset));
        StartCoroutine(EffectRotationTrace(effect, _target, _rotOffset));
        StartCoroutine(EffectTraceStoper(effect, _target));
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
        _effect.GetComponent<ParticleSystem>().Stop();
    }

    // 対象追従
    IEnumerator EffectPositionTrace(GameObject _effect, GameObject _target, Vector2 _posOffset)
    {
        while (_effect.activeSelf)
        {
            //Debug.Log("追跡中！ / " + _effect + "  / " + _target);
            if (_target != null)
            {
                _effect.transform.position = _target.transform.position + (Vector3)_posOffset;
            }

            yield return null;
        }
    }
    IEnumerator EffectRotationTrace(GameObject _effect, GameObject _target, float _rotOffset)
    {
        while (_effect.activeSelf)
        {
            //Debug.Log("追跡中！ / " + _effect + "  / " + _target);
            if (_target != null)
            {
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

    // 追従オブジェクト停止時にエフェクト停止
    IEnumerator EffectTraceStoper(GameObject _effect, GameObject _target)
    {
        while(_effect.activeSelf)
        {

            if (_target == null)
            {
                //Debug.Log("しんだお");
                _effect.GetComponent<ParticleSystem>().Stop();
            }
            else if (!_target.activeSelf)
            {
                //Debug.Log("あくてぃぶふぁるす");
                _effect.GetComponent<ParticleSystem>().Stop();
            }
            else
            {
                //Debug.Log(_target.name + "はうごいつるお");
            }


            yield return null;
        }

    }


    // 数値エフェクト
    [SerializeField, Header("要アタッチ")]
    GameObject numEffectPrefab;
    List<GameObject> numPoolList = new List<GameObject>();

    // 数値エフェクト起動
    public GameObject ActEffect_Num(int _num, Vector2 _pos, float _lifeTime)
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

    public GameObject ActEffect_Num(int _num, Vector2 _pos, float _lifeTime, Color _color, float _size)
    {
        GameObject obj = ActEffect_Num(_num, _pos,_lifeTime);
        obj.GetComponent<TextMesh>().color = _color;
        obj.transform.localScale *= new Vector2(_size, _size);
        return obj;
    }

    IEnumerator NumEffectLifeTime(GameObject _numEffect, float _lifeTime)
    {
        float timer = 0;
        TextMesh textMesh = _numEffect.GetComponent<TextMesh>();
        while (timer <= _lifeTime)
        {
            float alpha = 1 - (timer / _lifeTime);
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
            yield return null;
            timer += Time.deltaTime;
        }
        _numEffect.SetActive(false);
    }
}
