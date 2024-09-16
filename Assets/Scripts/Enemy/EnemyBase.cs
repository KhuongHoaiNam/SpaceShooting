using ND_VariaBULLET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AAGame;
// Thêm dòng này
public abstract class EnemyBase : ShotCollisionDamage
{
    public Istate currentState;
    public EnemyState enemystate;
    public EnemyInfo EnemyInfo;
    // Các điểm đường dẫn
    private List<Vector3> pathPoints;
    private int currentPointIndex = 0;

    // Tốc độ di chuyển
    [SerializeField] private float moveSpeed = 5.0f;

    // Đối tượng PathCreator để lấy các điểm 
    private PathCreator pathCreator;
    public Vector3 endPos;
    public bool isKill = false;

    public virtual void SwichState(Istate state)
    {
        if (currentState != state)
        {
            //currentState.EnterState();
            currentState = state;
            currentState.EnterState();
        }
    }

    public override void Start()
    {
        base.Start();
        rend = GetComponent<SpriteRenderer>();
        NormalColor = rend.color;
        //  SetupIndex();
        SwichState(new SpawnerState(this));
        currentState.EnterState();
    }

    public override void setDamage(float damage)
    {
        base.setDamage(damage);
        HP -= damage;
        if (HP <= 0)
        {
            if (DeathExplosion != "")
            {
                string explosion = DeathExplosion;
                GameObject finalExplode = GlobalShotManager.Instance.ExplosionRequest(explosion, this);

                finalExplode.transform.position = this.transform.position;
                finalExplode.transform.parent = null;
                finalExplode.transform.localScale = new Vector2(finalExplode.transform.localScale.x * FinalExplodeFactor, finalExplode.transform.localScale.y * FinalExplodeFactor);

            }
            DropItemManager.Instance.DropItem(Item.upgradeitem, 1, this.transform);
            Destroy(this.gameObject);
            LevelControler.Instance.lstEnemySpawner.Remove(this);
            LevelControler.Instance.CheckSpawner();
        }
    }

    #region EnemyState

    #region SpawnState
    public virtual void EnterSpawnState()
    {

    }
    public virtual void UpdateSpawnState()
    {

    }
    public virtual void ExitSpawnState() 
    { 
    
    }
    #endregion

    #endregion

    #region EnemyMoving
    // di chuyen theo duong
    public void SetPathCreator(PathCreator pathCreator, int idLIne)
    {
        this.pathCreator = pathCreator;
        pathPoints = pathCreator.getPoints(idLIne); // Lấy đường dẫn đầu tiên
        StartCoroutine(MoveAlongPath());
       
    }

    private IEnumerator MoveAlongPath()
    {
        while (currentPointIndex < pathPoints.Count)
        {
            Vector3 targetPoint = pathPoints[currentPointIndex];
            while (Vector3.Distance(transform.position, targetPoint) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
                yield return null;
            }
            currentPointIndex++;
        }
        StartCoroutine(MovingEndPos());
    }

    public IEnumerator MovingEndPos()
    {
        while (Vector3.Distance(transform.position, endPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
    #endregion
}
public enum EnemyState
{   
    none,
    SpawnState,
}