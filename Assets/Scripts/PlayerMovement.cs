using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [Range(0f,1f)] public float moveSmooth;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public float timeReuseShield = 10f;
    public bool isActivated;
    private float timeShieldActive = 3f;
    private bool shieldIsActive = false;
    [HideInInspector] public bool isPlaying;
    float tempTime = 0f;
    Vector2 velocity = Vector2.zero;

    public GameObject shieldCountTimeDisplay;
    private TMP_Text shieldCountTimeText;

    public bool isWinning;

    Animator animator;
    ParticleSystem ps;
    GameObject shield;
    Values values;

    void Awake()
    {
        animator = GetComponent<Animator>();
        values = GameObject.Find("Values").GetComponent<Values>();
        rb = GetComponent<Rigidbody2D>();
        ps = transform.Find("Bubbles").GetComponent<ParticleSystem>();
        ps.Play();
        shield = transform.Find("shield").gameObject;
        isPlaying = false;
        isWinning = false;
    }

    void Start()
    {
        shield.SetActive(false);
        tempTime = timeReuseShield;
        isActivated = values.isActivated;
        shieldCountTimeText = shieldCountTimeDisplay.transform.Find("Text").GetComponent<TMP_Text>();
    }

    void Update()
    {
        animator.SetFloat("horizontal", moveInput.x);

        var emission = ps.emission;
        var main = ps.main;
        var vel = ps.velocityOverLifetime;
        if (moveInput == Vector2.zero)
        {   
            emission.rateOverTime = 3f;
            main.gravityModifier = -0.1f;
            main.startLifetime = 1f;
            vel.enabled = false;
            if (transform.localScale.x == 1f) main.startSpeed = 1f;
            else main.startSpeed = -1f;
        }
        else
        {
            emission.rateOverTime = 10f;
            main.gravityModifier = -0.3f;
            main.startLifetime = 2f;
            main.startSpeed = 0f;
            vel.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !shieldIsActive && tempTime >= timeReuseShield && isPlaying && isActivated)
        {
            shieldIsActive = true;
            shield.SetActive(true);
            tempTime = timeShieldActive;
            shieldCountTimeText.text = ((int)tempTime).ToString();
        }
        if (shieldIsActive)
        {
            tempTime -= Time.deltaTime;
            gameObject.tag = "Player2";
            shieldCountTimeText.text = ((int)tempTime).ToString();
        }
        if (tempTime <= 0)
        {
            shieldIsActive = false;
            shield.SetActive(false);
            gameObject.tag = "Player";
            shieldCountTimeDisplay.SetActive(false);
        }
        if (!shieldIsActive && tempTime <= timeReuseShield)
        {
            tempTime += Time.deltaTime;
        }
        if (tempTime >= timeReuseShield && isActivated)
        {
            shieldCountTimeDisplay.SetActive(true);
            shieldCountTimeText.text = timeShieldActive.ToString();
        }
    }

    void FixedUpdate() 
    {
        Vector2 desiredVelocity = moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, desiredVelocity, ref velocity, moveSmooth);
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Chest"))
        {
            isWinning = true;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            rb.velocity = new Vector2(0,0);
            this.gameObject.GetComponent<PlayerMovement>().enabled = false;
            FindObjectOfType<AudioManager>().Play("youwon");
        }
    }
}
