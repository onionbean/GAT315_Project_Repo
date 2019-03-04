using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Running money manager for the player
public class MoneyManager : MonoBehaviour {

    private static MoneyManager mmanager;
    public static MoneyManager instance
    {
        get
        {
            // If current instance doesn't exist, find it 
            if (!mmanager)
            {
                mmanager = FindObjectOfType(typeof(MoneyManager)) as MoneyManager;

                // If found init
                if (mmanager)
                    mmanager.Init();
                else
                    Debug.LogError("No active Money Manager attached to an object in the scene!");
            }

            return mmanager;
        }
    }
    // Starting money 
    [SerializeField] private int _startingMoney = 0;

    // Current money 
    [SerializeField] public int CurrentMoney = 0;

    
    void Init()
    {
        CurrentMoney = PlayerPrefs.GetInt("PlayerMoney");
    }

	// Use this for initialization
	void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void ResetMoney()
    {
        instance.CurrentMoney = 0;
        SaveMoney();
    }

    public static int GetMoney()
    {
        return instance.CurrentMoney;
    }

    public static void AddMoney(int amount)
    {
        instance.CurrentMoney += amount;
        SaveMoney();
    }

    public static bool PayMoney(int cost)
    {
        if (cost > instance.CurrentMoney)
        {
            return false;
        }

        instance.CurrentMoney -= cost;

        SaveMoney();

        return true;
    }

    public static void SaveMoney()
    {
        PlayerPrefs.SetInt("PlayerMoney", instance.CurrentMoney);
    }
}
