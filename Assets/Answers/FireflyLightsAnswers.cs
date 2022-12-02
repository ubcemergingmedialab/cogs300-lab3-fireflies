using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyLightsAnswers : MonoBehaviour
{
    public Rigidbody rb;

    public Light lightsource;
    public int sightDistance;

    public float delayMultiplier = 0.1f;

    // Each of the following represents a number of milliseconds

    //The time it takes to reach the top of the threshold. 
    //Represented by THRESHOLD LEVEL (dotted line) in the diagram
     float chargeThreshold = 800;

    //The time between message sending and light flashing. 
    //Represented by timescale distance between MESSAGE and FLASH on diagram
     float sendDelay = 200;

    //The time between message sending and the chargingProcess restarting from zero. 
    //Represented by timescale distance between MESSAGE and the bottom of the chargingProgress on diagram
     float waitDelay = 200;
     float flashTimer = 200;

    public float flashProgress = 0;

    public float chargingProgress = 0;

    public float sendingProgress = 0;
    public float waitProgress = 0;
    // Start is called before the first frame update
    void Start()
    {
        chargeThreshold *= delayMultiplier;
        sendDelay *= delayMultiplier; 
        waitDelay *= delayMultiplier; 
        flashTimer *= delayMultiplier;

        rb = GetComponent<Rigidbody>();
        // rb.velocity = new Vector3(Random.Range(-5, 5),Random.Range(-5, 5),Random.Range(-5, 5));
        EventManager.current.OnFireflyFlash += SenseFlashes;
        chargingProgress = Random.Range(0,chargeThreshold);
        StartCoroutine(Charge());
        lightsource.intensity = 0;
    }

    IEnumerator Charge(){
        while (chargingProgress < chargeThreshold){ //TODO: change the logic here so that the firefly charges up until the threshold
            //TODO: Charge the firefly
            chargingProgress++;
            

            yield return new WaitForSeconds(0.001f);
        }
        //TODO: After you are done charging, reset your variables and call the next function(s) to continue the flashing cycle
        chargingProgress = 0;
        waitProgress = 0;
        StartCoroutine(SendMessageToFlash());
        StartCoroutine(WaitToCharge());
    }

    IEnumerator SendMessageToFlash() {
        while (sendingProgress < sendDelay) { //TODO: Change the logic here so that the firefly flashes at the appropriate time
            //TODO: keep track of how long you've been waiting here
            sendingProgress++;

            yield return new WaitForSeconds(0.001f);
        }
        Flash();

        //TODO: Don't forget to reset your variables!
        sendingProgress = 0;
        

    }

    // Wait will increment Waiting progress by 1 every 0.001s, stopping when it hits the threshold,
    // Used to control the timing between when a message is sent to flash and when the firefly begins charging again
    IEnumerator WaitToCharge() {
        while (waitProgress < waitDelay) { //TODO: Change logic here so the firefly begins charging again at the right time
            //TODO: Keep track of how long you've been waiting
            waitProgress++;

            yield return new WaitForSeconds(0.001f);
        }
        
        //TODO: Reset your variables and call the next function in the sequence
        waitProgress = 0;
        StartCoroutine(Charge());

    }

    private void SenseFlashes(Vector3 position){
        float distance = Vector3.Distance(this.transform.position, position);

        if(distance > 0 && distance < sightDistance){
            //TODO: Implement what should happen when the firefly sees another nearby flash. 
            //What should change? Be sure to think about both late and early flashes.
            chargingProgress = 0;
            waitProgress = waitDelay;
        }
    }

    // -------------------------------- Ignore me. --------------------------------------
    private void Flash(){
        StartCoroutine(FlashColor());
        EventManager.current.FireflyFlash(this.transform.position);
    }

    IEnumerator FlashColor(){
        while(flashProgress < flashTimer){
            flashProgress++;
            if(flashProgress < flashTimer/2){
                lightsource.intensity += 0.02f / delayMultiplier;
            }
            else{
                lightsource.intensity -= 0.02f / delayMultiplier;
            }
            yield return new WaitForSeconds(0.001f);
        }
        flashProgress = 0;
        lightsource.intensity = 0f;
    }

}
