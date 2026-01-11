using Cysharp.Threading.Tasks;
using UnityEngine;
public class ShutDownWindowSystem : WindowSystem
{
    Vector2 shutDownWindowSize = new Vector2(500, 500);
    public override async UniTask PopUpWindow()
    {
        windowRect.sizeDelta += new Vector2(0, 10);
        while (windowRect.rect.width < shutDownWindowSize.x)
        {
            windowRect.sizeDelta += new Vector2(shutDownWindowSize.x / 10, 0);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        while (windowRect.rect.height < shutDownWindowSize.y)
        {
            windowRect.sizeDelta += new Vector2(0, shutDownWindowSize.y / 10);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        windowRect.sizeDelta = shutDownWindowSize;
        dragCollider.enabled = true;
        buttonObj.SetActive(true);
    }
    public void ShutDownButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
