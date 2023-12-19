using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOnBox : MonoBehaviour
{
    public Box box;

    void Update()
    {
        if (box.on){
            GetComponent<Animator>().speed = 1;
        } else{
            GetComponent<Animator>().speed = 0;
        }
    }
}
