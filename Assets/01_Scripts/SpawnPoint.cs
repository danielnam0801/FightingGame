using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private float distance = 2;

    private Transform spawnPoint1;
    private Transform spawnPoint2;

    private void Awake()
    {
        spawnPoint1 = transform.Find("Player1SpawnPoint").transform;
        spawnPoint2 = transform.Find("Player2SpawnPoint").transform;

        spawnPoint1.transform.localPosition = new Vector3(0, 0, -distance);
        spawnPoint2.transform.localPosition = new Vector3(0, 0, +distance);
    }
}
