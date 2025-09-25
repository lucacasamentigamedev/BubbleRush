using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPop : MonoBehaviour
{
    private float timer;
    private float destructionTime;
    // Update is called once per frame
    private void OnEnable()
    {
        timer = 0;
        destructionTime = Random.Range(0.2f, 0.7f);
        LevelManager.Get().OnWinLevel += OnEndLevel;
        LevelManager.Get().OnLoseLevel += OnEndLevel;
        LevelManager.Get().OnLoseEndlessLevel += OnEndLevel;
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > destructionTime)
        {
            gameObject.SetActive(false);
        }
    }
    void OnEndLevel(int i)
    {
        gameObject.SetActive(false);
    }
    void OnEndLevel()
    {
        gameObject.SetActive(false);
    }
}
