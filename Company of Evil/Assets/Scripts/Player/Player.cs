using System.Collections;
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

    [SerializeField]
    private GameObject[] spellPrefab;

    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex = 2;

    private Transform target;

    [SerializeField]
    private Block[] blocks;

    // Start is called before the first frame update
    protected override void Start()
    {
        health.Initialize(initHealth, initHealth);
        mana.Initialize(initMana, initMana);
        target = GameObject.Find("Target").transform;
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
            exitIndex = 0;
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            exitIndex = 2;
            direction += Vector2.down;

        }
        if (Input.GetKeyDown(KeyCode.I))
            health.MyCurrentValue -= 10;

        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 1;
            direction += Vector2.right;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Block();
            if (!isAttacking && !IsMoving && InLineOfSight())
            {
                attackRoutine = StartCoroutine(Attack());
            }

        }
    }



    private IEnumerator Attack()
    {


        isAttacking = true;

        myAnimator.SetBool("attack", isAttacking);

        //hardcodded must remove
        yield return new WaitForSeconds(1);
        CastSpell();

        StopAttack();
    }

    public void CastSpell()
    {
        Instantiate(spellPrefab[0], exitPoints[exitIndex].position, Quaternion.identity);

    }

    private bool InLineOfSight()
    {
        Vector3 targetDirection = (target.transform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, target.transform.position), 512);


        if (hit.collider == null)
        {
            return true;
        }

        return false;
    }

    private void Block()
    {
        foreach (Block b in blocks)
        {
            b.Deactivate();
        }
        blocks[exitIndex].Activate();
    }
}
