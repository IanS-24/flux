using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FadeMusic : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public string song;
    public float volume;
    public bool horizontal;
    int n = 1; //inverts direction
    AudioManager audio;

    //fix drastic change when you die while fading! (since you go back to previous checkpoint, music immediately drops off)
    
    void Start(){
        audio = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (horizontal){
            var pos = GameObject.Find("Player").transform.position;
            if (start.position.x > end.position.x){
                n = -1;
            }
            if (pos.x*n > start.position.x*n && pos.x*n < end.position.x*n && Mathf.Abs(pos.y - start.position.y) < 5) //if between the two points
            {
                ChangeVolume((pos.x - start.position.x) * (volume/(end.position.x - start.position.x)));
            }
            else if (pos.x*n > end.position.x*n) { //if past end
                audio.activeSongs.Add(song);
            }
            else if (pos.x*n < start.position.x*n) { //if before start
                audio.activeSongs.Remove(song);
                ChangeVolume(0);
            }
        } 
        else {
            var pos = GameObject.Find("Player").transform.position;
            if (start.position.y > end.position.y){
                n = -1;
            }
            if (pos.y*n > start.position.y*n && pos.y*n < end.position.y*n && Mathf.Abs(pos.x - start.position.x) < 5) //if between the two points
            {
                ChangeVolume((pos.y - start.position.y) * (volume/(end.position.y - start.position.y)));
            }
            else if (pos.y*n > end.position.y*n) { //if past end
                audio.activeSongs.Add(song);
            }
            else if (pos.y*n < start.position.y*n) { //if before start
                audio.activeSongs.Remove(song);
                ChangeVolume(0);
            }
        }
    }

    void ChangeVolume(float volume){
        Sound s = Array.Find(audio.sounds, sound => sound.name == song);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = volume;
    }
}
