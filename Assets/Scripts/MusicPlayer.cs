using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Header("Configurações do Player de Música")]

    [Tooltip("Áudio a ser reproduzido")]
    public AudioSource introSource, loopSource; // Referências aos componentes AudioSource para a música de introdução e loop

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        introSource.Play(); // Inicia a reprodução da música de introdução
        loopSource.PlayScheduled(AudioSettings.dspTime + introSource.clip.length); // Inicia a reprodução da música de loop após o término da introdução
        
    }


}
