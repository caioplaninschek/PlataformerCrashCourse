using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Tooltip("Quantidade de vida restaurada ao coletar o item")]
    public int healthRestore = 20; // Quantidade de vida restaurada ao coletar o item
    public UnityEngine.Vector3 spinRotationSpeed = new UnityEngine.Vector3(0, 180, 0); // Velocidade de rotação do item de coleta

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Rotaciona o item de coleta continuamente
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable)
        {
            bool wasHealed = damageable.Heal(healthRestore); // Tenta curar o objeto que colidiu com o item de coleta

            if (wasHealed)
                Destroy(gameObject); // Destroi o item de coleta após ser coletado
        }
    }

}
