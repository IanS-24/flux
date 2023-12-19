using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string targetScene;

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Loading Screen")
        {
            StartCoroutine(Loading());
        }
        Time.timeScale = 1;
    }

    IEnumerator Loading() {
        yield return null;
        SceneManager.LoadScene(targetScene);
    }

    public void NewGame()
    {
        GameObject.Find("SceneChanger").GetComponent<SceneChanger>().targetScene = "Chapter 1";
        var script = GameObject.Find("Stat Holder").GetComponent<StatHolder>();
        script.chapter1Pos = new Vector3(-151.5f, 184.5f, -4);
        script.chapter2Pos = new Vector3(2.3f, -2.3f, 0);
        for (int i = 0; i < script.hiddenMemories.Length; i++)
        {
            script.hiddenMemories[i] = 0;
        }
        script.chapter1Memories = new List<string>();
        script.chapter2Memories = new List<string>();
        script.chapter1Time = 0;
        script.chapter2Time = 0;
        SceneManager.LoadScene("Loading Screen");
        //when we do save and load, add stuff here to save a new file
    }

    public void Play()
    {
        SceneManager.LoadScene("Chapter Select");
        //can you pick any chapter you want from the start?
        //when add save and load, can show data from our saved file here
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Title Screen");
    }


    public void Chapter1()
    {
        GameObject.Find("SceneChanger").GetComponent<SceneChanger>().targetScene = "Chapter 1";
        SceneManager.LoadScene("Loading Screen");
    }

    public void Chapter2()
    {
        GameObject.Find("SceneChanger").GetComponent<SceneChanger>().targetScene = "Chapter 2";
        SceneManager.LoadScene("Loading Screen");
    }
    
    public void Quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }
}
