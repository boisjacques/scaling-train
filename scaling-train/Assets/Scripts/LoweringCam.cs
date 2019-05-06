using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoweringCam : MonoBehaviour
{
    private Vector3 _startMarker;
    private Vector3 _endMarker;

    public float lerpSpeed = 6.0F;
    public float slerpSpeed = 6.0F;


    private float _startTime;

    private float _journeyLength;
    
    private float _lastCamY = Single.MaxValue;

    public GameObject camTarget;

    public float slerpOffset;

    public float sceneChangeDelay = 0.7f;

    void Start()
    {
        _startMarker = transform.position;
        _endMarker = _startMarker;
        _endMarker.y = 3;
        _startTime = Time.time;

        _journeyLength = Vector3.Distance(_startMarker, _endMarker);
    }

    void FixedUpdate()
    {
        LerpCamera();
        SlerpCamera();
    }

    void LerpCamera()
    {
        float distCovered = (Time.time - _startTime) * lerpSpeed;

        float fracJourney = distCovered / _journeyLength;

        transform.position = Vector3.Lerp(_startMarker, _endMarker, fracJourney);   
    }
    
    void SlerpCamera()
    {
        Transform trf = transform;
        Quaternion rotation = trf.rotation;
        Vector3 targetPosition = camTarget.transform.position;
        targetPosition.y += slerpOffset;

        Vector3 position = trf.position;
        rotation = Quaternion.Slerp(rotation,
            Quaternion.LookRotation(targetPosition - position),
            Time.deltaTime * slerpSpeed);
        transform.rotation = rotation;
        if (Math.Abs(_lastCamY - position.y) < 0.001f)
        {
            StartCoroutine(Finish());
        }
        _lastCamY = position.y;
    }

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(sceneChangeDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
