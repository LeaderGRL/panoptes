using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f) * Time.deltaTime * 0.2f;
    }
}

