using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    public string tag;

    void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag(tag);
        if (tag == "Audio"){
            Debug.Log(obj.Length);
        }
        if(obj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}