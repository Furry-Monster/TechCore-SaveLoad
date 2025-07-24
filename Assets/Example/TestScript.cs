using MonsterSave.Runtime;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Start()
    {
        int a = 42;
        MonsterSaveMgr.Save("number", a);

        var b = (int)MonsterSaveMgr.Load("number");
        Debug.Log(b);
    }
}