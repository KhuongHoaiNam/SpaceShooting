using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// Class để tạo và quản lý đường dẫn với các điểm
namespace AAGame
{
    public class PathCreator : MonoBehaviour
    {
        // Biến xác định đường dẫn có phải là vòng tròn không
/*        [SerializeField]
        bool isLoop = false;*/

        // Mảng các đường dẫn
        public line[] Line;

        // Vị trí ban đầu của đối tượng
        protected Vector3 _originalTransformPosition;
        public Vector3 originalTransformPosition => _originalTransformPosition;

        // Trạng thái của vị trí ban đầu
        protected bool _originalTransformPositionStatus = false;
        public bool originalTransformPositionStatus => _originalTransformPositionStatus;

        // Khởi tạo thông tin khi đối tượng bắt đầu
        protected virtual void Start()
        {
            Initialization();
        }

        // Phương thức khởi tạo thông tin
        protected virtual void Initialization()
        {
            for (int i = 0; i < Line.Length; i++)
            {
                // Kiểm tra xem danh sách điểm có hợp lệ không
                if (Line[i].List_Points == null || Line[i].List_Points.Count < 1)
                {
                    return; // Nếu không có điểm, thoát khỏi phương thức
                }

                // Lưu trữ vị trí ban đầu của đối tượng nếu chưa được lưu
                if (!_originalTransformPositionStatus)
                {
                    _originalTransformPositionStatus = true;
                    _originalTransformPosition = transform.position;
                }
                // Đặt lại vị trí của đối tượng về vị trí ban đầu
                transform.position = _originalTransformPosition;
            }
        }

        // Trả về danh sách các điểm của một đường dẫn
        public List<Vector3> getPoints(int index)
        {
            List<Vector3> tmp = new List<Vector3>();
            for (int i = 0; i < Line[index].List_Points.Count; i++)
            {
                tmp.Add(this.transform.position + Line[index].List_Points[i]);
            }

            return tmp;
        }

        // Trả về điểm đầu tiên của một đường dẫn
        public Vector3 getStartPoints(int index)
        {
            Vector3 tmp = new Vector3();
            tmp = this.transform.position + Line[index].List_Points[0];
            return tmp;
        }

#if UNITY_EDITOR
        // Vẽ các điểm và đường kẻ trong Scene view của Unity Editor
        protected virtual void OnDrawGizmos()
        {
            for (int k = 0; k < Line.Length; k++)
            {
                // Kiểm tra xem danh sách điểm có hợp lệ không
                if (Line[k].List_Points == null)
                {
                    return;
                }

                if (Line[k].List_Points.Count == 0)
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
                for (int i = 0; i < Line[k].List_Points.Count; i++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(_originalTransformPosition + Line[k].List_Points[i], 0.2f);

                    if ((i + 1) < Line[k].List_Points.Count)
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawLine(_originalTransformPosition + Line[k].List_Points[i], _originalTransformPosition + Line[k].List_Points[i + 1]);
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

        public void CreateSymmetricalPoints(int lineIndex, bool flipX, bool flipY)
        {
            if (lineIndex < 0 || lineIndex >= Line.Length)
            {
                Debug.LogWarning("Line index is out of bounds.");
                return;
            }

            List<Vector3> originalPoints = Line[lineIndex].List_Points;
            List<Vector3> symmetricalPoints = new List<Vector3>();

            foreach (var point in originalPoints)
            {
                Vector3 symmetricalPoint = new Vector3(
                    flipX ? -point.x : point.x,
                    flipY ? -point.y : point.y,
                    point.z // giữ nguyên giá trị z
                );
                symmetricalPoints.Add(symmetricalPoint);
            }

            // Tạo một phần tử line mới và gán List_Points đối xứng vào đó
            line newLine = new line();
            newLine.List_Points = symmetricalPoints;

            // Mở rộng mảng Line để chứa phần tử mới
            Array.Resize(ref Line, Line.Length + 1);
            Line[Line.Length - 1] = newLine;
        }


#endif
    }


    // Class để định nghĩa một đường dẫn với danh sách các điểm
    [Serializable]
    public class line
    {
        [SerializeField]
        public List<Vector3> List_Points;
    }



}
