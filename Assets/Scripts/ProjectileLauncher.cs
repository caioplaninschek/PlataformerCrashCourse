using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [Tooltip("Ponto de lançamento do projétil")]
    public Transform launchPoint; // Ponto de lançamento do projétil

    [Tooltip("Prefab do projétil a ser lançado")]
    public GameObject projectilePrefab; // Prefab do projétil a ser lançado

    public void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation); // Instancia o projétil no ponto de lançamento
        UnityEngine.Vector3 origScale = projectile.transform.localScale; // Armazena a escala original do projétil

        // Vira o projétil na direção em que o personagem está olhando
        projectile.transform.localScale = new UnityEngine.Vector3( // Define a escala do projétil para a original
            origScale.x * transform.localScale.x > 0 ? 1 : -1, // Multiplica a escala original pelo sinal da escala local do personagem
            origScale.y, // Mantém a escala Y original
            origScale.z // Mantém a escala Z original
        );
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
