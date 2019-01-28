using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{

    #region Move Values

    [Header("Move Values")]
    [SerializeField]
    private int rangeOnMoveAxis;
    [SerializeField]
    private int positionInTileOnRange = 0;
    [SerializeField]
    private PipeParameter.MoveAxis moveAxis;
    private System.Tuple<Vector2, Vector2> moveDirTuple;

    private float resetTime;
    private float speed;

    private Vector2 targetPosition;
    private Vector2 direction;

    private bool move = false;

    #endregion

    #region Shoot Values

    [Space]
    [Header("Shoot Values")]
    [SerializeField]
    private PipeParameter.ShootDirection shootDirection;
    private System.Tuple<int, int, Vector2> shootDirTuple;
    [SerializeField]
    private LayerMask collisionMask;
    [SerializeField]
    private GameObject bombPrefab;

    #endregion

    //GENERAL LOGIC
    private BombParameter bombParameter;

    private bool once = false;
    private bool isOdd;

    public void SetPrimaryValues(float _speed, float _resetTime)
    {
        speed = _speed;
        resetTime = _resetTime;
    }

    void Start()
    {
        bombParameter = GetComponentInParent<BombParameter>();

        shootDirTuple = PipeParameter.SetShootValues(shootDirection);
        moveDirTuple = PipeParameter.SetMoveValues(moveAxis);

        if (positionInTileOnRange == rangeOnMoveAxis) isOdd = false;
        else isOdd = true;
    }

    void FixedUpdate()
    {
         Move();
    }

    private void Move()
    {
        if (move == false)
        {
            int targetPosInt = ExtensionMethods.GetRandomSpecificInt(0, rangeOnMoveAxis, isOdd);

            if (positionInTileOnRange > targetPosInt) direction = moveDirTuple.Item2;
            else direction = moveDirTuple.Item1;

            float moveAmount = Mathf.Abs(positionInTileOnRange - targetPosInt);
            direction *= moveAmount;

            targetPosition = Grid.GetExtentTilePosition(this.transform.position, direction);

            positionInTileOnRange = targetPosInt;

            move = true;
            once = true;
        }

        if (move == true && once == true)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetPosition, speed * Time.deltaTime);

            if ((Vector2)this.transform.position == targetPosition)
            {
                Shoot();
                StartCoroutine(ResetTimer());
                once = false;
                return;
            }
        }
    }

    private void Shoot()
    {
        Vector2 launchPoint = Grid.GetExtentTilePosition(this.transform.position, shootDirTuple.Item3 * 2);
        Vector2 hitPoint = Vector2.zero;

        RaycastHit2D hit = Physics2D.Raycast(launchPoint, shootDirTuple.Item3, shootDirTuple.Item2, collisionMask);

        if (hit)
        {
            hitPoint = hit.point - (shootDirTuple.Item3 * 0.5f);
            Debug.DrawLine(launchPoint, hitPoint, Color.blue);
        }
        else Debug.DrawLine(launchPoint, (Vector2)this.transform.position + (shootDirTuple.Item3 * (shootDirTuple.Item2 + 2)), Color.red);

        int distance = Mathf.RoundToInt(Vector2.Distance(launchPoint, hitPoint));

        int landPointDistance;
        if (hit) landPointDistance = Random.Range(1 + shootDirTuple.Item1, distance + 1);
        else landPointDistance = Random.Range(1 + shootDirTuple.Item1, shootDirTuple.Item2 + 1);

        Debug.Log(landPointDistance);

        GameObject bomb = Instantiate(bombPrefab, launchPoint, Quaternion.identity);
        if (distance == shootDirTuple.Item1)
        {
            Instantiate(BombParameter.explosionEffects[0], launchPoint, Quaternion.identity);
            IHittable hittedObj = hit.transform.GetComponent<IHittable>();
            if (hittedObj != null) hittedObj.onHit(bomb.GetComponent<Bomb>(), shootDirTuple.Item3);

            Destroy(bomb);
        }

        Bomb bombScript = bomb.GetComponent<Bomb>();
        bombScript.SetDestination(launchPoint + (shootDirTuple.Item3 * landPointDistance));
        bombScript.SetPrimaryValues(Random.Range(bombParameter.fuseTimeRange.x, bombParameter.fuseTimeRange.y),
                                        Random.Range(bombParameter.radiusRange.x, bombParameter.radiusRange.y));
    }

    IEnumerator ResetTimer()
    {
        yield return new WaitForSeconds(resetTime);
        move = false;
    }
}
