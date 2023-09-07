using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferfishWaterCheck : MonoBehaviour
{
    public bool inWater = true;
    public Transform pufferfishLocation;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Water") {
            inWater = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Water") {
            inWater = false;
            GetComponent<AudioSource>().Play();
        }
    }
}
