using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveableObject
{
    // Nudge the object in dir
    // Returns true if successful, otherwise, false
    bool Shift(int absDir);
}
