using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : SteeringBehaviour
{
    public GameObject target;

    Vector3 _targetPos;

    public void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _targetPos);
        }
    }

    public override Vector3 Calculate()
    {
        Vector3 position = target.transform.position;
        float dist = Vector3.Distance(position, transform.position);
        float time = dist / boid.maxSpeed;

        _targetPos = position + (target.GetComponent<Rigidbody>().velocity * time);

        return boid.SeekForce(_targetPos);
    }
}
