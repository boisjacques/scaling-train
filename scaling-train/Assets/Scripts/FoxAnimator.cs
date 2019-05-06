using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxAnimator : MonoBehaviour
{
    private static readonly int Walking = Animator.StringToHash("walking");
    private static readonly int Running = Animator.StringToHash("running");
    public bool walking;
    public bool running;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool(Walking, walking);
        _animator.SetBool(Running, running);
    }
}