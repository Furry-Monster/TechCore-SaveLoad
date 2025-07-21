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
            // ���Լ���Ĭ������
            _selectedConfig = Resources.Load<MonsterSaveConfig>("DefaultConfig");
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("MonsterSave Configuration", EditorStyles.boldLabel);

        if (_currentState == State.Modifying)
        {
            // ����Ƿ�ѡ��������
            if (_selectedConfig == null)
            {
                Debug.LogWarning("û��ѡ�������ļ�.");
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
        // ���ù���
        GUILayout.Label("���ù���", EditorStyles.boldLabel);
        _selectedConfig = EditorGUILayout.ObjectField(
            "��ǰ����",
            _selectedConfig,
            typeof(MonsterSaveConfig),
            false) as MonsterSaveConfig;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("�½�����", GUILayout.Width(100)))
        {
            _currentState = State.Creating;
        }
        if (GUILayout.Button("�޸ĵ�ǰ", GUILayout.Width(100)))
        {
            _currentState = State.Modifying;
        }
        if (GUILayout.Button("ɾ������", GUILayout.Width(100)))
        {
            if (EditorUtility.DisplayDialog("ȷ��ɾ��", $"ȷ��Ҫɾ������ {_selectedConfig.name} ��", "ɾ��", "ȡ��"))
            {
                DeleteConfig(_selectedConfig);
            }
        }
        EditorGUILayout.EndHorizontal();

        // �����
        GUILayout.Label("����", EditorStyles.boldLabel);
        if (GUILayout.Button("�������"))
        {
            TestSaveLoad(_selectedConfig);
        }
        if (GUILayout.Button("�򿪰����ĵ�"))
        {
            Application.OpenURL("https://github.com/your-plugin-doc-url");
        }
        GUILayout.Label("����汾: 0.0.1", EditorStyles.miniLabel);
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

        GUILayout.Label("�½�����", EditorStyles.boldLabel);
        _createdFileName = EditorGUILayout.TextField("�����ļ���", _createdFileName);

        GUILayout.Label("General", EditorStyles.boldLabel);
        _createdFormat = (Format)EditorGUILayout.EnumPopup("���л���ʽ", _createdFormat);
        _createdMedia = (Media)EditorGUILayout.EnumPopup("�洢����", _createdMedia);
        _createdEncryption = (Encryption)EditorGUILayout.EnumPopup("���ܸ�ʽ", _createdEncryption);
        _createdStoragePath = EditorGUILayout.TextField("�洢·��", _createdStoragePath);

        GUILayout.Label("Advanced", EditorStyles.boldLabel);
        _createdScheduledSync = EditorGUILayout.Toggle("��ʱ����", _createdScheduledSync);
        _createdTypeCache = EditorGUILayout.Toggle("���ͻ���", _createdTypeCache);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("����"))
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "����MonsterSave����",
                _createdFileName,
                "asset",
                "��ѡ�񱣴����õ�·��"
            );

            var newConfig = ScriptableObject.CreateInstance<MonsterSaveConfig>();

            if (!string.IsNullOrEmpty(path))
            {
                // д�������ֶΣ�ȷ������
                EditorUtility.SetDirty(newConfig);
                AssetDatabase.CreateAsset(newConfig, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("�ɹ�", "�������ѱ���", "ȷ��");
                _currentState = State.None;

                // ���¼����ѱ��������
                _selectedConfig = AssetDatabase.LoadAssetAtPath<MonsterSaveConfig>(path);
            }
        }

        if (GUILayout.Button("ȡ��"))
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
        _editFormat = (Format)EditorGUILayout.EnumPopup("���л���ʽ", _editFormat);
        _editMedia = (Media)EditorGUILayout.EnumPopup("�洢����", _editMedia);
        _editEncryption = (Encryption)EditorGUILayout.EnumPopup("���ܸ�ʽ", _editEncryption);
        _editStoragePath = EditorGUILayout.TextField("�洢·��", _editStoragePath);

        // Advanced
        GUILayout.Label("Advanced", EditorStyles.boldLabel);
        _editScheduledSync = EditorGUILayout.Toggle("��ʱ����", _editScheduledSync);
        _editTypeCache = EditorGUILayout.Toggle("���ͻ���", _editTypeCache);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("����"))
        {
            // ֻ�е�������д��
            _selectedConfig.format = _editFormat;
            _selectedConfig.media = _editMedia;
            _selectedConfig.encryption = _editEncryption;
            _selectedConfig.storagePath = _editStoragePath;
            _selectedConfig.scheduledSync = _editScheduledSync;
            _selectedConfig.typeCache = _editTypeCache;

            EditorUtility.SetDirty(_selectedConfig);
            AssetDatabase.SaveAssets();
            Debug.Log($"�ɹ������޸ĺ�����õ� {_selectedConfig.name}.");
            _currentState = State.None;
            _editInitialized = false;
        }
        if (GUILayout.Button("ȡ��"))
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
            EditorUtility.DisplayDialog("����", "Ĭ�����ò���ɾ��", "ȷ��");
            return;
        }

        string path = AssetDatabase.GetAssetPath(config);
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        _selectedConfig = null;
        EditorUtility.DisplayDialog("�ɹ�", "������ɾ��", "ȷ��");
    }

    private void TestSaveLoad(MonsterSaveConfig config)
    {
        if (config == null)
        {
            EditorUtility.DisplayDialog("����", "δѡ������", "ȷ��");
            return;
        }

        // ���������ģ��
        string json = JsonUtility.ToJson(config);
        var temp = ScriptableObject.CreateInstance<MonsterSaveConfig>();
        JsonUtility.FromJsonOverwrite(json, temp);
        EditorUtility.DisplayDialog("���Խ��", $"����/����ģ��ɹ�\n{json}", "ȷ��");
        DestroyImmediate(temp);
    }
}