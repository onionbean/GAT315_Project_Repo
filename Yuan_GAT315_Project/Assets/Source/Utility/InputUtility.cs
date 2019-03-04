using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputUtility
{ 

    public static bool GetAxisDown(string axis, bool positive)
    {
        if (positive)
            return Input.GetButtonDown(axis) && Input.GetAxis(axis) > 0;

        return Input.GetButtonDown(axis) && Input.GetAxis(axis) < 0;
    }
}
