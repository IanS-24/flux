using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    public Vector3 offset;
    public bool on;
    public bool activated;
    public string[] messages;
    public float[] times;
    public Color32 offColor;
    public Color32 onColor;
    public GameObject mapFog;

    void Update(){
        if (on) {
            gameObject.GetComponent<SpriteRenderer>().color = onColor;
            if (mapFog != null) {
                mapFog.SetActive(false);
            }
            //Map checkpoint light up
            GameObject[] checkpointTPs = GameObject.FindGameObjectsWithTag("CheckpointTeleporter");
            foreach (GameObject o in checkpointTPs)
            {
                o.GetComponent<Image>().color = new Color32(135, 135, 135, 255);
                if (o.name == name)
                {
                    o.GetComponent<Image>().color = new Color32(122, 200, 207, 255);
                }
            }
            //gameObject.GetComponent<SpriteRenderer>().color = new Color32(118, 174, 197, 255);
            //176, 243, 155, 175 - light, faded (original)
            //54, 192, 57 - vibrant (original from palette)
            //46, 164, 49 - darker
            //155, 129, 255 - purple
            //163, 224, 250 - light blue

        } else {
            //gameObject.GetComponent<SpriteRenderer>().color = new Color32(79, 79, 79, 255); //gray
            gameObject.GetComponent<SpriteRenderer>().color = offColor;
        }
    }
}
