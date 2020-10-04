using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyLightControl : MonoBehaviour
{
    //How slower the regular flashing cycle will be compared to real fireflies
    //At 1, the cycle will take approx. the same time as a real firefly (around 1s)
    //Increasing this value will increase the time it takes for each cycle
    //e.g. delayMultiplier = 3 means the firefly will take around 3s per regular cycle
    const int delayMultiplier = 1;

    // Each of the following represents a number of milliseconds

    //The time it takes to reach the top of the threshold. 
    //Represented by THRESHOLD LEVEL (dotted line) in the diagram
    const int chargeThreshold = 800 * delayMultiplier;

    //The time between message sending and light flashing. 
    //Represented by timescale distance between MESSAGE and FLASH on diagram
    const int sendDelay = 200 * delayMultiplier; 

    //The time between message sending and the chargingProcess restarting from zero. 
    //Represented by timescale distance between MESSAGE and the bottom of the chargingProgress on diagram
    const int waitDelay = 200 * delayMultiplier;  


    // Amount of time firefly has been charging. 
    //Represented by solid line on diagram -> bottom == zero, top == chargeThreshold
    public int chargingProgress = 0; 
    
    // Amount of time which has passed so far between the MESSAGE and FLASH. 
    //Not represented on diagram
    public int sendingProgress = 0;

    // Amount of time which has passed so far between the MESSAGE and the chargingProgress restarting from zero. 
    //Not represented on diagram.
    public int waitProgress = 0;
  
    void Start()
    {      
        AssignMat();  //You can ignore this. 
                    //This is to make each firefly have a diffrent mat instance as the emission color is tied to the material
        
        
        StartCoroutine(Charge());
    }

    // Charge will increment chargingProgress by 1 every 0.001s, stopping when it hits the threshold. 
    //Used to control when the message is sent to flash
    IEnumerator Charge() {      
        while (false) { //TODO: change the logic here so that the firefly charges up until the threshold
            //TODO: Charge the firefly
            

            yield return new WaitForSeconds(0.001f);
        }

        //TODO: After you are done charging, reset your charge and call the next function(s) to continue the flashing cycle
        

    }

    // SendMessageToFlash will increment sendingProgress by 1 every 0.001s, stopping when it hits the threshold.
    // Used to control when a flash is emitted after the message to flash has been sent
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

    // This function is called when the firefly is hit by light from other fireflies
    public void sawFlash() {
        //TODO: Implement what should happen when the firefly sees another nearby flash. 
        //What should change? Be sure to think about both late and early flashes.
        
        
    }


    
    // -------------------------------- Ignore me. --------------------------------------
    // Simulate seeing flash from a light source when left mouse button is clicked
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Saw Flash");
            sawFlash();
        }
    }
    
    // helper to assign a copy of of FireflyMat as the material for this firefly
    void AssignMat() {
        Material fireflyMat = (Material) Resources.Load<Material>("FireflyMat"); 

        gameObject.GetComponent<Renderer>().material = new Material(fireflyMat);
    }
}