using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoving : SingletonMono<TestMoving>
{
    public GameObject objectToSpawn; // Đối tượng bạn muốn sinh ra
    public Transform spawnPoint;     // Điểm sinh ra đối tượng
    public float spawnInterval = 0.3f; // Thời gian giữa các lần sinh ra
    public Transform[] points; // Mảng các điểm

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
