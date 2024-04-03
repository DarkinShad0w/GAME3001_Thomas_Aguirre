using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackAction : ActionNode
{
    public AttackAction()
    {
        name = "Attack Action";
    }

    public override void Action()
    {
        // Enter Functionality for action
        if(Agent.GetComponent<Starship>().state != ActionState.ATTACK)
        {
            Debug.Log("Starting " + name);
            Starship ss = Agent.GetComponent<Starship>();
            ss.state = ActionState.ATTACK;

            //custom enter action
        }

        // Every frame
        Debug.Log("Performing " + name);
    }
}
