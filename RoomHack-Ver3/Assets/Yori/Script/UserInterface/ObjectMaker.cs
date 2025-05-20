using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMarker : MonoBehaviour
{
    // �I�u�W�F�N�g���f���J����
    [SerializeField] private Camera _targetCamera;

    // UI��\��������ΏۃI�u�W�F�N�g
    [SerializeField] private Transform _target;

    // �\������UI
    [SerializeField] private Transform _targetUI;

    // �I�u�W�F�N�g�ʒu�̃I�t�Z�b�g
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
        // �J�������w�肳��Ă��Ȃ���΃��C���J�����ɂ���
        if (_targetCamera == null)
            _targetCamera = Camera.main;
    }

    // UI�̈ʒu�𖈃t���[���X�V
    private void Update()
    {
        OnUpdatePosition();
    }

    // UI�̈ʒu���X�V����
    private void OnUpdatePosition()
    {
        // �I�u�W�F�N�g�̈ʒu
        var targetWorldPos = _target.position + _worldOffset;

        // �I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ϊ�
        var targetScreenPos = _targetCamera.WorldToScreenPoint(targetWorldPos);

        // �X�N���[�����W�ϊ���UI���[�J�����W�ϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransform�̃��[�J�����W���X�V
        _targetUI.localPosition = uiLocalPos;
    }
}
