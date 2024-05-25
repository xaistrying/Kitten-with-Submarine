using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class Scrap : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    private float speed;
    GameManager gm;

    void Start()
    {
        speed = Random.Range(minSpeed,maxSpeed);
        gm = GameObject.Find("GM").GetComponent<GameManager>();
    }

    void Update()
    {
        transform.Rotate(0 ,0 ,speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            // count ++
            gm.scrapCount++;
            gm.ScapCount();
            // dissapear
            Destroy(gameObject);
        }
    }
}
