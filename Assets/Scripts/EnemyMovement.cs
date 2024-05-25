using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [HideInInspector] public float moveSpeed;
    private Rigidbody2D rb;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public byte collideCount = 0;

    float minSpeed = 3f;
    float maxSpeed = 7f;

    GameManager gm;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = Random.Range(minSpeed, maxSpeed);
        gm = GameObject.Find("GM").GetComponent<GameManager>();
    }

    void Update()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && this.tag != "Scrap")
        {
            GameObject.Find("KaBOOM").transform.position = other.transform.position;
            GameObject.Find("KaBOOM").GetComponent<ParticleSystem>().Play();
            Destroy(other.gameObject);
            gm.isPlaying = false;
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.name == "Border")
        {
            Destroy(gameObject);
        }
    }
}
