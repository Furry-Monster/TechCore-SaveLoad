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
            score = new List<int> { 1, 2, 3, 4, 5, 6, 7 },
            position = new Vector3(11, 45, 14),
            rotation = new Quaternion(19, 19, 81, 0)
        };

        var result = MSerializer.Serialize(playerData, new SerializerSetting
        {
            Format = Format.JSON,
            Encryption = Encryption.AES
        });

        File.WriteAllBytes(Application.dataPath + "/Resources/save1", result);

        var deserialized = MSerializer.Deserialize<PlayerData>(result);
        Debug.Log(deserialized.name);
        Debug.Log(deserialized.score.ToString());
    }

    [Serializable]
    public class PlayerData
    {
        public string name;
        public List<int> score;
        public Vector3 position;
        public Quaternion rotation;
    }
}