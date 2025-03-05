using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class tmpInven : MonoBehaviour
{
    Grabbable m_grabbable;

    public GameObject m_inventoryObj;
    
    // Start is called before the first frame update
    void Start()
    {
        m_grabbable = GetComponent<Grabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_grabbable.BeingHeld)
        {
            m_inventoryObj.SetActive(false);
        }
        else if(m_grabbable.BeingHeld)
        {
            m_inventoryObj.SetActive(true);
        }
    }
}
