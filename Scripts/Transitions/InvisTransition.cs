using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisTransition : MonoBehaviour
{
    public GameObject hazardToHide;
    public GameObject wallToFlip;
    public Player player;
    public GameObject wallToMove;
    bool transitioned;

    void Update()
    {
        if (gameObject.GetComponent<WallOpen>().open == true && !transitioned)
        {
            transitioned = true;
            player.inLightning = false;
            hazardToHide.SetActive(false);

            var scale = wallToFlip.transform.localScale;
            wallToFlip.transform.localScale = new Vector3(scale.x, scale.y * -1, scale.z);

            wallToMove.transform.position = new Vector3(wallToMove.transform.position.x, -68.7f, wallToMove.transform.position.z);
            
            if (GameObject.Find("Triangle(Clone)") != null){
                GameObject.Find("Player").GetComponent<Player>().triangleExists = false;
                GameObject.Find("Triangle(Clone)").SetActive(false);
            }
        }
    }
}
