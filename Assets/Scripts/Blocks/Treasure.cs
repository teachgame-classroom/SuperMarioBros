using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Treasure : Block
{
    public ItemType itemType;
    public AudioClip emptySound;
    private AudioClip hitSound;


    private GameObject itemInstance;

    private static List<Treasure> treasures = new List<Treasure>();

    // Start is called before the first frame update
    void Start()
    {
        OnStart();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadItemPrefab(ItemType itemType)
    {
        string path = GetItemPrefabPathByType(itemType);
        itemInstance = Resources.Load<GameObject>(path);
    }

    string GetItemPrefabPathByType(ItemType itemType)
    {
        string ret = "Prefabs/Items/";

        switch(itemType)
        {
            case ItemType.Coin:
                ret += "0_Coin_Rotate";
                break;
            case ItemType.Mushroom_Red:
                ret += "1_Mushroom_Red";
                break;
            case ItemType.Flower:
                ret += "4_Flower";
                break;
            default:
                break;
        }

        return ret;
    }

    protected override void OnStart()
    {
        if(itemType == ItemType.Coin)
        {
            hitSound = Resources.Load<AudioClip>("Sounds/smb_coin");
        }
        else
        {
            hitSound = Resources.Load<AudioClip>("Sounds/smb_powerup_appears");
        }
               
        LoadItemPrefab(itemType);

        // 每个宝箱在生成的时候，将自己加入静态列表
        treasures.Add(this);

        base.OnStart();
    }

    protected override void OnHit(GameObject gameObject)
    {
        if (currentHitCount <= 0)  // 金币已经顶空的时候，不播动画，只播顶空声音
        {
            AudioSource.PlayClipAtPoint(emptySound, Camera.main.transform.position);
        }
        else // 还有金币的时候
        {
            // 如果生成的不是金币，就延时0.5秒再生成
            if(itemType == ItemType.Coin)
            {
                SpawnItem();
            }
            else
            {
                Invoke("SpawnItem", 0.5f);
            }

            // 扣掉金币计数
            currentHitCount--;

            // 扣掉以后的剩余金币为0的时候，将动画状态机的empty变量设为true，播放顶空动画，否则播放普通顶中动画
            if (currentHitCount <= 0)
            {
                anim.SetBool("empty", true);
            }

            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
            anim.SetTrigger("hit");
        }
    }

    private void SpawnItem()
    {
        Instantiate(itemInstance, transform.position, Quaternion.identity);
    }

    protected override void BreakBlock()
    {
        base.BreakBlock();
        // 如果生成的不是金币，就延时0.5秒再生成
        if (itemType == ItemType.Coin)
        {
            SpawnItem();
        }
        else
        {
            Invoke("SpawnItem", 0.5f);
        }

        // 扣掉金币计数
        currentHitCount--;

        // 扣掉以后的剩余金币为0的时候，将动画状态机的empty变量设为true，播放顶空动画，否则播放普通顶中动画
        if (currentHitCount <= 0)
        {
            anim.SetBool("empty", true);
        }

        AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
        anim.SetTrigger("hit");

    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    private void ChangeItemByMarioState(int newState)
    {
        if(itemType == ItemType.Flower && newState == Mario.MARIO_SMALL)
        {
            itemType = ItemType.Mushroom_Red;
            LoadItemPrefab(itemType);
        }

        if (itemType == ItemType.Mushroom_Red && (newState == Mario.MARIO_BIG || newState == Mario.MARIO_FIRE) )
        {
            itemType = ItemType.Flower;
            LoadItemPrefab(itemType);
        }
    }

    public static void OnMarioStateChange(int newState)
    {
        Debug.Log("宝箱收到马里奥状态变化通知：" + newState);

        for(int i = 0; i < treasures.Count; i++)
        {
            treasures[i].ChangeItemByMarioState(newState);
        }
    }
}
