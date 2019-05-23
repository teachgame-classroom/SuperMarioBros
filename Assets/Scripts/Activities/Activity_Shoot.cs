using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTools;

public class Activity_Shoot : Activity
{
    protected Activity_Input input;
    protected Activity_StateManagement stateManagement;

    protected GameObject bulletPrefab;
    protected Transform shotPos;

    protected AudioClip shootClip;

    

    public Activity_Shoot() : base()
    {
    }

    public override void SetOwner(Actor owner)
    {
        base.SetOwner(owner);
        shotPos = owner.transform.Find("ShotPos");
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Mario_Bullet");
        shootClip = Resources.Load<AudioClip>("Sounds/smb_fireball");

        stateManagement = container.Get<Activity_StateManagement>();

        input = container.Get<Activity_Input>();
        input.onButtonDown_Fire += Shoot;
    }

    public override void Update()
    {
        
    }

    private void Shoot()
    {
        if(stateManagement.state == AppConst.MARIO_FIRE)
        {
            GameObject.Instantiate(bulletPrefab, shotPos.position, shotPos.rotation);
            AudioSource.PlayClipAtPoint(shootClip, Camera.main.transform.position);
            EventManager.ExecuteEvent("MarioShoot");
        }
    }

}
