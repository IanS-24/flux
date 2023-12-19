using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject children;
    float startTimer = 0;
    public LayerMask hazard;
    public LayerMask blue;
    public LayerMask pink;
    public LayerMask checkpointLayer;
    [HideInInspector] public GameObject[] checkpoints;
    [HideInInspector] public bool triangleExists;
    GameObject camera;

    [HideInInspector] public bool dying;
    [HideInInspector] public bool frozen;
    GameManager manager;
    AudioManager audio;
    Dialogue dialogue;

    public int memories;
    public int keys;
    public GameObject keyCountText;
    public LayerMask key;
    public LayerMask hiddenMemoryLayer;
    float hiddenMemoryTimer = 0;
    public bool inLightning;

    //Movement
    public float xSpeed;
    public float ySpeed;
    public float hInput;
    public float vInput;
    float multiplier;
    public float dashReset;
    [HideInInspector] public float dashTimer;
    public bool dashEnabled;
    public bool stealthEnabled;
    public bool stealthed;
    public float stealthTimer;
    public GameObject stealthIndicator;
    [HideInInspector] public string color;

    void Start()
    {
        stealthEnabled = true;
        camera = GameObject.Find("Main Camera");
        manager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audio = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        dialogue = GameObject.Find("Dialogue Manager").GetComponent<Dialogue>();
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn() {
        Color32 c = GetComponent<SpriteRenderer>().color;
        for (float i = 0; i <= 1; i+= 0.033f){
            c = GetComponent<SpriteRenderer>().color;
            c.a = (byte)(255*i);
            GetComponent<SpriteRenderer>().color = c;
            yield return new WaitForSeconds(0.1f);
        }
        c.a = 255;
        GetComponent<SpriteRenderer>().color = c;
    }

    void FixedUpdate()
    {
        if (startTimer <= 0)
        {
            if (dashTimer <= dashReset-0.1f)
            {
                hInput = Input.GetAxisRaw("Horizontal");
                vInput = Input.GetAxisRaw("Vertical");
            }
            rb.velocity = new Vector2(hInput * xSpeed * Time.deltaTime * multiplier, vInput * ySpeed * Time.deltaTime * multiplier);
        }
    }

    void Update()
    {
        children.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -1);

        //Stealth Indicator
        if (stealthTimer <= 0 || !stealthEnabled){
            stealthIndicator.SetActive(false);
        } else {
            stealthIndicator.SetActive(true);
            stealthIndicator.transform.localScale = new Vector3(1.4f*(stealthTimer/6), stealthIndicator.transform.localScale.y, 1);
        }

        //Key Text
        if (inLightning) {
            keyCountText.SetActive(true);
            keyCountText.GetComponent<TMPro.TextMeshProUGUI>().text = "" + keys + "/5";
            if (keys == 5) {
                keyCountText.GetComponent<TMPro.TextMeshProUGUI>().color = new Color32(106, 217, 114, 255); //green
            } else {
                keyCountText.GetComponent<TMPro.TextMeshProUGUI>().color = new Color32(185, 185, 185, 255); //gray
            }
        } else {
            keyCountText.SetActive(false);
        }

        //Pick up key
        if (Physics2D.OverlapBox(transform.position, new Vector2(1, 1), 0, key))
        {
            keys++;
            Physics2D.OverlapBox(transform.position, new Vector2(1, 1), 0, key).gameObject.SetActive(false);
        }

        //Pick up hidden memories
        hiddenMemoryTimer -= Time.deltaTime;
        if (Physics2D.OverlapBox(transform.position, new Vector2(1.1f, 1.1f), 0, hiddenMemoryLayer) && hiddenMemoryTimer < 0)
        {
            GameObject.Find("Stat Holder").GetComponent<StatHolder>().hiddenMemories[int.Parse(SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length-1))-1]++;
            memories--;
            GameObject.Find("Stat Holder").GetComponent<StatHolder>().memories[int.Parse(SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length - 1)) - 1]--;
            hiddenMemoryTimer = 5;
            //Physics2D.OverlapBox(transform.position, new Vector2(1, 1), 0, hiddenMemoryLayer).gameObject.SetActive(false);
        }

        //Constraints
        if (dying || frozen) {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        } else {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        //Move through colored walls
        color = gameObject.GetComponent<ChangeColor>().color;
        if (color == "pink")
        {
            Physics2D.IgnoreLayerCollision(8, 6, true);
            Physics2D.IgnoreLayerCollision(8, 7, false);
        } else {
            Physics2D.IgnoreLayerCollision(8, 7, true);
            Physics2D.IgnoreLayerCollision(8, 6, false);
        }

        //Timers
        if (Time.timeScale != 0) {
            dashTimer -= Time.deltaTime / Time.timeScale;
            stealthTimer -= Time.deltaTime / Time.timeScale;
            if (startTimer > 0)
            {
                startTimer -= Time.deltaTime / Time.timeScale;
            }
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.Space) && dashTimer <= 0 && !frozen && !dying && dashEnabled)
        {
            dashTimer = dashReset;
            gameObject.GetComponent<Animator>().Play("PlayerDash");
            if (Mathf.Abs(hInput) == 1 && Mathf.Abs(vInput) == 1)
            {
                hInput *= Mathf.Sqrt(0.5f);
                vInput *= Mathf.Sqrt(0.5f);
            }
            if (hInput == 0 && vInput == 0)
            {
                hInput = 1;
            }
        }
        if (dashTimer <= dashReset-0.1f) {
            multiplier = 1;
        } else {
            multiplier = 7;
        }

        //Stealth
        if (Input.GetKeyDown(KeyCode.E) && stealthTimer <= 0 && !frozen && !dying && stealthEnabled){
            stealthTimer = 6;
            StartCoroutine(Stealth());
        }

        //Checkpoint
        if (Physics2D.OverlapBox(transform.position, new Vector2(1.01f, 1.01f), 0, checkpointLayer))
        {
            GameObject obj = Physics2D.OverlapBox(transform.position, new Vector2(1.01f, 1.01f), 0, checkpointLayer).gameObject;
            manager.checkpoint = obj.transform.position;

            for (int i = 0; i < checkpoints.Length; i++)
            {
                if (checkpoints[i].name == obj.name)
                {
                    GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>().currentCheckpoint = obj.name;
                    obj.GetComponent<Checkpoint>().on = true;
                    GameObject[] triangles = GameObject.FindGameObjectsWithTag("Triangle");
                    foreach (GameObject triangle in triangles){
                        triangle.GetComponent<FollowPlayer>().offset = obj.GetComponent<Checkpoint>().offset;
                    }
                    if (SceneManager.GetActiveScene().name == "Chapter 2")
                    {
                        if (obj.name == "Checkpoint 11" && !obj.GetComponent<Checkpoint>().activated) {
                            obj.GetComponent<Checkpoint>().activated = true;
                            inLightning = true;
                            GameObject.Find("Lightning").GetComponent<Lightning>().timer = 0;
                            GameObject.Find("DoorClose").GetComponent<Animator>().Play("DoorClose");
                            audio.Play("DoorSlam");
                        }
                        if (obj.name == "Checkpoint 21" && !obj.GetComponent<Checkpoint>().activated){
                            obj.GetComponent<Checkpoint>().activated = true;
                            GameObject.Find("DoorClose (1)").GetComponent<Animator>().Play("DoorClose1");
                            audio.Play("DoorSlam");
                            if (triangleExists)
                            {
                                GameObject.Find("Triangle(Clone)").SetActive(false);
                                triangleExists = false;
                            }
                        }
                    }
                } else {
                    checkpoints[i].GetComponent<Checkpoint>().on = false;
                }
            }
        }

        //Die
        if (Physics2D.OverlapBox(transform.position, new Vector2(0.97f, 0.97f), 0, hazard) && !dying && !frozen)
        {
            dying = true;
            StartCoroutine(manager.GameOver());
        }
        else if (Physics2D.OverlapBox(transform.position, new Vector2(1.01f, 1.01f), 0, pink) && color != "pink" && !dying && !frozen) {
            dying = true;

            StartCoroutine(manager.GameOver());
        }
        else if (Physics2D.OverlapBox(transform.position, new Vector2(1.01f, 1.01f), 0, blue) && color != "blue" && !dying && !frozen)
        {
            dying = true;
            StartCoroutine(manager.GameOver());
        }
    }

    IEnumerator Stealth(){
        Color32 c = GetComponent<SpriteRenderer>().color;
        c.a = 80;
        GetComponent<SpriteRenderer>().color = c;
        stealthed = true;

        yield return new WaitForSeconds(1.5f);
        //add better animation to show when stealth is expiring -- probably not necessary
        //also add indication of timer

        stealthed = false;
        if (!dying) {
            c.a = 255;
            GetComponent<SpriteRenderer>().color = c;
        }

    }
}
