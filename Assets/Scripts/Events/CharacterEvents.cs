using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvents
{
    // Evento disparado quando um personagem recebe dano
    public static UnityAction <GameObject, int> characterDamaged;

    // Evento disparado quando um personagem é curado
    public static UnityAction <GameObject, int> characterHealed;
}