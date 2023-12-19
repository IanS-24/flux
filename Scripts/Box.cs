using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Box : MonoBehaviour
{
    public GameObject socket;
    public Light2D light;
    public GameObject doorIndicator;
    public Vector2 targetPos;
    public bool on;
    Vector3 startingPos;
    Vector3 currentPos;

    public LayerMask player;


    void Start(){
        startingPos = transform.position;
        currentPos = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (transform.position != currentPos && currentPos != new Vector3(0, 0, 0)){
            GameObject.Find("Game Manager").GetComponent<GameManager>().sceneModified = true;
        }
        currentPos = transform.position;

        if (GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>().waitToStart){
            transform.position = startingPos;
            on = false;
            socket.SetActive(true);
            light.color = new Color32(255, 128, 121, 255);
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (Physics2D.OverlapCircle(transform.position, 2, player) && Input.GetKeyDown(KeyCode.R) && !on){
            transform.position = startingPos;
        }

        var pos = transform.position;
        if (Mathf.Abs(pos.x-targetPos.x) < 0.1f && Mathf.Abs(pos.y-targetPos.y) < 0.1f && !on)
        {
            on = true;
            socket.SetActive(false);
            light.color = new Color32(106, 217, 114, 255);
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            //106, 217, 114
            //171, 255, 121 - original

            //door indicator
            if (doorIndicator != null) {
                doorIndicator.GetComponent<SpriteRenderer>().color = new Color32(171, 255, 121, 255);
                foreach(Transform child in doorIndicator.transform) {
                    child.GetComponent<SpriteRenderer>().color = new Color32(171, 255, 121, 255);
                }
            }
            GameObject.Find("LightningFader").GetComponent<Animator>().Play("Lightning");
        }
    }
}
