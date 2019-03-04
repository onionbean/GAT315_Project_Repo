using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines stat ui behavior for bars / radial fills 
/// </summary>
public class StatBar : MonoBehaviour {
    // If using radial
    [SerializeField] private bool IsRadial = false;

    // Current value slider
    [Range(0, 1)]
    [SerializeField] public float NormalizedValue = 1;

    // Reference to text component
    [SerializeField] Text StatText;
    [SerializeField] Text StatName;

    [SerializeField] private Image StatImage;

    [SerializeField] private Image RadialImageRef;

    [SerializeField] private bool ShowOnlyCurrentVal = false;

    private RectTransform _rectTrans;

    private void Awake()
    {
        if (IsRadial && RadialImageRef == null)
        {
            RadialImageRef = GetComponent<Image>();
        }

        _rectTrans = GetComponent<RectTransform>();
    }
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        UpdateUI();
	}

    public void SetBarValue(float current, float max)
    {
        if (max == 0)
            NormalizedValue = 1;
        else
            NormalizedValue = current / max;

        NormalizedValue = Mathf.Clamp(NormalizedValue, 0, 1);
        if (StatText != null)
        {
            if (ShowOnlyCurrentVal)
                StatText.text = (int)current + "";
            else
                StatText.text = (int)current + "/" + max;
        }
    }

    public void SetUIElements(string name, Sprite image)
    {
        if (StatName != null && name != null)
            StatName.text = name;

        if (StatImage != null)
            StatImage.sprite = image;
    }

    void UpdateUI()
    {
        // If not using radial, set scale, otherwise get image and set fill amount
        if (IsRadial)
        {
            RadialImageRef.fillAmount = NormalizedValue;
        }
        // Set scale when not using radial
        else
        {
            _rectTrans.localScale = new Vector3(NormalizedValue, _rectTrans.localScale.y, _rectTrans.localScale.z);
        }
    }

    public void Disable()
    {
        // disable sprites 
        Image[] images = GetComponentsInChildren<Image>();
        Text[] texts = GetComponentsInChildren<Text>();
        GameUtility.SetVisible(ref images, false);
        GameUtility.SetVisible(ref texts, false);
    }

    public void Enable()
    {
        // disable sprites 
        Image[] images = GetComponentsInChildren<Image>();
        Text[] texts = GetComponentsInChildren<Text>();
        GameUtility.SetVisible(ref images, true);
        GameUtility.SetVisible(ref texts, true);
    }
}
