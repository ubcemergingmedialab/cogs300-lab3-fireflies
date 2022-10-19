using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyEventsTesting : MonoBehaviour
{
    public int sightDistance = 5;

    const int chargeThreshold = 800;

    const int sendDelay = 200;

    const int waitDelay = 200;

    public int chargingProgress = 0;

    public int sendingProgress = 0;
    public int waitProgress = 0;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.current.OnFireflyFlash += SenseFlashes;
        chargingProgress = Random.Range(0, 800);
        StartCoroutine(Charge());
    }


    private void SenseFlashes(Vector3 position){
        float distance = Vector3.Distance(this.transform.position, position);
        if(distance > 0 && distance < sightDistance){
            Debug.Log(distance);
            chargingProgress = 0;
        }
        
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("space"))
        {
           TurnColor();
        }
    }

    private void Flash(){
        TurnColor();
        EventManager.current.FireflyFlash(this.transform.position);
        StartCoroutine(Charge());
    }

    void TurnColor()
    {
        Color col = new Color(Random.value, Random.value, Random.value);
        GetComponent<Renderer>().material.color = col;
    }

    IEnumerator Charge(){
        while (chargingProgress < chargeThreshold){
            chargingProgress += 1;

            yield return new WaitForSeconds(0.001f);
        }
        chargingProgress = 0;
        Flash();
    }

    IEnumerator SendMessageToFlash() {
        while (false) { //TODO: Change the logic here so that the firefly flashes at the appropriate time
            //TODO: keep track of how long you've been waiting here
            

            yield return new WaitForSeconds(0.001f);
        }

        FireflyLightFlash lightFlash = gameObject.GetComponent<FireflyLightFlash>();
        lightFlash.flashed = false;          

        //TODO: Don't forget to reset your variables!
        

    }

    // Wait will increment Waiting progress by 1 every 0.001s, stopping when it hits the threshold,
    // Used to control the timing between when a message is sent to flash and when the firefly begins charging again
    IEnumerator WaitToCharge() {
        while (false) { //TODO: Change logic here so the firefly begins charging again at the right time
            //TODO: Keep track of how long you've been waiting
            

            yield return new WaitForSeconds(0.001f);
        }
        
        //TODO: Reset your variables and call the next function in the sequence
        

    }
}
