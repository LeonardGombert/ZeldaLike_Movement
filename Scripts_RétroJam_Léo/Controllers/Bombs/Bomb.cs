using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IHittable
{
    #region Primary Values
    [HideInInspector]
    public float fuseTime;
    [HideInInspector]
    public float fuseTimeLeft;


    public int radius;

    private const float speed = 4;
    #endregion

    #region Detection Values
    [SerializeField]
    private LayerMask collisionMask;
    [SerializeField]
    private LayerMask collisionMaskNextTile;

    bool isOnLevel = false;
    #endregion

    #region Move Values
    [HideInInspector]
    public Vector2 direction;
    private Vector2[] directions = new Vector2[4] { Vector2.right, Vector2.down, Vector2.left, Vector2.up };

    private Vector2 destination;
    private bool destinationReached;
    private GameObject currentStopTarget;
    private bool applyEffect;
    #endregion

    #region Explosion Values
    private bool immediateExplosion;
    private List<Vector2>[] explosionLines = new List<Vector2>[4]; 
    private Dictionary<List<Vector2>, int> anglesForExplosionLines = new Dictionary<List<Vector2>, int>();
    #endregion

    //GENERAL LOGIC
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private aTile tileScript;

    [HideInInspector]
    public bool opacity;

    void Start()
    {
        Debug.Log(" radius is : " + radius);
        for (int i = 0; i < explosionLines.Length; i++)
        {
            explosionLines.SetValue(new List<Vector2>(), i);
        }
        anglesForExplosionLines = BombParameter.FillDicoAnglesForExplosionDir(explosionLines);

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FuseBeforeExplosion());
    }

    void Update()
    {
        if (opacity) spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
        else spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);

        Debug.DrawLine(transform.position, destination, Color.yellow);
        if (destinationReached && applyEffect != false)
        {
            System.Tuple<bool, GameObject> tileCheck = Grid.isPosOnPlayerTile(transform.position);
            if (tileCheck.Item1)
            {
                tileScript = tileCheck.Item2.GetComponent<aTile>();
                applyEffect = true;
            }
        }

        if (Grid.GetTilePosition(transform.position, Vector2.zero) != Vector2.zero) isOnLevel = true;
        Move();
    }

    public void SetPrimaryValues(float _fuseTime, int _radius)
    {
        fuseTime = _fuseTime;
        radius = _radius;
    }

    public void SetDestination(Vector2 _destination)
    {
        destination = _destination;
        direction = (destination - (Vector2)transform.position);
    }

    public void Explosion()
    {
        for (int i = 0; i < directions.Length; i++)
        {
            if(Grid.GetTilePosition(transform.position, directions[i]) != Vector2.zero)
            {
                Vector2 launchPoint = (Vector2)transform.position + (directions[i] * 0.51f);
                RaycastHit2D hit = Physics2D.Raycast(launchPoint, directions[i], radius, collisionMask);

                int distance = Mathf.RoundToInt(Vector2.Distance(launchPoint, hit.point));

                if (hit)
                {
                    IHittable objectHit = hit.transform.GetComponent<IHittable>();
                    if (objectHit != null) objectHit.onHit(this, directions[i]);
                }

                if (distance != 0)
                {
                    int fireLength;

                    if (distance > radius) fireLength = radius;
                    else fireLength = distance;

                    for (int ii = 1; ii <= fireLength; ii++)
                    {
                        explosionLines[i].Add(Grid.GetTilePosition(transform.position, directions[i] * ii));
                    }
                }
            }
            Destruct();
        }
    }

    public void onHit(Bomb bomb, Vector2 hitDirection)
    {
        StopAllCoroutines();

        if (bomb == this) return;
        else
        {
            immediateExplosion = true;
            SetDestination(Grid.GetTilePosition(transform.position, Vector2.zero));
        }
    }

    private void Move()
    {
        RaycastHit2D hitNextTile = Physics2D.Raycast((Vector2)transform.position + (direction.normalized * 0.51f), direction, 0.5f, collisionMaskNextTile);

        if (hitNextTile)
        {
            if (hitNextTile.transform.CompareTag("Bomb") && hitNextTile.transform.gameObject != this.gameObject || hitNextTile.transform.CompareTag("Player") )
            {           
                SetDestination(Grid.GetTilePosition(transform.position, Vector2.zero));
            }
            else if (hitNextTile.transform.CompareTag("Tile") && applyEffect != true)
            {
                if (currentStopTarget != hitNextTile.transform.gameObject)
                {
                    SetDestination(hitNextTile.transform.position);
                    tileScript = hitNextTile.transform.GetComponent<aTile>();
                    applyEffect = true;
                }
                currentStopTarget = hitNextTile.transform.gameObject;
            }
        }

        if(hitNextTile && isOnLevel && hitNextTile.transform.CompareTag("Extent"))
        {
            SetDestination(Grid.GetTilePosition(transform.position, Vector2.zero));
        }

        if ((Vector2)transform.position != destination) destinationReached = false;
        else
        {
            destinationReached = true;
            opacity = false;
            if (applyEffect) CallEffect(tileScript);
        }

        if (!destinationReached)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }
        else if (destinationReached && immediateExplosion) Explosion();
    }

    private void CallEffect(aTile tile)
    {
        if(tile != null)
        {
            tile.BombEffect(this);
            applyEffect = false;
            Debug.Log("effect called on Bomb");
        }
    }

    private void Destruct()
    {
        for (int y = 0; y < explosionLines.Length; y++)
        {
            for (int x = 0; x < explosionLines[y].Count; x++)
            {
                if (x == explosionLines[y].Count - 1)
                {
                    Instantiate(BombParameter.explosionEffects[2], explosionLines[y][x], Quaternion.Euler(0, 0, anglesForExplosionLines[explosionLines[y]]));
                }
                else Instantiate(BombParameter.explosionEffects[1], explosionLines[y][x], Quaternion.Euler(0, 0, anglesForExplosionLines[explosionLines[y]]));
            }
        }
        if (explosionLines[0].Count > 0) Instantiate(BombParameter.explosionEffects[3], transform.position, Quaternion.identity);
        Instantiate(BombParameter.explosionEffects[0], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator FuseBeforeExplosion()
    {
        for (fuseTimeLeft = fuseTime; fuseTimeLeft > 0; fuseTimeLeft -= Time.deltaTime)
        {
            animator.speed = 1 + (5 -((fuseTimeLeft / fuseTime) * 5));
            yield return null;
        }

        Explosion();
    }
}
