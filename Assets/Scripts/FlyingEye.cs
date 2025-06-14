using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    [Header("Configurações do Olho Voador")]

    public float flightSpeed = 2f; // Velocidade de voo do olho voador
    public float waypointReachedDistance = 0.1f; // Distância mínima para considerar que o olho voador chegou a um waypoint
    public DetectionZone biteDetectionZone;

    public Collider2D deathCollider; // Colisor que define a área de morte do olho voador
    public List<Transform> waypoints; // Lista de pontos de caminho que o olho voador pode seguir

    Animator animator; // Referência ao Animator do olho voador para controlar as animações
    Rigidbody2D rb;
    Damageable damageable; // Referência ao script Damageable para aplicar dano

    Transform nextWaypoint; // Próximo ponto de caminho que o olho voador deve seguir
    int waypointNum = 0; // Número do ponto de caminho atual que o olho voador está seguindo    

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
            return animator.GetBool(AnimationStrings.canMove); // Retorna se o olho voador pode se mover
        }
    }

    void Awake()
    {
        transform.localScale *= new Vector2(-1, 1); // Inicia o FlyingEye virado para a esquerda;
        animator = GetComponent<Animator>(); // Obtém a referência ao Animator do olho voador
        rb = GetComponent<Rigidbody2D>(); // Obtém a referência ao Rigidbody2D do olho voador
        damageable = GetComponent<Damageable>(); // Obtém a referência ao script Damageable do olho voador
    }

    private void Start()
    {
        nextWaypoint = waypoints[waypointNum]; // Define o primeiro ponto de caminho como o próximo waypoint
    }

    private void OnEnable()
    {
        damageable.damageableDeath.AddListener(OnDeath); // Assina o evento de morte do Damageable para chamar o método OnDeath quando o olho voador morrer
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove)
            {
                Flight(); // Chama o método de voo se o olho voador puder se mover
            }
            else
            {
                rb.linearVelocity = Vector3.zero; // Se não puder se mover, zera a velocidade do Rigidbody2D
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0; // Verifica se há algum alvo na zona de detecção de mordida
    }

    private void Flight()
    {
        // Voando entre os pontos de caminho "Waypoints"
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized; // Calcula a direção para o próximo ponto de caminho

        // Checkando a distância para o próximo "Waypoint" para saber se já chegou lá
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.linearVelocity = directionToWaypoint * flightSpeed; // Define a velocidade do Rigidbody2D na direção do waypoint

        UpdateDirection();

        // Checkando se já estamos prontos para mudar de "Waypoint"
        if (distance <= waypointReachedDistance)
        {
            waypointNum++; // Incrementa o número do waypoint
            if (waypointNum >= waypoints.Count) // Se o número do waypoint for maior ou igual ao número de waypoints, reinicia para o primeiro waypoint
            {
                waypointNum = 0; // Reinicia o número do waypoint para 0
            }
            nextWaypoint = waypoints[waypointNum]; // Define o próximo waypoint
        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale; // Obtém a escala local do olho voador

        if (transform.localScale.x > 0)
        {
            // olhando para a direita
            if (rb.linearVelocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z); // Inverte a escala para olhar para a esquerda
            }
        }
        else
        {
            // olhando para a esquerda
            if (rb.linearVelocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z); // Inverte a escala para olhar 
            }
        }
    }

    public void OnDeath()
    {
        rb.gravityScale = 2f; // Se o olho voador não estiver mais vivo, aplica gravidade
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Mantém a velocidade horizontal enquanto aplica gravidade 
        deathCollider.enabled = true; // Ativa o colisor de morte para o olho voador
    }
}
