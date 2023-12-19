using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAside : MonoBehaviour
{
    public LayerMask player;
    bool open;
    public int direction;
    float xScale;
    public float targetScale = 0.6f;

    void Start()
    {
        xScale = transform.localScale.x;
    }

    void Update()
    {
        if (Physics2D.OverlapBox(transform.position, new Vector2(4, 10), 0, player))
        {
            if (!open)
            {
                open = true;
                StopCoroutine("Close");
                StartCoroutine("Open");
            }
        } else if (open) {
            open = false;
            StopCoroutine("Open");
            StartCoroutine("Close");
        }
    }
    
    IEnumerator Open()
    {
        while (transform.localScale.x > targetScale*xScale)
        {
            transform.localScale = new Vector2(transform.localScale.x - xScale*0.2f, transform.localScale.y);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.35f * direction, 1);
            yield return new WaitForSeconds(0.025f);
        }
    }

    IEnumerator Close()
    {
        while (transform.localScale.x < xScale)
        {
            transform.localScale = new Vector2(transform.localScale.x + xScale*0.2f, transform.localScale.y);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.35f * direction * -1, 1);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
