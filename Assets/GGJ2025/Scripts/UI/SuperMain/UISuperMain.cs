using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISuperMain : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite[] sprites;

    private float timeInterval = 3f;  // Interval di tempo tra le immagini in secondi
    private float timer = 0f;    // Variabile per tenere traccia del tempo passato
    private int currentIndex = 0;  // Indice per scorrere nell'array delle immagini
    private bool stopAnimation;

    private float fadeTimer = 0f; // Timer per il fade-in
    private bool isFadingIn = true;  // Flag per sapere se stiamo facendo il fade-in
    private float fadeDuration = 3f;  // Durata del fade-in in secondi

    // Update is called once per frame
    void Update()
    {
        // Aumenta il timer con il tempo trascorso ogni frame
        timer += Time.deltaTime;
        // Se stiamo facendo il fade-in, aggiorna il timer
        if (isFadingIn)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, fadeTimer / fadeDuration);  // Calcola l'alpha in base al tempo

            // Imposta il nuovo colore con l'alpha aggiornato
            Color currentColor = image.color;
            currentColor.a = alpha;
            image.color = currentColor;

            // Quando il fade-in è completato, ferma il fade e inizia il timer per le immagini
            if (fadeTimer >= fadeDuration)
            {
                isFadingIn = false;
                fadeTimer = 0f;  // Reset del timer del fade
            }
        }
        if (timer >= timeInterval && !stopAnimation)
        {
            // Resetta il timer
            timer = 0f;

            // Cambia l'immagine
            image.sprite = sprites[currentIndex];
            if (currentIndex == sprites.Length - 1)
                stopAnimation = true;
            else
                // Incrementa l'indice per il prossimo ciclo (cicla alla fine dell'array)
                currentIndex = (currentIndex + 1) % sprites.Length;
        }
        else if (timer >= timeInterval && stopAnimation)
        {
            SceneManager.LoadScene("Main");
        }
    
    }
}
