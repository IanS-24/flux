using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    float timer = 1;
    string[] strings = new string[] {"Loading", "Loading .", "Loading . .", "Loading . . ."};
    int index = 0;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0){
            index++;
            if (index == 3) {
                timer = 1.5f;
            } else if (index == 4) {
                index = 0;
                timer = 0.33f;
            } 
            else {
                timer = 1;
            }

            GetComponent<TMPro.TextMeshProUGUI>().text = strings[index];
        }
    }
}
