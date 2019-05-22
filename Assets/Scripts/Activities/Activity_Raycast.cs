using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTools;
public class Activity_Raycast : Activity
{
    protected Rigidbody2D body;
    protected Collider2D col;

    protected Activity_UpdateAnim updateAnim;

    public bool isOnGround;
    public bool isHurt;
    public bool isEnemyStomped;

    public Activity_Raycast()
    {
    }

    public override void SetOwner(Actor owner)
    {
        base.SetOwner(owner);
        body = owner.GetComponent<Rigidbody2D>();
        col = owner.GetComponent<Collider2D>();
        updateAnim = container.Get<Activity_UpdateAnim>();
    }

    public override void Update()
    {
        isOnGround = CheckGroundAndEnemy(out isEnemyStomped);
        isHurt = CheckHurt();

        updateAnim.isOnGround = isOnGround;

        if(isHurt)
        {
            EventManager.ExecuteEvent("RaycastHurt");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool CheckGroundAndEnemy(out bool isEnemyStomped)
    {
        body.gravityScale = 5;
        isEnemyStomped = false;

        Transform[] hitTrans;
        bool result = ThreeLineCast(col.bounds.center, Vector2.right * col.bounds.extents.x * 1.05f, Vector2.down, col.bounds.extents.y * 1.05f, out hitTrans);

        if (result)
        {
            for (int i = 0; i < hitTrans.Length; i++)
            {
                if (hitTrans[i])
                {
                    Enemy enemy = hitTrans[i].GetComponent<Enemy>();

                    if (enemy)
                    {
                        isEnemyStomped = true;
                        //enemy.OnStomp();
                        Scheduler.instance.Schedule(0.015f, false, Rebound);
                    }
                }
            }
            body.gravityScale = 1;
        }

        return result;
    }

    bool CheckHurt()
    {
        bool rightHurt = CheckHurt(Vector3.up * col.bounds.extents.y * 1.05f, Vector3.right, col.bounds.extents.x * 1.05f);
        bool leftHurt = CheckHurt(Vector3.up * col.bounds.extents.y * 1.05f, Vector3.left, col.bounds.extents.x * 1.05f);
        bool topHurt = CheckHurt(Vector3.right * col.bounds.extents.x * 1.05f, Vector3.up, col.bounds.extents.y * 1.05f);

        return rightHurt || leftHurt || topHurt;
    }

    bool CheckHurt(Vector3 offset, Vector3 direction, float distance)
    {
        Transform[] hitTrans;
        bool hurtResult = false;

        bool lineCastResult = ThreeLineCast(col.bounds.center, offset, direction, distance, out hitTrans);

        if (lineCastResult)
        {
            for (int i = 0; i < hitTrans.Length; i++)
            {
                if (hitTrans[i])
                {
                    Enemy enemy = hitTrans[i].GetComponent<Enemy>();

                    if (enemy)
                    {
                        Debug.Log("Hurt");
                        hurtResult = true;
                        break;
                    }
                }
            }
        }
        return hurtResult;
    }


    void Rebound()
    {
        body.velocity = Vector2.right * body.velocity.x + Vector2.up * 10;
    }



    bool ThreeLineCast(Vector3 center, Vector3 offset, Vector3 direction, float distance, out Transform[] hitTrans, bool queriesStartInColliders = false)
    {
        bool originQuerySetting = Physics2D.queriesStartInColliders;

        Physics2D.queriesStartInColliders = queriesStartInColliders;

        hitTrans = new Transform[3];
        hitTrans[0] = Physics2D.Raycast(center, direction, distance).transform;
        hitTrans[1] = Physics2D.Raycast(center + offset, direction, distance).transform;
        hitTrans[2] = Physics2D.Raycast(center - offset, direction, distance).transform;

        Debug.DrawLine(center, center + direction * distance, Color.red);
        Debug.DrawLine(center + offset, center + offset + direction * distance, Color.red);
        Debug.DrawLine(center - offset, center - offset + direction * distance, Color.red);

        Physics2D.queriesStartInColliders = originQuerySetting;

        return hitTrans[0] || hitTrans[1] || hitTrans[2];
    }

}
