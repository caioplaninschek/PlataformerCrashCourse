using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements.Experimental;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))] // Garante que o GameObject tem Rigidbody2D e TouchingDirections

public class Knight : MonoBehaviour
{
    [Header("Configurações do Cavaleiro")]

    [Tooltip("Velocidade que o cavaleiro anda")]
    public float walkAcceleration = 50f; // Velocidade que o cavaleiro anda

    [Tooltip("Velocidade máxima que o cavaleiro pode atingir")]
    public float maxSpeed = 3.8f; // Velocidade máxima que o cavaleiro pode atingir 

    [Tooltip("Taxa de desaceleração do cavaleiro ao parar")]
    public float walkStopRate = 0.05f; // Taxa de desaceleração do cavaleiro ao parar
  

    [Tooltip("Zona de detecção de ataque do cavaleiro")]
    public DetectionZone attackZone; // Referência à zona de detecção de ataque do cavaleiro

    [Tooltip("Zona de detecção do chão do cavaleiro")]
    public DetectionZone cliffDetectionZone; // Referência à zona de detecção do chão do cavaleiro



    [Header("Referências do Cavaleiro")]
    [Tooltip("Referência ao Rigidbody2D do cavaleiro")]
    Rigidbody2D rb; // Referência ao Rigidbody2D do cavaleiro
    TouchingDirections touchingDirections; // Referência ao script TouchingDirections para verificar as direções de contato
    Animator animator; // Referência ao Animator do cavaleiro para controlar as animações
    Damageable damageable; // Referência ao script Damageable para aplicar dano

    public enum WalkableDirection { Right, Left } // Awake é chamado quando o script é carregado

    private WalkableDirection _walkDirection; // Direção de caminhada do cavaleiro, pode ser esquerda ou direita
    private Vector2 walkDirectionVector = Vector2.right; // Vetor que representa a direção de caminhada do cavaleiro


    public WalkableDirection WalkDirection
    {
        get
        {
            return _walkDirection; // Retorna a direção de caminhada do cavaleiro
        }
        set
        {
            if (_walkDirection != value)
            {
                // Inverte a escala do cavaleiro para mudar a direção
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right; // Define a direção de caminhada para a direita
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left; // Define a direção de caminhada para a esquerda
                }

            }
            _walkDirection = value; // Define a direção de caminhada do cavaleiro
        }
    }

    public bool _hasTarget = false; // Informa que o cavaleiro inicialmente não tem alvo

    public bool HasTarget
    {
        get
        {
            return _hasTarget; // Retorna se o cavaleiro tem um alvo na zona de detecção de ataque
        }
        set
        {
            _hasTarget = value; // Define se o cavaleiro tem um alvo na zona de detecção de ataque
            animator.SetBool(AnimationStrings.hasTarget, value); // Atualiza o Animator com o estado de alvo
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove); // Retorna se o cavaleiro pode se mover baseado no Animator
        }
    }

    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.attackCooldown); // Retorna o tempo de recarga do ataque baseado no Animator
        }
        private set
        {
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0)); // Define o tempo de recarga do ataque no Animator, garantindo que não seja negativo
        }
    }

    // Awake é chamado quando o script é carregado
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtém o componente Rigidbody2D do cavaleiro
        touchingDirections = GetComponent<TouchingDirections>(); // Obtém o componente TouchingDirections do cavaleiro
        animator = GetComponent<Animator>(); // Obtém o componente Animator do cavaleiro
        damageable = GetComponent<Damageable>(); // Obtém o componente Damageable do cavaleiro
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0; // Verifica se há algum alvo na zona de detecção de ataque

        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime; // Diminui o tempo de recarga do ataque
        }

    }

    private bool wasOnWall = false;

    private float flipCooldown = 0.2f;
    private float lastFlipTime = -1f;

    // FixedUpdate é chamado a uma taxa fixa, ideal para física
    private void FixedUpdate()
    {
        // Detecta transição: não estava na parede, agora está
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall && !wasOnWall)
        {
            if (Time.time - lastFlipTime > flipCooldown)
            {
                FlipDirection();
                lastFlipTime = Time.time;
            }
        }
        wasOnWall = touchingDirections.IsOnWall;

        if (!damageable.LockVelocity)
        {
            if (CanMove && touchingDirections.IsGrounded)
            {
                rb.linearVelocity = new Vector2(
                    Mathf.Clamp(rb.linearVelocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed),
                    rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, walkStopRate), rb.linearVelocity.y);
            }
        }
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left; // Inverte a direção para a esquerda
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right; // Inverte a direção para a direita
        }
        else
        {
            // Se a direção de caminhada não for válida, exibe um erro no console
            Debug.LogError("Current walkable direction is not set to legal values of left or right");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y); // Aplica o knockback na direção do dano
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded)
        {
            FlipDirection(); // Se o cavaleiro estiver no chão e detectar um penhasco, inverte a direção
        }
    }
}
