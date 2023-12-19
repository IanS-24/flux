using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNextBox : MonoBehaviour
{
    bool activated;
    public GameObject nextBox;
    public SlowPlayer slowScript;

    void Update()
    {
        if (GetComponent<Box>().on && !activated)
        {
            activated = true;
            nextBox.SetActive(true);
            slowScript.Slow(-1);
        }
    }
}
