// Assets/Editor/StateCreator.cs
using System.IO;
using UnityEditor;
using UnityEngine;

public class StateCreator : EditorWindow
{
    private string className = "NewState";

    [MenuItem("Assets/Create/State/Create IState with Name", false, 80)]
    public static void OpenWindow()
    {
        StateCreator window = ScriptableObject.CreateInstance<StateCreator>();
        window.titleContent = new GUIContent("Create IState");
        window.minSize = new Vector2(300, 80);
        window.ShowUtility();
    }

    void OnGUI()
    {
        GUILayout.Label("ステートクラス名を入力してください", EditorStyles.boldLabel);
        className = EditorGUILayout.TextField("Class Name", className);

        GUILayout.Space(10);

        if (GUILayout.Button("Create Script"))
        {
            CreateStateScript(className);
            Close();
        }
    }

    private void CreateStateScript(string name)
    {
        name += "State";
        if (string.IsNullOrWhiteSpace(name))
        {
            Debug.LogError("クラス名が空です。");
            return;
        }

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
        {
            path = "Assets";
        }

        string filePath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, name + ".cs"));

        string script = $@"using UnityEngine;

public class {name} : IState
{{
    public void Enter()
    {{
        Debug.Log(""{name}: Enter"");
    }}

    public void Execute()
    {{
        Debug.Log(""{name}: Execute"");
    }}

    public void Exit()
    {{
        Debug.Log(""{name}: Exit"");
    }}
}}";

        File.WriteAllText(filePath, script);
        AssetDatabase.Refresh();
        Debug.Log($"IState 実装クラス '{name}' を作成しました: {filePath}");
    }
}
