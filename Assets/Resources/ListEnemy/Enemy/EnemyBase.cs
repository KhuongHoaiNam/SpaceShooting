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
    private List<Vector3> pathPoints;
    private int currentPointIndex = 0;

    [SerializeField] private float moveSpeed = 5.0f;

    private PathCreator pathCreator;
    public Vector3 endPos;
    public bool isKill = false;
    public GameObject objGun;
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
            LevelControler.Instance.SwitchWave();
        }
    }


    #region EnemySpawner
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
            SwichState(new IdleStateEnemy(this));
            currentState.EnterState();
            yield return null;
        }
    }
    #endregion

    #region EnemyIdle
    public void EnterIdleState()
    {
        enemystate = EnemyState.IdleState;
    }
    public void ExitIdleState() { }
    public void UpdateIdleState() { }
    #endregion



    #region EnemyAttack

    public void EnterAttackState()
    {
        enemystate = EnemyState.AttackState;
        objGun.gameObject.SetActive(true);
    }
    public void ExitAttackStates() {

        objGun.gameObject.SetActive(false);
    }
    public void UpdateAttackState() { }
    #endregion


    public virtual void OnMovingEnemes()
    {

    }

}
public enum EnemyState
{   
    none,
    IdleState,
    MovingState,
    AttackState,
}