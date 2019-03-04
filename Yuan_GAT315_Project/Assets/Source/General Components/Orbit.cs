using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {
    // Point to orbit 
    [SerializeField] public GameObject PivotPoint;

    [SerializeField] public float RotationSpeed = 5;

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.RotateAround(PivotPoint.transform.position, Vector3.forward, RotationSpeed * Time.deltaTime);
    }
}
