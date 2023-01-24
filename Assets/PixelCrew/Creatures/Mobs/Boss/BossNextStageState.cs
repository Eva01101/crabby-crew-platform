using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components.GoBased;
using PixelCrew.Creatures.Mobs.Boss;
using UnityEngine;

public class BossNextStageState : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var spawner = animator.GetComponent<CircularProjectileSpawner>();
        spawner.Stage++;

        var changeLight = animator.GetComponent<ChangeLightsComponent>();
        changeLight.SetColor();
    }
}
//сначала анимация проигрывается, затем освещение