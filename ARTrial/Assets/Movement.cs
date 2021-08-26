using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 forward;

    // Start is called before the first frame update
    void Start()
    {
        forward = new Vector3(-1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * forward);
    }
}
