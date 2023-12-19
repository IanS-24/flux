using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public LayerMask player;
    public Dialogue dialogue;
    public string[] messages;
    public float[] times;
    //public float[] delays;
    bool activated;


    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, 5, player) && !activated){
            StartCoroutine(PlayLines());
            activated = true;
        }
    }

    IEnumerator PlayLines(){
        for (int i = 0; i < messages.Length; i++){
            StartCoroutine(dialogue.DialogueFade(messages[i], times[i]));
            yield return new WaitForSeconds(times[i] + 2.5f);
        }
        //Destroy(gameObject);
    }
}
