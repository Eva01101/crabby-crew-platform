using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components.GoBased;
using UnityEngine;

public class BossShootState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var spawner = animator.GetComponent<CircularProjectileSpawner>();
        spawner.LaunchProjectiles();
    }
   
}
