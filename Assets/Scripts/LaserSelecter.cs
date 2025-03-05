using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class LaserSelecter : MonoBehaviour
{
    [SerializeField]
    float FiringRate = 0.5f;
    [SerializeField]
    float SlowMoRateOfFire = 0.3f;
    float lastShotTime;

    [SerializeField]
    Transform m_rayST;
    [SerializeField]
    float m_rayLength;
    [SerializeField]
    AudioSource m_audioSource;
    [SerializeField]
    AudioClip m_clip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(InputBridge.Instance.RightTriggerDown)
        {
            float shotInterval = Time.timeScale < 1 ? SlowMoRateOfFire : FiringRate;
            if (Time.time - lastShotTime < shotInterval)
            {
                return;
            }
            InputBridge.Instance.VibrateController(0.1f, 0.2f, 0.1f, ControllerHand.Right);
            m_audioSource.PlayOneShot(m_clip);
            lastShotTime = Time.time;

            Ray _ray = new Ray(m_rayST.position, m_rayST.forward);
            RaycastHit _hit;
                        

            if (Physics.Raycast(_ray, out _hit, m_rayLength))// && _hit.transform.gameObject.layer == 9)
            {
                _hit.transform.rotation *= Quaternion.Euler(90, 0, 0);
                Debug.Log(_hit.transform.gameObject.name); 
            }
        }
    }
}
