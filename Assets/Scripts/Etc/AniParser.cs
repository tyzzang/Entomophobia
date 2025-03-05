using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniParser : MonoBehaviour
{
    /// <summary>
    /// �������� �̺�Ʈ
    /// </summary>
    [SerializeField] MyEvent m_EndEvent = null;

    /// <summary>
    /// ���ϸ��̼� �������� ����
    /// </summary>
    /// <param name="argValue"></param>
    public void AniState(AniStateType.Type argValue)
    {
        if (m_EndEvent == null)
            return;
        m_EndEvent.Invoke(argValue);
    }
}
