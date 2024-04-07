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
        if(Agent.GetComponent<AgentObject>().state != ActionState.ATTACK)
        {
            Debug.Log("Starting " + name);
            AgentObject ao = Agent.GetComponent<AgentObject>();
            ao.state = ActionState.ATTACK;

            //custom enter action
            if (AgentScript is CloseCombatEnemy cce)
            {

            }
            else if (AgentScript is RangeCombatEnemy rce)
            {

            }
        }

        // Every frame
        Debug.Log("Performing " + name);
    }
}
