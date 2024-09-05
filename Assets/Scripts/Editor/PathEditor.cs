using AAGame;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
// Custom Editor cho PathCreator, cho phép chỉnh sửa đường dẫn trong Scene view
[CustomEditor(typeof(PathCreator), true)]
[InitializeOnLoad]
public class PathEditor : Editor
{
    // Vẽ các công cụ chỉnh sửa trong Scene view
    protected virtual void OnSceneGUI()
    {
        Handles.color = Color.green;
        PathCreator t = (target as PathCreator);    

        // Nếu chưa lưu trữ vị trí ban đầu, không làm gì cả
        if (t.originalTransformPositionStatus == false)
        {
            return;
        }

        ShowMousePos(); // Hiển thị vị trí chuột

        GUIStyle style = new GUIStyle();

        // Vẽ và chỉnh sửa các điểm trên đường dẫn
        for (int k = 0; k < t.Line.Length; k++)
        {
            for (int i = 0; i < t.Line[k].List_Points.Count; i++)
            {
                EditorGUI.BeginChangeCheck();

                // Vị trí điểm cũ
                Vector3 oldPoint = t.originalTransformPosition + t.Line[k].List_Points[i];

                // Hiển thị chỉ số của điểm
                style.normal.textColor = Color.yellow;
                Handles.Label(t.originalTransformPosition + t.Line[k].List_Points[i] + (Vector3.down * 0.4f) + (Vector3.right * 0.4f), "" + i, style);

                // Công cụ chỉnh sửa để di chuyển điểm
                var fmh_48_65_638495976862286452 = Quaternion.identity;
                Vector3 newPoint = Handles.FreeMoveHandle(oldPoint, .5f, new Vector3(.25f, .25f, .25f), Handles.CircleHandleCap);

                // Cập nhật điểm nếu có thay đổi
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Free Move Handle");
                    t.Line[k].List_Points[i] = newPoint - t.originalTransformPosition;
                }
            }
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // Vẽ giao diện mặc định của PathCreator

        PathCreator pathCreator = (PathCreator)target;

        // Thêm nút Load
        if (GUILayout.Button("Load Symmetrical Points"))
        {
            // Gọi hàm CreateSymmetricalPoints khi nhấn nút
            pathCreator.CreateSymmetricalPoints(0, true, false);

            // Đánh dấu đối tượng đã thay đổi để Unity có thể lưu lại
            EditorUtility.SetDirty(pathCreator);
        }
    }
    // Hiển thị vị trí chuột và cho phép thêm điểm mới vào đường dẫn
    void ShowMousePos()
    {
        if (!Event.current.control)
            return;
        PathCreator t = (target as PathCreator);

        Vector2 mousePosition = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        mousePosition = ray.origin;

        GUIStyle style = new GUIStyle();

        float nearestPoint = 1000;
        int indexNearestPoint = 0;
        Vector2 nearestPos = Vector3.zero;

        // Tìm điểm gần nhất và vẽ hình vuông quanh điểm đó
        for (int k = 0; k < t.Line.Length; k++)
        {
            for (int i = 0; i < t.Line[k].List_Points.Count - 1; i++)
            {
                Vector2 mouseProject = HandleUtility.ProjectPointLine(mousePosition, t.originalTransformPosition + t.Line[k].List_Points[i], t.originalTransformPosition + t.Line[k].List_Points[i + 1]);

                if (nearestPoint > Vector2.Distance(mousePosition, mouseProject))
                {
                    nearestPoint = Vector2.Distance(mousePosition, mouseProject);
                    indexNearestPoint = i + 1;
                    nearestPos = mouseProject;
                }
            }

            style.normal.textColor = Color.red;

            HandleUtility.Repaint();

            EditorGUI.BeginChangeCheck();

            Handles.DrawWireCube(nearestPos, new Vector2(1.5f, 1.5f));

            // Thêm điểm mới vào đường dẫn khi nhấn chuột
            if (Event.current.type == EventType.MouseDown)
            {
                Undo.RecordObject(target, "Insert another point");
                t.Line[k].List_Points.Insert(indexNearestPoint, mousePosition - (Vector2)t.originalTransformPosition);
            }

            EditorGUI.EndChangeCheck();
        }
    }
}
