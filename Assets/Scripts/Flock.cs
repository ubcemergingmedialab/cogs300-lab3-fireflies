using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Spawn Setup")]
    public FlockUnit firefly;
    public int spawnNumber = 100;
    public FlockUnit[] allFireflies {get; set;}

    [Header("Speed Setup")]
    [Range(0, 10)]
    [SerializeField] private float _minSpeed;
    public float minSpeed {get {return _minSpeed;}}
    [Range(0, 10)]
    [SerializeField] private float _maxSpeed;
    public float maxSpeed {get {return _maxSpeed;}}

    [Header("Detection Distances")]
    [Range(0, 10)]
    [SerializeField] private float _cohesionDistance;
    public float cohesionDistance {get {return _cohesionDistance;}}
    [Range(0, 10)]
    [SerializeField] private float _alignmentDistance;
    public float alignmentDistance {get {return _alignmentDistance;}}
    [Range(0, 10)]
    [SerializeField] private float _avoidanceDistance;
    public float avoidanceDistance {get {return _avoidanceDistance;}}

    [Range(0, 10)]
    [SerializeField] private float _boundsDistance;
    public float boundsDistance {get {return _boundsDistance;}}

    [Header("Behaviour Weights")]
    [Range(0, 10)]
    [SerializeField] private float _cohesionWeight;
    public float cohesionWeight {get {return _cohesionWeight;}}
    [Range(0, 10)]
    [SerializeField] private float _alignmentWeight;
    public float alignmentWeight {get {return _alignmentWeight;}}
    [Range(0, 10)]
    [SerializeField] private float _avoidanceWeight;
    public float avoidanceWeight {get {return _avoidanceWeight;}}
    [Range(0, 10)]
    [SerializeField] private float _boundsWeight;
    public float boundsWeight {get {return _boundsWeight;}}

    void Start()
    {
        GenerateFireflies();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < allFireflies.Length; i++){
            allFireflies[i].MoveUnit();
        }
    }

    private void GenerateFireflies(){
        allFireflies = new FlockUnit[spawnNumber];
        for(int i = 0; i < spawnNumber; i++){
            FlockUnit newFirefly = Instantiate(firefly);
            allFireflies[i] = newFirefly;
            newFirefly.transform.position = new Vector3(Random.Range(-20, 20),Random.Range(-20, 20),Random.Range(-20, 20));
            newFirefly.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            newFirefly.transform.parent = this.transform;
            newFirefly.InititializeSpeed(Random.Range(minSpeed, maxSpeed));
            newFirefly.AssignFlock(this);
        }
    }
}
