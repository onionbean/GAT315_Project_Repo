using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M))
            MoneyManager.AddMoney(1000);
        if (Input.GetKey(KeyCode.M) && Input.GetKey(KeyCode.R))
            MoneyManager.ResetMoney();
	}
}
