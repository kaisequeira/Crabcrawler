using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBlast : MonoBehaviour
{
    public PlayerController playerScript;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "WaterPlayerCheck") {
            playerScript.inBubbleBlast = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "WaterPlayerCheck") {
            playerScript.inBubbleBlast = false;
        }
    }
}
