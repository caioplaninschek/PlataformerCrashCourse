using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public UnityEvent noCollidersRemain; // Evento chamado quando não há mais colliders na zona de detecção

    public List<Collider2D> detectedColliders = new List<Collider2D>(); // Lista para armazenar os colliders detectados
    Collider2D col;


    private void Awake()
    {
        col = GetComponent<Collider2D>(); // Obtém o componente Collider2D do GameObject
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedColliders.Add(collision); // Adiciona o collider que entrou no gatilho à lista

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedColliders.Remove(collision); // Adiciona o collider que entrou no gatilho à lista

        if (detectedColliders.Count <= 0)
        {
            noCollidersRemain.Invoke(); // Invoca o evento se não houver mais colliders na lista
        }
    }
}