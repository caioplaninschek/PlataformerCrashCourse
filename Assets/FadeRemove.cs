using UnityEngine;

public class FadeRemove : StateMachineBehaviour
{
    public float fadeTime = 0.5f; // Duração do fade
    private float timeElapsed = 0f; // Tempo decorrido desde o início do fade
    SpriteRenderer spriteRenderer; // Referência ao SpriteRenderer do GameObject
    GameObject objToRemove; // Referência ao GameObject que será removido
    Color startColor; // Cor inicial do SpriteRenderer

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0f; // Reseta o tempo decorrido
        spriteRenderer = animator.GetComponent<SpriteRenderer>(); // Obtém o SpriteRenderer do GameObject
        startColor = spriteRenderer.color; // Armazena a cor inicial do SpriteRenderer
        objToRemove = animator.gameObject; // Obtém o GameObject associado ao Animator
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed += Time.deltaTime; // Atualiza o tempo decorrido

        float newAlpha = startColor.a * (1 - (timeElapsed / fadeTime)); // Calcula o novo valor de alpha baseado no tempo decorrido e na duração do fade


        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha); // Aplica o fade na cor do SpriteRenderer

        if (timeElapsed > fadeTime)
        {
            Destroy(objToRemove); // Remove o GameObject após o tempo de fade
        }
    }
}

