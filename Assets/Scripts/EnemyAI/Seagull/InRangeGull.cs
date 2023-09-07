using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeGull : MonoBehaviour
{
    public SeagullAI script;
    public Collider2D gullRange;
    public Animator animator;
    [SerializeField] private float radiusIncrease;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            script.inRange = true; 
            animator.SetBool("inRange", script.inRange); 
            script.startIdle = false;
            gullRange.transform.localScale = new Vector3(radiusIncrease, radiusIncrease, radiusIncrease);
            GetComponent<AudioSource>().Play();   
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            script.inRange = false;
            animator.SetBool("inRange", script.inRange); 
            script.UpddateIdleStart();
            GetComponent<AudioSource>().Pause();
        }
    }
}
