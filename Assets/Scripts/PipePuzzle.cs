using BNG;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PipePuzzle : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_pipeDial;

    [SerializeField]
    List<PipePoint> m_pipePoint = new List<PipePoint>();

    [SerializeField]
    Light[] m_light;

    [SerializeField]
    PipePoint[] m_fusePoint;

    [SerializeField]
    UnityEvent m_ConnectEvent;

    [SerializeField]
    AudioClip[] m_audioClip;

    [SerializeField]
    tmpEvent m_tmpEventScript;

    [SerializeField]
    GameObject[] m_correctLight;

    [SerializeField]
    AudioSource m_particleAudio;

    [SerializeField]
    GameObject[] m_tmpObj;

    [SerializeField]
    FireDoor m_fireDoor;
    bool m_creatureFlag = false;
    void Start()
    {
        foreach (GameObject pipe in m_pipeDial)
        {
            foreach (Transform tr in pipe.transform)
            {
                if (tr.name.Contains("point"))
                {
                    if (tr.GetComponent<PipePoint>().IsCorrect)
                        m_pipePoint.Add(tr.GetComponent<PipePoint>());
                }
            }
        }
        //foreach (GameObject pipe in m_pipeDial)
        //{
        //    m_pipePoint.Add(pipe.GetComponent<PipePoint>());
        //    foreach (Transform tr in pipe.transform)
        //    {
        //        if (tr.name.Contains("point"))
        //        {
        //            if (tr.GetComponent<PipePoint>().IsCorrect)
        //                m_pipePoint.Add(tr.GetComponent<PipePoint>());
        //        }
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        fuseCheck();
    }
    

    public void CheckConnected(bool _flag)
    {
        bool _check = true;

        for (int i = 0; i < m_pipePoint.Count; i++)
        {
            if (!m_pipePoint[i].IsConnect)
            {
                Debug.Log(m_pipePoint[i].transform.parent.name);
                _check = false;
                transform.GetComponent<AudioSource>().PlayOneShot(m_audioClip[1]);
                Debug.Log("?");
                return;
            }

        }

        if(_check && _flag) //Ã¹¹øÂ° ÆÛÁñ
        {
            m_correctLight[0].SetActive(false);
            m_correctLight[1].SetActive(true);
            m_light[2].enabled = (true);
            m_light[3].enabled = (true);
            GManager.Instance.IsFlashScript.m_light[0].SetActive(false);            
            GManager.Instance.IsFlashScript.m_flag = true;

            transform.GetComponent<AudioSource>().PlayOneShot(m_audioClip[0]);
            
            GManager.Instance.IsParticleObj.Stop();
            m_particleAudio.Stop();
            m_tmpObj[2].GetComponent<BoxCollider>().enabled = false;
            GManager.Instance.IsParticleParent.GetComponent<tmpEvent>().enabled = false;            

            m_tmpObj[0].SetActive(false);
            m_tmpObj[1].transform.DOMoveY(m_tmpObj[1].transform.position.y - 3.4f, 1f)
            .SetEase(Ease.InOutQuad);
            m_tmpObj[3].SetActive(true);
            this.enabled = false;
        }

        if(_check && !_flag)
        {
            m_correctLight[0].SetActive(false);
            m_correctLight[1].SetActive(true);
            for (int i =0; i<m_light.Length; i++)
            {
                m_light[i].GetComponent<Light>().intensity = 3.0f;
            }
            m_fireDoor.ShutterUp();

            transform.GetComponent<AudioSource>().PlayOneShot(m_audioClip[0]);
            GManager.Instance.IsMainCreature.transform.SetParent(null);
            
            GManager.Instance.IsMainCreature.transform.position = new Vector3(0f, 0.5f, 10f);
            GManager.Instance.IsMainCreature.transform.localEulerAngles = new Vector3(0, 180, 0);
            GManager.Instance.IsMainCreature.SetActive(true);
            GManager.Instance.IsMainCreature.transform.DOMove(new Vector3(0f, 0.5f, 10f), 1f).OnComplete(() =>
            {
                
                GManager.Instance.IsMainAnimator.SetBool("Scare",true);
                transform.GetComponent<AudioSource>().PlayOneShot(GManager.Instance.IsCreatureSounds[9]);
                GManager.Instance.IsAgent.enabled = true;
               
                DOVirtual.DelayedCall(1.5f, () =>
                {
                    GManager.Instance.IsAgent.speed = 1.0f;
                    GManager.Instance.m_flag = true;
                });
            });
            
            //GManager.Instance.IsCartList[0].gameObject.SetActive(true);
            //GManager.Instance.IsMainCreature.transform.parent = GManager.Instance.IsCartList[1].transform;
            //GManager.Instance.IsMainCreature.transform.localPosition = new Vector3(0, 0.7f, 0);
            //GManager.Instance.IsMainCreature.transform.localRotation = Quaternion.identity;
            
            //StartCoroutine(GManager.Instance.BlinkLightsCoroutine());
            //StartCoroutine(GManager.Instance.StartPlayAudioClipsAfterDelay(1.5f, GManager.Instance.PlayAudioClips(transform,GManager.Instance.IsSoundClips[2], 4)));
            
        }
        
    }

    public void fuseCheck()
    {
        int _count = 0;


        for (int i = 0; i < m_fusePoint.Length; i++)
        {
            if (m_fusePoint[i].IsConnect)
            {
                _count++;
            }
        }
        
        if (_count >= 2 && !m_creatureFlag)
        {
            m_creatureFlag = true;
            GManager.Instance.IsCartList[0].gameObject.SetActive(true);
            //GManager.Instance.IsMainCreature.transform.parent = GManager.Instance.IsCartList[0].transform;
            //GManager.Instance.IsMainCreature.transform.localPosition = new Vector3(0, 0.4f, 0);
            //GManager.Instance.IsMainCreature.transform.localRotation = Quaternion.identity;
            GManager.Instance.IsMainCreature.GetComponentInChildren<MainCreature>().m_tmpEventScript = m_tmpEventScript;
            
            //AudioSource _tmpAudio = GManager.Instance.IsMainCreature.GetComponent<AudioSource>();
            //_tmpAudio.PlayOneShot(GManager.Instance.IsCreatureSounds[0]);
                
            //m_audio.PlayOneShot(m_creatureSound);
        }
    }

   
}
