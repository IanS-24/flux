using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAbility : MonoBehaviour
{
    AudioManager audio;

    public GameObject nameText;
    public GameObject descriptionText;
    public GameObject subText;
    public GameObject emptyParent;
    
    public GameObject dashPreview;
    public GameObject stealthPreview;
    public GameObject stealthEnemy;

    public class Ability
    {
        public string name;
        public string description;
        public string subtext;
        public GameObject[] previews;

        public Ability(string name, string description, string subtext, GameObject[] previews)
        {
            this.name = name;
            this.description = description;
            this.subtext = subtext;
            this.previews = previews;
        }
    }

    public Ability dash;
    public Ability stealth;

    void Start()
    {
        audio = GameObject.Find("Audio Manager").GetComponent<AudioManager>();

        dash = new Ability("Dash", "Press space to dash in the direction you are moving.", "resets every 0.5 seconds", new GameObject[] {dashPreview});
        stealth = new Ability("Stealth", "Press E to enter stealth, becoming invisible to enemies.", "lasts 1.5 seconds. resets every 6 seconds", new GameObject[] {stealthPreview, stealthEnemy});
    }

    public IEnumerator Preview(Ability ability)
    {
        audio.Play("New Ability");

        yield return new WaitForSeconds(2);

        emptyParent.SetActive(true);
        nameText.GetComponent<TMPro.TextMeshProUGUI>().text = ability.name;
        descriptionText.GetComponent<TMPro.TextMeshProUGUI>().text = ability.description;
        subText.GetComponent<TMPro.TextMeshProUGUI>().text = ability.subtext;
        foreach (GameObject preview in ability.previews){
            preview.SetActive(true);
            preview.GetComponent<Animator>().Play(preview.name);
        }

        for (float i = 0; i <= 1; i+=0.01f){
            emptyParent.GetComponent<CanvasGroup>().alpha = i;
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        foreach (GameObject preview in ability.previews){
            preview.SetActive(false);
        }
        emptyParent.SetActive(false);
    }

}
