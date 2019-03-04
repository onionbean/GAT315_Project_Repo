using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif 

[CreateAssetMenu(fileName = "SO_AimModeNone", menuName = "ScriptableObjects/AbilitySlotObjects/AimModeNone", order = 1)]
public class AimModeNone : AbilityAimMode {

    public override bool AimModeTick(ref Vector2 aimPosition, string axis, GameObject target = null)
    {
        return true; // Always true
    }
}
