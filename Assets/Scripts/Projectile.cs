using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("Dano causado pelo projétil")]
    public int damage = 5; // Dano causado pelo projétil

    [Tooltip("Velocidade do projétil")]
    public Vector2 moveSpeed = new Vector2(3f, 0); // Velocidade do projétil

    [Tooltip("Força do knockback aplicada ao atingir um alvo")]
    public Vector2 knockback = new Vector2(0, 0); // Força do knockback aplicada ao atingir um alvo

    Rigidbody2D rb; // Referência ao Rigidbody2D do projétil

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtém o componente Rigidbody2D do projétil
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
        // Define a velocidade do Rigidbody2D com base na velocidade do projétil e na escala local   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y); // Verifica a direção do ataque e ajusta o knockback
            damageable.Hit(damage, deliveredKnockback); // Aplica dano, não importa se foi bem-sucedido
            Destroy(gameObject); // Sempre destrói o projétil ao colidir com inimigo
        }
        else
        {
            Destroy(gameObject); // Destrói ao colidir com qualquer outra coisa (ex: Ground)
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
