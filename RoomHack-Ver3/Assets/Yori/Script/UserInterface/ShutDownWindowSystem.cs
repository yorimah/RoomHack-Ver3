using UnityEngine;
using Cysharp.Threading.Tasks;
public class ShutDownWindowSystem : WindowSystem
{
    Vector2 shutDownWindowSize = new Vector2(500, 500);
    public override async UniTask PopUpWindow()
    {
        windowRect.sizeDelta += new Vector2(0, 10);
        while (windowRect.rect.width < shutDownWindowSize.x)
        {
            windowRect.sizeDelta += new Vector2(shutDownWindowSize.x, 0);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        while (windowRect.rect.height <shutDownWindowSize.y)
        {
            windowRect.sizeDelta += new Vector2(0, shutDownWindowSize.y);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        windowRect.sizeDelta = new Vector2(Screen.width / 2, shutDownWindowSize.y);
        dragCollider.enabled = true;
        buttonObj.SetActive(true);
    }
}
