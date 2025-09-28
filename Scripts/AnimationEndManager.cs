using UnityEngine;

public class AnimationStateEnd : StateMachineBehaviour
{
    // This is called when the animation finishes
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the GameObject associated with this animator
        GameObject gameObject = animator.gameObject;

        // Check if the GameObject has the "Player" tag
        if (!gameObject.CompareTag("Player"))
        {
            // Call your static function when the animation finishes
        }
    }
}
