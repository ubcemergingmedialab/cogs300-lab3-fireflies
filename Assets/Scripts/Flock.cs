using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    // Start is called before the first frame update


    // -------- STUDENTS CAN IGNORE THIS FILE --------

    [Header("Spawn Setup")]
    public FlockUnit firefly;
    public int spawnNumber = 100;

    public FlockUnit[] allFireflies { get; set; }

    [Header("Flashing Setup (Cannot be changed during runtime)")]

    [Range(0, 10)]
    [SerializeField] private int _sightDistance;
    public int sightDistance { get { return _sightDistance; } }

    [Range(0, 5)]
    [SerializeField] private float _delayMultiplier;
    public float delayMultiplier { get { return _delayMultiplier; } }


    [Header("Speed Setup")]
    [Range(0, 10)]
    [SerializeField] private float _minSpeed;
    public float minSpeed { get { return _minSpeed; } }
    [Range(0, 10)]
    [SerializeField] private float _maxSpeed;
    public float maxSpeed { get { return _maxSpeed; } }


    [Header("Detection Distances")]
    [Header("--------Flocking Behaviours-----------")]


    [Range(0, 10)]
    [SerializeField] private float _cohesionDistance;
    public float cohesionDistance { get { return _cohesionDistance; } }
    [Range(0, 10)]
    [SerializeField] private float _alignmentDistance;
    public float alignmentDistance { get { return _alignmentDistance; } }
    [Range(0, 10)]
    [SerializeField] private float _avoidanceDistance;
    public float avoidanceDistance { get { return _avoidanceDistance; } }




    [Range(0, 10)]
    [SerializeField] private float _boundsDistance;
    public float boundsDistance { get { return _boundsDistance; } }

    [Header("Behaviour Weights")]
    [Range(0, 10)]
    [SerializeField] private float _cohesionWeight;
    public float cohesionWeight { get { return _cohesionWeight; } }
    [Range(0, 10)]
    [SerializeField] private float _alignmentWeight;
    public float alignmentWeight { get { return _alignmentWeight; } }
    [Range(0, 10)]
    [SerializeField] private float _avoidanceWeight;
    public float avoidanceWeight { get { return _avoidanceWeight; } }
    [Range(0, 10)]
    [SerializeField] private float _boundsWeight;
    public float boundsWeight { get { return _boundsWeight; } }

    void Start()
    {
        GenerateFireflies();
    }

    // Update is called once per frame
    void Update()
    {
        MoveFireflies();
    }

    private void MoveFireflies()
    {
        for (int i = 0; i < allFireflies.Length; i++)
        {
            //reset the vectors for the current FF
            allFireflies[i].ResetVectors();

            //clear neighbours
            allFireflies[i].ClearNeighbours();

            for (int j = i; j < allFireflies.Length; j++)
            {
                //we don't need to look at the same FF
                if (i != j)
                {
                    float currentNeighbourDistanceSqr = Vector3.SqrMagnitude(allFireflies[j].transform.position - allFireflies[i].transform.position);
                    if (currentNeighbourDistanceSqr <= cohesionDistance * cohesionDistance)
                    {
                        allFireflies[i].AddCohesionNeighbour(allFireflies[j]);
                        allFireflies[j].AddCohesionNeighbour(allFireflies[i]);
                    }
                    if (currentNeighbourDistanceSqr <= alignmentDistance * alignmentDistance)
                    {
                        allFireflies[i].AddAlignmentNeighbours(allFireflies[j]);
                        allFireflies[j].AddAlignmentNeighbours(allFireflies[i]);
                    }
                    if (currentNeighbourDistanceSqr <= avoidanceDistance * avoidanceDistance)
                    {
                        allFireflies[i].AddAvoidanceNeighbours(allFireflies[j]);
                        allFireflies[j].AddAvoidanceNeighbours(allFireflies[i]);
                    }
                }
            }
            allFireflies[i].MoveUnit();
        }
    }

    private void GenerateFireflies()
    {
        allFireflies = new FlockUnit[spawnNumber];
        for (int i = 0; i < spawnNumber; i++)
        {
            FlockUnit newFirefly = Instantiate(firefly);
            allFireflies[i] = newFirefly;
            newFirefly.transform.position = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));
            newFirefly.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            newFirefly.transform.parent = this.transform;
            FireflyLights lightsScript = newFirefly.GetComponent<FireflyLights>();
            lightsScript.sightDistance = _sightDistance;
            lightsScript.delayMultiplier = _delayMultiplier;
            newFirefly.InititializeSpeed(Random.Range(minSpeed, maxSpeed));
            newFirefly.AssignFlock(this);
        }
    }
}
