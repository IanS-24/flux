using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float delay;
    public float startingDelay;
    public GameObject spawnedObj;
    public Vector3 direction;
    public float speed;
    public float despawnDistance;
    float timer = 0;

    void Start()
    {
        timer = startingDelay;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            GameObject blade = Instantiate(spawnedObj, transform.position, transform.rotation);
            blade.GetComponent<BladeRay>().direction = direction;
            blade.GetComponent<BladeRay>().speed = speed;
            blade.GetComponent<BladeRay>().despawnDistance = despawnDistance;
            timer = delay;
        }
    }
}
