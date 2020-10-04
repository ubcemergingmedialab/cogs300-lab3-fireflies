using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyLightFlash : MonoBehaviour
{
    public bool flashed;  //indicate whether firefly has flashed in this cycle
    public float lightRadius = 2.5f; //radius of the 'light bubble' that the firefly light can reach

    void Start()
    {
        flashed = true; //set to true to prevent the light from flashing at instantiation
    }

    void Update()
    {        
        if (!flashed) {
            StartCoroutine(Flash());
        }
    }


    // main function to flash light (turn on and then off)
    IEnumerator Flash() {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Material mat = renderer.material;

        float colorVal = 0f;
        while (colorVal <= 255f) {
            Color EmissionColor = new Color(colorVal / 255f, colorVal / 255f, 0f);
            mat.SetColor ("_EmissionColor", EmissionColor);

            colorVal += 1f;
            yield return null;
        }

        CastLight();

        colorVal = 255f;
        while (colorVal >= 0f) {
            Color EmissionColor = new Color(colorVal / 255f, colorVal / 255f, 0f);
            mat.SetColor ("_EmissionColor", EmissionColor);

            colorVal -= 1f;
            yield return null;
        }

        flashed = true;
    }

    // function that is called when the light is at its peak
    // it will return all objects that is in the 'light bubble'
    // then call the ExternalStimuliHit of all fireflies that were in the bubble
    void CastLight() {
        Collider[] hitObjs = Physics.OverlapSphere(transform.position, lightRadius);

        foreach (var hitObj in hitObjs) {
            if ( hitObj.transform != transform & hitObj.tag == "Firefly") {
                FireflyLightControl lc = hitObj.GetComponent<FireflyLightControl>();
                lc.sawFlash();
            }
        }
    }

}
