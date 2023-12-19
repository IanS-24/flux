using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    GameObject player;
    //public GameObject darkness;
    public GameObject light;
    public Animator fader;
    public Animator lightning;
    [HideInInspector] public float timer;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (player.GetComponent<Player>().inLightning)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StartCoroutine(LightningStrike());
            }
        }
        else
        {
            timer = 4.5f;
        }

    }

    public IEnumerator LightningStrike()
    {
        timer = 4.5f;
        GameObject[] invisWalls = GameObject.FindGameObjectsWithTag("Invisible");
        foreach (GameObject obj in invisWalls)
        {
            obj.GetComponent<Animator>().Play("InvisibleHazard");
        }
        //darkness.SetActive(false);
        light.transform.localScale = new Vector3(10, 10, 10);
        lightning.Play("Lightning");
        yield return new WaitForSeconds(1);
        //darkness.SetActive(true);
        for (int i = 1; i <= 10; i++)
        {
            light.transform.localScale = new Vector3(10 - i/2, 10 - i/2, 10 - i/2);
            yield return new WaitForSeconds(0.05f);
        }

    }
}
