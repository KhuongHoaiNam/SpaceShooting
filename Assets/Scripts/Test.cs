using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void Start()
    {
        Debug.Log(Datamanager.Instance.user.currentLevel);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Datamanager.Instance.ComplateLeves();
        }
    }
    /*public GameObject objectPrefabA; // Prefab của GameObject A
    public Transform parentTransformB; // GameObject B hoặc Transform của nó

    void Start()
    {
        if (objectPrefabA == null || parentTransformB == null)
        {
            Debug.LogError("Prefab A hoặc Transform B chưa được gán.");
            return;
        }

        // Sinh ra GameObject A tại vị trí của ObjectSpawner với góc quay mặc định
        GameObject objectA = Instantiate(objectPrefabA, parentTransformB.position, parentTransformB.rotation);

        // Gán objectA làm con của parentTransformB
        objectA.transform.SetParent(parentTransformB, false); // false để không thay đổi vị trí và góc quay
    }*/
}
