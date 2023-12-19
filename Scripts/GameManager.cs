using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject camera;
    public Vector3 checkpoint = new Vector3(0, 0, -1);
    GameObject[] checkpoints;
    public GameObject pauseMenu;
    public GameObject map;
    public bool paused;
    public bool enemiesEnabled;
    public bool sceneModified;
    GameObject[] keys;
    DialogueManager dialogue;
    
    public int deaths;
    float timeScale = 1;
    public float timeScaleToPrint;

    public Canvas canvas;

    void Start()
    {
        dialogue = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>();
        keys = GameObject.FindGameObjectsWithTag("Key");
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        /*for (int i = 0; i < checkpoints.Length; i++)
        {
            GameObject obj = Instantiate(checkpointButton, new Vector2(canvas.scaleFactor*(585 + (i % 7) * 150), canvas.scaleFactor*(660 - (i / 7) * 100)), transform.rotation);
            //GameObject obj = Instantiate(checkpointButton, new Vector2(480 + (i%6)*115, 545 - (i/6)*85), transform.rotation);
            //GameObject obj = Instantiate(checkpointButton, new Vector2(225 + (i%6)*60, 245 - (i/6)*50), transform.rotation);
            obj.transform.SetParent(checkpointButtonParent.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "" + (i+1);
            obj.GetComponent<Button>().onClick.AddListener(delegate { teleportToCheckpoint(obj.transform.GetChild(1).gameObject); });
        }*/
    }

    public void teleportToCheckpoint(GameObject checkpoint)
    {
        if (!player.GetComponent<Player>().dying && !player.GetComponent<Player>().frozen)
        {
            sceneModified = false;
            /*while(dialogue.passedCheckpoints.Count >= n){
                dialogue.passedCheckpoints.RemoveAt(dialogue.passedCheckpoints.Count-1);
            }
            Debug.Log("Length before removal: " + dialogue.passedCheckpoints.Count);
            bool removed = false;
            while (removed == false) {
                removed = true;
                foreach (GameObject cpt in dialogue.passedCheckpoints){
                    if (int.Parse(cpt.name.Substring(cpt.name.Length-2)) > n){
                        dialogue.passedCheckpoints.Remove(cpt);
                        removed = false;
                        break;
                    }
                }
            }
            Debug.Log("Length after removal: " + dialogue.passedCheckpoints.Count);*/
            GameObject[] checkpointTPs = GameObject.FindGameObjectsWithTag("CheckpointTeleporter");
            foreach (GameObject obj in checkpointTPs)
            {
                obj.GetComponent<Image>().color = new Color32(135, 135, 135, 255);
                if (obj.name == checkpoint.name)
                {
                    obj.GetComponent<Image>().color = new Color32(122, 200, 207, 255);
                }
            }
            player.transform.position = checkpoint.transform.position;
            paused = false;
            map.SetActive(false);
            Time.timeScale = timeScale;
            dialogue.areaTimer = 0;
            dialogue.waitToStart = true;
        }
    }

    void Update()
    {
        timeScaleToPrint = Time.timeScale;
        //int n = dialogue.currentCheckpoint;
        //selectBox.transform.position = new Vector2(canvas.scaleFactor * (585 + ((n-1) % 7) * 150), canvas.scaleFactor * (660 - ((n-1) / 7) * 100));
        //selectBox.transform.position = new Vector2(480 + ((n-1)%6)*115, 545 - ((n-1)/6)*85);
        //selectBox.transform.position = new Vector2(225 + ((n-1)%6) * 60, 245 - ((n-1)/6) * 50);

        /*if (Input.GetKeyDown(KeyCode.F)) {
            Time.timeScale = 0.1f;
        }*/

        //Pause & Open Map
        if (Input.GetKeyDown(KeyCode.Escape) && !player.GetComponent<Player>().frozen)
        {
            paused = !paused;
            if (paused) { //if entering pause
                pauseMenu.SetActive(true);
                timeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            else { //if exiting pause
                pauseMenu.SetActive(false);
                map.SetActive(false);
                Time.timeScale = timeScale;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && !player.GetComponent<Player>().frozen)
        {
            paused = !paused;
            if (paused) { //if entering pause
                map.SetActive(true);
                timeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            else { //if exiting pause
                if (pauseMenu.activeSelf){
                    map.SetActive(true);
                    pauseMenu.SetActive(false);
                    paused = true;
                }
                else {
                    map.SetActive(false);
                    Time.timeScale = timeScale; 
                }
            }
        }
    }

    public IEnumerator GameOver()
    {
        deaths++;
        dialogue.deaths++;
        /*if (script.areaTimer < script.bestTimes[script.bestTimes.Count-1] || script.bestTimes[script.bestTimes.Count-1] == 0){
            script.bestTimes[script.bestTimes.Count-1] = script.areaTimer;
        }*/
        if (!sceneModified) {
            dialogue.areaTimer = 0;
            dialogue.waitToStart = true;
        }

        var playerScript = player.GetComponent<Player>();
        if (playerScript.inLightning) {
            StartCoroutine(GameObject.Find("Lightning").GetComponent<Lightning>().LightningStrike());
        }
        playerScript.keys = 0;
        playerScript.dashTimer = 0;
        playerScript.stealthTimer = 0;
        GameObject.Find("CameraParent").transform.SetParent(null);
        player.GetComponent<Animator>().Play("PlayerDie");

        enemiesEnabled = false;

        yield return new WaitForSeconds(0.25f*Time.timeScale);

        player.GetComponent<Renderer>().enabled = false;
        
        yield return new WaitForSeconds(1*Time.timeScale);

        //player.transform.position = new Vector3(checkpoint.x, checkpoint.y, -4);
        foreach (GameObject cpt in checkpoints){
            if (cpt.GetComponent<Checkpoint>().on == true){
                player.transform.position = cpt.transform.position;
            }
        }
        GameObject.Find("CameraParent").transform.SetParent(GameObject.Find("Player Children").transform);
        player.GetComponent<Renderer>().enabled = true;
        player.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        enemiesEnabled = true;
        foreach (GameObject key in keys)
        {
            key.SetActive(true);
        }

        Color32 c = player.GetComponent<SpriteRenderer>().color;
        for (float i = 0; i <= 1; i+= 0.2f){
            c = player.GetComponent<SpriteRenderer>().color;
            c.a = (byte)(255*i);
            player.GetComponent<SpriteRenderer>().color = c;
            yield return new WaitForSeconds(0.1f*Time.timeScale);
        }
        c.a = 255;
        player.GetComponent<SpriteRenderer>().color = c;

        player.GetComponent<Player>().dying = false;

    }
}
