using UnityEngine;
using UnityEngine.InputSystem;

public class MoveInput
{
    private GameInputs _gameInputs;

    private Vector2 _moveInputValue;


    public void Init()
    {
        _gameInputs = new GameInputs();

        // Actionイベント登録
        _gameInputs.Player.Move.started += OnMove;
        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnMove;
        _gameInputs.Enable();
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        // Moveアクションの入力取得
        _moveInputValue = context.ReadValue<Vector2>();
    }

    public Vector2 MoveValue()
    {
        return _moveInputValue;
    }

    public void OnDestroy()
    {
        // 自身でインスタンス化したActionクラスはIDisposableを実装しているので、
        // 必ずDisposeする必要がある
        _gameInputs?.Dispose();
    }

}
