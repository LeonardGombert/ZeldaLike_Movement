using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class aTile : MonoBehaviour
{
    #region Public Variables
    [BoxGroup("Values")]
    public int tileBoost;
    [BoxGroup("Values")]
    public int propulsionBoost;
    [BoxGroup("Values")]
    public bool useDir;
    [BoxGroup("Values")]
    public Vector2 direction;

    [BoxGroup("Infos")]
    public GameObject origin;
    [BoxGroup("Infos")][SerializeField]
    private Animator animator;
    [BoxGroup("Infos")][SerializeField]
    private BoxCollider2D collider2D;

    private Transform childIcon;
    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        childIcon = GetComponentsInChildren<Transform>()[1];

        animator = GetComponent<Animator>();
        collider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        if (useDir)
        {
            float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            childIcon.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
    }

    #endregion


    #region Private Methods
    public virtual void PlayerEffect(LPlayer player)
    {

    }

    public virtual void BombEffect(Bomb bomb)
    {

    }

    public void ShowIcon()
    {
        childIcon.GetComponent<SpriteRenderer>().enabled = true;
    }

    public IEnumerator DisapearCoroutine()
    {
        childIcon.GetComponent<SpriteRenderer>().enabled = false;
        animator.SetBool("Disappear", true);
        yield return new WaitForSeconds(0.417f);
        Destroy(gameObject);
    }

    #endregion
}
