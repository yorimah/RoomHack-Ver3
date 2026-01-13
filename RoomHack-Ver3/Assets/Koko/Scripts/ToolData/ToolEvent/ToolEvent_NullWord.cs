using UnityEngine;

public class ToolEvent_NullWord : ToolEventBase, IToolEvent_Time, IToolEvent_ToolManager
{
    public override ToolTag thisToolTag { get; set; } = ToolTag.NullWord;


    // IToolEvent_Time
    public float setTime { get; set; } = 3;
    public float timer { get; set; } = 0;


    // ToolManager
    public ToolManager toolManager { get; set; }
    public void GetToolManager()
    {
        toolManager = ToolManager.Instance;
    }


    protected override void Enter()
    {
        timer = setTime;
        GetToolManager();

        // ramを0にする
        toolManager.useableRam.RamChange(-toolManager.useableRam.RamNow);

        // 手札を全て捨てる
        int handNum = toolManager.GetHandData().Count;
        for (int i = 0; i < handNum; i++)
        {
            toolManager.HandTrash(0);
        }

        //EventAdd();
    }

    protected override void Execute()
    {
        // 終了条件
        timer -= GameTimer.Instance.GetScaledDeltaTime();
        if (timer <= 0)
        {
            EventEnd();
        }
    }

    protected override void Exit()
    {
        // Ramを全回復し、3ドロー
        toolManager.useableRam.RamChange(toolManager.useableRam.RamCapacity);
        toolManager.DeckDraw();
        toolManager.DeckDraw();
        toolManager.DeckDraw();

        //EventRemove();
    }



}
