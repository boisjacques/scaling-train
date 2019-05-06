using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowingCam : MonoBehaviour
{
    public GameObject target;
    public float x;
    public float y;
    public bool birdcam;
    public float sceneChangeDelay = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = transform.position;
        x = position.x;
        y = position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Transform trf = transform;
        Vector3 position = trf.position;
        if (position.z > 252 && birdcam)
        {
            enabled = false;
            GetComponent<LoweringCam>().enabled = true;
        }

        if (position.z > 110 && !birdcam)
        {
            StartCoroutine(Finish());
        }


        Vector3 targetPosition = target.transform.position;
        position.z = targetPosition.z;
        position.x = x;
        position.y = y;

        trf.position = position;
    }
    
    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(sceneChangeDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}