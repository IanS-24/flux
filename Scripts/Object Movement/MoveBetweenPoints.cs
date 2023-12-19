using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    public GameObject[] points;
    int index = 0;
    public float speed;

    void FixedUpdate()
    {
        float distanceToPoint = Mathf.Sqrt(Mathf.Pow(points[index].transform.position.x - transform.position.x, 2) + Mathf.Pow(points[index].transform.position.y - transform.position.y, 2));
        if (distanceToPoint < 0.1f){
            if (index == points.Length-1) {
                index = 0;
            } else {
                index++;
            }
        } else {
            transform.position = new Vector3(transform.position.x + ((points[index].transform.position.x - transform.position.x)*speed*Time.timeScale/distanceToPoint), transform.position.y + ((points[index].transform.position.y - transform.position.y)*speed*Time.timeScale/distanceToPoint), -2);
        }
    }
}
