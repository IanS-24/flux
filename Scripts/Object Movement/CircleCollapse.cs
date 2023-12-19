using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollapse : MonoBehaviour
{
    public LayerMask player;
    Player playerScript;
    bool open;
    bool reset;

    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (GameObject.Find("Game Manager").GetComponent<GameManager>().enemiesEnabled && !reset)
        {
            reset = true;
            open = false;
            gameObject.GetComponent<Animator>().Play("CircleIdle");
        } 
        else if (!GameObject.Find("Game Manager").GetComponent<GameManager>().enemiesEnabled)
        {
            reset = false;
        }

        if (Physics2D.OverlapBox(transform.position, new Vector2(6, 10), 0, player))
        {
            if (!open)
            {
                open = true;
                gameObject.GetComponent<Animator>().Play("CircleCollapse");
            }
        }
        else
        {
            gameObject.GetComponent<Animator>().Play("CircleOpen");
            open = false;
        }
    }
}
