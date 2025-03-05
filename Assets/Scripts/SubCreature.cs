using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SubCreature : MonoBehaviour
{
    public Transform player;
    public float jumpForce = 5f;
    public Rigidbody m_rig;
    public float m_distance;
    float m_currentDistance;
    public AudioClip m_audioClip;
    AudioSource m_audioSource;
    bool _flag = false;

    public CinemachineDollyCart m_subCreatureCart;

    public CinemachineSmoothPath m_subCreaturePath;

    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        //transform.DOMoveY(-1.21f, 0.3f).OnComplete(() =>
        //{
        //    // 2. Y축으로 180도 회전하여 플레이어 바라보기
        //    Vector3 targetRotation = new Vector3(0, 0, 0);
        //    transform.DORotate(targetRotation, 2f).OnComplete(() =>
        //    {
        //        DOVirtual.DelayedCall(1.5f, () =>
        //        {
        //            transform.DORotate(new Vector3(-80f, 0, 0),0.01f).OnComplete(() =>
        //            {
        //                Vector3 _dir = (player.transform.position - transform.position).normalized;
        //                //transform.DOMove(player.position, 0.7f).SetEase(Ease.OutCirc);
        //                m_rig.AddForce(_dir * jumpForce + Vector3.up * jumpForce, ForceMode.Impulse); 
        //            });
        //        });
        //    });
        //});
    }

    void Update()
    {
        Vector3 _localPos = m_subCreaturePath.transform.InverseTransformPoint(GManager.Instance.IsSubCameraPos.position);
        m_subCreaturePath.m_Waypoints[m_subCreaturePath.m_Waypoints.Length - 1].position = _localPos;

        if (m_subCreatureCart.m_Position >= m_subCreatureCart.m_Path.PathLength)
        {
            if (_flag) return;
            m_audioSource.PlayOneShot(m_audioClip);
            GManager.Instance.IsSubAnimator[0].SetBool("Attack", true);
            transform.SetParent(GManager.Instance.IsSubCameraPos);
            transform.localPosition = Vector3.zero;
            _flag = true;
        }
        //Vector3 currentPos = transform.position;
        //Vector3 targetPos = GManager.Instance.IsSubCameraPos.transform.position;

        //// x, z축 좌표만 사용하여 거리 계산
        //float distanceXZ = Vector2.Distance(new Vector2(currentPos.x, currentPos.z), new Vector2(targetPos.x, targetPos.z));

        //if (distanceXZ <= m_distance)
        //{
        //    Debug.Log("닿았음");
        //    m_rig.useGravity = false;
        //    m_rig.isKinematic = true;
        //    transform.DOMove(GManager.Instance.IsSubCameraPos.position, 0.3f).OnComplete(() =>
        //    {
        //        GManager.Instance.IsSubAnimator[0].SetBool("Attack", true);
        //    });          
        //}    
    }

    public void GameEnd()
    {
        GManager.Instance.GameEnding();
    }
}
