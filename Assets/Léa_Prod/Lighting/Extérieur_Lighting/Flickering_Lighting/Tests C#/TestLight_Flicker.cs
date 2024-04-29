using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class TestLight_Flicker : MonoBehaviour

[SerializeField] float firstVariable;
[SerializeField] float secondVariable;
[SerializeField] float secondsBetweenFlickers;

{
    Light_Normal_Flickering;

    private void Start()
    {
         Light_Normal_Flickering = GetComponent<Light2D>();
         StartCoroutine(LightFlicker());
    }

    IEnumerator LightFlicker()
    {
        yield return new WaitForSeconds (secondsBetweenFlickers);
        Light_Normal_Flickering.pointLightOuterRadius = Random.Range(firstVariable, secondVariable);
        StartCoroutine(LightFlicker());
    }
}
