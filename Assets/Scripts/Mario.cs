using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevTools;

public class Mario : Actor
{
    public int state = AppConst.MARIO_SMALL;

    [Range(0,20)]
    public float movePower = 10;

    [Range(0, 20)]
    public float jumpMinPower = 20;

    [Range(20, 30)]
    public float jumpMaxPower = 25;

    public Sprite[] mario_s;
    public Sprite[] mario_b;
    public Sprite[] mario_f;

    public RuntimeAnimatorController[] marioControllers;
    private ActivityContainer activityContainer;

    // Start is called before the first frame update
    void Start()
    {
        InitActivityContainer();
    }

    private void InitActivityContainer()
    {
        activityContainer = new ActivityContainer(this, "Mario");
    }

    // Update is called once per frame
    void Update()
    {
        activityContainer.Update();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        TryPickupItem(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        EventManager.ExecuteEvent<Collider2D>("MarioTriggerEnter", collider);
    }

    void TryPickupItem(GameObject itemObject)
    {
        ItemMarker itemMarker = itemObject.GetComponent<ItemMarker>();

        if(itemMarker)
        {
            PickupItem(itemMarker.itemType);
            Destroy(itemObject);
        }

        IScore s = itemObject.GetComponent<IScore>();

        if(s != null)
        {
            GameController.instance.AddScore(s);
        }
        else
        {
            Debug.Log("这个东西不加分");
        }
    }

    void PickupItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Mushroom_Red:
                //BeginChangeState(AppConst.MARIO_BIG);
                break;
            case ItemType.Flower:
                //if(state == AppConst.MARIO_SMALL) BeginChangeState(AppConst.MARIO_BIG);
                //else BeginChangeState(AppConst.MARIO_FIRE);
                break;
            default:
                break;
        }
    }
}
