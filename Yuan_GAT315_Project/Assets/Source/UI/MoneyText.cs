using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyText : MonoBehaviour {
    // Text ref
    [SerializeField] private Text text;

    private void Awake()
    {
        if (text == null)
            text = GetComponent<Text>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        text.text = "$" + MoneyManager.GetMoney();
	}
}
