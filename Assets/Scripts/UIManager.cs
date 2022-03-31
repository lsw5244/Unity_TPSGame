using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    [SerializeField]
    private Image _hpbarImage;

    void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogWarning("씬에 두개 이상의 UI 매니저가 존재합니다!");
            Destroy(gameObject);
        }        
    }

    public void UpdateHpbar(float currentHpPercent)
    {
        _hpbarImage.fillAmount = currentHpPercent;
    }
}
