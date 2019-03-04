using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//-----------------------------------------------------------------------------
/// <summary>
/// Switches out ui elements to match the current selected hero, including 
/// moving the selection arrow 
/// </summary>
//-----------------------------------------------------------------------------
public class HeroUIManager : MonoBehaviour {

    // Reference to hero controller
    [SerializeField] private HeroController _heroController;

    // References to ui elements 
    // Selection arrow
    // HP bar and PP bar references
    [SerializeField] private StatBar HPBar;
    [SerializeField] private StatBar PPBar;
    [SerializeField] private StatBar Ability1UI;
    [SerializeField] private StatBar Ability2UI;
    [SerializeField] private Text HeroNameText;

    AbilityManager _heroAbilities;  // Reference to current hero's abilities
    BasicStatManager _heroStats;    // Ref to current hero's basic stats

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        EventManager.Register("OnHeroSwitch", OnHeroSwitch);
    }

    private void OnDisable()
    {
        EventManager.Unregister("OnHeroSwitch", OnHeroSwitch);
    }
    // Use this for initialization
    void Start () {
        OnHeroSwitch(_heroController.currentHero.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        UpdateUI();
	}

    
    // Show current hero's stats 
    void UpdateUI()
    {
        // Update hp and pp bars
        if (_heroStats != null)
        {
            HPBar.SetBarValue(_heroStats.GetHP(), _heroStats.MaxHP);
            PPBar.SetBarValue(_heroStats.GetPP(), _heroStats.MaxPP);
        }

        // Update abilities
        if (_heroAbilities != null)
        {
            Ability1UI.SetBarValue(_heroAbilities.Ability1.GetCurrentCooldown(), _heroAbilities.Ability1.Cooldown);
            Ability2UI.SetBarValue(_heroAbilities.Ability2.GetCurrentCooldown(), _heroAbilities.Ability2.Cooldown);
        }
    }

    // Update 
    void OnHeroSwitch(GameObject hero, GameObject reciever = null)
    {
        _heroAbilities = hero.GetComponent<AbilityManager>();
        _heroStats = hero.GetComponent<BasicStatManager>();
        
        // Setup ability ui
        if (_heroAbilities != null)
        {
            Ability1UI.Enable();
            Ability2UI.Enable();
            Ability1UI.SetUIElements(_heroAbilities.Ability1.AbilityName, _heroAbilities.Ability1.UISprite);
            Ability2UI.SetUIElements(_heroAbilities.Ability2.AbilityName, _heroAbilities.Ability2.UISprite);
        }
        else
        {
            Ability1UI.Disable();
            Ability2UI.Disable();
        }

        if (_heroStats != null)
        {
            HeroNameText.text = _heroStats.name;
        }
        // Move marker to current hero

    }
}
