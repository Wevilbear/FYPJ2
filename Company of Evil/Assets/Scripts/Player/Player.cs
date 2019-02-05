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

    private SpellBook spellBook;

    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex = 2;

    public Transform MyTarget { get; set; }

    [SerializeField]
    private Block[] blocks;

    // Start is called before the first frame update
    protected override void Start()
    {
        spellBook = GetComponent<SpellBook>();
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


    }



    private IEnumerator Attack(int spellIndex)
    {
        Transform currentTarget = MyTarget; 

        Spell newSpell = spellBook.CastSpell(spellIndex);

        isAttacking = true;

        myAnimator.SetBool("attack", isAttacking);

        //hardcodded must remove
        yield return new WaitForSeconds(newSpell.MyCastTime);

        if(currentTarget != null && InLineOfSight())
        {
            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

            s.MyTarget = currentTarget;
        }

      

        StopAttack();
    }

    public void CastSpell(int spellIndex)
    {
        Block();
        if (MyTarget != null && !isAttacking && !IsMoving && InLineOfSight())
        {
            attackRoutine = StartCoroutine(Attack(spellIndex));
        }



    }

    private bool InLineOfSight()
    {
        Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 512);


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

    public override void StopAttack()
    {
        //Stop the spellbook from casting
        spellBook.StopCasting();

        //Makes sure that we stop the cast in our character.
        base.StopAttack();
    }
}
