using System.Collections;
using Monsters;
using UnityEngine;

namespace UI
{
    public class JumpscareDisplay : MonoBehaviour
    {
        [SerializeField] private Animator jumpscareAnimator;
        [SerializeField] private GameObject deathPanel;
        
        private void OnValidate()
        {
            if (jumpscareAnimator == null)
                jumpscareAnimator = GetComponent<Animator>();
        }
        
        private void OnEnable()
        {
            MonsterEvents.OnMonsterKill += PlayJumpscareAnimation;
        }
        
        private void OnDisable()
        {
            MonsterEvents.OnMonsterKill -= PlayJumpscareAnimation;
        }
        
        private void PlayJumpscareAnimation(string animationName)
        {
            if (jumpscareAnimator != null)
            {
                jumpscareAnimator.Play(animationName);
                StartCoroutine(WaitForAnimation(animationName));
            }
        }
        
        private IEnumerator WaitForAnimation(string animationName)
        {
            // Wait for the animation to complete
            AnimatorStateInfo state = jumpscareAnimator.GetCurrentAnimatorStateInfo(0);

            // Wait until it starts playing the correct state
            while (!state.IsName(animationName))
            {
                yield return null;
                state = jumpscareAnimator.GetCurrentAnimatorStateInfo(0);
            }

            yield return new WaitForSeconds(state.length);
    
            deathPanel.SetActive(true);
        }
    }
}
