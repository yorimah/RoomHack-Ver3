using Cysharp.Threading.Tasks;
using UnityEngine;
public enum WindowState
{
    Closing,
    Closed,
    Opening,
    Opened,
    Maximized,
}
public class WindowSystem : MonoBehaviour
{
    protected float waitSeconds = 0.01f;

    public WindowState State { get; private set; } = WindowState.Closed;

    [SerializeField]
    protected RectTransform windowRect;

    Vector2 prevSize;
    Vector2 prevPos;

    [SerializeField, Header("windowSize")]
    private Vector2 setWindowSize;

    public void InitWindow(RectTransform _windowRect)
    {
        windowRect = _windowRect;
    }
    public async UniTask Open()
    {
        if (State != WindowState.Closed) return;

        State = WindowState.Opening;
        await PlayOpenAnim();
        OnOpened();
        State = WindowState.Opened;
    }
    protected virtual void OnOpened()
    {
        // デフォルト：何もしない
    }
    public async UniTask Close()
    {
        if (State == WindowState.Closed) return;

        State = WindowState.Closing;
        await PlayCloseAnim();
        OnClosed();
        windowRect.transform.localPosition = Vector3.zero;
        State = WindowState.Closed;
    }
    protected virtual void OnClosed()
    {
        // デフォルト
    }

    public async UniTask Minimize()
    {
        if (State != WindowState.Opened) return;
        await PlayCloseAnim();
        State = WindowState.Closed;
        Debug.Log("b" + State);
    }

    public async UniTask ToggleMaximize()
    {
        if (State == WindowState.Maximized)
        {
            Restore();
            State = WindowState.Opened;
        }
        else if (State == WindowState.Opened)
        {
            SaveCurrent();
            await PlayMaximizeAnim();
            State = WindowState.Maximized;
        }
    }

    void SaveCurrent()
    {
        prevSize = windowRect.sizeDelta;
        prevPos = windowRect.localPosition;
    }

    void Restore()
    {
        windowRect.sizeDelta = prevSize;
        windowRect.localPosition = prevPos;
    }

    async UniTask PlayOpenAnim()
    {
        windowRect.sizeDelta += new Vector2(0, 10);
        while (windowRect.rect.width < setWindowSize.x)
        {
            windowRect.sizeDelta += new Vector2(setWindowSize.x / 10, 0);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        while (windowRect.rect.height < setWindowSize.y)
        {
            windowRect.sizeDelta += new Vector2(0, setWindowSize.y / 10);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        windowRect.sizeDelta = setWindowSize;
    }
    async UniTask PlayCloseAnim()
    {
        while (windowRect.rect.height > 10)
        {
            windowRect.sizeDelta -= new Vector2(0, Screen.height / 10);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        while (windowRect.rect.width > 10)
        {
            windowRect.sizeDelta -= new Vector2(Screen.width / 10, 0);
            await UniTask.WaitForSeconds(waitSeconds);
        }
    }
    async UniTask PlayMaximizeAnim()
    {
        windowRect.localPosition = Vector3.zero;
        while (windowRect.rect.width < Screen.width)
        {
            windowRect.sizeDelta += new Vector2(Screen.width / 10, 0);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        while (windowRect.rect.height < Screen.height)
        {
            windowRect.sizeDelta += new Vector2(0, Screen.height / 10);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        windowRect.sizeDelta = new Vector2(Screen.width, Screen.height);
    }
}

