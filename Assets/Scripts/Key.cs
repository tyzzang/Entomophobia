using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class Key : MonoBehaviour
{
    [SerializeField]    
    Grabbable m_grabbable;

    [SerializeField]
    float m_rayLength;

    [SerializeField]
    int m_keyholeLayer;

    [SerializeField]
    GameObject m_doorObj;

    [SerializeField]
    Vector3 m_rotateAngle;

    [SerializeField]
    AudioSource m_audio;

    [SerializeField]
    AudioClip m_clip;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OpenDoor();
        //if(m_grabbable.BeingHeld)
        //{
        //    Ray _ray = new Ray(transform.position, transform.forward);
        //    RaycastHit _hit;
        //    Debug.DrawRay(transform.position, transform.forward, Color.magenta);

        //    if (Physics.Raycast(_ray, out _hit, m_rayLength)&&_hit.transform.gameObject.layer== m_keyholeLayer)
        //    {
        //        Debug.Log("¿­·Á¶ó");
        //    }    
        //}
    }

    public void OpenDoor()
    {
        if (m_grabbable.BeingHeld)
        {
            if (GManager.Instance.CheckKey(transform, m_rayLength, m_keyholeLayer, m_doorObj, m_rotateAngle, m_audio, m_clip))
            {
                Destroy(this.gameObject);
            }
            
        }
    }
}
