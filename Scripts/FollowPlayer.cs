using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    GameObject player;
    GameManager manager;
    public float baseSpeed;
    float speed;
    public Vector3 offset;
    Vector3 pos;
    Vector3 startingPos;
    public bool started;
    bool respawned;
    public float detectionRange;
    [SerializeField] Vector2 randomPoint;
    public float patrolSpeed;
    public GameObject topLeft;
    public GameObject botRight;
    public LayerMask defaultLayer;

    void Start()
    {
        randomPoint = transform.position;
        manager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        startingPos = transform.position;
        pos = player.transform.position;
    }

    void FixedUpdate()
    {
        if (manager.enemiesEnabled == false)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            respawned = false;
            var checkpt = manager.checkpoint;
            if (detectionRange == 0){
                transform.position = new Vector3(checkpt.x, checkpt.y, -2) + offset;
            } else {
                transform.position = startingPos;
            }
        } else {
            gameObject.GetComponent<Renderer>().enabled = true;
            if (!respawned) {
                gameObject.GetComponent<Animator>().Play("TriangleSpawn");
                respawned = true;
            }
        }
        
        if (player.GetComponent<Player>().dying){
            started = false;
        }
        if (pos != player.transform.position){
            started = true;
        }

        pos = player.transform.position;

        if (player.GetComponent<Player>().dying == false && !player.GetComponent<Player>().frozen && started && !player.GetComponent<Player>().stealthed){
            float distance = Mathf.Sqrt(Mathf.Pow(pos.x - transform.position.x, 2) + Mathf.Pow(pos.y - transform.position.y, 2));
            if (distance < detectionRange || detectionRange == 0){ //if detect the player
                if (transform.childCount > 0){
                    transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("DetectingPlayer", true);
                }
                Turn(player);
                speed = baseSpeed * ((int)distance/5 + 1);
                transform.position = new Vector3(transform.position.x + ((pos.x - transform.position.x)*speed*Time.timeScale/distance), transform.position.y + ((pos.y - transform.position.y)*speed*Time.timeScale/distance), -2);
            } else {
                transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("DetectingPlayer", false);
                //move to random point
                float distanceToPoint = Mathf.Sqrt(Mathf.Pow(randomPoint.x - transform.position.x, 2) + Mathf.Pow(randomPoint.y - transform.position.y, 2));
                if (distanceToPoint < 0.1f){
                    if (topLeft != null && botRight != null)
                    {
                        randomPoint = new Vector2(Random.Range(topLeft.transform.position.x, botRight.transform.position.x), Random.Range(topLeft.transform.position.y, botRight.transform.position.y));
                        while (Physics2D.Linecast(transform.position, randomPoint, defaultLayer).collider != null) {
                            randomPoint = new Vector2(Random.Range(topLeft.transform.position.x, botRight.transform.position.x), Random.Range(topLeft.transform.position.y, botRight.transform.position.y));
                        }
                    }
                } else {
                    transform.position = new Vector3(transform.position.x + ((randomPoint.x - transform.position.x)*patrolSpeed*Time.timeScale/distanceToPoint), transform.position.y + ((randomPoint.y - transform.position.y)*patrolSpeed*Time.timeScale/distanceToPoint), -2);
                }
            }
        } else {
            if (transform.childCount > 0){
                transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("DetectingPlayer", false);
            }
        }
    }

    void Turn(GameObject target){
        // get the angle
        Vector3 diff = (target.gameObject.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        // rotate to angle
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(0, 0, angle - 90);
        transform.rotation = rotation;
    }
}
