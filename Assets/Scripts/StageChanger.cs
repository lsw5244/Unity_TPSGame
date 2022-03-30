using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageChanger : MonoBehaviour
{
    public int monsterCount = 1;
    public Image fadeImage;

    public string nextSceneName;

    private bool _runningFadeAnimation = false;



    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        //Debug.Log(fadeImage.color.a);   // 0�� ���� 1�� ������
        if(Input.GetKeyDown(KeyCode.Tab) && _runningFadeAnimation == false)
        {
            StartCoroutine(FadeIn());
        }
        if(Input.GetKeyDown(KeyCode.CapsLock) && _runningFadeAnimation == false)
        {
            StartCoroutine(FadeOut());
        }
    }

    public void RemoveMonsterCount()
    {
        monsterCount--;
        if(monsterCount <= 0)
        {
            ClearStage();
        }
    }

    void ClearStage()
    {

    }

    IEnumerator FadeIn() // �������
    {
        _runningFadeAnimation = true;
        Color fadeColor = fadeImage.color;
        
        while (fadeColor.a > 0)
        {
            fadeColor.a -= 0.1f;
            fadeImage.color = fadeColor;
            yield return new WaitForSeconds(0.1f);
        }
        _runningFadeAnimation = false;
    }

    IEnumerator FadeOut() // ��ο�����
    {
        _runningFadeAnimation = true;
        Color fadeColor = fadeImage.color;

        while (fadeColor.a < 1)
        {
            fadeColor.a += 0.1f;
            fadeImage.color = fadeColor;
            yield return new WaitForSeconds(0.1f);
        }
        _runningFadeAnimation = false;

        ChangeScene();
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneName); // �� ��ȣ�� �Ű������� �ٲ� �� �ִ�.
    }
}
