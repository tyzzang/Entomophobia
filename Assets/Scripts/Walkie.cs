using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Walkie : MonoBehaviour
{
    [SerializeField]
    AudioSource m_audio;

    [SerializeField]
    List<AudioClip> m_audioList;

    [SerializeField]
    TMP_Text m_text;

    [SerializeField]
    RectTransform m_textRect;

    [SerializeField]
    AudioClip[] m_textSound;

    [SerializeField]
    List<string> m_subtitleList;

    [SerializeField]
    Grabbable grabbable;

    [SerializeField]
    GameObject m_toolTip;

    bool IsPlayText = false;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(playText(m_subtitleList[0]));
        m_toolTip.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbable.BeingHeld && !IsPlayText)
        {
            m_toolTip.SetActive(true);
            switch(LanguageManager.Instance.m_langType)
            {
                case LangType.Type.English:
                    StartCoroutine(playText(m_subtitleList[(int)LangType.Type.English],213f,0.05f));
                    m_audio.PlayOneShot(m_textSound[(int)LangType.Type.English]);
                    break;
                case LangType.Type.Japanese:
                    StartCoroutine(playText(m_subtitleList[(int)LangType.Type.Japanese], 161f,0.1f));
                    m_audio.PlayOneShot(m_textSound[(int)LangType.Type.Japanese]);
                    break;
                case LangType.Type.Korean:
                    StartCoroutine(playText(m_subtitleList[(int)LangType.Type.Korean], 161f, 0.1f));
                    m_audio.PlayOneShot(m_textSound[(int)LangType.Type.Japanese]);
                    break;
            }
            
        }
        else if (grabbable.BeingHeld)
        {
            m_toolTip.SetActive(true);
        }
        else if (!grabbable.BeingHeld && IsPlayText)
        {
            m_toolTip.SetActive(false);
        }
    }
 
    public IEnumerator playText(string argText, float argScroll, float argSecond)
    {
        IsPlayText = true;
        m_audio.clip = null;
        m_text.text = null;
        m_text.text = argText;
        float increment = argScroll / (argText.Length-50);
        
        for (int i = 0; i < argText.Length; i++)
        {
            if(i >= 50)
                m_textRect.anchoredPosition = new Vector2(m_textRect.anchoredPosition.x,m_textRect.anchoredPosition.y + increment);
            yield return new WaitForSeconds(argSecond);                             
        }
       
    }
}
