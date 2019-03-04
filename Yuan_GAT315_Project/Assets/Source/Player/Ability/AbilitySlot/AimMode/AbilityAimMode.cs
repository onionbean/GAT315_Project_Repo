using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Aiming mode for the ability */
public abstract class AbilityAimMode : ScriptableObject {
    public abstract bool AimModeTick(ref Vector2 aimPosition, string axis, GameObject target = null); 
}
