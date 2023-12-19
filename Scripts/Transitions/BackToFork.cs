using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToFork : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject arrow;
    public GameObject triangle;
    GameObject player;
    Dialogue dialogue;
    bool activated;

    void Start(){
        dialogue = GameObject.Find("Dialogue Manager").GetComponent<Dialogue>();
        player = GameObject.Find("Player");
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
        Destroy(arrow);
        
        yield return new WaitForSeconds(1.5f);
        
        yield return new WaitUntil(() => dialogue.finished == true);
        yield return new WaitForSeconds(0.5f);

        NewAbility newAbility = GameObject.Find("AbilityPreviewer").GetComponent<NewAbility>();
        if (gameObject.name == "Memory (2)")
        {
            StartCoroutine(newAbility.Preview(newAbility.stealth));
            player.GetComponent<Player>().stealthEnabled = true;
        } 
        else {
            StartCoroutine(newAbility.Preview(newAbility.dash));
            player.GetComponent<Player>().dashEnabled = true;
        }

        if (player.GetComponent<Player>().memories >= 3)
        {
            //send player to end
            player.transform.position = new Vector3(-81.8f, -32.28f, -4);
            
            yield return new WaitUntil(() => player.GetComponent<Player>().frozen == false);

            AudioManager audios = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
            audios.Play("Giving Up");
            StartCoroutine(audios.StartFade("Giving Up", 2, 0.1f));
            StartCoroutine(audios.StartFade("In the Dark", 2, 0));
            StartCoroutine(audios.StartFade("In the Dark 2", 2, 0));
            Sound s = Array.Find(audios.sounds, sound => sound.name == "Giving Up");
            if (s == null)
            {
                Debug.Log("Sound: Giving Up not found!");
            } else {
                s.source.pitch = 1.4f;
            }
        }
        else {
            player.transform.position = new Vector3(62, -2, -4);

            if (gameObject.name == "Memory (2)") 
            {
                GameObject triangleObj = Instantiate(triangle, new Vector3(59, -2, -4), transform.rotation);
                triangleObj.GetComponent<FollowPlayer>().offset = new Vector3(0, 3, 0);
                triangleObj.GetComponent<FollowPlayer>().baseSpeed = 0.05f;
                player.GetComponent<Player>().triangleExists = true;

                yield return new WaitUntil(() => player.GetComponent<Player>().frozen == false);

                /*StartCoroutine(dialogue.DialogueFade("Another chance to run from my fears...", 2));
                yield return new WaitForSeconds(4.1f);
                StartCoroutine(dialogue.DialogueFade("Wonderful", 1));*/
            } 
            else {
                yield return new WaitUntil(() => player.GetComponent<Player>().frozen == false);

                //StartCoroutine(dialogue.DialogueFade("I'll tell them next time...", 2)); 
            }
        }


    }
}
