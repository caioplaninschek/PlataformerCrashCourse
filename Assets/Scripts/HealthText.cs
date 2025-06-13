using System.Collections;
using System.Collections.Generic;
using TMPro; // TextMeshPro é usado para exibir texto na interface do usuário
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public UnityEngine.Vector3 moveSpeed = new UnityEngine.Vector3(0, 75, 0);
    public float timeToFade = 1.0f;

    RectTransform textTransform;
    TextMeshProUGUI textMeshPro;

    private float timeElapsed = 0.0f;
    private Color startColor;

    void Awake()
    {
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }

    private void Update()
    {
        textTransform.position += moveSpeed * Time.deltaTime;

        timeElapsed += Time.deltaTime;
        if (timeElapsed < timeToFade)
        {
            float fadeAlpha = startColor.a * (1 - timeElapsed / timeToFade);
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
        }
        else
        {
            Destroy(gameObject); // Destroi o o objeto após o tempo de fade
        }
    }
}
