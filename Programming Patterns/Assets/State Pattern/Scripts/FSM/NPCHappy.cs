using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

public class HappyState : INPCState
{
    private bool doneBeingHappy = false;
    private float happyTime = 0;
    public INPCState DoState(NPCSearch_ClassBased npc)
    {

        BeHappy(npc);

        if (happyTime > 3)
        {
            happyTime = 0;
            return npc.wanderState;
        }
        else
            return npc.happyState;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void BeHappy(NPCSearch_ClassBased  npc)
    {
        happyTime += Time.deltaTime;
        Debug.Log("I'm so happy! I got a critter!");
    }
}
