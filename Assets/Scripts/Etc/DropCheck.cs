using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCheck : MonoBehaviour
{
    [SerializeField]
    Transform m_InitPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -5.0f)
        {
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = m_InitPos.position;
        }
    }
}
