using System;
using System.Collections.Generic;
using System.IO;
using MonsterSerializer;
using UnityEngine;

public class TestMSerializer : MonoBehaviour
{
    private void Start()
    {
        var playerData = new PlayerData
        {
            name = "Elysia",
            score = new List<int> { 1, 2, 3, 4, 5, 6, 7 }
        };

        var result = MSerializer.Serialize(playerData, new SerializerSetting
        {
            Format = Format.XML
        });

        File.WriteAllBytes(Application.dataPath + "/Resources/save1", result);

        var deserialized = MSerializer.Deserialize<PlayerData>(result);
        Debug.Log(deserialized.name);
    }

    [Serializable]
    public class PlayerData
    {
        public string name;
        public List<int> score;
    }
}