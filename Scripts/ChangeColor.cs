using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public float timer;
    public int seed;
    public int duration = 3;
    public string color = "pink";
    bool start = true;
    Color32 pink = new Color32(234, 98, 235, 255);
    Color32 blue = new Color32(51, 159, 222, 255);

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (!start)
            {
                duration = Random.Range(5, 15);
                start = false;
            }
            timer = Random.Range(2, 10) + duration; //2, 10

            int previousSeed = seed;
            while (seed == previousSeed) //only makes sense for more than 2 colors...
            {
                seed = Random.Range(0, 2);
            }
            if (seed == 0)
            {
                StartCoroutine(Fade(pink, duration));
            }
            else
            {
                StartCoroutine(Fade(blue, duration));
            }
        }
    }

    IEnumerator Fade(Color32 end, int duration)
    {
        Color32 originalColor = GetComponent<SpriteRenderer>().color;
        //get total differences
        int rDiff = end.r - originalColor.r;
        int gDiff = end.g - originalColor.g;
        int bDiff = end.b - originalColor.b;

        //increment every .1 seconds
        int d = duration * 10;
        for (int i = 0; i < d; i++)
        {
            if (i == d/2)
            {
                if (color == "blue")
                {
                    color = "pink";
                } else {
                    color = "blue";
                }
            }

            Color32 c = GetComponent<SpriteRenderer>().color;
            int r = c.r + rDiff / d;
            int g = c.g + gDiff / d;
            int b = c.b + bDiff / d;
            GetComponent<SpriteRenderer>().color = new Color32((byte)r, (byte)g, (byte)b, (byte)c.a);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
