using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPop : MonoBehaviour
{
    private float timer;
    private float destructionTime;
    // Update is called once per frame
    private void Awake()
    {
        destructionTime = Random.Range(0.2f, 0.7f);
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > destructionTime)
        {
            Destroy(gameObject);
        }
    }
}
