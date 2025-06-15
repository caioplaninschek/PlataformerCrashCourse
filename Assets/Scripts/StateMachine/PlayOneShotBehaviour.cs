using UnityEngine;
using UnityEngine.Rendering;

public class PlayOneShotBehaviour : StateMachineBehaviour
{
    public AudioClip soudToPlay; // Áudio a ser reproduzido quando o estado é ativado
    public float volume = 1f; // Volume do áudio a ser reproduzido
    public bool playOnEnter = true, playOnExit = false, playAfterDelay = false; // Flags para controlar quando o áudio deve ser reproduzido

    // Temporizador para controlar o atraso na reprodução do áudio (controlar o delay)
    public float playDelay = 0.25f; // Atraso em "1/4 de 1 segundo" antes de reproduzir o áudio
    private float timeSinceEntered = 0; // Tempo desde que o estado foi ativado
    private bool hasDelayedSoundPlayed = false; // Flag para verificar se o áudio já foi reproduzido com atraso

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnEnter)
        {
            AudioSource.PlayClipAtPoint(soudToPlay, animator.gameObject.transform.position, volume);
        }

        timeSinceEntered = 0f;
        hasDelayedSoundPlayed = false; 
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playAfterDelay && !hasDelayedSoundPlayed)
        {
            timeSinceEntered += Time.deltaTime;

            if (timeSinceEntered >= playDelay)
            {
                AudioSource.PlayClipAtPoint(soudToPlay, animator.gameObject.transform.position, volume);
                hasDelayedSoundPlayed = true; // Marca que o áudio com atraso já foi reproduzido    
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnExit)
        {
            AudioSource.PlayClipAtPoint(soudToPlay, animator.gameObject.transform.position, volume);
        }

    }
}
