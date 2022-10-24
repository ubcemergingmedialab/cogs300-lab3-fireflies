using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyEventsTesting : MonoBehaviour
{
    public Rigidbody rb;

    public Light lightsource;
    public int sightDistance;

   const float delayMultiplier = 0.1f;

    // Each of the following represents a number of milliseconds

    //The time it takes to reach the top of the threshold. 
    //Represented by THRESHOLD LEVEL (dotted line) in the diagram
    const float chargeThreshold = 800 * delayMultiplier;

    //The time between message sending and light flashing. 
    //Represented by timescale distance between MESSAGE and FLASH on diagram
    const float sendDelay = 200 * delayMultiplier; 

    //The time between message sending and the chargingProcess restarting from zero. 
    //Represented by timescale distance between MESSAGE and the bottom of the chargingProgress on diagram
    const float waitDelay = 200 * delayMultiplier; 
    const float flashTimer = 200 * delayMultiplier;

    public float flashProgress = 0;

    public float chargingProgress = 0;

    public float sendingProgress = 0;
    public float waitProgress = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // rb.velocity = new Vector3(Random.Range(-5, 5),Random.Range(-5, 5),Random.Range(-5, 5));
        EventManager.current.OnFireflyFlash += SenseFlashes;
        chargingProgress = Random.Range(0,chargeThreshold);
        StartCoroutine(Charge());
        lightsource.intensity = 0;
    }


    private void SenseFlashes(Vector3 position){
        float distance = Vector3.Distance(this.transform.position, position);
        
        if(distance > 0 && distance < sightDistance){
            chargingProgress = 0;
            waitProgress = waitDelay;
        }
        
        
        
    }
    // Update is called once per frame
    void Update()
    {

        // if (Input.GetKeyDown("space"))
        // {
        //    rb.velocity = new Vector3(Random.Range(-5, 5),Random.Range(-5, 5),Random.Range(-5, 5));
        // }

        // if(rb.velocity.sqrMagnitude != 3.5f*3.5f){
        //     rb.velocity = rb.velocity.normalized * 3.5f;
        // }
        // if(rb.velocity != Vector3.zero){
        //  transform.forward = rb.velocity;
        // }
    }

    private void Flash(){
        StartCoroutine(FlashColor());
        EventManager.current.FireflyFlash(this.transform.position);
    }

    IEnumerator FlashColor(){
        // TurnYellow();
        // lightsource.SetActive(true);
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
        // lightsource.SetActive(false);
        // TurnWhite();
    }

    void TurnYellow()
    {
        Color col = new Color(252, 207, 0);
        GetComponent<Renderer>().material.color = col;
    }

    void TurnWhite(){
        Color col = new Color(200, 200, 200);
        GetComponent<Renderer>().material.color = col;
    }

    IEnumerator Charge(){
        while (chargingProgress < chargeThreshold){
            chargingProgress++;
            

            yield return new WaitForSeconds(0.001f);
        }
        chargingProgress = 0;
        StartCoroutine(SendMessageToFlash());
    }

    IEnumerator SendMessageToFlash() {
        while (sendingProgress < sendDelay) { //TODO: Change the logic here so that the firefly flashes at the appropriate time
            //TODO: keep track of how long you've been waiting here
            sendingProgress++;

            yield return new WaitForSeconds(0.001f);
        }
        sendingProgress = 0;
        Flash();
        StartCoroutine(WaitToCharge());

        //TODO: Don't forget to reset your variables!
        

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
}
