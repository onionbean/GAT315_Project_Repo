using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif 

[CreateAssetMenu(fileName = "SO_AimModeAiming", menuName = "ScriptableObjects/AbilitySlotObjects/AimModeAiming", order = 2)]
public class AimModeAiming : AbilityAimMode {

    public override bool AimModeTick(ref Vector2 aimPosition, string axis, GameObject target = null)
    {
        // Turn mouse cursor to desired sprite
        // True when we decide on a location
        if (InputUtility.GetAxisDown(axis, true))
        {
            aimPosition = Input.mousePosition;
            return true;
        }

        // false if we haven't decided on a location yet 
        return false;
    }
}
