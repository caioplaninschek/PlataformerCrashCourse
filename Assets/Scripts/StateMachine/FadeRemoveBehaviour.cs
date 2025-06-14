using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.Animations;

public class FadeRemoveBehaviour : StateMachineBehaviour
{
    [Header("Configurações de Fade")]

    [Tooltip("Tempo total de fade (em segundos)")]
    public float fadeTime = 0.5f;

    [Tooltip("Tempo de espera antes de iniciar o fade (em segundos)")]
    public float fadeDelay = 0.0f;

    private float timeElapsed = 0f;
    private float fadeDelayElapsed = 0f;

    SpriteRenderer spriteRenderer;
    GameObject objToRemove;
    Color startColor;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0f;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        objToRemove = animator.gameObject;
    }


    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (fadeDelay > fadeDelayElapsed)
        {
            fadeDelayElapsed += Time.deltaTime;
        }
        else
        {
            timeElapsed += Time.deltaTime;

            float newAlpha = startColor.a * (1 - (timeElapsed / fadeTime));

            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

            if (timeElapsed > fadeTime)
            {
                Destroy(objToRemove);
            }
        }
    }
}