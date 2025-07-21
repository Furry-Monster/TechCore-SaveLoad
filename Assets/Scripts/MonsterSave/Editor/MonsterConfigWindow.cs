using MonsterSave.Runtime;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MonsterConfigWindow : EditorWindow
{
    private enum State
    {
        None,
        Creating,
        Modifying,
    }

    private MonsterSaveConfig _selectedConfig;
    private State _currentState = State.None;
    private Vector2 _scrollPos;

    [MenuItem("Tools/MonsterSave/PluginConfig")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<MonsterConfigWindow>("MonsterSave Config");
    }

    private void Awake()
    {
        if (_selectedConfig == null)
        {
            // 尝试加载默认配置
            _selectedConfig = Resources.Load<MonsterSaveConfig>("DefaultConfig");
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("MonsterSave Configuration", EditorStyles.boldLabel);

        if (_currentState == State.Modifying)
        {
            // 检查是否选择了配置
            if (_selectedConfig == null)
            {
                Debug.LogWarning("没有选择配置文件.");
                _currentState = State.None;
            }

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            ShowModifyView();
            EditorGUILayout.Space();
            EditorGUILayout.EndScrollView();
        }
        else if (_currentState == State.Creating)
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            ShowCreatingView();
            EditorGUILayout.Space();
            EditorGUILayout.EndScrollView();
        }
        else
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            ShowMainView();

            EditorGUILayout.Space();
            EditorGUILayout.EndScrollView();
        }
    }

    private void ShowMainView()
    {
        // 配置管理
        GUILayout.Label("配置管理", EditorStyles.boldLabel);
        _selectedConfig = EditorGUILayout.ObjectField(
            "当前配置",
            _selectedConfig,
            typeof(MonsterSaveConfig),
            false) as MonsterSaveConfig;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("新建配置", GUILayout.Width(100)))
        {
            _currentState = State.Creating;
        }
        if (GUILayout.Button("修改当前", GUILayout.Width(100)))
        {
            _currentState = State.Modifying;
        }
        if (GUILayout.Button("删除配置", GUILayout.Width(100)))
        {
            if (EditorUtility.DisplayDialog("确认删除", $"确定要删除配置 {_selectedConfig.name} 吗？", "删除", "取消"))
            {
                DeleteConfig(_selectedConfig);
            }
        }
        EditorGUILayout.EndHorizontal();

        // 杂项功能
        GUILayout.Label("杂项", EditorStyles.boldLabel);
        if (GUILayout.Button("插件测试"))
        {
            TestSaveLoad(_selectedConfig);
        }
        if (GUILayout.Button("打开帮助文档"))
        {
            Application.OpenURL("https://github.com/your-plugin-doc-url");
        }
        GUILayout.Label("插件版本: 0.0.1", EditorStyles.miniLabel);
    }

    private string _createdFileName;
    private Format _createdFormat;
    private Media _createdMedia;
    private Encryption _createdEncryption;
    private string _createdStoragePath;
    private bool _createdScheduledSync;
    private bool _createdTypeCache;
    private bool _createdInitialized = false;

    private void ShowCreatingView()
    {
        if (!_createdInitialized)
        {
            _createdFileName = "MonsterSaveConfig.asset";
            _createdFormat = _selectedConfig.format;
            _createdMedia = _selectedConfig.media;
            _createdEncryption = _selectedConfig.encryption;
            _createdStoragePath = _selectedConfig.storagePath;
            _createdScheduledSync = _selectedConfig.scheduledSync;
            _createdTypeCache = _selectedConfig.typeCache;
            _createdInitialized = true;
        }

        EditorGUI.BeginChangeCheck();

        GUILayout.Label("新建配置", EditorStyles.boldLabel);
        _createdFileName = EditorGUILayout.TextField("配置文件名", _createdFileName);

        GUILayout.Label("General", EditorStyles.boldLabel);
        _createdFormat = (Format)EditorGUILayout.EnumPopup("序列化格式", _createdFormat);
        _createdMedia = (Media)EditorGUILayout.EnumPopup("存储介质", _createdMedia);
        _createdEncryption = (Encryption)EditorGUILayout.EnumPopup("加密格式", _createdEncryption);
        _createdStoragePath = EditorGUILayout.TextField("存储路径", _createdStoragePath);

        GUILayout.Label("Advanced", EditorStyles.boldLabel);
        _createdScheduledSync = EditorGUILayout.Toggle("定时保存", _createdScheduledSync);
        _createdTypeCache = EditorGUILayout.Toggle("类型缓存", _createdTypeCache);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("保存"))
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "保存MonsterSave配置",
                _createdFileName,
                "asset",
                "请选择保存配置的路径"
            );

            var newConfig = ScriptableObject.CreateInstance<MonsterSaveConfig>();

            if (!string.IsNullOrEmpty(path))
            {
                // 写回所有字段，确保最新
                EditorUtility.SetDirty(newConfig);
                AssetDatabase.CreateAsset(newConfig, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("成功", "新配置已保存", "确定");
                _currentState = State.None;

                // 重新加载已保存的配置
                _selectedConfig = AssetDatabase.LoadAssetAtPath<MonsterSaveConfig>(path);
            }
        }

        if (GUILayout.Button("取消"))
        {
            _currentState = State.None;
        }
        GUILayout.EndHorizontal();
    }

    private Format _editFormat;
    private Media _editMedia;
    private Encryption _editEncryption;
    private string _editStoragePath;
    private bool _editScheduledSync;
    private bool _editTypeCache;
    private bool _editInitialized = false;

    private void ShowModifyView()
    {
        if (_selectedConfig == null)
            return;

        if (!_editInitialized)
        {
            _editFormat = _selectedConfig.format;
            _editMedia = _selectedConfig.media;
            _editEncryption = _selectedConfig.encryption;
            _editStoragePath = _selectedConfig.storagePath;
            _editScheduledSync = _selectedConfig.scheduledSync;
            _editTypeCache = _selectedConfig.typeCache;
            _editInitialized = true;
        }

        EditorGUI.BeginChangeCheck();

        // General
        GUILayout.Label("General", EditorStyles.boldLabel);
        _editFormat = (Format)EditorGUILayout.EnumPopup("序列化格式", _editFormat);
        _editMedia = (Media)EditorGUILayout.EnumPopup("存储介质", _editMedia);
        _editEncryption = (Encryption)EditorGUILayout.EnumPopup("加密格式", _editEncryption);
        _editStoragePath = EditorGUILayout.TextField("存储路径", _editStoragePath);

        // Advanced
        GUILayout.Label("Advanced", EditorStyles.boldLabel);
        _editScheduledSync = EditorGUILayout.Toggle("定时保存", _editScheduledSync);
        _editTypeCache = EditorGUILayout.Toggle("类型缓存", _editTypeCache);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("保存"))
        {
            // 只有点击保存才写回
            _selectedConfig.format = _editFormat;
            _selectedConfig.media = _editMedia;
            _selectedConfig.encryption = _editEncryption;
            _selectedConfig.storagePath = _editStoragePath;
            _selectedConfig.scheduledSync = _editScheduledSync;
            _selectedConfig.typeCache = _editTypeCache;

            EditorUtility.SetDirty(_selectedConfig);
            AssetDatabase.SaveAssets();
            Debug.Log($"成功保存修改后的配置到 {_selectedConfig.name}.");
            _currentState = State.None;
            _editInitialized = false;
        }
        if (GUILayout.Button("取消"))
        {
            _currentState = State.None;
            _editInitialized = false;
        }
        GUILayout.EndHorizontal();
    }

    private void DeleteConfig(MonsterSaveConfig config)
    {
        if (config.name == "DefaultConfig")
        {
            EditorUtility.DisplayDialog("错误", "默认配置不能删除", "确定");
            return;
        }

        string path = AssetDatabase.GetAssetPath(config);
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        _selectedConfig = null;
        EditorUtility.DisplayDialog("成功", "配置已删除", "确定");
    }

    private void TestSaveLoad(MonsterSaveConfig config)
    {
        if (config == null)
        {
            EditorUtility.DisplayDialog("错误", "未选择配置", "确定");
            return;
        }

        // 这里仅做简单模拟
        string json = JsonUtility.ToJson(config);
        var temp = ScriptableObject.CreateInstance<MonsterSaveConfig>();
        JsonUtility.FromJsonOverwrite(json, temp);
        EditorUtility.DisplayDialog("测试结果", $"保存/加载模拟成功\n{json}", "确定");
        DestroyImmediate(temp);
    }
}