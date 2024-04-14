using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CloseCombatCondition : ConditionNode
{
    public bool IsWithinCombatRange { get; set; }

    public CloseCombatCondition()
    {
        name = "Close Combat Condition";
        IsWithinCombatRange = false;
    }

    public override bool Condition()
    {
        Debug.Log("Checking " + name);
        return IsWithinCombatRange;
    }
}
