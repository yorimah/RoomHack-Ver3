using Cysharp.Threading.Tasks;
using UnityEngine;
public class WindowButtons : MonoBehaviour
{
    [SerializeField] WindowSystem window;

    public void OnOpen() => window.Open().Forget();
    public void OnClose() => window.Close().Forget();
    public void OnMinimize() => window.Minimize().Forget();
    public void OnMaximize() => window.ToggleMaximize().Forget();
}
