using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    #region Variáveis Públicas e SerializeFields
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3.25f;
    public float jumpImpulse = 10f;

    [SerializeField]
    [Tooltip("Indica se o personagem está se movendo")]
    private bool _isMoving = false;

    [SerializeField]
    [Tooltip("Indica se o personagem está correndo")]
    private bool _isRunning = false;

    [SerializeField]
    [Tooltip("Indica se o personagem está virado para a direita")]
    public bool _isFacingRight = true;
    #endregion

    #region Variáveis Privadas de Componentes
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable; // Referência ao componente Damageable para aplicar dano
    Rigidbody2D rb;
    Animator animator;
    #endregion

    #region Propriedades
    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    // Se o personagem está no chão, usa a velocidade normal de caminhada ou corrida
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        // Velocidade de movimento Aéreo
                        return airWalkSpeed;
                    }
                }
                else
                {   // velocidade do status Idle do Jogador, é 0
                    return 0f;
                }
            }
            else
            {
                // Movimento não permitido, retorna 0 (trava do movimento)
                return 0f;
            }
        }
    }

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        set
        {
            if (_isFacingRight != value)
            {
                // Vire a escala local para fazer o personagem olhar na direção oposta
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        { 
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }
    #endregion

    #region Métodos Unity
    private void Awake()
    {
        transform.localScale *= new Vector2(-1, 1); // Inicia o personagem virado para a esquerda;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        if (!damageable.LockVelocity)
        {
            rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);    
        }
        
        animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);
    }
    #endregion

    #region Métodos de Input
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            // Se o personagem está vivo, permite movimento
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            // Se o personagem não está vivo, não permite movimento
            IsMoving = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            _isRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // A fazer depois: Implementar checagem se está vivo também
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }
    #endregion

    #region Métodos Utilitários
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x < 0 && !IsFacingRight)
        {
            // Virado para a esquerda
            IsFacingRight = true;
        }
        else if (moveInput.x > 0 && IsFacingRight)
        {
            // Virado para a direita
            IsFacingRight = false;
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y); // aplica o knockback na direção do dano
    }
    #endregion
}