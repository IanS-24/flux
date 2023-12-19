using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMove : MonoBehaviour
{
    public float speed;
    int direction = 1;
    public float top;
    public float bottom;
    public bool horizontal;
    public float delay;
    public float startingDelay;
    float delayTimer;

    void Start ()
    {
        delayTimer = startingDelay;
    }

    void FixedUpdate()
    {
        delayTimer -= Time.deltaTime;
        float pos;
        if (horizontal) {
            pos = transform.position.x;
        } else {
            pos = transform.position.y;
        }
        if (delayTimer <= 0)
        {
            if (pos >= top) {
                direction = -1;
                delayTimer = delay;
            }
            else if (pos <= bottom) {
                direction = 1;
                delayTimer = delay;
            }
            if (horizontal) {
                transform.position = new Vector3(transform.position.x + direction*speed, transform.position.y, transform.position.z);
            }
            else {
                transform.position = new Vector3(transform.position.x, transform.position.y + direction*speed, transform.position.z);
            }

        }
    }
}
