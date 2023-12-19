using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTransition : MonoBehaviour
{
    public LayerMask playerLayer;
    GameObject player;
    public GameObject arrow;
    public GameObject triangle;
    bool activated = false;
    AudioManager audio;
    Dialogue dialogue;
    public WallOpen openScript;

    void Start()
    {
        player = GameObject.Find("Player");
        audio = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        StartCoroutine(audio.StartFade("Rain", 2, 0.1f));
        audio.Play("In the Dark");
        dialogue = GameObject.Find("Dialogue Manager").GetComponent<Dialogue>();
    }

    void Update()
    {
        if (Physics2D.OverlapBox(transform.position, new Vector2(1, 1), 0, playerLayer) && !activated)
        {
            activated = true;
            StartCoroutine(EditLevel());
        }
    }

    IEnumerator EditLevel()
    {
        StartCoroutine(audio.StartFade("In the Dark", 3, 0));
        StartCoroutine(audio.StartFade("In the Dark2", 3, 0.35f));

        yield return new WaitForSeconds(3);

        StartCoroutine(openScript.Open());
        openScript.open = true;
        Instantiate(arrow, new Vector3(transform.position.x, transform.position.y - 3, 2), transform.rotation);
        Destroy(GameObject.Find("Arrow (1)"));

        //yield return new WaitUntil(() => dialogue.finished == true);

        while (dialogue.finished == false){
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1);
        
        GameObject triangleObj = Instantiate(triangle, new Vector3(player.transform.position.x, player.transform.position.y + 3, -2), transform.rotation);
        triangleObj.GetComponent<FollowPlayer>().offset = new Vector3(0, 3, 0);
        player.GetComponent<Player>().triangleExists = true;
        
        yield return new WaitForSeconds(1);

        StartCoroutine(dialogue.DialogueFade("Another anxiety closing in on me...", 2));

        yield return new WaitForSeconds(5.1f);

        StartCoroutine(dialogue.DialogueFade("I have to get out of here", 1));
    }
}