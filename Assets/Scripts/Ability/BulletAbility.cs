using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAbility : MonoBehaviour
{
    public static BulletAbility _instance;
    public static BulletAbility Instance { get { return _instance; } }

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogWarning($"BulletAbility�� �������� �����Ǿ� {gameObject.name}�� �����մϴ�.");
            Destroy(gameObject);
        }
    }

    public void DebugFunc()
    {
        Debug.Log("@@@@@@@@@");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
