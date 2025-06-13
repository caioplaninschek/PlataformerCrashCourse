using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Collider2D attackCollider; // Referência ao Collider2D do ataque
    [SerializeField]
    [Tooltip("Dano causado pelo ataque")]
    public int attackDamage = 10; // Dano causado pelo ataque, pode ser ajustado no Inspector
    public Vector2 knockback = Vector2.zero; // Status default da "Força" do knockback

    private void Awake()
    {
        attackCollider = GetComponent<Collider2D>(); // Obtém o Collider2D do objeto
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        // Vê se o objeto colidido tem um componente Damageable
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2 (-knockback.x, knockback.y); // Verifica a direção do ataque e ajusta o knockback

            // Se o objeto colidido é um Damageable, aplica dano
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback); // Aplica dano e knockback ao objeto colidido

            if (gotHit)
            {
                Debug.Log(collision.name + "Dano aplicado: " + attackDamage); // Log para verificar o dano aplicado
            }
           
        }
    }
}
