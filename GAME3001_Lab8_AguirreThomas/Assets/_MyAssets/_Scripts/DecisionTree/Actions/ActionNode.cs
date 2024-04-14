using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ActionNode : TreeNode
{
    public GameObject Agent { get; set; }

    public Component AgentScript { get; set; }

    public ActionNode()
    {
        isLeaf = true;
        Agent = null;
        AgentScript = null;
    }

    public void SetAgent(GameObject agent, Type script)
    {
        Agent = agent;
        AgentScript = agent.GetComponent(script);
    }

    public abstract void Action(); // Abstract method for tree.
}
