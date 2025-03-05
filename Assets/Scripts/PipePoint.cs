using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePoint : MonoBehaviour
{
    public bool IsCorrect = false;
    public bool IsConnect = false;

    /// <summary>
    /// IsCorrect 된 애들이 전부 충돌이 들어오면 클리어인거 아님?
    /// </summary>

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void OnTriggerStay(Collider other)
    {
        if (IsCorrect)
        {
            if(other.transform.tag == "pipePoint")
            {
                if (other.transform.GetComponent<PipePoint>().IsCorrect)
                {
                    IsConnect = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsCorrect)
        {
              IsConnect = false;
        }
    }
}
