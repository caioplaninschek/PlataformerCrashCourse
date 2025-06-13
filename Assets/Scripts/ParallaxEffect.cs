using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    // Posição inicial para o game object Parallax
    Vector2 startingPosition;

    // Valor inicial de Z do game object Parallax
    float startingZ;

    // Distancia que a câmera se moveu desde o incio do objeto Parallax
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    // 
    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    // Se o objeto está na frente do alvo, usa nearClipPlane, se está atrás do alvo, usa farClipPlane
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    // Quanto mais longe o objeto Parallax está do alvo, mais rápido ParallaxEffect object se move. Aproxima o valor de Z para o alvo para ficar mais devagar
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;    
    }

    // Update is called once per frame
    void Update()
    {
        // Quando o alvo se move, move o objeto Parallax na mesma distancia, vezes um mulltiplicador
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;

        // A posição de X/Y muda baseado na velocidade percorrida pelo alvo, vezes o fator de parallax, mas Z se mantém constante
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
