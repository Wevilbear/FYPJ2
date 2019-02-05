using UnityEngine;
using System.Collections.Generic;
using FSM;
using UnityEngine.UI;

public enum MovementAnimation
{
    Idle,
    Run
}

//[RequireComponent(typeof(StateMachine))]
public class Character : MonoBehaviour
{
    //[SerializeField] private Animator animator;

    //[SerializeField] private Transform target;

    //[SerializeField] private float singleMoveTime = 0.5f;
    //[SerializeField] private float rotationSpeed = 5f;

    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] CraftingWindow craftingWindow;
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;

    private BaseItemSlot dragItemSlot;

    //private StateMachine fsm;

    private const string idleTrigger = "Idle";
    private const string runTrigger = "Run";

    //Queue<Node> path;

    public CharacterStat Strength;
    public CharacterStat Agility;
    public CharacterStat Intelligence;
    public CharacterStat Vitality;

    [SerializeField] float speed;

    protected Animator myAnimator;

    protected Vector2 direction;

    private Rigidbody2D myRigidbody;

    protected bool isAttacking;

    protected Coroutine attackRoutine;

    [SerializeField]
    protected Transform hitBox;

    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }

    protected virtual void Start()
    {
        statPanel.SetStats(Strength, Agility, Intelligence, Vitality);
        statPanel.UpdateStatValues();

        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        //Setup Events:

        //Right Click
        inventory.OnRightClickEvent += Equip;
        equipmentPanel.OnRightClickEvent += Unequip;
        //Pointer Enter
        inventory.OnPointerEnterEvent += ShowTooltip;
        equipmentPanel.OnPointerEnterEvent += ShowTooltip;
        craftingWindow.OnPointerEnterEvent += ShowTooltip;
        //Pointer Exit
        inventory.OnPointerExitEvent += HideTooltip;
        equipmentPanel.OnPointerExitEvent += HideTooltip;
        craftingWindow.OnPointerExitEvent += HideTooltip;
        //Begin Drag
        inventory.OnBeginDragEvent += BeginDrag;
        equipmentPanel.OnBeginDragEvent += BeginDrag;
        //End Drag
        inventory.OnEndDragEvent += EndDrag;
        equipmentPanel.OnEndDragEvent += EndDrag;
        //Drag
        inventory.OnDragEvent += Drag;
        equipmentPanel.OnDragEvent += Drag;
        //Drop
        inventory.OnDropEvent += Drop;
        equipmentPanel.OnDropEvent += Drop;

        //fsm = GetComponent<StateMachine>();

        //fsm.Initialize(this);

        //Messenger.OnCheckNode += OnCheckNode;
        //Messenger.OnSelectedTarget += OnSelectedTarget;

        Debug.Log("Character is started");
    }



    //private void OnDestroy()
    //{
    //    Messenger.OnCheckNode -= OnCheckNode;
    //    Messenger.OnSelectedTarget -= OnSelectedTarget;
    //}

    //private void MoveToTarget(Node targetNode)
    //{
    //    path = GetPath(targetNode);

    //    fsm.ChangeState(new FollowState(fsm, path, singleMoveTime, rotationSpeed));
    //}

    //private Queue<Node> GetPath(Node targetNode)
    //{
    //    Queue<Node> path = null;

    //    Node startNode = GridManager.Instance.GetClosestNodeToWorldPosition(Transform.position);

    //    List<Node> nodes = GridManager.Instance.GetPath(startNode, targetNode);

    //    if (nodes != null && nodes.Count > 0)
    //    {
    //        path = new Queue<Node>();

    //        for (int i = 0; i < nodes.Count; i++)
    //        {
    //            path.Enqueue(nodes[i]);
    //        }
    //    }

    //    return path;
    //}

    //private void OnCheckNode(Node node)
    //{
    //    path = GetPath(node);
    //}

    //private void OnSelectedTarget(Node node)
    //{
    //    MoveToTarget(node);
    //}

    //public void PlayMovementAnimation(MovementAnimation movementAnimation)
    //{
    //    switch (movementAnimation)
    //    {
    //        case MovementAnimation.Idle:
    //            animator.ResetTrigger(runTrigger);
    //            animator.SetTrigger(idleTrigger);
    //            break;
    //        case MovementAnimation.Run:
    //            animator.ResetTrigger(idleTrigger);
    //            animator.SetTrigger(runTrigger);
    //            break;
    //    }
    //}

    //private Transform myTransform;

    //public Transform Transform
    //{
    //    get
    //    {
    //        if (myTransform == null)
    //        {
    //            myTransform = GetComponent<Transform>();
    //        }

    //        return myTrahannsform;
    //    }

    //    set
    //    {
    //        myTransform = value;
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    TryToDrawPath();
    //}

    //private void TryToDrawPath()
    //{
    //    if (path != null && path.Count > 0)
    //    {
    //        Vector3 offset = Vector3.up;

    //        foreach (Node pathNode in path)
    //        {
    //            Gizmos.color = Color.red;
    //            Gizmos.DrawWireCube(GridManager.Instance.GetWorldPosition(pathNode), Vector3.one);
    //        }

    //        Node startPoint = GridManager.Instance.GetClosestNodeToWorldPosition(myTransform.position);

    //        Gizmos.DrawWireCube(GridManager.Instance.GetWorldPosition(startPoint), Vector3.one);
    //    }
    //}

    //INVENTORY CODE

    private void Equip(BaseItemSlot itemslot)
    {
        EquipableItem equipableItem = itemslot.Item as EquipableItem;
        if (equipableItem != null)
        {
            Equip(equipableItem);
        }
    }

    private void Unequip(BaseItemSlot itemslot)
    {
        EquipableItem equipableItem = itemslot.Item as EquipableItem;
        if (equipableItem != null)
        {
            Unequip(equipableItem);
        }
    }

    private void ShowTooltip(BaseItemSlot itemSlot)
    {
        EquipableItem equipableItem = itemSlot.Item as EquipableItem;
        if (equipableItem != null)
        {
            itemTooltip.ShowTooltip(equipableItem);
        }
    }

    private void HideTooltip(BaseItemSlot itemSlot)
    {
        itemTooltip.HideTooltip();
    }

    private void BeginDrag(BaseItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            dragItemSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.Icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.enabled = true;
        }
    }

    private void EndDrag(BaseItemSlot itemSlot)
    {
        dragItemSlot = null;
        draggableItem.enabled = false;
    }

    private void Drag(BaseItemSlot itemSlot)
    {
        if (draggableItem.enabled)
        {
            draggableItem.transform.position = Input.mousePosition;
        }

    }

    private void Drop(BaseItemSlot dropItemSlot)
    {
        if (dragItemSlot == null) return;

        if (dropItemSlot.CanAddStack(dragItemSlot.Item))
        {
            AddStacks(dropItemSlot);
        }

        else if (dropItemSlot.CanReceiveItem(dragItemSlot.Item) && dragItemSlot.CanReceiveItem(dropItemSlot.Item))
        {
            SwapItems(dropItemSlot);
        }

    }

    private void SwapItems(BaseItemSlot dropItemSlot)
    {
        EquipableItem dragItem = dragItemSlot.Item as EquipableItem;
        EquipableItem dropItem = dropItemSlot.Item as EquipableItem;

        if (dragItemSlot is EquipmentSlot)
        {
            if (dragItem != null)
                dragItem.Unequip(this);
            if (dropItem != null)
                dropItem.Equip(this);
        }

        if (dropItemSlot is EquipmentSlot)
        {
            if (dragItem != null)
                dragItem.Equip(this);
            if (dropItem != null)
                dropItem.Unequip(this);
        }
        statPanel.UpdateStatValues();

        Item draggedItem = dragItemSlot.Item;
        int draggedItemAmount = dragItemSlot.Amount;

        dragItemSlot.Item = dropItemSlot.Item;
        dragItemSlot.Amount = dropItemSlot.Amount;

        dropItemSlot.Item = draggedItem;
        dropItemSlot.Amount = draggedItemAmount;
    }

    private void AddStacks(BaseItemSlot dropItemSlot)
    {
        int numAddableStacks = dropItemSlot.Item.MaximumStacks - dropItemSlot.Amount;
        int stacksToAdd = Mathf.Min(numAddableStacks, dragItemSlot.Amount);

        dropItemSlot.Amount += stacksToAdd;
        dragItemSlot.Amount -= stacksToAdd;
    }

    public void Equip(EquipableItem item)
    {
        if (inventory.RemoveItem(item))
        {
            EquipableItem previousItem;
            if (equipmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(this);
                    statPanel.UpdateStatValues();
                }
                item.Equip(this);
                statPanel.UpdateStatValues();
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquipableItem item)
    {
        if (!inventory.IsFull() && equipmentPanel.RemoveItem(item))
        {
            item.Unequip(this);
            statPanel.UpdateStatValues();
            inventory.AddItem(item);
        }
    }

    //Movement code

    protected virtual void Update()
    {
        HandleLayers();
       
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
       myRigidbody.velocity = direction.normalized *speed;
   
    }

    public void HandleLayers()
    {
        //Checks if we are moving or standing still, if we are moving then we need to play the move animation
        if (IsMoving)
        {
            ActivateLayer("WalkLayer");

            //Sets the animation parameter so that he faces the correct direction
            myAnimator.SetFloat("x", direction.x);
            myAnimator.SetFloat("y", direction.y);

            StopAttack();
        }
        else if (isAttacking)
        {
            ActivateLayer("AttackLayer");
        }
        else
        {
            //Makes sure that we will go back to idle when we aren't pressing any keys.
            ActivateLayer("IdleLayer");
        }
    }

    

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < myAnimator.layerCount; i++)
        {
            myAnimator.SetLayerWeight(i, 0);
        }

        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName),1);
    }

    public virtual void StopAttack()
    {
        isAttacking = false; //Makes sure that we are not attacking

        myAnimator.SetBool("attack", isAttacking); //Stops the attack animation

        if (attackRoutine != null) //Checks if we have a reference to an co routine
        {
            StopCoroutine(attackRoutine);
        }
    }

    private void OnValidate()
    {
        if (itemTooltip == null)
            itemTooltip = FindObjectOfType<ItemTooltip>();


    }
}
