using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spin an object at a certain rate
public class Spin : MonoBehaviour
{
    [SerializeField] public float SpinSpeed = 10;

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    private void FixedUpdate()
    {
        Rotate();
    }

    // Rotate around z axis 
    void Rotate()
    {
        transform.Rotate(Vector3.forward, SpinSpeed * Time.deltaTime);
    }
}
