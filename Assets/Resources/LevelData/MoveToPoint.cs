using UnityEngine;

public class MoveToPoint : MonoBehaviour
{
    public TyperMoving currentMoving; // Kiểu di chuyển hiện tại
    public float duration = 2f; // Thời gian di chuyển giữa các điểm
    public float height = 2f; // Độ cao của quỹ đạo cong

    private Vector3 startPosition; // Vị trí bắt đầu
    private int currentTargetIndex = 0; // Chỉ số điểm mục tiêu hiện tại
    private float startTime; // Thời gian bắt đầu di chuyển
    private Transform currentTarget; // Điểm mục tiêu hiện tại

    private void Start()
    {
        if (TestMoving.Instance.points.Length > 0)
        {
            startPosition = transform.position; // Lưu vị trí bắt đầu
            startTime = Time.time; // Lưu thời gian bắt đầu
            currentTarget = TestMoving.Instance.points[0]; // Điểm mục tiêu đầu tiên
        }
    }

    private void Update()
    {
        if (TestMoving.Instance.points.Length == 0)
            return;

        float elapsedTime = Time.time - startTime; // Thời gian đã trôi qua
        float t = Mathf.Clamp01(elapsedTime / duration); // Tính toán tỷ lệ di chuyển

        switch (currentMoving)
        {
            case TyperMoving.Normal:
                t = EaseInOut(t);
                transform.position = Vector3.Lerp(startPosition, currentTarget.position, t);
                break;

            case TyperMoving.Parabol:
                t = EaseInOut(t);
                Vector3 start = startPosition;
                Vector3 end = currentTarget.position;
                Vector3 mid = (start + end) / 2;
                mid.y += height; // Tăng độ cao để tạo quỹ đạo cong
                transform.position = QuadraticBezier(start, mid, end, t);
                break;

           
        }

        if (t >= 1f)
        {
            // Cập nhật để chuyển đến điểm tiếp theo
            currentTargetIndex++;
            if (currentTargetIndex < TestMoving.Instance.points.Length)
            {
                currentTarget = TestMoving.Instance.points[currentTargetIndex];
                startPosition = transform.position; // Cập nhật vị trí bắt đầu cho điểm mới
                startTime = Time.time; // Cập nhật thời gian bắt đầu cho điểm mới
                RandomizeMovement(); // Chọn kiểu di chuyển ngẫu nhiên cho đoạn đường mới
            }
            else
            {
                enabled = false; // Hoặc thực hiện hành động khác khi kết thúc
            }
        }
    }

    private Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }

    private float EaseInOut(float t)
    {
        return t * t * (3f - 2f * t);
    }

    // Hàm chọn kiểu di chuyển ngẫu nhiên
    private void RandomizeMovement()
    {
        // Chọn ngẫu nhiên kiểu di chuyển từ enum TyperMoving
        currentMoving = (TyperMoving)Random.Range(0, System.Enum.GetValues(typeof(TyperMoving)).Length);
    }
}

public enum TyperMoving
{
    Normal,
    Parabol,
}
