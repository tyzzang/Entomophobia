using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDoor : MonoBehaviour
{
    [SerializeField]
    float moveDistance; // 방화벽이 올라갈 거리
    [SerializeField]
    float moveDuration;
    [SerializeField]
    Vector3 initPos;
    [SerializeField]
    Light[] m_flashLight;
    [SerializeField]
    AudioClip m_audioClip;
    [SerializeField]
    GameObject[] m_tmpObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShutterUp()
    {
        transform.DOMoveY(transform.position.y + moveDistance, moveDuration)
            .SetEase(Ease.InOutQuad);

        
        transform.GetComponent<AudioSource>().PlayOneShot(m_audioClip);

        if (m_flashLight == null)
            return;

        for (int i = 0; i < m_flashLight.Length; i++)
        {
            m_flashLight[i].gameObject.SetActive(true);
        }

    }

    public void ShutterUpCreature()
    {
        transform.DOMoveY(transform.position.y + moveDistance, moveDuration)
            .SetEase(Ease.InOutQuad);


        transform.GetComponent<AudioSource>().PlayOneShot(m_audioClip);

        if (m_flashLight == null)
            return;

        for (int i = 0; i < m_flashLight.Length; i++)
        {
            m_flashLight[i].gameObject.SetActive(true);
        }

        GManager.Instance.m_flag = false;
        GManager.Instance.IsAgent.speed = 0.0f;
        GManager.Instance.IsMainAnimator.SetBool("Stun", true);
        transform.GetComponent<AudioSource>().PlayOneShot(GManager.Instance.IsCreatureSounds[3]);
        StartCoroutine(TurnOffFlashLightAfterDelay(0.7f));

        //m_tmpObj[0].SetActive(true);
        //m_tmpObj[1].SetActive(true);
    }

    private IEnumerator TurnOffFlashLightAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (m_flashLight == null)
            yield break;

        GManager.Instance.BlinkLight(8, 9);
    }
}
