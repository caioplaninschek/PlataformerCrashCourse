using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit; // Evento que é chamado quando o objeto recebe dano
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged; // Evento que é chamado quando a saúde do objeto muda

    Animator animator; // Referência ao Animator do objeto para controlar animações

    [SerializeField]
    [Tooltip("Quantidade de dano que o objeto pode receber antes de ser destruído")]
    private int _maxHealth = 100; // Quantidade máxima de vida do objeto
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    [Tooltip("Quantidade atual de vida do objeto")]
    private int _health = 100; // Quantidade atual de vida do objeto

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            healthChanged?.Invoke(_health, _maxHealth); // Invoca o evento de mudança de saúde, passando a saúde atual e máxima

            // Atualiza a vida do objeto e verifica se ele deve ser destruído
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    [Tooltip("Indica se o objeto está vivo")]
    private bool _isAlive = true; // Indica se o objeto está vivo  

    [SerializeField]
    [Tooltip("Indica se o objeto é invencível")]
    private bool isInvincible = false; // Indica se o objeto é invencível
    private float timeSinceHit = 0f; // Tempo desde o último dano recebido

    [SerializeField]
    [Tooltip("Tempo de invencibilidade após receber dano")]
    public float invincibilityTime = 0.25f; // Tempo de invencibilidade após receber dano;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive: " + value);

            if (value == false)
            {
                damageableDeath.Invoke(); // Invoca o evento de morte se o objeto não estiver mais vivo
            }
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

    void Awake()
    {
        // Obtém o componente Animator do objeto
        animator = GetComponent<Animator>();
    }

    // Update é chamado uma vez por frame
    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false; // Desativa a invencibilidade após o tempo definido
                timeSinceHit = 0f; // Reseta o tempo desde o último dano
            }

            timeSinceHit += Time.deltaTime; // Atualiza o tempo desde o último dano

        }
    }

    // Retorna se o objeto damageable tomou dano ou não
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {

            Health -= damage; // Reduz a vida do objeto pelo dano recebido
            isInvincible = true; // Ativa a invencibilidade temporária

            animator.SetTrigger(AnimationStrings.hitTrigger); // Ativa a animação de hit
            LockVelocity = true; // Bloqueia a velocidade do objeto durante o hit
            damageableHit?.Invoke(damage, knockback); // Invoca o evento de dano, passando o dano e o knockback
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);


            return true; // Retorna verdadeiro indicando que o dano foi aplicado           
        }

        return false; // Retorna falso indicando que o dano não foi aplicado (objeto não está vivo ou é invencível)
    }

    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0); // Calcula a quantidade máxima de cura possível sem ultrapassar o máximo
            int actualHeal = Mathf.Min(maxHeal, healthRestore); // Calcula a cura real, garantindo que não ultrapasse o máximo
            Health += actualHeal; // Restaura a vida do objeto

            CharacterEvents.characterHealed(gameObject, actualHeal); // Invoca o evento de cura, passando o objeto e a quantidade de cura
            return true; // Retorna verdadeiro indicando que a cura foi aplicada
        }
        return false; // Retorna falso indicando que a cura não foi aplicada (objeto não está vivo ou já está com vida máxima)
    }
}
