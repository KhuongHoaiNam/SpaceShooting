using ND_VariaBULLET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpaceShipBase : ShotCollisionDamage
{
    public float moveSpeed = 10f; // Tốc độ di chuyển của nhân vật
    [SerializeField] private bool CheckPhone = false;
    [SerializeField] private DataSpaceShip dataLevelSpaceShip;
    public Transform _pos;
    public int levelInGame =0;

    public Istate currentState;
    public StateSpaceShip stateShip;
    public void OnEnable()
    {
    }
   public void Update()
    {
        // Kiểm tra nếu có ít nhất một ngón tay chạm màn hình
        if (CheckPhone)
        {
            MovingInPhone();
        }
        else
        {

            // Lấy vị trí của chuột trên màn hình
            Vector3 mousePosition = Input.mousePosition;

            // Chuyển đổi vị trí chuột từ màn hình thành vị trí trong thế giới
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Đặt vị trí của trục z thành 0 để di chuyển trong không gian 2D
            mousePosition.z = 0;

            // Cập nhật vị trí của GameObject theo vị trí của chuột
            transform.position = mousePosition;

        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SwichState(new ShipShootingState(this));
            currentState.EnterState();
        }
        else if (Input.GetKeyDown(KeyCode.I)){
            SwichState(new ShipLevelUpState(this));
            currentState.EnterState();
        }
        currentState.UpdateState();

    }

    public void MovingInPhone()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Lấy thông tin ngón tay đầu tiên chạm màn hình

            // Chuyển đổi vị trí từ màn hình sang thế giới
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0f; // Đặt giá trị Z thành 0 để đảm bảo nhân vật vẫn ở mặt phẳng 2D

            // Di chuyển nhân vật từ từ tới vị trí ngón tay
            transform.position = Vector3.Lerp(transform.position, touchPosition, moveSpeed * Time.deltaTime);
        }
    }

    public override void Start()
    {
        base.Start();
        rend = GetComponent<SpriteRenderer>();
        NormalColor = rend.color;
        GameEventManager.Instance.StartListening(TyperEvent.OnUpdateLevelShipOnGame, updateIndex);

        SwichState(new ShipShootingState(this));
        currentState.EnterState();
        SetupIndex();
    }
       

    public void SetupIndex()
    {
        Dame = dataLevelSpaceShip.dataConfigLevelSpaceShips[levelInGame].Dm;
        HP = dataLevelSpaceShip.dataConfigLevelSpaceShips[levelInGame].Hp;
        dataLevelSpaceShip.SpawnGunbaseForLevel(levelInGame, _pos);
    }
    
    public void updateIndex()
    {
        if(levelInGame < 3)
        {
            levelInGame++;
            SetupIndex();

        }
        else
        {
            Debug.Log("supper");
        }
    }

    public virtual void SwichState(Istate state)
    {
        if (currentState != state)
        {
            //currentState.EnterState();
            currentState = state;
            currentState.EnterState();
        }
    }

    #region LevelUp
    public virtual void OnEnterLevelUpState()
    {
        updateIndex();
        GameEventManager.Instance.TriggerEvent<bool>(TyperEvent.OnUpdateShooting, false);
       
    }
    public virtual void OnUpdateLevelUpState()
    {

    }
    public virtual void OnExitLevelUpState()
    {

    }
    #endregion

    #region ShootingState
    public virtual void OnEnterShooting()
    {
        GameEventManager.Instance.TriggerEvent<bool>(TyperEvent.OnUpdateShooting, true);
    }
    public virtual void OnUpgradeShooting()
    {

    }
    public virtual void OnExitShooting()
    {

    }
    #endregion


    public void OnDisable()
    {
        GameEventManager.Instance.StopListening(TyperEvent.OnUpdateLevelShipOnGame, updateIndex);
    }

}
