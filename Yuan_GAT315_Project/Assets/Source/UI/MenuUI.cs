using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//[System.Serializable]
//public class 
public class MenuUI : MonoBehaviour {
    [Tooltip("Whether this menu is disabled at the start")]
    public bool DisabledAtStart = false;

    [Tooltip("The time it takes to transition to new scene")]
    public float SceneTransitionTime = 1;

    [Tooltip("Whether we get out of this menu by pressing ESC ")]
    public bool QuickExitMenu = true;
    public MenuUI DefaultExitMenu;

    [Tooltip("Whether we pause when this menu is open")]
    public bool PauseWhenOpen = false;

    [Tooltip("Whether mouse is enabled ")]
    public bool EnableMouse = true;

    [Header("UI TRANSITION ANIMATIONS")]
    [Tooltip("In transition animation")]
    public Animation InTransitionAnimation;
    public float InTransitionTime = 1;

    [Tooltip("Out transition animation")]
    public Animation OutTransitionAnimation;
    public float OutTransitionTime = 1;

    private bool _transitioning = false;
    [SerializeField] private Canvas _canvas;
    private void Awake()
    {
        if (_canvas == null)
            _canvas = GetComponent<Canvas>();
    }

    // Use this for initialization
    void Start ()
    {
        if (!DisabledAtStart)
            InitMenu();
        else
            _canvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (_canvas != null && _canvas.enabled)
        {
            if (Input.GetButtonDown("Cancel") && QuickExitMenu && DefaultExitMenu != null)
            {
                // Quick exit menu
                if (DefaultExitMenu != null)
                    OpenMenu(DefaultExitMenu);
                else
                    StartCoroutine(OutTransition());
            }
        }
	}

    
    // Load a level from the button click 
    public void OnLevelButton(string level)
    {
        StartCoroutine(SceneTransition(level));
    }

    // Transition to another menu
    public void OpenMenu(MenuUI otherMenu)
    {
        StartCoroutine(OutTransition(otherMenu));
    }

    // Open a menu without disabling current menu
    public void OpenMenuLayered(MenuUI otherMenu)   
    {
        StartCoroutine(OutTransition(otherMenu, true));
    }

    public void InitMenu()
    {
        if (_canvas != null)
            _canvas.enabled = true;
        if (EnableMouse)
            Cursor.visible = true;
        else
            Cursor.visible = false;

        // If we are to pause
        if (PauseWhenOpen)
            EventManager.TriggerEvent("Pause", gameObject);

        StartCoroutine(InTransition());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // In transition process 
    IEnumerator InTransition()
    {
        // In transition animation
        // wait 
        yield return null;
    }

    // Out transition process
    IEnumerator OutTransition(MenuUI otherMenu = null, bool layered = false)
    {
        if (PauseWhenOpen)
            EventManager.TriggerEvent("Unpause", gameObject);

        // Out transition animation
        // wait 
        yield return new WaitForSecondsRealtime(OutTransitionTime);

        if (otherMenu != null)
        {
            // enable other menu    
            otherMenu.GetComponent<Canvas>().enabled = true;

            // Init that menu 
            otherMenu.InitMenu();
        }
        // disable self
        if (_canvas != null && !layered)
            _canvas.enabled = false;

        yield return null;
    }

    // Coroutine to transition
    IEnumerator SceneTransition(string level)
    {
        _transitioning = true;

        // Animate for fade out
        yield return new WaitForSecondsRealtime(SceneTransitionTime);
        SceneManager.LoadScene(level);

        _transitioning = false;
        yield return null;
    }
}
