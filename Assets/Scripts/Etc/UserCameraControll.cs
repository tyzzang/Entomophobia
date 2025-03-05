using BNG;
using DG.Tweening;
using FIMSpace.FProceduralAnimation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCameraControll : MonoBehaviour
{
    // Start is called before the first frame update

    

    [SerializeField]
    Camera m_mainCamera;

    [SerializeField]
    Transform m_LookAtTrans;

    [SerializeField]
    ScreenFader m_screenFader;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraControl()
    {
        GManager.Instance.IsHMD.enabled = false;
        
        //InputBridge.Instance.enabled = false;
        GManager.Instance.IsScreenFader.DoFadeIn();

        m_mainCamera.transform.DOLookAt(m_LookAtTrans.position, 0.2f).
            OnComplete(() =>
            {
                m_screenFader.DoFadeOut();
                GManager.Instance.IsMainAnimator.SetBool("Bite", true);
                //m_trackedHMD.enabled = true;
            });
        
    }
}
