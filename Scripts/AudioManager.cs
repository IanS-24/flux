using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioSource[] audios;
    public List<string> activeSongs = new List<string>();
    bool menuPlaying;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = 1;
            s.source.loop = s.loop;
        }
        audios = gameObject.GetComponents<AudioSource>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Chapter 1") {
            menuPlaying = false;
            Stop("Menu Theme");
            Play("New Beginnings");
            Sound s = Array.Find(sounds, sound => sound.name == "New Beginnings");
            s.source.volume = 0.05f;
            StartCoroutine(StartFade("New Beginnings", 3, 0.2f));
            activeSongs.Add("New Beginnings");
            Play("Dark Beginnings");
            Play("Exciting Beginnings");
            Play("Focused Beginnings");
            Play("Uncertain Beginnings");
        }
        else if (scene.name == "Chapter 2") {
            menuPlaying = false;
            Stop("Menu Theme");
            Play("In the Dark");
            Play("Rain");
            activeSongs.Add("In the Dark");
            activeSongs.Add("Rain");
        }
        else {
            foreach (string s in activeSongs){
                Stop(s);
            }
            if (menuPlaying == false){
                Play("Menu Theme");
                menuPlaying = true;
            }
        }
    }
    
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public IEnumerator StartFade(string name, float duration, float end)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            yield break;
        }

        float currentTime = 0;
        float start = s.source.volume;

        /*if (start == 0 && end != 0){
            Play(name);
        }*/

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            s.source.volume = Mathf.Lerp(start, end, currentTime / duration);
            yield return null;
        }
    }
}