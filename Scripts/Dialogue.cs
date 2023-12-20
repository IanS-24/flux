using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public GameObject txt;
    GameObject currentFrame;
    [HideInInspector] public bool cutscene = false;
    bool skip = false;
    public GameObject[] framePrefabs;
    public GameObject player;
    public bool finished = true;
    bool firstLine = true;
    public GameObject spacePrompt;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && cutscene == false)
        {
            skip = true;
        }
    }

    public IEnumerator DialogueFade(string line, float time)
    {
        txt.GetComponent<TMPro.TextMeshProUGUI>().text = line;
        txt.GetComponent<CanvasGroup>().alpha = 0;
        txt.GetComponent<TMPro.TextMeshProUGUI>().fontSize = 40;

        if (SceneManager.GetActiveScene().name == "Level 2") {
            txt.GetComponent<TMPro.TextMeshProUGUI>().color = new Color32(118, 118, 118, 255);    
        } else {
            txt.GetComponent<TMPro.TextMeshProUGUI>().color = new Color32(90, 90, 90, 255);
        }
        
        txt.transform.SetParent(GameObject.Find("txtEmpty").transform, false);
        txt.transform.position = new Vector3(30.5f, -364, 0);
        txt.transform.SetParent(GameObject.Find("Canvas").transform, false);

        txt.SetActive(true);
        for (float i = 0; i < 1; i+= 0.1f){
            txt.GetComponent<CanvasGroup>().alpha = i;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(time);
        
        for (float j = 1; j > 0; j-= 0.1f){
            txt.GetComponent<CanvasGroup>().alpha = j;
            yield return new WaitForSeconds(0.1f);
        }
        txt.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        txt.SetActive(false);
    }

    public IEnumerator PlayDialogue(string[] lines, /*int[] frames,*/ Vector3 position, int font = 48)
    {
        txt.GetComponent<CanvasGroup>().alpha = 1;
        finished = false;
        //player.GetComponent<Player>().frozen = true;

        GameObject currentFrame = txt;
        txt.transform.SetParent(GameObject.Find("txtEmpty").transform, false);
        txt.transform.position = new Vector3(position.x + 30.5f, position.y + 56, position.z);
        txt.transform.SetParent(GameObject.Find("Canvas").transform, false);
        txt.GetComponent<TMPro.TextMeshProUGUI>().fontSize = font;
        txt.SetActive(true);

        int lastFrame = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (!firstLine) {
                spacePrompt.SetActive(false);
            }

            skip = false;
            IEnumerator talkCor = Talk(lines[i]);
            StartCoroutine(talkCor);
            for (float j = 0; j < dialogueTime(lines[i]); j+=0.1f)
            {
                if (skip == false)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }

            if (firstLine) {
                spacePrompt.SetActive(true);
                firstLine = false;
            }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            StopCoroutine(talkCor);
            txt.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
        spacePrompt.SetActive(false);
        txt.SetActive(false);
        //Destroy(currentFrame);
        
        yield return new WaitForSeconds(0.1f);
        if (!cutscene){
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        //player.GetComponent<Player>().frozen = cutscene;
        finished = true;
    }

    IEnumerator Talk(string line)
    {
        string currentText = "";
        foreach (char c in line)
        {
            if (c == '*')
            {
                yield return new WaitForSeconds(0.5f);
            } 
            else {
                currentText += c;
                txt.GetComponent<TMPro.TextMeshProUGUI>().text = currentText;
                if (skip == false)
                {
                    if (c == ' ') {
                        yield return new WaitForSeconds(0.04f);
                    }
                    else if (c == '.' || c == ',') {
                        yield return new WaitForSeconds(0.15f);
                    }
                    else if (line.Length > 30) {
                        yield return new WaitForSeconds(0.05f);
                    }
                    else {
                        yield return new WaitForSeconds(0.065f);
                    }
                }
            }
        }
    }

    float dialogueTime(string line)
    {
        float time = 0;
        foreach (char c in line)
        {
            if (c == ' ') {
                time += 0.04f;
            }
            else if (c == '.' || c == ',') {
                time += 0.15f;
            } 
            else if (c == '*') {
                time += 0.1f;
            }
            else if (line.Length > 30) {
                time += 0.06f;
            }
            else {
                time += 0.065f;
            }
        }
        return time;
    }
}
