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
        // A 오브젝트의 크기 계산
        Vector3 _size = m_boardObject.GetComponent<MeshRenderer>().bounds.size;
        float _cellSize = _size.x / gridSize; // 정사각형이므로 x와 z의 크기가 동일하다고 가정
        Debug.Log(_size);
        // 그리드의 시작 지점 계산 (A의 중심을 기준으로)
        Vector3 startPos = m_boardObject.transform.position - new Vector3(_size.x, _size.y, 0) / 2 + new Vector3(_cellSize, _cellSize, 0) / 2;
        Debug.Log(startPos);
        // 5x5 그리드 생성
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                // 각 셀의 위치 계산
                Vector3 cellPosition = startPos + new Vector3(i * _cellSize, j * _cellSize, 0);

                // 셀 오브젝트 생성
                GameObject cell = Instantiate(m_gridPoint, cellPosition, Quaternion.identity);

                // 셀 크기 조정
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
