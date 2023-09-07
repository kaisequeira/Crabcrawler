using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetectorSeagull : MonoBehaviour
{

    public Animator animator;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Water") {
            animator.SetBool("onWater", true);
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Water") {
            animator.SetBool("onWater", false); 
        }
    }
}
