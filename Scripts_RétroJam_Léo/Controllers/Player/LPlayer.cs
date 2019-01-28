using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class LPlayer : MonoBehaviour, IHittable
{
    [SerializeField]
    private GameObject wantedTile;
    #region Private Values
    #region References
    Animator animator;
    SpriteRenderer spriteRenderer;
    Collider2D collider2D;
    #endregion
    [BoxGroup("Debug")][SerializeField]
    Vector2 destination;
    [BoxGroup("Debug")]
    public Vector2 direction;
    [BoxGroup("Debug")]
    public Vector2 aimDirection;
    [BoxGroup("Debug")][SerializeField]
    Vector2 origin;
    [BoxGroup("Debug")][SerializeField]
    Vector2 lockedDirection;
    [BoxGroup("Debug")][SerializeField]
    Vector2 spawnPoint;
    [BoxGroup("Debug")][SerializeField]
    LayerMask layerMask;
    [BoxGroup("Debug")]
    public bool ejected;
    [BoxGroup("Debug")][SerializeField]
    int ejectAmount;
    [BoxGroup("Debug")] [SerializeField]
    bool applyEffect;
    [BoxGroup("Debug")][SerializeField]
    aTile tileOn;


    [BoxGroup("Status")]
    public int lives;
    [BoxGroup("Status")]
    public int dmgMeter;
    [BoxGroup("Status")][ShowInInspector]
    public float speed { get; set; }
    [BoxGroup("Status")]
    public GameObject selectedTile;
    [BoxGroup("Status")]
    public int tileBoost;
    [BoxGroup("Status")]
    public int propulsionBoost;
    [BoxGroup("Status")]
    public int tileCapacity;
    [BoxGroup("Status")]
    public bool stunned;
    [BoxGroup("Status")]
    public int invicibilityFrameNbr;
    private bool invicible = false;

    [FoldoutGroup("Stats")]
    public int baseLives;
    [FoldoutGroup("Stats")]
    public float baseSpeed;
    [FoldoutGroup("Stats")]
    public float stunTime;
    [FoldoutGroup("Stats")]
    public GameObject[] tiles = new GameObject[3];
    [FoldoutGroup("Stats")]
    public float offsetY;
  
    [HideInInspector]
    public bool tilePlacedOnSelf;
    [HideInInspector]
    public bool firstTilePlaced;

    private bool strobing;
    private bool opacity;
    private bool dontChangeOpacity;

    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        //References
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        selectedTile = tiles[0];
        //Snapping player to grid position with offset
        this.transform.position = Grid.GetTilePosition(this.transform.position, Vector2.zero) + new Vector2(0, 0.25f);
        //Setting Origin
        origin = Grid.GetTilePosition(this.transform.position, Vector2.zero);
        //Setting Respawn point
        spawnPoint = transform.position;
        //Setting stats
        lives = baseLives;
        speed = baseSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //When Colliding, goes back to previous position
        lockedDirection = direction;
        Debug.Log("Collided with solid object " + collision.gameObject.name);
        destination = origin;
        direction = -direction;
    }

    #endregion

    #region Public Methods

    public void Move(Vector2 moveInputs, bool overrideStun = false)
    {
        if (!overrideStun)
        {
            if (stunned) return;
        }

        if (moveInputs == Vector2.zero)
        {
            lockedDirection = Vector2.zero;
            if(destination == Vector2.zero) animator.SetBool("Walking", false);
        }

        if (destination == Vector2.zero)
        {
            Vector2 wantedDir = RemoveDiagonales(moveInputs);
            if (wantedDir == lockedDirection) return;
            else direction = wantedDir;
        }

        if (direction != Vector2.zero && destination == Vector2.zero)
        {

                destination = Grid.GetTilePosition(this.transform.position, direction);
            
        }

        if (overrideStun) MoveToward(true);
        else MoveToward();
    }

    void Update()
    {
        if(destination == Vector2.zero && invicible == false)
        {
            CheckForTile();

            if (applyEffect)
            {
                tilePlacedOnSelf = true;
                ApplyEffect(tileOn);
            }
        }

        if (opacity) spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
        else if (dontChangeOpacity == false) spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);

        /*RaycastHit2D hit = Physics2D.Raycast(Grid.GetTilePosition(transform.position, Vector2.zero), direction, 0.5f, layerMask);
        if (hit && Grid.isPosOnPlayerTile(Grid.GetTilePosition(transform.position, Vector2.zero)).Item1)
        {
            invicible = true;
        }
        else invicible = false;*/
    }

    private void MoveToward(bool ejected = false)
    {
        if (destination != Vector2.zero)
        {
            if (Grid.GetExtentTilePosition(destination, Vector2.zero) != Vector2.zero && ejected == false)
            {
                destination = origin;
                direction = -direction;
            }

            if (ExtensionMethods.RoundVect2(destination) == (ExtensionMethods.RoundVect2(transform.position) - new Vector2(0, offsetY)))
            {

                if (invicible == false) CheckForTile();

                if (destination != origin)
                {
                    lockedDirection = Vector2.zero;

                    if (applyEffect)
                    {
                        tilePlacedOnSelf = false;
                        ApplyEffect(tileOn);
                    }

                    if (ejectAmount > 1) ejectAmount -= 1;
                    else EndEjection();
                }

                origin = Grid.GetTilePosition(this.transform.position, Vector2.zero);

                if (destination != origin) Fall();

                destination = Vector2.zero;
            }
            else
            {
                if (origin != destination) direction = RemoveDiagonales(destination - origin);
                //Set player walking and facing good direction
                spriteRenderer.flipX = ejected ? spriteRenderer.flipX :
                                       direction.x < 0 ? true : false;
                if (!ejected) animator.SetBool("Walking", true);

                //Moving player toward destination at speed
                Vector2 velocity = direction * speed;
                transform.position += (Vector3)velocity * Time.deltaTime;
            }
        }
    }

    private void CheckForTile()
    {
        if (!applyEffect)
        {
            System.Tuple<bool, GameObject> tileCheck = Grid.isPosOnPlayerTile(Grid.GetTilePosition(transform.position, Vector2.zero));

            if (tileCheck.Item1 == true)
            {
                tileOn = tileCheck.Item2.GetComponent<aTile>();
                applyEffect = true;
            }
        }
    }

    private Vector2 RemoveDiagonales(Vector2 moveInputs)
    {
        Vector2 dir = Vector2.zero;
        if (moveInputs != Vector2.zero) ;
        {
            dir = moveInputs.normalized;
            if (moveInputs.x != 0 && moveInputs.y != 0)
            {
                dir = Mathf.Abs(moveInputs.y) > Mathf.Abs(moveInputs.x) ?
                    new Vector2(0, Mathf.Sign(moveInputs.y) * 1) :
                    new Vector2(Mathf.Sign(moveInputs.x) * 1, 0);
            }
        }
        return ExtensionMethods.FloorVect2(dir);
    }

    public void AimInput(Vector2 aimInput)
    {
        if (aimInput != Vector2.zero)
        {
            aimDirection = RemoveDiagonales(aimInput);
        }
    }

    public void SwitchTile(int i)
    {
        selectedTile = tiles[i];
    }

    public void PlaceTile(bool placeOnSelf)
    {
        Vector2 tilePos;

        if (!placeOnSelf)
        {
            tilePos = Grid.GetTilePosition(this.transform.position, aimDirection);
            if (tilePos != Vector2.zero && Grid.GetTilePosition(tilePos, Vector2.zero) != Vector2.zero) TilePlacement(tilePos);
        }
        else
        {           
            tilePos = Grid.GetTilePosition(this.transform.position, Vector2.zero);
            TilePlacement(tilePos, true);
        }
    }

    private void TilePlacement(Vector2 tilePos, bool ignoreRay = false)
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + (aimDirection / 2), aimDirection, 0.51f, layerMask);

        if (hit.collider != null && selectedTile != tiles[1])
        {
            return;
        }


        GameManager gameManager = GameManager.Instance;
        int tilesNbr = 0;
        for(int i = 0; i < gameManager.tilesStock.transform.childCount; i++)
        {
            if (gameManager.GetComponentsInChildren<aTile>()[i].origin == this.gameObject) tilesNbr++;
        }

        for (int i = 0; i < tilesNbr - tileCapacity; i++)
        {
            for(int ii = 0; ii < gameManager.tilesStock.transform.childCount; ii++)
            {
                if (gameManager.GetComponentsInChildren<aTile>()[i].origin == this.gameObject)
                {
                    GameObject playerTile = gameManager.GetComponentsInChildren<aTile>()[i].gameObject;
                    Grid.playerTiles.Remove(System.Tuple.Create(new Bounds(playerTile.transform.position, Vector2.one), playerTile));
                    aTile playerTileScript = playerTile.GetComponent<aTile>();
                    playerTileScript.StartCoroutine(playerTileScript.DisapearCoroutine());
                    break;
                }
            }
        }

        GameObject tileObj = Instantiate(selectedTile, tilePos, Quaternion.identity, gameManager.tilesStock.transform);
        aTile tile = tileObj.GetComponent<aTile>();

        tile.direction = aimDirection;
        tile.tileBoost = tileBoost;
        tile.propulsionBoost = propulsionBoost;
        tile.origin = this.gameObject;

        Grid.playerTiles.Add(System.Tuple.Create(new Bounds(tile.transform.position, Vector2.one), tileObj));
    }

    private void ApplyEffect(aTile tile)
    {
        if(tile != null)
        {
            tile.PlayerEffect(this);
            applyEffect = false;
        }
    }

    private void Stunned()
    {

    }

    public void onHit(Bomb bomb, Vector2 hitDirection)
    {
        if(invicible == false)
        {
            stunned = true;
            Debug.Log("I Got Hit AIE!!!");
            animator.SetTrigger("Hurt");
            dmgMeter += 1;
            Eject(dmgMeter, hitDirection);
        }
    }

    private Vector2 ejectionDirection;

    public void Eject(int amount, Vector2 _direction, bool ghost = false)
    {
        stunned = true;
        direction = _direction;
        ejectionDirection = _direction;
        ejectAmount = amount;

        if (ghost)
        {
            opacity = true;
            collider2D.enabled = false;
        }
        ejected = true;
    }

    public void EndEjection()
    {
        opacity = false;
        collider2D.enabled = true;
        ejected = false;
        stunned = false;
    }

    private void Fall(int falling = 0)
    {
        if (falling == 0)
        {
            Debug.Log("launchedFall 0");
            ejected = false;
            collider2D.enabled = false;
            stunned = true;
            animator.SetTrigger("Fall");
        }
        else
        {
            Debug.Log("launchedFall not 0");
            if (direction != Vector2.down) spriteRenderer.sortingLayerName = "Background";
            StartCoroutine(Falling());
        }
    }

    public void Respawn()
    {
        //Resets everything
        transform.position = spawnPoint;
        collider2D.enabled = true;

        destination = Vector2.zero;
        direction = Vector2.zero;
        origin = Grid.GetTilePosition(this.transform.position, Vector2.zero);

        spriteRenderer.sortingLayerName = "Default";
        animator.SetTrigger("Idle");
        spriteRenderer.color = new Color(1, 1, 1, 1);
        dmgMeter = 0;

        ejected = false;
        stunned = false;
        animator.SetTrigger("Hurt");
    }

    IEnumerator Falling()
    {
        float time = 1.5f;
        Vector2 fallDir = new Vector2(direction.x / 5, -1.5f);

        for (float i = time; i > 0; i -= Time.deltaTime)
        {
            Vector2 velocity = fallDir * speed;
            transform.position += (Vector3)velocity * Time.deltaTime;
            spriteRenderer.color = new Color(1, 1, 1, i / time);
            yield return null;
        }

        lives -= 1;

        if (lives >= 1)
        {
            StrobeColor(invicibilityFrameNbr, 0.35f);
            Respawn();
        }
        else Destroy(gameObject);
    }

    public void StrobeColor(int _strobeCount, float a)
    {
        if (strobing)
            return;

        invicible = true;
        strobing = true;

        SpriteRenderer mySprite = this.GetComponent<SpriteRenderer>();
        Color oldColor = new Color(mySprite.color.r, mySprite.color.b, mySprite.color.g, 1);
        Color toStrobe = new Color(mySprite.color.r, mySprite.color.b, mySprite.color.g, a);

        StartCoroutine(StrobeColorHelper(0, ((_strobeCount * 2) - 1), mySprite, oldColor, toStrobe));

    }

    public IEnumerator StrobeColorHelper(int _i, int _stopAt, SpriteRenderer _mySprite, Color _color, Color _toStrobe)
    {
        dontChangeOpacity = true;

        if (_i <= _stopAt)
        {
            if (_i % 2 == 0)
                _mySprite.color = _toStrobe;
            else
                _mySprite.color = _color;

            yield return new WaitForSeconds(0.1f);
            StartCoroutine(StrobeColorHelper((_i + 1), _stopAt, _mySprite, _color, _toStrobe));
        }
        else
        {
            strobing = false;
            invicible = false;
            dontChangeOpacity = false;
        }
    }
    #endregion
}