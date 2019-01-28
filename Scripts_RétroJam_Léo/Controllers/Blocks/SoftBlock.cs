using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBlock : MonoBehaviour, IHittable
{
    private Animator animator;
    private bool destroyAfterAnim = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void LateUpdate()
    {
        if(destroyAfterAnim == true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f && animator.GetCurrentAnimatorStateInfo(0).IsName("SoftBlock Destroy"))
            {
                Grid.softBlocksPowerups.Remove(new Bounds(Grid.GetTilePosition(transform.position, Vector2.zero), Vector2.one));
                Destroy(gameObject);
            }
        }
    }

    public void onHit(Bomb bomb, Vector2 hitDirection)
    {      
        animator.SetBool("Destroy", true);
        destroyAfterAnim = true;
    }
}
