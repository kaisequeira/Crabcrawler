using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

            if (currentOxygen / maxOxygen < 0.2f && currentOxygen > 0) {
                warningAnimator.SetBool("Warning", true);
            } else {
                warningAnimator.SetBool("Warning", false);
            }
        } else {
            warningAnimator.SetBool("Warning", false);
        }
    }

    public void PlayPop() {
        oxygenAnimator.SetBool("Pop", false);
    }

    public void Add() {
        currentOxygen = maxOxygen;
        oxygenBar.fillAmount = 0.2f + (0.625f * (currentOxygen / maxOxygen));
    }
}