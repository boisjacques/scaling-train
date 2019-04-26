using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Approach : SteeringBehaviour
{
    public Vector3 targetPosition = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override Vector3 Calculate()
    {
        return boid.ArriveForce(targetPosition, 5);
    }
}
