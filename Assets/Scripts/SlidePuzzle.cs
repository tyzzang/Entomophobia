using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePuzzle : MonoBehaviour
{
    [SerializeField]
    GameObject m_boardObject;
    [SerializeField]
    GameObject m_gridPoint;
    [SerializeField]
    Grabbable m_grabbable;

    int gridSize=5;
    // Start is called before the first frame update
    void Start()
    {
        m_grabbable = GetComponent<Grabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        ForKinematic();
    }

    void CreateGrid()
    {
        // A ������Ʈ�� ũ�� ���
        Vector3 _size = m_boardObject.GetComponent<MeshRenderer>().bounds.size;
        float _cellSize = _size.x / gridSize; // ���簢���̹Ƿ� x�� z�� ũ�Ⱑ �����ϴٰ� ����
        Debug.Log(_size);
        // �׸����� ���� ���� ��� (A�� �߽��� ��������)
        Vector3 startPos = m_boardObject.transform.position - new Vector3(_size.x, _size.y, 0) / 2 + new Vector3(_cellSize, _cellSize, 0) / 2;
        Debug.Log(startPos);
        // 5x5 �׸��� ����
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                // �� ���� ��ġ ���
                Vector3 cellPosition = startPos + new Vector3(i * _cellSize, j * _cellSize, 0);

                // �� ������Ʈ ����
                GameObject cell = Instantiate(m_gridPoint, cellPosition, Quaternion.identity);

                // �� ũ�� ����
                cell.transform.localScale = new Vector3(_cellSize, cell.transform.localScale.y, _cellSize);

            }
        }
    }

    void ForKinematic()
    {
        if (m_grabbable == null)
            return;

        if (m_grabbable.BeingHeld)
        {
            transform.GetComponent<Rigidbody>().isKinematic = false;
            
        }else
        {
            if (transform.localPosition.y > 0.7f)
            {                
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.03f, transform.localPosition.z);
            }
            transform.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
