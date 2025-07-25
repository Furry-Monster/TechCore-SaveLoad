using System.Collections.Generic;
using MonsterSave.Runtime;
using UnityEngine;

public class MonsterSaveTest : MonoBehaviour
{
    private void Start()
    {
        // 1. 配置存储后端和序列化格式
        var config = ScriptableObject.CreateInstance<MonsterSaveConfig>();
        config.backend = Backend.LocalFile; // 可切换为PlayerPrefs/Cloud/Database
        config.format = Format.JSON; // 可切换为Binary/XML
        config.storagePath = Application.persistentDataPath + "/testsave.ms";
        MonsterSaveMgr.Config = config;
        Debug.Log("Pass case 1");

        // 2. 基本写入和读取
        MonsterSaveMgr.Set("playerName", "FurryMonster");
        MonsterSaveMgr.Set("score", 12345);
        MonsterSaveMgr.Set("isAlive", true);
        Debug.Log("Pass case 2");

        Debug.Log("playerName: " + MonsterSaveMgr.Get<string>("playerName"));
        Debug.Log("score: " + MonsterSaveMgr.Get<int>("score"));
        Debug.Log("isAlive: " + MonsterSaveMgr.Get<bool>("isAlive"));
        Debug.Log("Pass case 3");

        // 3. 存在性判断
        Debug.Log("Has 'score'? " + MonsterSaveMgr.Exist("score"));
        Debug.Log("Has 'level'? " + MonsterSaveMgr.Exist("level"));
        Debug.Log("Pass case 4");

        // 4. 删除
        MonsterSaveMgr.Delete("isAlive");
        Debug.Log("After delete, isAlive: " + MonsterSaveMgr.Get<bool>("isAlive", false));
        Debug.Log("Pass case 5");

        // 5. 批量同步
        var allData = new Dictionary<string, object>
        {
            { "playerName", "NewName" },
            { "score", 99999 },
            { "level", 7 }
        };
        MonsterSaveMgr.SyncAll(allData);
        Debug.Log("Pass case 6");

        // 6. 批量读取
        var loaded = MonsterSaveMgr.LoadAll();
        foreach (var kv in loaded)
        {
            Debug.Log($"[LoadAll] {kv.Key} = {kv.Value}");
        }

        Debug.Log("Pass case 7");
    }
}