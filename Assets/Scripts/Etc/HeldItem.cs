using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldItem : MonoBehaviour
{
    [SerializeField]
    Grabbable m_grabbable;
    [SerializeField]
    AudioSource m_audioSource;
    [SerializeField]
    AudioClip m_clip;

    bool m_flag=true;
    void Update()
    {
        if(m_grabbable.BeingHeld && m_flag)
        {
            m_audioSource.PlayOneShot(m_clip);
            m_flag = false;
        }else if(!m_grabbable.BeingHeld && !m_flag)
        {
            m_flag = true;
        }
    }
}
