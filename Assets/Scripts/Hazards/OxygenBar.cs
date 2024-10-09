using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the oxygen bar mechanics and depletion based on player status (in or out of water).
/// </summary>
public class OxygenBar : MonoBehaviour
{
    [SerializeField] private float maxOxygen;
    [SerializeField] private float oxygenRate;
    [SerializeField] private WinConditionScript winSet;
    private float currentOxygen;
    public Image oxygenBar;

    public PlayerController playerScript;
    public PlayerHealth healthScript;
    public Animator oxygenAnimator;
    public Animator warningAnimator;

    void Start() {
       currentOxygen = maxOxygen;
       oxygenBar.fillAmount = 1;
    }

    /// <summary>
    /// Updates the oxygen level periodically based on the player's environment and health.
    /// </summary>
    void FixedUpdate() {
        if (!winSet.levelWon) {
            if (currentOxygen <= 0 && !healthScript.isDead) {
                healthScript.removeHealth();
                oxygenAnimator.SetBool("Pop", true);
                Invoke("PlayPop", 1.05f);
            } else if (!playerScript.inWater && !healthScript.isDead) {
                currentOxygen -= oxygenRate * Time.deltaTime;
                oxygenBar.fillAmount = 0.2f + (0.625f * (currentOxygen / maxOxygen));
            } else if (playerScript.inWater && !healthScript.isDead) {
                Add();
            }

            warningAnimator.SetBool("Warning", currentOxygen / maxOxygen < 0.2f && currentOxygen > 0);
        } else {
            warningAnimator.SetBool("Warning", false);
        }
    }

    /// <summary>
    /// Plays oxygen depletion animation.
    /// </summary>
    public void PlayPop() {
        oxygenAnimator.SetBool("Pop", false);
    }

    /// <summary>
    /// Refills the oxygen bar when the player is in water.
    /// </summary>
    public void Add() {
        currentOxygen = maxOxygen;
        oxygenBar.fillAmount = 0.2f + (0.625f * (currentOxygen / maxOxygen));
    }
}
