using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockUnit : MonoBehaviour
{
    [SerializeField] private float smoothDamp;
    private List<FlockUnit> cohesionNeighbours = new List<FlockUnit>();
    private List<FlockUnit> alignmentNeighbours = new List<FlockUnit>();
    private List<FlockUnit> avoidanceNeighbours = new List<FlockUnit>();
    private Flock assignedFlock;
    private Vector3 currentVelocity;
    private float speed;
    private Vector3 avoidanceVector;
    private Vector3 alignmentVector;
    private Vector3 cohesionVector;

    public Transform myTransform { get; set; }

    private void Awake()
    {
        myTransform = transform;
    }

    public void AssignFlock(Flock flock)
    {
        assignedFlock = flock;
    }

    public void InititializeSpeed(float speed)
    {
        this.speed = speed;
    }


    public void MoveUnit()
    {
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

    public void AddCohesionNeighbour(FlockUnit flockUnit)
    {
        cohesionNeighbours.Add(flockUnit);
        speed += flockUnit.speed;
    }

    public void AddAlignmentNeighbours(FlockUnit flockUnit)
    {
        alignmentNeighbours.Add(flockUnit);
        alignmentVector += flockUnit.myTransform.forward;
    }

    public void AddAvoidanceNeighbours(FlockUnit flockUnit)
    {
        avoidanceNeighbours.Add(flockUnit);
        avoidanceVector += (myTransform.position - flockUnit.myTransform.position);
    }

    public void ResetVectors()
    {
        speed = 0;
        avoidanceVector = Vector3.zero;
        alignmentVector = myTransform.forward;
        cohesionVector = Vector3.zero;
    }

    public void ClearNeighbours()
    {
        cohesionNeighbours.Clear();
        alignmentNeighbours.Clear();
        avoidanceNeighbours.Clear();
    }

    private void CalculateSpeed()
    {
        if (cohesionNeighbours.Count == 0)
        {
            speed = assignedFlock.minSpeed;
            return;
        }
        speed /= cohesionNeighbours.Count;
        speed = Mathf.Clamp(speed, assignedFlock.minSpeed, assignedFlock.maxSpeed);
    }

    private Vector3 CalculateAvoidanceVector()
    {
        if (avoidanceNeighbours.Count == 0)
        {
            return avoidanceVector;
        }
        avoidanceVector /= avoidanceNeighbours.Count;
        avoidanceVector = avoidanceVector.normalized;

        return avoidanceVector;
    }


    private Vector3 CalculateAlignmentVector()
    {
        if (alignmentNeighbours.Count == 0)
        {
            return alignmentVector;
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
        if (cohesionNeighbours.Count == 0)
        {
            return cohesionVector;
        }

        cohesionVector /= cohesionNeighbours.Count;
        cohesionVector -= myTransform.position;
        cohesionVector = cohesionVector.normalized;

        return cohesionVector;
    }
}
