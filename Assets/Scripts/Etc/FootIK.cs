using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIK : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GManager.Instance.IsMainAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot,1);
    }
}
