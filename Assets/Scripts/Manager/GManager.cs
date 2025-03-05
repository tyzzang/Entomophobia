using Cinemachine;
using DG.Tweening;
using NavKeypad;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using BNG;
using UnityEngine.SceneManagement;


public class GManager : MonoBehaviour
{
    [SerializeField]
    Vector2Int m_sceneNumber = Vector2Int.zero;
    

    [Header("For Layer")]
    [SerializeField]
    int m_userMaskIndex;
    [SerializeField]
    int m_keyMaskIndex;

    [SerializeField]
    GameObject m_doorObj = null;

    [SerializeField]
    private Keypad keypad;

    [SerializeField]
    ParticleSystem m_smokeParticle;


    /// <summary>
    /// 유저 오브젝트
    /// </summary>
    [SerializeField]
    GameObject m_userObj = null;

    [SerializeField]
    GameObject m_mainCreature;

    [SerializeField]
    GameObject m_subCreature;

    [SerializeField]
    CinemachineDollyCart[] m_cartList;

    [SerializeField]
    AudioClip[] m_creatureSounds;

    [SerializeField]
    Animator m_mainCreatureAnimator;

    [SerializeField]
    Animator[] m_subCreatureAnimator;

    [SerializeField]
    AudioClip[] m_soundClip;

    [SerializeField]
    AudioClip[] m_musicClip;

    [Tooltip("0~3 방과 복도, ")]
    [SerializeField]
    GameObject[] m_lightList;

    [SerializeField]
    Transform m_mainCameraPos;

    [SerializeField]
    Transform m_subCameraPos;

    [SerializeField]
    NavMeshAgent m_mainCreatureNavMeshAgent;

    [SerializeField]
    AudioSource m_soundPlayer;

    [SerializeField]
    GameObject m_particleParent;

    [SerializeField]
    FlashLight m_flashLightScript;

    [SerializeField]
    TrackedDevice m_trackedHMD;

    [SerializeField]
    Transform m_endPos;

    [SerializeField]
    GameObject m_userParentObj;

    [SerializeField]
    CharacterController m_characterController;

    [SerializeField]
    Image m_gameOverUI;

    [SerializeField]
    ScreenFader m_screenFader;

    [SerializeField]
    public bool m_flag = false;


    public float[] m_aniStateArr { get; set; } = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, };


    public static GManager Instance
    {
        get;
        private set;
    } = null;

    /// <summary>
    /// 씬 매니저
    /// </summary>
    public ChangeSceneManager isSceneManager { get; private set; } = null;

    private void Awake()
    {
        if (GManager.Instance == null)
        {
            Instance = this;
            //isSceneManager = transform.Find("ChangeSceneManager").GetComponent<ChangeSceneManager>();
            m_userMaskIndex = LayerMask.NameToLayer("User");
            m_soundPlayer.clip = m_musicClip[0];
            m_soundPlayer.Play();
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    /// <summary>
    /// 각 씬 별 초기셋팅
    /// 초기 셋팅
    /// </summary>
    public void Setting(GameObject argUserObj)
    {
        m_userObj = argUserObj;
    }

    public GameObject IsUserObj { get { return m_userObj; } }

    public int IsKeyMaskIndex { get { return m_keyMaskIndex; } }

    public ParticleSystem IsParticleObj { get { return m_smokeParticle; } }

    public GameObject IsParticleParent { get { return m_particleParent; } }

    public CinemachineDollyCart[] IsCartList { get { return m_cartList; } }

    public AudioClip[] IsCreatureSounds { get { return m_creatureSounds; } }

    public AudioClip[] IsSoundClips { get { return m_soundClip; } }

    public AudioClip[] IsMusicClips { get { return m_musicClip; } }

    public GameObject IsMainCreature { get { return m_mainCreature; } }

    public GameObject IsSubCreature { get { return m_subCreature; } }

    public Animator IsMainAnimator { get { return m_mainCreatureAnimator; } }

    public Animator[] IsSubAnimator { get { return m_subCreatureAnimator; } }

    public Transform IsMainCameraPos { get { return m_mainCameraPos; } }
    public Transform IsSubCameraPos { get { return m_subCameraPos; } }

    public NavMeshAgent IsAgent { get { return m_mainCreatureNavMeshAgent; } }

    public AudioSource IsMusicPlayer { get { return m_soundPlayer; } }

    public GameObject[] IsLightList {  get { return m_lightList; } }

    public FlashLight IsFlashScript { get { return m_flashLightScript; } }

    public TrackedDevice IsHMD { get { return m_trackedHMD; } }

    public CharacterController IsChaController { get { return m_characterController; } }

    public ScreenFader IsScreenFader { get { return m_screenFader; } }


    /// <summary>
    /// 이동할 씬
    /// </summary>
    public string IsMoveSceneNumber
    {
        get { return string.Format("Stage_{0:D}", Random.Range(m_sceneNumber.x, m_sceneNumber.y + 1)); }
    }

    /// <summary>
    /// 유저와의 거리체크
    /// </summary>
    /// <param name="argPos">현재위치</param>
    /// <param name="argLength">측정범위</param>
    /// <returns></returns>
    public bool CheckUserLength(Vector3 argPos, float argLength)
    {
        return Vector3.Distance(m_userObj.transform.position, argPos) <= argLength ? true: false;
    }
    
    public void KeypadValueTransfer(string input)
    {
        keypad.AddInput(input);
    }

    public void IsLeverDown(GameObject argObj)
    {
        argObj.SetActive(true);
    }
    public void IsLeverUp(GameObject argObj)
    {
        argObj.SetActive(false);
    }

    public bool CheckUserLayer(Transform argLookAtTrans, float argLength)
    {
        Ray _ray = new Ray(argLookAtTrans.position, argLookAtTrans.forward);
        RaycastHit _hit;
        Debug.DrawRay(argLookAtTrans.position, argLookAtTrans.forward, Color.magenta);

        return Physics.Raycast(_ray, out _hit, argLength) && _hit.transform.gameObject.layer == m_userMaskIndex ? true : false;
    }

    public bool CheckCardLayer(Transform argTrans, float argLength)
    {
        Ray _ray = new Ray(argTrans.position, argTrans.forward);
        RaycastHit _hit;

        
        return Physics.Raycast(_ray, out _hit, argLength) && _hit.transform.gameObject.layer == m_keyMaskIndex ? true : false;
    }

    public bool CheckCardNumber(Transform argTrans, float argLength, int argNum)
    {
        Ray _ray = new Ray(argTrans.position, argTrans.forward);
        RaycastHit _hit;

        return Physics.Raycast(_ray, out _hit, argLength) && int.Parse(_hit.transform.gameObject.name) == argNum ? true : false;
    }

    /// <summary>
    /// 열쇠로 문열기
    /// </summary>
    /// <param name="argTrans">열쇠 트랜스폼</param>
    /// <param name="argLength">Ray 거리</param>
    /// <param name="argLayer">체크 레이어</param>
    /// <param name="argDoorObj">열릴 오브젝트</param>
    /// <param name="argAngle">열릴 오브젝트의 회전값</param>
    /// <returns></returns>
    public bool CheckKey(Transform argTrans, float argLength, int argLayer, GameObject argDoorObj, Vector3 argAngle, AudioSource argAudio, AudioClip argClip)
    {
        bool _flag = false;
        Ray _ray = new Ray(argTrans.position, argTrans.forward);
        RaycastHit _hit;

        if(Physics.Raycast(_ray, out _hit, argLength) && _hit.transform.gameObject.layer == argLayer)
        {
            _flag = true;
            Debug.Log(_hit.transform.gameObject.name);

            argDoorObj.transform.DOLocalRotate(argAngle, 1.0f, RotateMode.Fast);
            argAudio.PlayOneShot(argClip);

        }
        return _flag;
    }

    /// <summary>
    /// 애니메이션 스테이츠 값 변경
    /// </summary>
    /// <param name="argAniStateType"></param>
    /// <param name="argValue"> T: 1..0f F:0.0f</param>
    public void SetAniState(AniStateType.Type argAniStateType, bool argFlag)
    {
        m_aniStateArr[(int)argAniStateType] = argFlag ? 1.0f : 0.0f;
    }

    /// <summary>
    /// 애니 스테이츠 값 반환
    /// </summary>
    /// <param name="argAniStateType"></param>
    /// <returns></returns>
    public float GetAniState(AniStateType.Type argAniStateType)
    {
        return m_aniStateArr[(int)argAniStateType];
    }

    public void OpenDoor(GameObject argDoorObj)
    {
        argDoorObj.transform.DOLocalRotate(new Vector3(0f,-105.0f,0f), 1.0f, RotateMode.Fast);
    }

    /// <summary>
    /// 0~3 방과 복도, 4 고치방, 6~7 고치방 플래쉬, 8~9 지하실
    /// </summary>
    /// <param name="argStartValue"></param>
    /// <param name="argLength"></param>
    public void BlinkLight(int argStartValue, int argLength)
    {
        for(int i= argStartValue; i<=argLength; i++ )
        {
            m_lightList[i].GetComponent<Animator>().enabled = true;
        }

    }

    public void GameEnding()
    {
        Debug.Log("wnr");
        Instance.IsScreenFader.DoFadeIn();
        GManager.Instance.IsChaController.transform.position = m_endPos.position;
        
        m_gameOverUI.DOFade(1, 3f).OnComplete(() =>
        {
            SceneManager.LoadScene("EndScene");
        });
    }
    //public IEnumerator BlinkLightsCoroutine()
    //{
    //    //m_flashHoldFlag = true;
    //    // 모든 Light 오브젝트 가져오
    //    // 불빛을 깜빡이는 시간
    //    float blinkDuration = 2.0f; // 깜빡임 지속 시간 5초로 설정
    //    float elapsedTime = 0f;

    //    while (elapsedTime < blinkDuration)
    //    {
    //        foreach (Light light in m_lightList)
    //        {
    //            if (light != null)
    //            {
    //                // 불빛 상태 전환 (켜짐 -> 꺼짐, 꺼짐 -> 켜짐)
    //                light.enabled = !light.enabled;

    //                // 다음 깜빡임까지의 대기 시간 랜덤 설정 (0.01초에서 0.1초 사이)
    //                float randomBlinkTime = Random.Range(0.005f, 0.006f);
    //                yield return new WaitForSeconds(randomBlinkTime);

    //                // 랜덤 대기 시간 동안 elapsedTime 증가
    //                elapsedTime += randomBlinkTime;

    //                // 깜빡임 시간이 blinkDuration을 초과하면 루프 종료
    //                if (elapsedTime >= blinkDuration)
    //                {
    //                    break;
    //                }
    //            }
    //        }
    //    }

    //    // 모든 불빛을 끄기
    //    foreach (Light light in m_lightList)
    //    {
    //        if (light != null)
    //        {
    //            light.enabled = false;
    //        }
    //    }
    //}

    public IEnumerator StartPlayAudioClipsAfterDelay(float delay, IEnumerator argcounritne)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(argcounritne);
    }

    public IEnumerator PlayAudioClips(Transform argTrans,AudioClip audioClip, int argCount)
    {
        for (int i = 0; i < argCount; i++)
        {
            argTrans.GetComponent<AudioSource>().PlayOneShot(audioClip);
            // 오디오 클립이 끝날 때까지 대기

            yield return new WaitForSeconds(audioClip.length - 1.3f);
        }
        //GManager.Instance.IsMainAnimator.SetBool("Destroy", true);
    }

    public IEnumerator PlayAudioClips(Transform argTrans, AudioClip audioClip, AudioClip audioClip2, int argCount)
    {
        for (int i = 0; i < argCount; i++)
        {
            argTrans.GetComponent<AudioSource>().PlayOneShot(audioClip2);
            argTrans.GetComponent<AudioSource>().PlayOneShot(audioClip);
            // 오디오 클립이 끝날 때까지 대기
            yield return new WaitForSeconds(audioClip.length);
        }

    }


}
