using MonsterSave.Runtime;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Start()
    {
        var config = ScriptableObject.CreateInstance<MonsterSaveConfig>();
        config.backend = Backend.MemoryOnly;
        config.format = Format.JSON;
        MonsterSaveMgr.Config = config;
    }
}