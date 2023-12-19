using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeRay : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float despawnDistance;
    float distanceTraveled;

    void FixedUpdate()
    {
        transform.position += direction*speed*Time.timeScale;
        distanceTraveled += Mathf.Sqrt(Mathf.Pow(direction.x*speed*Time.timeScale, 2) + Mathf.Pow(direction.y*speed*Time.timeScale, 2));
        if (distanceTraveled > despawnDistance)
        {
            Destroy(gameObject);
        }
    }
}
