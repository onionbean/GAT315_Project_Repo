using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDeath : MonoBehaviour {
    [SerializeField] float Time = 1;

    private void Update()
    {
        Destroy(gameObject, Time);
    }
}
