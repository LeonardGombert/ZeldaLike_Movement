using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralPickupClass : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public LPlayer playerScript;

    public float waitTime;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(TimerAppear());
    }

    //Destroy the spriterenderer on pickup
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerScript = player.GetComponent<LPlayer>();

            StartCoroutine(TimerDisapear());
            pickupEffect();
        }
    }

    IEnumerator TimerAppear()
    {
        GetComponent<BoxCollider2D>().enabled = false;

        float baseTime = 2f;
        for (float timeToGo = 0; timeToGo <= baseTime; timeToGo += Time.deltaTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, timeToGo / baseTime);
            yield return null;
        }

        GetComponent<BoxCollider2D>().enabled = true;
    }

    //Amount of time before object disapears
    IEnumerator TimerDisapear()
    {
        GetComponent<BoxCollider2D>().enabled = false;

        float baseTime = 2f;
        for (float timeLeft = baseTime; timeLeft > 0; timeLeft-= Time.deltaTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, timeLeft / baseTime);
            yield return null;
        }
    }

    public virtual void pickupEffect()
    {

    }

}
