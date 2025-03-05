using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class tset : MonoBehaviour
{
    public CinemachineDollyCart cart;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(cart.m_Speed < 0.1f)
            {
                cart.m_Speed = 2.0f;
            }else if(cart.m_Speed > 1.5f)
            {
                cart.m_Speed = 0.0f;
            }
             
        }

    }
}
