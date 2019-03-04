using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach to buttons that have a bribe, if players don't have the right amount this button is greyed out
/// </summary>
public class PayButton : MonoBehaviour {
    [SerializeField] private int PayAmount = 500;
    [SerializeField] Button ButtonRef;
    [SerializeField] Text TextRef;
    [SerializeField] bool DisableOnPay = false;

    bool paid = false;
	// Use this for initialization
	void Start () {
        if (ButtonRef == null)
            ButtonRef = GetComponent<Button>();

        if (TextRef == null && ButtonRef != null)
            TextRef = ButtonRef.GetComponentInChildren<Text>();

        if (TextRef != null)
            TextRef.text += "($" + PayAmount + ")";
	}
	
	// Update is called once per frame
	void Update () {
        if (MoneyManager.GetMoney() < PayAmount)
            ButtonRef.interactable = false;
        else
            ButtonRef.interactable = true;
	}

    public void Bribe()
    {
        MoneyManager.PayMoney(PayAmount);
        paid = true;
        if (DisableOnPay)
        {
            ButtonRef.GetComponent<Image>().enabled = false;
            ButtonRef.enabled = false;
            TextRef.enabled = false;

        }
    }
}
