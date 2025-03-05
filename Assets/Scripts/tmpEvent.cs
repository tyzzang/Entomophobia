using BNG;
using Cinemachine;
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class tmpEvent : MonoBehaviour
{
    public NavMeshAgent m_navAgent;

    
    

    public EventType.TYPE m_eventType = EventType.TYPE.Ani;

    public Animator m_animator = null;

    public SkinnedMeshRenderer m_meshRenderer = null;

    public AudioSource m_audio = null;

    public AudioClip[] m_audioClip = null;


    public Camera m_mainCamera;

    public CharacterController m_characterController;

    public BNG.TrackedDevice m_trackedDevice;

    public int m_passwordNum;

    public CinemachineDollyCart m_cart;

    public TimeController m_timeController;

    public string m_keyName;

    public LaserPointer m_handLaser;

    public LaserSelecter m_selecter;

    public GameObject m_slidePuzzleObj;
    public GameObject m_doorObj = null;

    public Transform m_rayST = null;
    public Transform m_raySTforCard = null;
    
    public GameObject m_cocoonObj = null;

    public Grabbable m_flashLight = null;

    public GameObject m_tmpWall = null;

    public GameObject[] m_subCreatureObj = null;
    public float m_jumpForce;
    public Ease m_ease;
    public GameObject m_wall;
    

    public FlashLight m_flashLightScript;

    Vector3 m_viewPos = Vector3.zero;

    public bool m_viewInFlag = false;

    public bool m_checkDistance = false;

    public float m_length = 0.0f;
    public float m_rayLength = 0.0f;

    public bool m_userViewFlag = false;
    public bool m_SEPlayFlag = false;
    public bool m_tagFlag = false;
    public bool m_moveFlag = false;
    public bool m_particleFlag = false;
    public bool m_creatureFlag = false;
    public bool m_flashHoldFlag = false;


    private tmpEvent m_eventScript;
    bool m_flag = false;

    private void Start()
    {
        m_eventScript  = transform.GetComponent<tmpEvent>();
    }

    void Update()
    {
        if (CheckUserDistance)
        {
            switch (m_eventType)
            {
                case EventType.TYPE.SubCreature:
                    if (m_subCreatureObj != null && !m_flag)
                    {
                        m_wall.SetActive(true);
                        m_subCreatureObj[0].SetActive(true);
                        
                        GManager.Instance.IsSubCreature.GetComponent<Rigidbody>().isKinematic = false;
                        GManager.Instance.IsSubCreature.transform.DOMoveY(-1.21f, 0.5f).OnComplete(() =>
                        {
                            Vector3 targetRotation = new Vector3(0, 0, 0);
                            GManager.Instance.IsSubCreature.transform.DORotate(targetRotation, 1f).OnComplete(() =>
                            {
                                DOVirtual.DelayedCall(1.0f, () =>
                                {
                                    GManager.Instance.IsSubCreature.transform.DORotate(new Vector3(-80f, 0, 0), 0.01f).OnComplete(() =>
                                    {                                        
                                        m_subCreatureObj[0].GetComponent<SubCreature>().enabled = true;
                                        m_subCreatureObj[0].GetComponent<SubCreature>().m_subCreatureCart.m_Speed = 10f;
                                        //Vector3 _dir = (GManager.Instance.IsSubCameraPos.transform.position - GManager.Instance.IsSubCreature.transform.position).normalized;
                                        //float _dis = Vector3.Distance(GManager.Instance.IsSubCameraPos.transform.position, GManager.Instance.IsSubCreature.transform.position);
                                        //m_jumpForce = m_jumpForce + (_dis / 2.7f);
                                        //Debug.Log(_dis);
                                        ////transform.DOMove(player.position, 0.7f).SetEase(Ease.OutCirc);
                                        //GManager.Instance.IsSubCreature.GetComponent<Rigidbody>().AddForce(_dir * m_jumpForce + Vector3.up * (m_jumpForce), ForceMode.Impulse);
                                        //GManager.Instance.IsSubCreature.GetComponent<Rigidbody>().useGravity = true;
                                    });
                                });
                            });
                        });

                        m_flag = true;
                    }
                break;
                case EventType.TYPE.AtBasement:
                    if(!m_flag)
                    {
                        m_audio.PlayOneShot(GManager.Instance.IsCreatureSounds[4]);
                        m_flag = true;
                    }                    
                    break;
                case EventType.TYPE.BasementShutDoor:
                    if(!m_flag)
                    {
                        m_doorObj.transform.DOLocalRotate(new Vector3(0, 0, 0), 1.0f, RotateMode.Fast);
                        m_audio.PlayOneShot(m_audioClip[0]);
                        m_flag = true;
                    }
                    break;

            }
        }

        if (CheckInUser)
        {
            switch(m_eventType)
            {
                case EventType.TYPE.Ani:
                    if (m_animator == null) return;
                    break;

                case EventType.TYPE.Particle:
                    if (m_particleFlag) return;
                    GManager.Instance.IsParticleObj.Play();
                    
                    m_SEPlayFlag = transform;
                    m_audio.PlayOneShot(m_audioClip[0]);
                    
                    m_audio.Play();
                    m_tmpWall.GetComponent<BoxCollider>().enabled = true;
                    m_particleFlag = true;
                    //transform.gameObject.SetActive(false);
                    break;

                case EventType.TYPE.Move:
                    if (m_cart == null) return;
                    
                    transform.SetParent(m_cart.transform);
                    m_cart.m_Speed = 5.0f;
                    
                    break;

                case EventType.TYPE.Sound:
                    if(m_SEPlayFlag || m_audio==null)
                    {
                        return;
                    }

                    m_SEPlayFlag = true;
                    m_audio.PlayOneShot(m_audioClip[0]);
                    break;

                case EventType.TYPE.CardTag:
                    if (GManager.Instance.CheckCardNumber(transform, m_rayLength, m_passwordNum))
                    {
                        //m_doorObj.transform.DORotate(new Vector3(0, -70.0f, 0), 2.0f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutQuad);
                        m_SEPlayFlag = transform;
                        m_audio.PlayOneShot(m_audioClip[0]);
                        m_eventScript.enabled = false;
                    }
                    break;

                case EventType.TYPE.FirstPuzzle:
                    if (m_flag) return;

                    m_doorObj.transform.DOLocalRotate(new Vector3(0,0,0), 1.0f, RotateMode.Fast);
                    m_audio.PlayOneShot(m_audioClip[0]);
                    m_slidePuzzleObj.GetComponent<PipePuzzle>().enabled = true;
                    GManager.Instance.IsMusicPlayer.clip = GManager.Instance.IsMusicClips[1];
                    GManager.Instance.IsMusicPlayer.Play();
                    m_flag = true;
                    break;
                case EventType.TYPE.FirstPuzzleClear:
                    if (m_flag) return;
                    
                    StartCoroutine(GManager.Instance.StartPlayAudioClipsAfterDelay(1.0f, GManager.Instance.PlayAudioClips(transform,GManager.Instance.IsCreatureSounds[1], 1)));
                    GManager.Instance.IsCartList[0].m_Speed = 1.0f;
                    m_flag = true;
                    break;
                case EventType.TYPE.CreatureCheck:
                    
                    break;
                case EventType.TYPE.CoCoon:
                    if (m_flag) return;
                    m_audio.PlayOneShot(m_audioClip[0]);
                    m_cocoonObj.GetComponent<Rigidbody>().isKinematic = false;
                    m_flag = true;
                    break;
                case EventType.TYPE.SlowMotion:
                    m_timeController.SlowTime();
                    break;

                case EventType.TYPE.LaserOn:
                    m_handLaser.Active = true;
                    m_selecter.enabled = true;
                    break;

                case EventType.TYPE.LaserOff:
                    m_handLaser.Active = false;
                    m_selecter.enabled = false;
                    break;
                case EventType.TYPE.FlashLight:
                    //if (m_flashLight != null)
                    //{    
                    //    if (m_flashLight.BeingHeld && !m_flashHoldFlag)
                    //    {
                    //        //StartCoroutine(GManager.Instance.BlinkLightsCoroutine());
                    //        GManager.Instance.BlinkLight(0, 3);
                    //        m_flashHoldFlag = true;
                    //    }
                    //}
                    break;
            }

        }
        

    }

    bool CheckInUser
    {
        get
        {
            m_userViewFlag = false;
            m_viewPos = Camera.main.WorldToViewportPoint(transform.position);
            m_viewInFlag = (m_viewPos.x >= 0 && m_viewPos.x <= 1 && m_viewPos.y >= 0 && m_viewPos.y <= 1 && m_viewPos.z > 0) ? true : false;
            m_checkDistance = GManager.Instance.CheckUserLength(transform.position, m_length);

            if(m_viewInFlag && m_checkDistance)
            {
                if(m_meshRenderer != null)
                {
                    m_meshRenderer.enabled = true;
                }                    
                m_rayST.LookAt(GManager.Instance.IsUserObj.transform);
                m_userViewFlag = GManager.Instance.CheckUserLayer(m_rayST, m_length);
      
            }
            else
            {
                if (m_meshRenderer != null)
                    m_meshRenderer.enabled = false;
            }
            
            return m_userViewFlag;
        }
    }

    bool CheckUserDistance
    {
        get
        {
            m_userViewFlag = false;
            
            m_checkDistance = GManager.Instance.CheckUserLength(transform.position, m_length);

            if (m_checkDistance)
            {
                if (m_meshRenderer != null)
                {
                    m_meshRenderer.enabled = true;
                }
                m_rayST.LookAt(GManager.Instance.IsUserObj.transform);
                m_userViewFlag = GManager.Instance.CheckUserLayer(m_rayST, m_length);

            }
            else
            {
                if (m_meshRenderer != null)
                    m_meshRenderer.enabled = false;
            }

            return m_userViewFlag;
        }
    }

    

}
