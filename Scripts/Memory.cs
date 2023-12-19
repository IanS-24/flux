using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Memory : MonoBehaviour
{
    public LayerMask playerLayer;
    public Animator fadeAnim;
    bool inCutscene;
    public string[] lines;
    //public int[] frames;
    public GameObject target;
    GameObject player;
    Dialogue dialogue;
    AudioManager audio;
    StatHolder stats;
    public bool unlocksAbility;
    public bool endOfChapter;
    public bool hiddenMemory;
    public bool newMusic;
    public string newSong;
    
    //original color: 250, 250, 140, 255

    void Start()
    {
        player = GameObject.Find("Player");
        dialogue = GameObject.Find("Dialogue Manager").GetComponent<Dialogue>();
        audio = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        stats = GameObject.Find("Stat Holder").GetComponent<StatHolder>();
    }

    void Update()
    {
        if (Physics2D.OverlapBox(transform.position, new Vector2(0.8f, 0.8f), 0, playerLayer) && !inCutscene)
        {
            inCutscene = true;
            StartCoroutine(Cutscene());
        }
    }

    IEnumerator Cutscene()
    {
        player.GetComponent<Player>().frozen = true;
        player.GetComponent<Player>().memories++;
        stats.memories[int.Parse(SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length - 1)) - 1]++;

        if (SceneManager.GetActiveScene().name == "Chapter 1") {
            stats.chapter1Memories.Add(name);
        } 
        else if (SceneManager.GetActiveScene().name == "Chapter 2") {
            stats.chapter2Memories.Add(name);
        }

        fadeAnim.Play("FadeIn");

        foreach (string song in audio.activeSongs){
            StartCoroutine(audio.StartFade(song, 1, 0));
        }

        yield return new WaitForSeconds(0.5f);

        if (hiddenMemory){
            audio.Play("Hidden Memory");
        } else {
            audio.Play("Memory");
        }

        yield return new WaitForSeconds(0.5f);
             
        if (target != null) {
            player.transform.position = target.transform.position;
        }

        StartCoroutine(dialogue.PlayDialogue(lines, /*frames,*/ new Vector3(0, -300, 0)));

        fadeAnim.speed = 0;

        gameObject.GetComponent<Renderer>().enabled = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        yield return new WaitUntil(() => dialogue.finished == true);
        
        yield return new WaitForSeconds(0.5f);

        if (unlocksAbility) {
            yield return new WaitForSeconds(4);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            fadeAnim.Play("FadeOut");
            fadeAnim.speed = 1;
        }
        else if (endOfChapter) {
            GameObject.Find("Time").GetComponent<TMPro.TextMeshProUGUI>().text = formatTime(GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>().timer);
            int totalHiddenMemories = int.Parse(GameObject.Find("Hidden Memories").GetComponent<TMPro.TextMeshProUGUI>().text);
            GameObject.Find("Hidden Memories").GetComponent<TMPro.TextMeshProUGUI>().text = "0/" + totalHiddenMemories;
            GameObject.Find("End Screen").GetComponent<CanvasGroup>().alpha = 1;
            yield return new WaitForSeconds(0.5f);
            int totalDeaths = GameObject.Find("Game Manager").GetComponent<GameManager>().deaths;
            for (int i = 0; i <= totalDeaths; i++) {
                GameObject.Find("Deaths").GetComponent<TMPro.TextMeshProUGUI>().text = "" + i;
                if (totalDeaths-i > 100) {
                    yield return null;
                    i += 5;
                } else if (totalDeaths-i > 50) {
                    yield return new WaitForSeconds(0.015f);
                } else if (totalDeaths-i > 30) {
                    yield return new WaitForSeconds(0.03f);
                } else if (totalDeaths-i > 10) {
                    yield return new WaitForSeconds(0.1f);
                } else if (totalDeaths-i > 3) {
                    yield return new WaitForSeconds(0.15f);
                } else {
                    yield return new WaitForSeconds(0.25f);
                }
            }
            for (int i = 0; i <= stats.hiddenMemories[int.Parse(SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length - 1)) - 1]; i++){
                GameObject.Find("Hidden Memories").GetComponent<TMPro.TextMeshProUGUI>().text = i + "/" + totalHiddenMemories;
                yield return new WaitForSeconds(0.3f);
            }
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            for (float i = 1; i > 0; i -= 0.02f) {
                GameObject.Find("End Screen").GetComponent<CanvasGroup>().alpha = i;
                yield return new WaitForSeconds(0.01f);
            }
            SceneManager.LoadScene("Chapter Select");
        }
        else {
            fadeAnim.Play("FadeOut");
            fadeAnim.speed = 1;
        }
        
        if (!newMusic){
            foreach (string song in audio.activeSongs){
                StartCoroutine(audio.StartFade(song, 1, 0.2f));
            }
        } else {
            audio.Play(newSong);
            StartCoroutine(audio.StartFade(newSong, 1.5f, 0.2f));
        }

        yield return new WaitForSeconds(1.5f);

        player.GetComponent<Player>().frozen = false;
        
        gameObject.SetActive(false); //or destroy
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
