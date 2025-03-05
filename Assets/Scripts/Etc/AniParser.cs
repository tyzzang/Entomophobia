using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniParser : MonoBehaviour
{
    /// <summary>
    /// 공격종료 이벤트
    /// </summary>
    [SerializeField] MyEvent m_EndEvent = null;

    /// <summary>
    /// 에니메이션 스테이츠 전송
    /// </summary>
    /// <param name="argValue"></param>
    public void AniState(AniStateType.Type argValue)
    {
        if (m_EndEvent == null)
            return;
        m_EndEvent.Invoke(argValue);
    }
}
