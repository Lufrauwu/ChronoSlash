using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFinish : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        layerIndex = animator.GetLayerIndex("Pause");
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(layerIndex);
        if (currentState.IsName("Pause"))
        {
            // Checa si la animación llegó al final.
            if (currentState.normalizedTime >= 1.0f)
            {
                Time.timeScale = 0;
                // Aquí puedes ejecutar cualquier lógica que necesites.
            }
        }
    }

    
}
