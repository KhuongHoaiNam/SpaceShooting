using AAGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMovingEnemy : MonoBehaviour
{
    public List<Vector3> List_Points;
    protected Vector3 _originalTransformPosition;
    public Vector3 originalTransformPosition => _originalTransformPosition;

    // Trạng thái của vị trí ban đầu
    protected bool _originalTransformPositionStatus = false;
    public bool originalTransformPositionStatus => _originalTransformPositionStatus;

    protected virtual void Start()
    {
        Init();
    }


    protected virtual void Init()
    {
        if(List_Points == null || List_Points.Count < 1)
        {
            return;
        }
        if (!_originalTransformPositionStatus)
        {
            _originalTransformPositionStatus = true;
        }
        transform.position = _originalTransformPosition;
    }

    public List<Vector3> GetPoint()
    {
        List<Vector3> points = new List<Vector3>();
        for(int i = 0; i < List_Points.Count; i++)
        {
            points.Add(this.transform.position + List_Points[i]);
        }
        return points;
    }

    public Vector3 GetStartPoint()
    {
        Vector3 point = new Vector3();
        point = this.transform.position + List_Points[0];
        return point;
    }
#if UNITY_EDITOR
    // Vẽ các điểm và đường kẻ trong Scene view của Unity Editor
    protected virtual void OnDrawGizmos()
    {
            // Kiểm tra xem danh sách điểm có hợp lệ không
            if (List_Points == null)
            {
                return;
            }

            if (List_Points.Count == 0)
            {
                return;
            }

            // Nếu chưa lưu trữ vị trí ban đầu, lưu trữ vị trí hiện tại
            if (_originalTransformPositionStatus == false)
            {
                _originalTransformPosition = transform.position;
                _originalTransformPositionStatus = true;
            }

            // Nếu không phải đang chạy trò chơi, cập nhật vị trí ban đầu nếu đối tượng đã thay đổi
            if (!Application.isPlaying)
                if (transform.hasChanged)
                {
                    _originalTransformPosition = transform.position;
                }

            // Vẽ các điểm và đường kẻ giữa các điểm
            for (int i = 0; i < List_Points.Count; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_originalTransformPosition + List_Points[i], 0.2f);

                if ((i + 1) <List_Points.Count)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(_originalTransformPosition +List_Points[i], _originalTransformPosition + List_Points[i + 1]);
                }
            }

            // Nếu là vòng tròn, nối điểm cuối với điểm đầu
            /*if (isLoop)
            {
                if (Line[k].List_Points.Count <= 2)
                {
                    isLoop = false;
                    return;
                }

                Gizmos.color = Color.white;
                Gizmos.DrawLine(_originalTransformPosition + Line[k].List_Points[Line[k].List_Points.Count - 1], _originalTransformPosition + Line[k].List_Points[0]);
            }*/
        }
    }

    

#endif





