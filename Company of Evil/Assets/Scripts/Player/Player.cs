﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private Stat health;

    [SerializeField]
    private Stat mana;

    [SerializeField]
    private float initHealth;

    [SerializeField]
    private float initMana;

    // Start is called before the first frame update
    protected override void Start()
    {
        health.Initialize(initHealth, initHealth);
        mana.Initialize(initMana, initMana);

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        //Executes the GetInput function
        GetInput();

        base.Update();
    }

    private void GetInput()
    {
        direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
           
        }
        if (Input.GetKeyDown(KeyCode.I))
            health.MyCurrentValue -= 10;

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
        }
    }
}