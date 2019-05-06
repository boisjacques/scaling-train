using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForestSpawner : MonoBehaviour
{
    public GameObject[] treeModels;

    public GameObject[] groundDecorationModels;

    public float depth = 100;

    public float width = 20;

    public bool addColliders = true;

    public bool shallowEdge;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < width; i += 3)
        {
            for (int j = 0; j < depth; j += 3)
            {
                int treeIndex;
                if (i >= width - 9  && shallowEdge)
                {
                    treeIndex = 0;
                }
                else
                {
                    treeIndex = Random.Range(0, treeModels.Length);
                }

                float x = Random.Range((float) i, (float) i + 3f);
                float z = Random.Range((float) j, (float) j + 3f);
                if (!(x > 28 && x < 32 && z > 251 && z < 261))
                {
                    GameObject tree = Instantiate(treeModels[treeIndex]);
                    tree.transform.position = new Vector3(x, 2.7f, z);
                    if (addColliders) tree.AddComponent<MeshCollider>();
                    tree.transform.parent = transform;
                }

                for (int k = 0; k < groundDecorationModels.Length; k++)
                {
                    x = Random.Range((float) i, (float) i + 3f);
                    z = Random.Range((float) j, (float) j + 3f);
                    GameObject groundDecoration = Instantiate(groundDecorationModels[k]);
                    groundDecoration.transform.position = new Vector3(x, 0.25f, z);
                    groundDecoration.transform.parent = transform;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}