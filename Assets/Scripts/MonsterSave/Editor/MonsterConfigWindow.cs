using MonsterSave.Runtime;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GUILayout;
using Cache = MonsterSave.Runtime.Cache;

namespace MonsterSave.Editor
{
    public class MonsterConfigWindow : EditorWindow
    {
        private string _createdApiKey;
        private Backend _createdBackend;
        private Cache _createdCache;
        private int _createdCacheSize;
        private Encryption _createdEncryption;

        private string _createdFileName;
        private Format _createdFormat;
        private bool _createdInitialized;
        private bool _createdScheduledSync;
        private string _createdStoragePath;
        private State _currentState = State.None;
        private string _editApiKey;
        private Backend _editBackend;
        private Cache _editCache;
        private int _editCacheSize;
        private Encryption _editEncryption;

        private Format _editFormat;
        private bool _editInitialized;
        private bool _editScheduledSync;
        private string _editStoragePath;
        private Vector2 _scrollPos;

        private MonsterSaveConfig _selectedConfig;

        private void Awake()
        {
            if (_selectedConfig == null)
                // 尝试加载默认配置
                _selectedConfig = Resources.Load<MonsterSaveConfig>("DefaultConfig");
        }

        private void OnGUI()
        {
            Label("MonsterSave Configuration", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            switch (_currentState)
            {
                case State.Modifying:
                {
                    // 检查是否选择了配置
                    if (!_selectedConfig)
                    {
                        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                        Debug.LogWarning("没有选择配置文件.");
                        _currentState = State.None;
                    }

                    // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                    ShowModifyView();
                    break;
                }
                case State.Creating:
                    ShowCreatingView();
                    break;
                case State.None:
                default:
                    ShowMainView();
                    break;
            }

            EditorGUILayout.Space();
            EditorGUILayout.EndScrollView();
        }

        [MenuItem("Tools/MonsterSave/PluginConfig")]
        public static void ShowWindow()
        {
            GetWindow<MonsterConfigWindow>("MonsterSave Config");
        }

        private void ShowMainView()
        {
            // 配置管理
            Label("配置管理", EditorStyles.boldLabel);
            _selectedConfig = EditorGUILayout.ObjectField(
                "当前配置",
                _selectedConfig,
                typeof(MonsterSaveConfig),
                false) as MonsterSaveConfig;
            EditorGUILayout.BeginHorizontal();
            if (Button("新建配置", Width(100))) _currentState = State.Creating;

            if (Button("修改当前", Width(100))) _currentState = State.Modifying;

            if (Button("删除配置", Width(100)))
                if (EditorUtility.DisplayDialog("确认删除", $"确定要删除配置 {_selectedConfig?.name} 吗？", "删除", "取消"))
                    DeleteConfig(_selectedConfig);

            EditorGUILayout.EndHorizontal();

            // 杂项功能
            Label("杂项", EditorStyles.boldLabel);
            if (Button("插件测试")) TestSaveLoad(_selectedConfig);

            if (Button("打开帮助文档")) Application.OpenURL("https://github.com/your-plugin-doc-url");

            Label("插件版本: 0.0.1", EditorStyles.miniLabel);
        }

        private void ShowCreatingView()
        {
            if (!_createdInitialized)
            {
                _createdFileName = "MonsterSaveConfig.asset";
                _createdFormat = _selectedConfig.format;
                _createdBackend = _selectedConfig.backend;
                _createdEncryption = _selectedConfig.encryption;
                _createdCache = _selectedConfig.cache;
                _createdStoragePath = _selectedConfig.storagePath;
                _createdApiKey = _selectedConfig.apiKey;
                _createdScheduledSync = _selectedConfig.scheduledSync;
                _createdCacheSize = _selectedConfig.cacheSize;
                _createdInitialized = true;
            }

            EditorGUI.BeginChangeCheck();

            Label("新建配置", EditorStyles.boldLabel);
            _createdFileName = EditorGUILayout.TextField("配置文件名", _createdFileName);

            Label("General", EditorStyles.boldLabel);
            _createdFormat = (Format)EditorGUILayout.EnumPopup("序列化格式", _createdFormat);
            _createdBackend = (Backend)EditorGUILayout.EnumPopup("存储后端", _createdBackend);
            _createdEncryption = (Encryption)EditorGUILayout.EnumPopup("加密格式", _createdEncryption);
            _createdCache = (Cache)EditorGUILayout.EnumPopup("缓存策略", _createdCache);
            _createdStoragePath = EditorGUILayout.TextField("存储路径", _createdStoragePath);
            _createdApiKey = EditorGUILayout.TextField("API Key", _createdApiKey);

            Label("Advanced", EditorStyles.boldLabel);
            _createdScheduledSync = EditorGUILayout.Toggle("定时保存", _createdScheduledSync);
            _createdCacheSize = EditorGUILayout.IntField("缓存大小", _createdCacheSize);

            BeginHorizontal();
            if (Button("保存"))
            {
                var path = EditorUtility.SaveFilePanelInProject(
                    "保存MonsterSave配置",
                    _createdFileName,
                    "asset",
                    "请选择保存配置的路径"
                );

                var newConfig = CreateInstance<MonsterSaveConfig>();

                if (!string.IsNullOrEmpty(path))
                {
                    // 写回所有字段，确保最新
                    newConfig.format = _createdFormat;
                    newConfig.backend = _createdBackend;
                    newConfig.encryption = _createdEncryption;
                    newConfig.cache = _createdCache;
                    newConfig.storagePath = _createdStoragePath;
                    newConfig.apiKey = _createdApiKey;
                    newConfig.scheduledSync = _createdScheduledSync;
                    newConfig.cacheSize = _createdCacheSize;

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

            if (Button("取消")) _currentState = State.None;

            EndHorizontal();
        }

        private void ShowModifyView()
        {
            if (!_selectedConfig)
                return;

            if (!_editInitialized)
            {
                _editFormat = _selectedConfig.format;
                _editBackend = _selectedConfig.backend;
                _editEncryption = _selectedConfig.encryption;
                _editCache = _selectedConfig.cache;
                _editStoragePath = _selectedConfig.storagePath;
                _editApiKey = _selectedConfig.apiKey;
                _editScheduledSync = _selectedConfig.scheduledSync;
                _editCacheSize = _selectedConfig.cacheSize;
                _editInitialized = true;
            }

            EditorGUI.BeginChangeCheck();

            // General
            Label("General", EditorStyles.boldLabel);
            _editFormat = (Format)EditorGUILayout.EnumPopup("序列化格式", _editFormat);
            _editBackend = (Backend)EditorGUILayout.EnumPopup("存储后端", _editBackend);
            _editCache = (Cache)EditorGUILayout.EnumPopup("缓存策略", _editCache);
            _editEncryption = (Encryption)EditorGUILayout.EnumPopup("加密格式", _editEncryption);
            _editStoragePath = EditorGUILayout.TextField("存储路径", _editStoragePath);
            _editApiKey = EditorGUILayout.TextField("API Key", _editApiKey);

            // Advanced
            Label("Advanced", EditorStyles.boldLabel);
            _editScheduledSync = EditorGUILayout.Toggle("定时保存", _editScheduledSync);
            _editCacheSize = EditorGUILayout.IntField("缓存大小", _editCacheSize);

            BeginHorizontal();
            if (Button("保存"))
            {
                // 只有点击保存才写回
                _selectedConfig.format = _editFormat;
                _selectedConfig.backend = _editBackend;
                _selectedConfig.cache = _editCache;
                _selectedConfig.encryption = _editEncryption;
                _selectedConfig.storagePath = _editStoragePath;
                _selectedConfig.scheduledSync = _editScheduledSync;
                _selectedConfig.cacheSize = _editCacheSize;

                EditorUtility.SetDirty(_selectedConfig);
                AssetDatabase.SaveAssets();
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                Debug.Log($"成功保存修改后的配置到 {_selectedConfig.name}.");
                _currentState = State.None;
                _editInitialized = false;
            }

            if (Button("取消"))
            {
                _currentState = State.None;
                _editInitialized = false;
            }

            EndHorizontal();
        }

        private void DeleteConfig(MonsterSaveConfig config)
        {
            if (config.name == "DefaultConfig")
            {
                EditorUtility.DisplayDialog("错误", "默认配置不能删除", "确定");
                return;
            }

            var path = AssetDatabase.GetAssetPath(config);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            _selectedConfig = null;
            EditorUtility.DisplayDialog("成功", "配置已删除", "确定");
        }

        private void TestSaveLoad(MonsterSaveConfig config)
        {
            if (!config)
            {
                EditorUtility.DisplayDialog("错误", "未选择配置", "确定");
                return;
            }

            // 这里仅做简单模拟
            var json = JsonUtility.ToJson(config);
            var temp = CreateInstance<MonsterSaveConfig>();
            JsonUtility.FromJsonOverwrite(json, temp);
            EditorUtility.DisplayDialog("测试结果", $"保存/加载模拟成功\n{json}", "确定");
            DestroyImmediate(temp);
        }

        private enum State
        {
            None,
            Creating,
            Modifying
        }
    }
}