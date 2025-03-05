using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneManager : MonoBehaviour
{
    /// <summary>
    /// 대기 플래그
    /// </summary>
    public bool IsWait { get; set; } = false;

    /// <summary>
    /// 페이드용 이미지
    /// </summary>
    [SerializeField]
    Image m_fadeImg = null;

    /// <summary>
    /// 페이드 컬러
    /// in: 0 out: 1
    /// </summary>
    [SerializeField]
    Color[] m_fadeColorArray = null;

    /// <summary>
    /// 현재 컬러 
    /// </summary>
    [SerializeField]
    Color m_nowFadeColor;
    /// <summary>
    /// 로드 플래그
    /// </summary>
    bool m_loadFlag = false;

    /// <summary>
    /// 씬 이동
    /// </summary>
    /// <param name="argSceneName">이동할 씬 이름</param>
    public void ChangeScene(string argSceneName)
    {
        if(m_loadFlag)
        {
            return;
        }
        m_loadFlag = true;
        StartCoroutine(FadeInOut(argSceneName));
    }

    /// <summary>
    /// 페이드 인 아웃
    /// </summary>
    /// <param name="argSceneName">이동할 씬 이름</param>
    /// <returns></returns>
    IEnumerator FadeInOut(string argSceneName)
    {
        m_nowFadeColor = m_fadeColorArray[0];
        m_fadeImg.color = m_nowFadeColor;
        m_fadeImg.raycastTarget = true;

        while(m_nowFadeColor != m_fadeColorArray[1])
        {
            m_nowFadeColor.a += Time.deltaTime;

            if(m_nowFadeColor.a > 1.0f)
            {
                m_nowFadeColor.a = 1.0f;
            }
            m_fadeImg.color = m_nowFadeColor;
            yield return null;
        }

        m_nowFadeColor = m_fadeColorArray[1];
        m_fadeImg.color = m_nowFadeColor;

        AsyncOperation _ao = SceneManager.LoadSceneAsync(argSceneName, LoadSceneMode.Single);
        _ao.allowSceneActivation = false;

        while (!_ao.isDone)
        {
            if(_ao.progress >= 0.9f)
            {
                ///애니메이션 적용시 애니메 종료 시에 allow 를 true로
                _ao.allowSceneActivation = true;
            }
            yield return null;
        }
        
        while(IsWait)
        {
            yield return null;
        }

        m_nowFadeColor = m_fadeColorArray[1];
        m_fadeImg.color = m_nowFadeColor;

        while (m_nowFadeColor != m_fadeColorArray[0])
        {
            m_nowFadeColor.a -= Time.deltaTime;

            if (m_nowFadeColor.a < 0.0f)
            {
                m_nowFadeColor.a = 0.0f;
            }
            m_fadeImg.color = m_nowFadeColor;
            yield return null;
        }

        m_nowFadeColor = m_fadeColorArray[0];
        m_fadeImg.color = m_nowFadeColor;
        m_fadeImg.raycastTarget = false;
        m_loadFlag = false;

    }
}
