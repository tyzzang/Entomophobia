using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{    
    public LangType.Type m_langType;

    public static LanguageManager Instance
    {
        get;
        private set;
    } = null;

    
    private void Awake()
    {
        if (LanguageManager.Instance == null)
        {
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
    }

}
