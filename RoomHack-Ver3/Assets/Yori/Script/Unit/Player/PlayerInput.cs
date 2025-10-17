using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput
{
    private GameInputs gameInput;

    private Vector2 _moveInputValue;

    public event Action ChangeState = delegate { };

    private bool isClick;
    private bool isReload;
    public PlayerInput()
    {
        gameInput = new GameInputs();
        // Actionイベント登録
        // Move
        gameInput.Player.Move.started += OnMove;
        gameInput.Player.Move.performed += OnMove;
        gameInput.Player.Move.canceled += OnMove;

        // Click
        gameInput.Player.Click.started += OnClick;
        gameInput.Player.Click.canceled += OffClick;

        // Reload
        gameInput.Player.Reload.started += OnReload;
        gameInput.Player.Reload.canceled += ReleseReload;
        gameInput.Enable();
        Updata();
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        // Moveアクションの入力取得
        _moveInputValue = context.ReadValue<Vector2>();
    }

    private void OnReload(InputAction.CallbackContext context)
    {
        isReload = true;
    }
    private void ReleseReload(InputAction.CallbackContext context)
    {
        isReload = false;
    }
    private void OnClick(InputAction.CallbackContext context)
    {
        isClick = true;
    }

    private void OffClick(InputAction.CallbackContext context)
    {
        isClick = false;
    }

    private async void Updata()
    {
        while (true)
        {
            await UniTask.Yield();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeState();
            }
        }
    }
    public Vector2 MoveValue()
    {
        return _moveInputValue;
    }

    public bool GetOnClick()
    {
        return isClick;
    }

    public bool GetOnReload()
    {
        return isReload;
    }

    public void OnDestroy()
    {
        // 自身でインスタンス化したActionクラスはIDisposableを実装しているので、
        // 必ずDisposeする必要がある
        gameInput?.Dispose();
    }
}
