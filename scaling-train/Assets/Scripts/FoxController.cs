using UnityEngine;

public class FoxController : MonoBehaviour
{
    private static readonly int Walking = Animator.StringToHash("walking");
    private static readonly int Running = Animator.StringToHash("running");

    private Animator _animator;

    private ChickenController _preyController;

    public float distanceToFoxHole;

    public float distanceToPrey;

    public bool doneEating;

    public bool hasCaught;

    public GameObject foxHole;

    public GameObject prey;

    public bool running;

    public bool walking;

    public bool eating;

    private void OnDrawGizmos()
    {
        if (prey != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, prey.transform.position);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        _preyController = null;
        _animator = GetComponent<Animator>();
        GetComponent<StateMachine>().ChangeState(new SearchTarget());
    }

    // Update is called once per frame
    private void Update()
    {
        if (prey != null && _preyController == null) _preyController = prey.GetComponent<ChickenController>();

        _animator.SetBool(Walking, walking);
        _animator.SetBool(Running, running);
        var position = transform.position;
        distanceToFoxHole = Vector3.Distance(foxHole.transform.position, position);
        if (prey != null)
        {
            distanceToPrey = Vector3.Distance(prey.transform.position, position);
            if (distanceToPrey < 2f)
            {
                _preyController.Catch();
                hasCaught = true;
            }
        }

        if (eating)
        {
            Eat();
        }

        var rot = transform.rotation.eulerAngles;
        rot.x = 0;
        rot.z = 0;
        transform.rotation = Quaternion.Euler(rot);
    }

    public void Eat()
    {
        new WaitForSeconds(5);
        doneEating = true;
        _preyController.Drop();
        prey = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("prey"))
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
    }
}

public class SearchTarget : State
{
    public override void Enter()
    {
        owner.GetComponent<NoiseWander>().enabled = true;
        owner.GetComponent<FoxController>().walking = true;
    }

    public override void Think()
    {
        GameObject target = GameObject.FindWithTag("prey");

        owner.GetComponent<FoxController>().prey = target;
        owner.GetComponent<Pursue>().target = owner.GetComponent<FoxController>().prey;
        if (owner.GetComponent<FoxController>().prey != null)
        {
            owner.ChangeState(new ApproachState());
        }
    }

    public override void Exit()
    {
        owner.GetComponent<NoiseWander>().enabled = false;
    }
}

public class ApproachState : State
{
    public override void Enter()
    {
        owner.GetComponent<Pursue>().enabled = true;
    }

    public override void Think()
    {
        if (Vector3.Distance(
                owner.GetComponent<FoxController>().prey.transform.position,
                owner.transform.position) < 4)
            owner.ChangeState(new AttackState());
    }

    public override void Exit()
    {
        owner.GetComponent<FoxController>().walking = false;
        owner.GetComponent<Seek>().enabled = false;
    }
}

public class AttackState : State
{
    public override void Enter()
    {
        owner.GetComponent<FoxController>().running = true;
        owner.GetComponent<Boid>().maxSpeed = 7.5f;
    }

    public override void Think()
    {
        if (owner.GetComponent<FoxController>().hasCaught) owner.ChangeState(new RetreatState());
    }

    public override void Exit()
    {
        owner.GetComponent<Pursue>().enabled = false;
        owner.GetComponent<FoxController>().running = false;
        owner.GetComponent<Boid>().maxSpeed = 5f;
    }
}

public class RetreatState : State
{
    public override void Enter()
    {
        owner.GetComponent<Arrive>().enabled = true;
        owner.GetComponent<Arrive>().targetGameObject = owner.GetComponent<FoxController>().foxHole;
        owner.GetComponent<FoxController>().walking = true;
    }

    public override void Think()
    {
        if (owner.GetComponent<FoxController>().distanceToFoxHole < 3) owner.ChangeState(new EatState());
    }

    public override void Exit()
    {
        owner.GetComponent<FoxController>().walking = false;
        owner.GetComponent<Arrive>().enabled = false;
    }
}

public class EatState : State
{
    public override void Enter()
    {
        owner.GetComponent<FoxController>().eating = true;
    }

    public override void Think()
    {
        if (owner.GetComponent<FoxController>().doneEating)
        {
            owner.GetComponent<FoxController>().doneEating = false;
            owner.ChangeState(new SearchTarget());
        }
    }

    public override void Exit()
    {
        owner.GetComponent<FoxController>().eating = false;
    }
}