using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneManager : MonoBehaviour
{
    /// <summary>
    /// ��� �÷���
    /// </summary>
    public bool IsWait { get; set; } = false;

    /// <summary>
    /// ���̵�� �̹���
    /// </summary>
    [SerializeField]
    Image m_fadeImg = null;

    /// <summary>
    /// ���̵� �÷�
    /// in: 0 out: 1
    /// </summary>
    [SerializeField]
    Color[] m_fadeColorArray = null;

    /// <summary>
    /// ���� �÷� 
    /// </summary>
    [SerializeField]
    Color m_nowFadeColor;
    /// <summary>
    /// �ε� �÷���
    /// </summary>
    bool m_loadFlag = false;

    /// <summary>
    /// �� �̵�
    /// </summary>
    /// <param name="argSceneName">�̵��� �� �̸�</param>
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
    /// ���̵� �� �ƿ�
    /// </summary>
    /// <param name="argSceneName">�̵��� �� �̸�</param>
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
                ///�ִϸ��̼� ����� �ִϸ� ���� �ÿ� allow �� true��
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
