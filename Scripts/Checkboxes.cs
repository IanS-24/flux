using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkboxes : MonoBehaviour
{
    public GameObject thingToShow;
    public GameObject checkmark;
    bool enabled;
    
    public void toggleObjectVisibility()
    {
        if (enabled){
            thingToShow.SetActive(false);
            checkmark.SetActive(false);
            enabled = false;
        } else {
            thingToShow.SetActive(true);
            checkmark.SetActive(true);
            enabled = true;
        }
    }

}
