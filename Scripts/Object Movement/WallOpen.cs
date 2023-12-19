using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOpen : MonoBehaviour
{
    public LayerMask player;
    public bool open;
    public int locks;
    public int memories;
    float lockedTimer;
    public GameObject[] boxes;
    public bool horizontal;
    public bool animated;
    public int direction = 1;
    Vector2 overlapArea = new Vector2(10, 10);

    void Start(){
        if (!horizontal){
            overlapArea = new Vector2(5, 10);
        } else{
            overlapArea = new Vector2(10, 5);
        }
    }

    void Update()
    {
        lockedTimer -= Time.deltaTime;
        
        bool boxesOn = true;
        if (boxes.Length > 0) {
            foreach(GameObject box in boxes) {
                if (!box.GetComponent<Box>().on) {
                    boxesOn = false;
                }
            }
            if (boxesOn && !open) {
                StartCoroutine(Open());
                open = true;
            }
        }
        if (memories > 0 && GameObject.Find("Player").GetComponent<Player>().memories >= memories)
        {
            StartCoroutine(Open());
            open = true;
        }

        if (Physics2D.OverlapBox(transform.position, overlapArea, 0, player) && !open && boxesOn) {
            if (GameObject.Find("Player").GetComponent<Player>().keys >= locks)
            {
                StartCoroutine(Open());
                open = true;
                GameObject.Find("Player").GetComponent<Player>().keys -= locks;
            }
            else if (lockedTimer < 0)
            {
                lockedTimer = 2;
                gameObject.GetComponent<Animator>().Play("WallLocked");
            }
        }
    }

    public IEnumerator Open() {
        if (animated) {
            GetComponent<Animator>().Play("DoorOpen");
        }
        else {
            for (int i = 0; i < 100; i++) {
                if (!horizontal) {
                    transform.position = new Vector3(transform.position.x, transform.position.y + direction*transform.localScale.y/100, transform.position.z);
                } else {
                    transform.position = new Vector3(transform.position.x + direction*transform.localScale.y/100, transform.position.y, transform.position.z);
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
