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
        // blink
        if (Input.GetKey(KeyCode.LeftShift) && player.specialActionCount > 0 && player.nowSpecialAction == Player.SpecialAction.Blink)
        {
            playerMove.Blink();
            player.specialActionCount--;
        }
    }
    public void Execute()
    {
        playerShot.Shot();

        // blink
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.specialActionCount > 0 && player.nowSpecialAction == Player.SpecialAction.Blink)
        {
            playerMove.Blink();
            player.specialActionCount--;
        }


        //if (Input.GetKeyDown(KeyCode.Space) && !Player.Instance.isRebooting)
        //{
        //    SeManager.Instance.StopImmediately("HackExit");
        //    SeManager.Instance.Play("HackStart");
        //    player.ChangeState(Player.StateType.Hack);
        //}

        // edgeRun
        if (Input.GetKey(KeyCode.LeftShift) && player.specialActionCount > 0 && player.nowSpecialAction == Player.SpecialAction.EdgeRun)
        {
            GameTimer.Instance.customTimeScale = 0.1f;
            playerMove.EdgeRun();
            player.specialActionCount -= Time.unscaledDeltaTime;
        }
        else
        {
            if (GameTimer.Instance.customTimeScale < 1)
            {
                GameTimer.Instance.customTimeScale *= 1.5f;
            }

            playerMove.PlMove();
        }

        //if (!Input.GetMouseButton(1))
        if (!Input.GetKey(KeyCode.W)
            && !Input.GetKey(KeyCode.A)
            && !Input.GetKey(KeyCode.S)
            && !Input.GetKey(KeyCode.D)
            && !Input.GetKey(KeyCode.Mouse0)
            && !Input.GetKey(KeyCode.LeftShift))
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
