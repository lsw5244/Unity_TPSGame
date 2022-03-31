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

    [SerializeField]
    private GameObject _clearPanel;
        
    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void RemoveMonsterCount()
    {
        monsterCount--;
        if(monsterCount <= 0)
        {
            _clearPanel.SetActive(true);
            //ClearStage();
        }
    }

    public void GameReset()
    {
        BulletAbility.Instance.ClearAbility();
        ImpactAbility.Instance.ClearAbility();
        HitAbility.Instance.ClearAbility();

        nextSceneName = "Stage1";
        _clearPanel.SetActive(false);

        StartCoroutine(FadeOut());
    }

    public void PlayerDie()
    {
        nextSceneName = "Stage1";
        StartCoroutine(FadeOut());
    }

    public void ClearStage()
    {
        _clearPanel.SetActive(false);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn() // 밝아지기
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

    IEnumerator FadeOut() // 어두워지기
    {
        _runningFadeAnimation = true;
        Color fadeColor = fadeImage.color;

        while (fadeColor.a < 1f)
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
        SceneManager.LoadScene(nextSceneName); // 씬 번호로 매개변수를 바꿀 수 있다.
    }
}
