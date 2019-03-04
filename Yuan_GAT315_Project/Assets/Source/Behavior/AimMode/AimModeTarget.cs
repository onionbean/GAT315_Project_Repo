using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_AimModeTarget", menuName = "ScriptableObjects/AbilitySlotObjects/AimModeTarget", order = 3)]
public class AimModeTarget : AbilityAimMode {

    // get target and return its position
    public override bool AimModeTick(ref Vector2 aimPosition, string axis, GameObject target = null)
    {
        if (target != null)
        {
            aimPosition = target.transform.position;
            return true;
        }

        return false;
    }
}
