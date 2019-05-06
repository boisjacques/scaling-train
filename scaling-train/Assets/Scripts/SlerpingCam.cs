using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlerpingCam : MonoBehaviour
{
    private float _lastCamY = Single.MaxValue;

    public GameObject camTarget;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Transform trf = transform;
        Quaternion rotation = trf.rotation;

        Vector3 position = trf.position;
        rotation = Quaternion.Slerp(rotation,
            Quaternion.LookRotation(camTarget.transform.position - position),
            Time.deltaTime * 1);
        transform.rotation = rotation;
        if (Math.Abs(_lastCamY - position.y) < 0.001f)
        {
            // StartCoroutine(Finish());
        }
        _lastCamY = position.y;
    }

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}