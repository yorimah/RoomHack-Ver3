using UnityEngine;

public class PlayerActionState : IState
{
    private Player player;
    private PlayerMove playerMove;
    private PlayerShot playerShot;
    public PlayerActionState(Player _player)
    {
        player = _player;
        playerMove = new PlayerMove(player);
        playerShot = new PlayerShot(player);
    }
    public void Enter()
    {

    }
    public void Execute()
    {
        playerMove.PlMove();
        playerShot.Shot();

        //if (Input.GetKeyDown(KeyCode.Space) && !Player.Instance.isRebooting)
        //{
        //    SeManager.Instance.StopImmediately("HackExit");
        //    SeManager.Instance.Play("HackStart");
        //    player.ChangeState(Player.StateType.Hack);
        //}

        GameTimer.Instance.SetTimeScale(1);

        //if (!Input.GetMouseButton(1))
        if (!Input.GetKey(KeyCode.W)
            && !Input.GetKey(KeyCode.A)
            && !Input.GetKey(KeyCode.S)
            && !Input.GetKey(KeyCode.D)
            && !Input.GetKey(KeyCode.Mouse0))
        {
            SeManager.Instance.StopImmediately("HackExit");
            SeManager.Instance.Play("HackStart");
            player.ChangeState(Player.StateType.Hack);
        }
    }

    public void Exit()
    {

    }
}
