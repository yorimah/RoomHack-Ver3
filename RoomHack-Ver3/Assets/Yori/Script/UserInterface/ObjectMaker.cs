using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMarker : MonoBehaviour
{
    // オブジェクトを映すカメラ
    [SerializeField] private Camera _targetCamera;

    // UIを表示させる対象オブジェクト
    [SerializeField] private Transform _target;

    // 表示するUI
    [SerializeField] private Transform _targetUI;

    // オブジェクト位置のオフセット
    [SerializeField] private Vector3 _worldOffset;

    private RectTransform _parentUI;
    public void Initialize(Transform target, Camera targetCamera = null)
    {
        _target = target;
        _targetCamera = targetCamera != null ? targetCamera : Camera.main;

        OnUpdatePosition() ;
    }
    private void Awake()
    {
        // カメラが指定されていなければメインカメラにする
        if (_targetCamera == null)
            _targetCamera = Camera.main;
    }

    // UIの位置を毎フレーム更新
    private void Update()
    {
        OnUpdatePosition();
    }

    // UIの位置を更新する
    private void OnUpdatePosition()
    {
        // オブジェクトの位置
        var targetWorldPos = _target.position + _worldOffset;

        // オブジェクトのワールド座標→スクリーン座標変換
        var targetScreenPos = _targetCamera.WorldToScreenPoint(targetWorldPos);

        // スクリーン座標変換→UIローカル座標変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransformのローカル座標を更新
        _targetUI.localPosition = uiLocalPos;
    }
}
