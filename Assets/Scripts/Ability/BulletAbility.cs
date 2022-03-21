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
            Debug.LogWarning($"BulletAbility가 여러개가 감지되어 {gameObject.name}를 삭제합니다.");
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
