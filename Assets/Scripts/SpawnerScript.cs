using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject barrelPrefab;
    public float spawnTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    private void Spawn() {
        Instantiate(barrelPrefab, transform.position, Quaternion.identity);
        Invoke(nameof(Spawn), spawnTime);
    }
}
