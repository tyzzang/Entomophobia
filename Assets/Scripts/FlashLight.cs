using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField]
    float FiringRate = 0.5f;
    [SerializeField]
    float SlowMoRateOfFire = 0.3f;
    float lastShotTime;
    [SerializeField]
    Grabbable m_grabbable;
    [SerializeField]
    public GameObject[] m_light;
    [SerializeField]
    AudioSource m_audioSource;
    [SerializeField]
    AudioClip[] m_clips;
    bool isLightOn = false;
    public bool m_flag = false;
    bool m_blinkFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
   

    void Update()
    {
        if (m_grabbable.BeingHeld)
        {          
            if (!m_blinkFlag)
            {
                GManager.Instance.BlinkLight(1, 7);
                m_audioSource.PlayOneShot(m_clips[0]);
                isLightOn = !isLightOn;
                m_blinkFlag =true;
            }
            
            if (InputBridge.Instance.AButtonDown)
            {
                if (m_flag) return;
                float shotInterval = Time.timeScale < 1 ? SlowMoRateOfFire : FiringRate;
                if (Time.time - lastShotTime < shotInterval)
                {
                    return;
                }
                InputBridge.Instance.VibrateController(0.1f, 0.2f, 0.1f, ControllerHand.Right);
                // 현재 상태를 반전시키기
                isLightOn = !isLightOn;
                m_light[0].SetActive(isLightOn);
                m_light[1].SetActive(isLightOn);
                m_audioSource.PlayOneShot(m_clips[1]);
            }
        }
    }
}
