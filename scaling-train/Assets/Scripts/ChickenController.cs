using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChickenController : MonoBehaviour
{
    public float force = 1;

    private float _stamina = 30;

    private Rigidbody _rb;

    private GameObject _predator;

    public bool flee;

    public float distanceToPredator = Single.MaxValue;
    public float distanceToAnchor = Single.MaxValue;

    public GameObject anchor;

    public float frequency = 0.3f;
    public float radius = 10.0f;

    public float theta = 0;
    public float amplitude = 80;
    public float distance = 5;

    private Vector3 _target;
    private Vector3 _worldTarget;

    public bool isCaught;

    public bool returnToAnchor;

    private bool _dropped;

    // Start is called before the first frame update
    void Start()
    {
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody>();
        }

        StartCoroutine(Jump());
        _predator = GameObject.FindWithTag("predator");
        GetComponent<StateMachine>().ChangeState(new PickingState());
        float angle = Random.Range(0, 360);
        transform.Rotate(Vector3.up, angle);
    }


    private void OnDrawGizmos()
    {
        if (!flee)
        {
            Vector3 localCP = (Vector3.forward * distance);
            Vector3 rot = transform.rotation.eulerAngles;
            rot.x = rot.z = 0;
            Vector3 worldCP = transform.position + (Quaternion.Euler(rot) * localCP);
            Gizmos.color = Color.cyan;

            Gizmos.DrawWireSphere(worldCP, radius);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_worldTarget, 0.5f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _worldTarget);
        }
    }

    void Update()
    {
        Vector3 position = transform.position;
        distanceToPredator = Vector3.Distance(_predator.transform.position, position);
        distanceToAnchor = Vector3.Distance(anchor.transform.position, position);


        if (flee)
        {
            Flee();
        }
        else if (!returnToAnchor)
        {
            Pick();
        }
        else if (returnToAnchor)
        {
            ToAnchor();
        }
        else if (isCaught)
        {
        }
    }

    private void ToAnchor()
    {
        Transform trf = transform;
        Quaternion rotation = trf.rotation;

        rotation = Quaternion.Slerp(rotation,
            Quaternion.LookRotation(anchor.transform.position - trf.position),
            Time.deltaTime * 1);
        transform.rotation = rotation;
    }

    public void Catch()
    {
        isCaught = true;
        tag = "Untagged";
        transform.parent = _predator.transform;
        transform.position = _predator.transform.GetChild(2).position;
        _rb.isKinematic = true;
        _rb.mass = 0;
        _rb.useGravity = false;
    }

    public void Drop()
    {
        _rb.useGravity = true;
        _rb.isKinematic = false;
        _rb.mass = 1;
        // Destroy(gameObject, 0.5f);
        transform.parent = null;
    }

    void Pick()
    {
        float n = (Mathf.PerlinNoise(theta, 1) * 2) - 1;
        float angle = n * Random.Range(70, 90) * Mathf.Deg2Rad;
        Transform trf = transform;
        Quaternion rotation = trf.rotation;
        Vector3 rot = rotation.eulerAngles;
        Vector3 position = trf.position;

        rot.x = 0;
        _target.x = Mathf.Sin(angle);
        _target.z = Mathf.Cos(angle);
        _target.y = 0;
        rot.z = 0;

        _target *= radius;
        Vector3 localTarget = _target + (Vector3.forward * distance); // projecting forward in local space
        _worldTarget = position + Quaternion.Euler(rot) * localTarget;

        theta += frequency * Time.deltaTime * Mathf.PI * 2.0f;

        rotation = Quaternion.Slerp(rotation,
            Quaternion.LookRotation(_worldTarget - position),
            Time.deltaTime * 1);
        transform.rotation = rotation;
    }

    void Flee()
    {
        Transform trf = transform;
        Vector3 heading = _predator.transform.position - trf.position;
        float distance = heading.magnitude;
        Vector3 direction = heading / distance;

        float angle = Vector3.Angle(trf.forward, direction);
        angle += 180;


        transform.rotation = Quaternion.Slerp(trf.rotation,
            Quaternion.AngleAxis(angle, Vector3.up),
            Time.deltaTime * 2);
    }

    IEnumerator Jump()
    {
        while (!isCaught)
        {
            float actualForce = force;
            float tenth = actualForce / 10;
            if (_stamina - actualForce <= 0)
            {
                float diff = _stamina - actualForce;
                actualForce -= Math.Abs(diff);
            }

            actualForce = Random.Range(actualForce - tenth, actualForce + tenth);
            Transform trf = transform;
            _rb.AddForce((trf.up + trf.forward) * actualForce, ForceMode.Impulse);
            _stamina -= actualForce;
            if (_stamina < 30)
            {
                _stamina += 1.5f;
            }

            Vector3 rot = transform.rotation.eulerAngles;
            rot.x = 0;
            rot.z = 0;
            transform.rotation = Quaternion.Euler(rot);
            float wait = Random.Range(0.9f, 1.1f);
            yield return new WaitForSeconds(wait);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("prey") || collision.gameObject.CompareTag("predator"))
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}

public class PickingState : State
{
    public override void Enter()
    {
        owner.GetComponent<ChickenController>().force = 1;
    }

    public override void Think()
    {
        if (owner.GetComponent<ChickenController>().isCaught)
        {
            owner.ChangeState(new CaughtState());
        }
        else if (owner.GetComponent<ChickenController>().distanceToPredator < 10)
        {
            owner.ChangeState(new FleeingState());
        }
        else if (owner.GetComponent<ChickenController>().distanceToAnchor > 9)
        {
            owner.ChangeState(new ReturningState());
        }
    }
}

public class FleeingState : State
{
    public override void Enter()
    {
        owner.GetComponent<ChickenController>().flee = true;
        owner.GetComponent<ChickenController>().force = 3;
    }

    public override void Think()
    {
        if (owner.GetComponent<ChickenController>().isCaught)
        {
            owner.ChangeState(new CaughtState());
        }
        else if (owner.GetComponent<ChickenController>().distanceToPredator > 20)
        {
            owner.ChangeState(new PickingState());
        }
        else if (owner.GetComponent<ChickenController>().distanceToAnchor > 9)
        {
            owner.ChangeState(new ReturningState());
        }
    }

    public override void Exit()
    {
        owner.GetComponent<ChickenController>().flee = false;
    }
}

public class CaughtState : State
{
}

public class ReturningState : State
{
    public override void Enter()
    {
        owner.GetComponent<ChickenController>().returnToAnchor = true;
        owner.GetComponent<ChickenController>().force = 2;
    }

    public override void Think()
    {
        if (owner.GetComponent<ChickenController>().distanceToPredator < 10)
        {
            owner.ChangeState(new FleeingState());
        }
        else if (owner.GetComponent<ChickenController>().distanceToAnchor < 3)
        {
            owner.ChangeState(new PickingState());
        }
    }

    public override void Exit()
    {
        owner.GetComponent<ChickenController>().returnToAnchor = false;
    }
}