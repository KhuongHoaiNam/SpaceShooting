using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgreadeItem : MonoBehaviour
{
    public Item id;
    public float attractRange =100F; // Khoảng cách bắt đầu hút
    public float attractSpeed = 100F; // Tốc độ hút vào player

    [SerializeField] private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        //lấy khoảng cách giữa 2 vật phẩm 
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Kiểm tra khoảng cách giữa itelayer
        if (distanceToPlayer <= attractRange)
        {
            // Di chuyển item về phía playerm và p
            transform.position = Vector3.MoveTowards(transform.position, player.position, attractSpeed * Time.deltaTime);

            // Kiểm tra nếu item đã đến gần player thì biến mất
            if (distanceToPlayer < 0.5f) // Điều chỉnh khoảng cách này tùy ý
            {
                CollectItem();
            }
        }
    }

   public void CollectItem()
    {
        // Thực hiện các hành động khi item được thu thập, ví dụ như tăng điểm, thêm vào inventory,...
        Debug.Log("Item collected!");
        GameEventManager.Instance.TriggerEvent(TyperEvent.OnUpdateLevelShipOnGame);
        // Hủy item khỏi scene
        Destroy(gameObject);
    }
}
