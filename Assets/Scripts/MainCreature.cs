using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using BNG;
using FIMSpace.FProceduralAnimation;

public class MainCreature : MonoBehaviour
{

    public tmpEvent m_tmpEventScript;
    public AudioSource m_audioSource;
    bool m_flag = false;
    bool m_checkFlag = false;

    bool m_recoverFlag = false;
    bool m_scareFlag = false;
    bool m_destroyFlag = false;
    bool m_stunFlag = false;
    bool m_killFlag = false;
    bool m_biteFlag = false;

    public GameObject m_UserCameraPos;

    public bool m_userViewFlag = false;
    public bool m_checkDistance = false;
    public Transform m_rayST;
    public float m_length = 0.0f;
    public float m_rayLength = 0.0f;

    public bool _flag = false;


    public Transform[] m_targetPos;

    public UnityEvent m_CreatureEvent;
    
    public LegsAnimator m_legAni;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(m_flag)
        //{
        //    PlayerCheck();
        //}        
        //AniPlay();

        if (CheckInUser)
        {
            if (_flag) return;
            GManager.Instance.IsAgent.speed = 0.0f;
            GManager.Instance.IsChaController.enabled = false;
            m_legAni.enabled = false;
            transform.LookAt(GManager.Instance.IsMainCameraPos.position);
            m_audioSource.PlayOneShot(GManager.Instance.IsSoundClips[4]);
            m_audioSource.PlayOneShot(GManager.Instance.IsCreatureSounds[10]);
            m_CreatureEvent.Invoke();
            _flag = true;
        }


        if(!m_flag && m_tmpEventScript != null)
        {
            if (!GManager.Instance.IsAgent.pathPending && GManager.Instance.IsAgent.remainingDistance <= GManager.Instance.IsAgent.stoppingDistance)
            {
                if (!GManager.Instance.IsAgent.hasPath || GManager.Instance.IsAgent.velocity.sqrMagnitude == 0f)
                {
                    m_tmpEventScript.m_doorObj.transform.DOLocalRotate(new Vector3(0, 100, 0), 1.0f, RotateMode.Fast);
                    m_tmpEventScript.m_audio.PlayOneShot(m_tmpEventScript.m_audioClip[1]);
                    //StartCoroutine(GManager.Instance.StartPlayAudioClipsAfterDelay(1.0f, GManager.Instance.PlayAudioClips(m_tmpEventScript.transform, GManager.Instance.IsSoundClips[3], 1)));
                    m_tmpEventScript.m_audio.PlayOneShot(GManager.Instance.IsSoundClips[3]);
                    GManager.Instance.BlinkLight(7, 8);
                    GManager.Instance.IsMainCreature.SetActive(false);
                    GManager.Instance.IsFlashScript.m_light[0].SetActive(true);
                    
                    GManager.Instance.IsFlashScript.m_flag = false;
                    GManager.Instance.IsAgent.enabled = false;
                    m_flag = true;
                }
            }
        }
        
        if(GManager.Instance.IsAgent.enabled && GManager.Instance.m_flag)
        {
            GManager.Instance.IsAgent.SetDestination(GManager.Instance.IsUserObj.transform.position);
        }

        if (m_tmpEventScript != null)
        {            
            //if (GManager.Instance.IsCartList[0].m_Position >= GManager.Instance.IsCartList[0].m_Path.PathLength
            //    && !m_flag)
            //{
            //    m_tmpEventScript.m_doorObj.transform.DOLocalRotate(new Vector3(0, 100, 0), 1.0f, RotateMode.Fast);
            //    m_tmpEventScript.m_audio.PlayOneShot(m_tmpEventScript.m_audioClip[1]);
            //    transform.gameObject.SetActive(false);
            //    m_flag = true;
            //}
        }
        
        if (transform.parent.parent!=null &&transform.parent.parent.name.Equals("First_cart"))
        {            
            if (GManager.Instance.IsCartList[0].m_Position >= GManager.Instance.IsCartList[0].m_Path.PathLength)
            {
                GManager.Instance.IsAgent.enabled = true;
                GManager.Instance.IsAgent.SetDestination(m_targetPos[0].position);
                GManager.Instance.IsMainCreature.transform.SetParent(null);
            }
        }

    }
    public void PlayerCheck()
    {

        Debug.Log("ssda");
        transform.parent = m_UserCameraPos.transform;
        transform.DOLocalMove(Vector3.zero, 2f);
        transform.DOLocalRotate(Vector3.zero, 0.5f);
        Kill();
        m_checkFlag = true;
        //transform.localEulerAngles = new Vector3(-20.0f, 0, 0f); 
    }
    public void AniPlay()
    {
        for (int i = 0; i < (int)AniStateType.Type.End; i++)
        {
            AniStateType.Type _type = (AniStateType.Type)i;
            GManager.Instance.IsMainAnimator.SetFloat(_type.ToString(), GManager.Instance.GetAniState(_type));

        }
    }
    public void Stun()
    {
        m_stunFlag = true;
        GManager.Instance.IsMainAnimator.SetBool("Stun", m_stunFlag);
    }
    public void Recover()
    {
        m_recoverFlag = true;
        GManager.Instance.IsMainAnimator.SetBool("Recover", m_recoverFlag);
    }

    public void CreatureDestroy()
    {
        m_destroyFlag = true;
        GManager.Instance.IsMainAnimator.SetBool("Destory", m_destroyFlag);
    }

    public void Scare()
    {
        m_scareFlag = true;
        GManager.Instance.IsMainAnimator.SetBool("Scare", m_scareFlag);
    }

    public void Kill()
    {
        m_killFlag = true;
        GManager.Instance.IsMainAnimator.SetBool("Kill", m_killFlag);
    }

    public void Bite()
    {
        m_killFlag = true;
        GManager.Instance.IsMainAnimator.SetBool("Kill", m_killFlag);
    }

    public void walkingSound(int argIndex)
    {
        m_audioSource.PlayOneShot(GManager.Instance.IsCreatureSounds[argIndex]);
    }

    public void GameEnd()
    {
        GManager.Instance.GameEnding();
    }


    public void AniEventParser(AniStateType.Type argType)
    {
        switch (argType)
        {
            case AniStateType.Type.Recover:
                m_recoverFlag = false;
                GManager.Instance.IsMainAnimator.SetBool("Recover", m_recoverFlag);
                Debug.Log("::" + argType);
                break;
            case AniStateType.Type.Stun:
                m_stunFlag = false;
                GManager.Instance.IsMainAnimator.SetBool("Stun", m_stunFlag);
                Debug.Log("::" + argType);
                break;
            case AniStateType.Type.Destroy:
                m_destroyFlag = false;
                GManager.Instance.IsMainAnimator.SetBool("Destroy", m_destroyFlag);
                Debug.Log("::" + argType);
                break;
            case AniStateType.Type.Scare:
                m_scareFlag = false;
                GManager.Instance.IsMainAnimator.SetBool("Scare", m_scareFlag);
                Debug.Log("::" + argType);
                break;
            case AniStateType.Type.Bite:
                m_biteFlag = false;
                GManager.Instance.IsMainAnimator.SetBool("Bite", m_biteFlag);
                Debug.Log("::" + argType);
                break;
        }
    }

    bool CheckInUser
    {
        get
        {
            m_userViewFlag = false;
            m_checkDistance = GManager.Instance.CheckUserLength(transform.position, m_length);
            if (m_checkDistance)
            {

                m_rayST.LookAt(GManager.Instance.IsUserObj.transform);
                m_userViewFlag = GManager.Instance.CheckUserLayer(m_rayST, m_length);

            }
            return m_userViewFlag;
        }
    }

}
