using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPlayer : MonoBehaviour
{
    public LayerMask player;
    public Player playerScript;
    public Vector2 box;
    public string line;
    public float speedAmount;
    bool activated;
    public string song;

    void Update()
    {
        if (Physics2D.OverlapBox(transform.position, box, 0, player) && !activated){
            activated = true;
            Slow(speedAmount);
        }  
    }

    public void Slow(float speedAmount)
    {
        playerScript.dashReset += 0.1f*speedAmount;
        StartCoroutine(GameObject.Find("Dialogue Manager").GetComponent<Dialogue>().DialogueFade(line, 1));
        Time.timeScale -= 0.1f*speedAmount;
        StartCoroutine(GameObject.Find("Main Camera").GetComponent<CameraShake>().Shake(0.15f, 0.15f));
        GameObject.Find("LightningFader").GetComponent<Animator>().Play("Lightning");

        AudioManager audios = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        Sound s = Array.Find(audios.sounds, sound => sound.name == song);
        if (s == null)
        {
            Debug.Log("Sound: " + song + " not found!");
            return;
        }
        s.source.pitch -= 0.1f*speedAmount;

        audios.Play("Rumble");

        GameObject[] particleSystems = GameObject.FindGameObjectsWithTag("Particle System");
        foreach (GameObject system in particleSystems){
            var main = system.GetComponent<ParticleSystem>().main;
            main.simulationSpeed -= 0.45f*speedAmount;
        }
    }
}
