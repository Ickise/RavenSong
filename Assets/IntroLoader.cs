using System.Collections;
using UnityEngine;

public class IntroLoader : MonoBehaviour
{
    public GameObject VidéoIntro;
    void Start()
    {
        StartCoroutine(Wait(4f));
    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Cancel Vidéo");
        VidéoIntro.SetActive(false);
    }
}
