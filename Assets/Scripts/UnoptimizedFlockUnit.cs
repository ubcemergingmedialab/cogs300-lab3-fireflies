using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoptimizedFlockUnit : MonoBehaviour
{

    [SerializeField] private float smoothDamp;
    private List<UnoptimizedFlockUnit> cohesionNeighbours = new List<UnoptimizedFlockUnit>();
    private List<UnoptimizedFlockUnit> alignmentNeighbours = new List<UnoptimizedFlockUnit>();
    private List<UnoptimizedFlockUnit> avoidanceNeighbours = new List<UnoptimizedFlockUnit>();
    private UnoptimizedFlock assignedFlock;
    private Vector3 currentVelocity;
    private float speed;

    public Transform myTransform { get; set; }

    private void Awake()
    {
        myTransform = transform;
    }

    public void AssignFlock(UnoptimizedFlock flock)
    {
        assignedFlock = flock;
    }

    public void InititializeSpeed(float speed)
    {
        this.speed = speed;
    }

    //Note: being called every update from flock.update().  BigO is as summarized:
    //n = number of ff
    //cohesion vector = n * cv
    //avoidance vector = n * avoidv
    //align vector = n * alignv
    //Calculatespeed = n * cv
    //boundsVector = n * 1 (constant time)
    //Findneighbours = n^2 (see below)
    // each FF looks at every other FF and does calculations.  It might be possible to reduce it to (n^2 / 2)?
    // ex: if ffA decides it needs to add ffB to cohesionNeighbours, then it must be same for ffB in relation to ffA.
    // this is the same for avoidanceNeighbours, alignment neighbours etc.
    public void MoveUnit()
    {
        FindNeighbours();
        CalculateSpeed();

        var cohesionVector = CalculateCohesionVector() * assignedFlock.cohesionWeight;
        var avoidanceVector = CalculateAvoidanceVector() * assignedFlock.avoidanceWeight;
        var alignmentVector = CalculateAlignmentVector() * assignedFlock.alignmentWeight;
        var boundsVector = CalculateBoundsVector() * assignedFlock.boundsWeight;


        var moveVector = cohesionVector + avoidanceVector + alignmentVector + boundsVector;
        moveVector = Vector3.SmoothDamp(myTransform.forward, moveVector, ref currentVelocity, smoothDamp);
        moveVector = moveVector.normalized * speed;
        if (moveVector != Vector3.zero)
        {
            myTransform.forward = moveVector;
        }

        myTransform.position += moveVector * Time.deltaTime;
    }

    private void FindNeighbours()
    {
        cohesionNeighbours.Clear();
        alignmentNeighbours.Clear();
        avoidanceNeighbours.Clear();
        var allFireflies = assignedFlock.allFireflies;
        for (int i = 0; i < assignedFlock.spawnNumber; i++)
        {
            var currentUnit = allFireflies[i];
            if (currentUnit != this)
            {
                float currentNeighbourDistanceSqr = Vector3.SqrMagnitude(currentUnit.transform.position - transform.position);
                if (currentNeighbourDistanceSqr <= assignedFlock.cohesionDistance * assignedFlock.cohesionDistance)
                {
                    cohesionNeighbours.Add(currentUnit);
                }
                if (currentNeighbourDistanceSqr <= assignedFlock.alignmentDistance * assignedFlock.alignmentDistance)
                {
                    alignmentNeighbours.Add(currentUnit);
                }
                if (currentNeighbourDistanceSqr <= assignedFlock.avoidanceDistance * assignedFlock.avoidanceDistance)
                {
                    avoidanceNeighbours.Add(currentUnit);
                }
            }
        }
    }

    private void CalculateSpeed()
    {
        speed = 0;
        if (cohesionNeighbours.Count == 0)
        {
            speed = assignedFlock.minSpeed;
            return;
        }
        for (int i = 0; i < cohesionNeighbours.Count; i++)
        {
            speed += cohesionNeighbours[i].speed;
        }
        speed /= cohesionNeighbours.Count;
        speed = Mathf.Clamp(speed, assignedFlock.minSpeed, assignedFlock.maxSpeed);
    }

    private Vector3 CalculateAvoidanceVector()
    {
        var avoidanceVector = Vector3.zero;
        if (avoidanceNeighbours.Count == 0)
        {
            return avoidanceVector;
        }
        for (int i = 0; i < avoidanceNeighbours.Count; i++)
        {
            avoidanceVector += (myTransform.position - avoidanceNeighbours[i].myTransform.position);
        }
        avoidanceVector /= avoidanceNeighbours.Count;
        avoidanceVector = avoidanceVector.normalized;

        return avoidanceVector;
    }

    private Vector3 CalculateAlignmentVector()
    {
        var alignmentVector = myTransform.forward;
        if (alignmentNeighbours.Count == 0)
        {
            return alignmentVector;
        }
        for (int i = 0; i < alignmentNeighbours.Count; i++)
        {
            alignmentVector += alignmentNeighbours[i].myTransform.forward;
        }
        alignmentVector /= alignmentNeighbours.Count;
        alignmentVector = alignmentVector.normalized;

        return alignmentVector;
    }

    private Vector3 CalculateBoundsVector()
    {
        var offsetToCenter = assignedFlock.transform.position - myTransform.position;
        bool isNearCenter = (offsetToCenter.magnitude >= assignedFlock.boundsDistance * 0.9f);
        return isNearCenter ? offsetToCenter.normalized : Vector3.zero;
    }

    private Vector3 CalculateCohesionVector()
    {
        var cohesionVector = Vector3.zero;

        if (cohesionNeighbours.Count == 0)
        {
            return cohesionVector;
        }
        for (int i = 0; i < cohesionNeighbours.Count; i++)
        {
            cohesionVector += cohesionNeighbours[i].myTransform.position;
        }
        cohesionVector /= cohesionNeighbours.Count;
        cohesionVector -= myTransform.position;
        cohesionVector = cohesionVector.normalized;

        return cohesionVector;
    }
}