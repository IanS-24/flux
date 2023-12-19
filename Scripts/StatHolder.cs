using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatHolder : MonoBehaviour
{
    public int[] hiddenMemories;
    public int[] memories;
    public List<string> chapter1Memories;
    public List<string> chapter2Memories;
    [HideInInspector] public Vector3 chapter1Pos;
    [HideInInspector] public Vector3 chapter2Pos;
    public float chapter1Time;
    public float chapter2Time;
    public int chapter1Deaths;
    public int chapter2Deaths;
    public string chapter1Checkpoint;
    public string chapter2Checkpoint;

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
            foreach (string s in chapter1Memories) {
                Destroy(GameObject.Find(s));
            }
            GameObject.Find("Player").transform.position = chapter1Pos;
            GameObject.Find("Player").GetComponent<Player>().memories = memories[0];
            GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>().timer = chapter1Time;
            GameObject.Find("Game Manager").GetComponent<GameManager>().deaths = chapter1Deaths;
            GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
            foreach (GameObject cpt in checkpoints) {
                if (cpt.name == chapter1Checkpoint){
                    cpt.GetComponent<Checkpoint>().on = true;
                }
                else {
                    cpt.GetComponent<Checkpoint>().on = false;
                }
            }
        }
        else if (scene.name == "Chapter 2") {
            foreach (string s in chapter2Memories)
            {
                Destroy(GameObject.Find(s));
            }
            GameObject.Find("Player").transform.position = chapter2Pos;
            GameObject.Find("Player").GetComponent<Player>().memories = memories[1];
            GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>().timer = chapter2Time;
            GameObject.Find("Game Manager").GetComponent<GameManager>().deaths = chapter2Deaths;
        }
    }

    void Awake() {
        chapter1Pos = new Vector3(-151.5f, 184.5f, -4);
        chapter2Pos = new Vector3(2.3f, -2.3f, 0);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Chapter Select"){
            GameObject.Find("Hidden Memories 1").GetComponent<TMPro.TextMeshProUGUI>().text = hiddenMemories[0] + "/6";
            GameObject.Find("Hidden Memories 2").GetComponent<TMPro.TextMeshProUGUI>().text = hiddenMemories[1] + "/3";
            GameObject.Find("Time 1").GetComponent<TMPro.TextMeshProUGUI>().text = formatTime(chapter1Time);
            GameObject.Find("Time 2").GetComponent<TMPro.TextMeshProUGUI>().text = formatTime(chapter2Time);
            GameObject.Find("Deaths 1").GetComponent<TMPro.TextMeshProUGUI>().text = "" + chapter1Deaths;
            GameObject.Find("Deaths 2").GetComponent<TMPro.TextMeshProUGUI>().text = "" + chapter2Deaths;
        }
        else if (SceneManager.GetActiveScene().name == "Chapter 1")
        {
            chapter1Pos = GameObject.Find("Player").transform.position;
            chapter1Time = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>().timer;
            chapter1Deaths = GameObject.Find("Game Manager").GetComponent<GameManager>().deaths;
            chapter1Checkpoint = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>().currentCheckpoint;
        }
        else if (SceneManager.GetActiveScene().name == "Chapter 2")
        {
            chapter2Pos = GameObject.Find("Player").transform.position;
            chapter2Time = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>().timer;
            chapter2Deaths = GameObject.Find("Game Manager").GetComponent<GameManager>().deaths;
            chapter2Checkpoint = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>().currentCheckpoint;
        }
    }

    string formatTime(float time)
    {
        int minutes = (int)time / 60;
        float seconds = Mathf.Round(time % 60);
        string secondsStr = "" + seconds;
        if (seconds < 10)
        {
            secondsStr = "0" + seconds;
        }
        return "" + minutes + ":" + secondsStr;
    }
}
