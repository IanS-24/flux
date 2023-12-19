using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public GameObject cameraParent;


    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = cameraParent.transform.localPosition;
        float elapsed = 0.0f;

        for (float i = 0; i < duration; i += 0.01f)
        {
            //Debug.Log(elapsed + " < " + duration + "?");
            //elapsed += Time.deltaTime;
            
            float x = Random.Range(-1f, 1f)*magnitude;
            float y = Random.Range(-1f, 1f)*magnitude;

            cameraParent.transform.localPosition = new Vector3(x, y, cameraParent.transform.localPosition.z);

            yield return new WaitForSeconds(0.01f);
            //yield return null;

        }

        cameraParent.transform.localPosition = originalPosition;
    }
}
