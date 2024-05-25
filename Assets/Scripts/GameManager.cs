using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] 
    public int scrapCount;
    public TMP_Text scraps;

    public TMP_Text depth;
    private float speedUpgrade = 1f;

    [HideInInspector]
    public bool isPlaying;
    public bool played;
    public GameObject background;
    public Slider depthMeasure;
    public GameObject PressSpacebar;

    float time = 0f;

    public TMP_Text shieldCostText;
    public TMP_Text speedCostText;
    public TMP_Text shieldInfoText;
    public TMP_Text speedInfoText;
    
    public GameObject shieldIcon;
    public GameObject GroundWithChest;
    public ParticleSystem congrats;
    int[] speedUpgradeCost, coolDownShieldUpgradeCost;
    int speedUpgradeCount, cdShieldUpgradeCount;

    Values values;
    PlayerMovement playerMovement;
    Spawner spawner;

    void Start()
    {
        spawner = GameObject.Find("Spawners").GetComponent<Spawner>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        values = GameObject.Find("Values").GetComponent<Values>();
        cdShieldUpgradeCount = values.cdShieldUpgradeCount;
        speedUpgradeCount = values.speedUpgradeCount;
        scrapCount = values.scrapCount;
        scraps.text = scrapCount.ToString();
        transform.Find("GroundBubbles").GetComponent<ParticleSystem>().Play();
        played = false;
        isPlaying = false;
        coolDownShieldUpgradeCost = new int[]{10, 20, 30};
        speedUpgradeCost = new int[]{10, 20, 30};

        
        if (values.cdShieldUpgradeCount < 3) 
        {
            if (values.cdShieldUpgradeCount == 1)
                shieldInfoText.text = "Upgraded: " + values.cdShieldUpgradeCount + "/3\nShield Activated";
            else if (values.cdShieldUpgradeCount == 2) 
                shieldInfoText.text = "Upgraded: " + values.cdShieldUpgradeCount + "/3\nCooldown -2s";
            shieldCostText.text = coolDownShieldUpgradeCost[values.cdShieldUpgradeCount].ToString();
        }
        else    
        {
            shieldInfoText.text = "Upgraded: " + values.cdShieldUpgradeCount + "/3\nCooldown -5s";
            shieldCostText.text = "";
        }
        playerMovement.timeReuseShield = values.timeReuseShield;
        
        if (values.speedUpgradeCount < 3)
        {
            if (values.speedUpgradeCount == 1)
                speedInfoText.text = "Upgraded: " + values.speedUpgradeCount + "/3\nDivingSpeed x3";
            else if (values.speedUpgradeCount == 2) 
                speedInfoText.text = "Upgraded: " + values.speedUpgradeCount + "/3\nDivingSpeed x5";
            speedCostText.text = speedUpgradeCost[values.speedUpgradeCount].ToString();
        }
        else if (values.speedUpgradeCount == 3)
        {
            speedInfoText.text = "Upgraded: " + values.speedUpgradeCount + "/3\nDivingSpeed x9";
            speedCostText.text = "";
        }

        speedUpgrade = values.speedUpgrade;

        if (values.isActivated == true)
        {
            shieldIcon.SetActive(true);
        }
        else if (values.isActivated == false)
        {
            shieldIcon.SetActive(false);
        }
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isPlaying && !played)
        {
            background.GetComponent<Animator>().SetTrigger("start");
            isPlaying = true;
            played = true;
        }

        if (isPlaying && time <= 1000f)
        {
            time += speedUpgrade * Time.deltaTime;
            depth.text = ((int)time).ToString();
            depthMeasure.value = time;
            PressSpacebar.SetActive(false);

            if (depthMeasure.value >= 300f && depthMeasure.value < 700)
            {
                spawner.timeBetweenSpawns = 0.7f;
            }
            if (depthMeasure.value >= 700 && depthMeasure.value < 900)
            {
                spawner.timeBetweenSpawns = 0.5f;
            }
            if (depthMeasure.value >= 900)
            {
                spawner.timeBetweenSpawns = 0.45f;
            }
            if (depthMeasure.value == 1000)
            {
                GroundWithChest.GetComponent<Animator>().enabled = true;
            }
        }

        if (time / speedUpgrade >= 2f && time / speedUpgrade <= 2.5f)
        {
            StartGame();
        }

        if (!isPlaying && played)
        {
            played = false;
            FindObjectOfType<AudioManager>().Play("KaBOOM");
            StartCoroutine(LoadGame());
        }

        if (playerMovement.isWinning == true)
        {
            GroundWithChest.GetComponent<Animator>().SetTrigger("youwon");
            congrats.Play();
        }
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void StartGame()
    {
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
        GameObject.Find("Player").GetComponent<PlayerMovement>().isPlaying = true;
        GameObject.Find("Spawners").GetComponent<Spawner>().enabled = true;
    }

    public void ScapCount()
    {
        scraps.text = scrapCount.ToString();
        values.scrapCount = scrapCount;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShieldCDCheckUpgrade()
    {
        int[] upgrade = new int[] {10, 7, 5};
        if (cdShieldUpgradeCount+1 < 3)
        {
            if (scrapCount >= coolDownShieldUpgradeCost[cdShieldUpgradeCount])
            {
                scrapCount -= coolDownShieldUpgradeCost[cdShieldUpgradeCount];
                cdShieldUpgradeCount++;
                if (cdShieldUpgradeCount == 1)
                {
                    playerMovement.isActivated = true;
                    shieldIcon.SetActive(true);
                    values.isActivated = true;
                    playerMovement.timeReuseShield = upgrade[0];
                    shieldCostText.text = coolDownShieldUpgradeCost[cdShieldUpgradeCount].ToString();
                    shieldInfoText.text = "Upgraded: " + cdShieldUpgradeCount + "/3\nShield Activated";
                }
                else 
                {
                    playerMovement.timeReuseShield = upgrade[1];
                    shieldCostText.text = coolDownShieldUpgradeCost[cdShieldUpgradeCount].ToString();
                    shieldInfoText.text = "Upgraded: " + cdShieldUpgradeCount + "/3\nCooldown -2s";
                }
            }
        }
        else if (cdShieldUpgradeCount+1 == 3)
        {
            if (scrapCount >= coolDownShieldUpgradeCost[cdShieldUpgradeCount])
            {
                scrapCount -= coolDownShieldUpgradeCost[cdShieldUpgradeCount];
                cdShieldUpgradeCount++;
                playerMovement.timeReuseShield = upgrade[2];
                shieldCostText.text = "";
                shieldInfoText.text = "Upgraded: " + cdShieldUpgradeCount + "/3\nCooldown -5s";
                playerMovement.timeReuseShield = upgrade[2];
            }
        }
        values.cdShieldUpgradeCount = cdShieldUpgradeCount;
        values.scrapCount = scrapCount;
        scraps.text = scrapCount.ToString();
        values.scrapCount = scrapCount;
        values.timeReuseShield = playerMovement.timeReuseShield;
    }

    public void SpeedCheckUpgrade()
    {
        int[] upgrade = new int[] {3, 5, 9};
        if (speedUpgradeCount+1 < 3) 
        {
            if (scrapCount >= speedUpgradeCost[speedUpgradeCount])
            {
                scrapCount -= speedUpgradeCost[speedUpgradeCount];
                speedUpgrade = upgrade[speedUpgradeCount];
                values.speedUpgrade = speedUpgrade;
                speedUpgradeCount++;
                speedCostText.text = speedUpgradeCost[speedUpgradeCount].ToString();

                if (speedUpgradeCount == 1) speedInfoText.text = "Upgraded: " + speedUpgradeCount + "/3\nDivingSpeed x3";
                else speedInfoText.text = "Upgraded: " + speedUpgradeCount + "/3\nDivingSpeed x5";
            }
        }
        else if (speedUpgradeCount+1 == 3)
        {
            if (scrapCount >= speedUpgradeCost[speedUpgradeCount])
            {
                scrapCount -= speedUpgradeCost[speedUpgradeCount];
                speedUpgrade = upgrade[speedUpgradeCount];
                values.speedUpgrade = speedUpgrade;
                speedUpgradeCount++;
                speedCostText.text = "";
                speedInfoText.text = "Upgraded: " + speedUpgradeCount + "/3\nDivingSpeed x9";
            }
        }
        values.speedUpgradeCount = speedUpgradeCount;
        values.scrapCount = scrapCount;
        scraps.text = scrapCount.ToString();
        values.scrapCount = scrapCount;
    }
}
