﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_Move : Activity
{
    private float movePower;
    private Rigidbody2D body;

    public Activity_Move(MonoBehaviour owner, float movePower)
    {
        body = owner.GetComponent<Rigidbody2D>();
        this.movePower = movePower;
    }

    public override void Update()
    {
        float h = Input.GetAxis("Horizontal");

        Vector3 force = Vector3.right * h;

        if (Time.timeScale > 0)
        {
            body.AddForce(force * movePower);
        }
    }
}