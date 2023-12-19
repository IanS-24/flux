using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    GameObject player;
    Dialogue dialogue;

    public LayerMask checkpointLayer;
    public string currentCheckpoint = "Checkpoint 1";
    public List<GameObject> passedCheckpoints = new List<GameObject>();
    
    public List<float> times = new List<float>();
    public float timer;
    public GameObject timerText;

    [HideInInspector] public bool waitToStart;
    public List<float> clearTimes = new List<float>();
    public float areaTimer;
    public GameObject clearText;
    List<int> totalDeaths = new List<int>();
    public int deaths;
    public int perfects = 0;
    bool fiveDeathsTriggered;

    //LINES!
    string[] highDeathLines = new string[] {"Wasn't sure I'd make it through that one...", "Finally...", "That was tough"};
    bool[] highDeathEnabled = new bool[] {true, true, true};

    string[] fiveDeathLines = new string[] { "This is tough", "One more try...", "I... don't think I can do this" };
    bool[] fiveDeathEnabled = new bool[] { true, true, true };

    string[] recoveryLines = new string[] {"Think I'm getting the hang of this", "Feeling better now"};
    bool[] recoveryEnabled = new bool[] {true, true};

    string[] successLines = new string[] {"This isn't as hard as I thought it would be", "Maybe I am good at this", "Feeling good!"};
    bool[] successEnabled = new bool[] {true, true, true};


    void Start(){
        player = GameObject.Find("Player");
        dialogue = GetComponent<Dialogue>();
        passedCheckpoints.Add(GameObject.Find("Checkpoint 1"));
        waitToStart = true;
    }

    void Update()
    {
        if (deaths == 5  && !fiveDeathsTriggered)
        {
            fiveDeathsTriggered = true;
            StartCoroutine(dialogue.DialogueFade(pickLine(fiveDeathLines, fiveDeathEnabled), 2));
        }

        clearText.GetComponent<TMPro.TextMeshProUGUI>().text = formatTime(areaTimer);
        timerText.GetComponent<TMPro.TextMeshProUGUI>().text = formatTime(timer);
        if (Time.timeScale != 0)
        {
            
            if (Time.timeScale != 0)
            {
                timer += Time.deltaTime / Time.timeScale;
            }
            if (!waitToStart)
            {
                areaTimer += Time.deltaTime;
            }
        }

        if (player.GetComponent<Rigidbody2D>().velocity != new Vector2(0, 0) && !player.GetComponent<Player>().dying)
        {
            waitToStart = false;
        }

        if (Physics2D.OverlapBox(player.transform.position, new Vector2(1.01f, 1.01f), 0, checkpointLayer) == true)
        {
            GameObject obj = Physics2D.OverlapBox(player.transform.position, new Vector2(1.01f, 1.01f), 0, checkpointLayer).gameObject;
            //New checkpoint
            if (!passedCheckpoints.Contains(obj)) {
                times.Add(timer); //timer for whole chapter (i.e at what time did you finish this room)
                totalDeaths.Add(deaths);
                if (deaths == 0) {
                    perfects++;
                } else {
                    perfects = 0;
                    deaths = 0;
                }
                fiveDeathsTriggered = false; //reset our ability to trigger the mid-room lines (ones that play when player dies for the 5th time)
                clearTimes.Add(areaTimer); //timer for single area (i.e how long did it take to do this last run of the room)
                areaTimer = 0;
                
                //Debug.Log(obj.name + ":    " + totalDeaths[totalDeaths.Count-1] + " deaths    "/* + Mathf.Round(100*(times[times.Count-1]%60))/100 + " seconds"*/);
                Debug.Log("Clear time: " + Mathf.Round(100*(clearTimes[clearTimes.Count-1]%60))/100);
                passedCheckpoints.Add(obj);

                if (dialogue.finished)
                {
                    //Checkpoint dialogue
                    var cpt = obj.GetComponent<Checkpoint>();
                    if (cpt.messages.Length != 0){
                        StartCoroutine(PlayLines(cpt.messages, cpt.times));
                    }
                    //High deaths ( >= 6 )
                    /*else if (totalDeaths[totalDeaths.Count-1] >= 6){
                        StartCoroutine(dialogue.DialogueFade(pickLine(highDeathLines, highDeathEnabled), 2));
                    }*/
                    else if (totalDeaths.Count >= 4){
                        //Getting better ( >= 6 then <= 3 for 3 rooms)
                        if (totalDeaths[totalDeaths.Count-4] >= 6 && totalDeaths[totalDeaths.Count-3] <= 3 && totalDeaths[totalDeaths.Count-2] <= 3 && totalDeaths[totalDeaths.Count-1] <= 3){
                            StartCoroutine(dialogue.DialogueFade(pickLine(recoveryLines, recoveryEnabled), 2));
                        }
                        //Perfect for 3 rooms
                        else if (perfects >= 3) {
                            perfects = 0;
                            StartCoroutine(dialogue.DialogueFade(pickLine(successLines, successEnabled), 2));
                        }
                    }
                }
                
                //Print total stats
                /*if (obj.name == endCheckpoint) {
                    finishedLevel = true;
                    //Total time
                    float totalTime = 0;
                    for (int i = 0; i < times.Count; i++){
                        totalTime += times[i];
                        //Debug.Log(times[i]);
                    }
                    Debug.Log("Total time: " + formatTime(timer));

                    //Speedrun time
                    float totalClearTime = 0;
                    for (int i = 0; i < clearTimes.Count; i++){
                        totalClearTime += clearTimes[i];
                        //Debug.Log(times[i]);
                    }
                    Debug.Log("Total clear time (excludes failed attempts): " + formatTime(totalClearTime));

                    //Deaths
                    int deathSum = 0;
                    for (int i = 0; i < totalDeaths.Count; i++){
                        deathSum += totalDeaths[i];
                    }
                    Debug.Log("Total deaths: " + deathSum);
                }*/
            }
        }
    }

    string formatTime(float time)
    {
        int minutes = (int)time / 60;
        float seconds = Mathf.Round(100 * (time % 60)) / 100;
        string secondsStr = "" + seconds;
        if (seconds < 10)
        {
            secondsStr = "0" + seconds;
        }
        return "" + minutes + ":" + secondsStr;
    }

    IEnumerator PlayLines(string[] messages, float[] times)
    {
        for (int i = 0; i < messages.Length; i++)
        {
            StartCoroutine(dialogue.DialogueFade(messages[i], times[i]));
            yield return new WaitForSeconds(times[i] + 2.5f);
        }
    }

    string pickLine(string[] lines, bool[] enabled) {
        //check if all disabled
        bool allDisabled = true;
        for (int i = 0; i < enabled.Length; i++){
            if (enabled[i]){
                allDisabled = false;
            }
        }

        if (allDisabled){
            for (int i = 0; i < enabled.Length; i++) {
                enabled[i] = true;
            }
        }
        //pick random line
        int r = Random.Range(0, lines.Length);
        while(enabled[r] == false){
            r = Random.Range(0, lines.Length);
        }
        enabled[r] = false;
        return lines[r];
    }
}
