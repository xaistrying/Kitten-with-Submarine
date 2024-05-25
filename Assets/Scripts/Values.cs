using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Values : MonoBehaviour
{
    private static Values values;
    // [HideInInspector]
    public int scrapCount, cdShieldUpgradeCount, speedUpgradeCount; 
    public float speedUpgrade;
    public float timeReuseShield;
    public bool isActivated;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (values == null)
        {
            values = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        cdShieldUpgradeCount = 0;
        speedUpgradeCount = 0;
        scrapCount = 0;
        speedUpgrade = 1f;
        timeReuseShield = 10f;
        isActivated = false;
    }
}
