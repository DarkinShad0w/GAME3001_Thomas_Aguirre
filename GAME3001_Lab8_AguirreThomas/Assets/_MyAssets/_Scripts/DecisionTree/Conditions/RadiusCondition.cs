using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RadiusCondition : ConditionNode
{
    public bool IsWithinRadius { get; set; }
     
    public RadiusCondition()
    {
        name = "Radius Condition";
        IsWithinRadius = false;
        
    }

    public override bool Condition()
    {
        Debug.Log("Checking " + name);
        return IsWithinRadius;
    }
}
