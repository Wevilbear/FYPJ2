using UnityEngine;
using System.Collections.Generic;
using FSM;
using UnityEngine.UI;

public enum MovementAnimation
{
    Idle,
    Run
}

[RequireComponent(typeof(StateMachine))]
public class Character : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Transform target;

    [SerializeField] private float singleMoveTime = 0.5f;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;

    private ItemSlot draggedSlot;



    private StateMachine fsm;

    private const string idleTrigger = "Idle";
    private const string runTrigger = "Run";

    Queue<Node> path;

    public CharacterStat Strength;
    public CharacterStat Agility;
    public CharacterStat Intelligence;
    public CharacterStat Vitality;

    private void Awake()
    {
        statPanel.SetStats(Strength, Agility, Intelligence, Vitality);
        statPanel.UpdateStatValues();

        //Setup Events:

        //Right Click
        inventory.OnRightClickEvent += Equip;
        equipmentPanel.OnRightClickEvent += Unequip;
        //Pointer Enter
        inventory.OnPointerEnterEvent += ShowTooltip;
        equipmentPanel.OnPointerEnterEvent += ShowTooltip;
        //Pointer Exit
        inventory.OnPointerExitEvent += HideTooltip;
        equipmentPanel.OnPointerExitEvent += HideTooltip;
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

    }

    private void Start()
    {
        fsm = GetComponent<StateMachine>();

        fsm.Initialize(this);

        Messenger.OnCheckNode += OnCheckNode;
        Messenger.OnSelectedTarget += OnSelectedTarget;
    }

    private void OnDestroy()
    {
        Messenger.OnCheckNode -= OnCheckNode;
        Messenger.OnSelectedTarget -= OnSelectedTarget;
    }

    private void MoveToTarget(Node targetNode)
    {
        path = GetPath(targetNode);

        fsm.ChangeState(new FollowState(fsm, path, singleMoveTime, rotationSpeed));
    }

    private Queue<Node> GetPath(Node targetNode)
    {
        Queue<Node> path = null;

        Node startNode = GridManager.Instance.GetClosestNodeToWorldPosition(Transform.position);

        List<Node> nodes = GridManager.Instance.GetPath(startNode, targetNode);

        if (nodes != null && nodes.Count > 0)
        {
            path = new Queue<Node>();

            for (int i = 0; i < nodes.Count; i++)
            {
                path.Enqueue(nodes[i]);
            }
        }

        return path;
    }

    private void OnCheckNode(Node node)
    {
        path = GetPath(node);
    }

    private void OnSelectedTarget(Node node)
    {
        MoveToTarget(node);
    }

    public void PlayMovementAnimation(MovementAnimation movementAnimation)
    {
        switch (movementAnimation)
        {
            case MovementAnimation.Idle:
                animator.ResetTrigger(runTrigger);
                animator.SetTrigger(idleTrigger);
                break;
            case MovementAnimation.Run:
                animator.ResetTrigger(idleTrigger);
                animator.SetTrigger(runTrigger);
                break;
        }
    }

    private Transform myTransform;

    public Transform Transform
    {
        get
        {
            if (myTransform == null)
            {
                myTransform = GetComponent<Transform>();
            }

            return myTransform;
        }

        set
        {
            myTransform = value;
        }
    }

    private void OnDrawGizmos()
    {
        TryToDrawPath();
    }

    private void TryToDrawPath()
    {
        if (path != null && path.Count > 0)
        {
            Vector3 offset = Vector3.up;

            foreach (Node pathNode in path)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(GridManager.Instance.GetWorldPosition(pathNode), Vector3.one);
            }

            Node startPoint = GridManager.Instance.GetClosestNodeToWorldPosition(myTransform.position);

            Gizmos.DrawWireCube(GridManager.Instance.GetWorldPosition(startPoint), Vector3.one);
        }
    }

    //INVENTORY CODE

    private void Equip(ItemSlot itemslot)
    {
        EquipableItem equipableItem = itemslot.Item as EquipableItem;
        if (equipableItem != null)
        {
            Equip(equipableItem);
        }
    }

    private void Unequip(ItemSlot itemslot)
    {
        EquipableItem equipableItem = itemslot.Item as EquipableItem;
        if (equipableItem != null)
        {
            Unequip(equipableItem);
        }
    }

    private void ShowTooltip(ItemSlot itemSlot)
    {
        EquipableItem equipableItem = itemSlot.Item as EquipableItem;
        if (equipableItem != null)
        {
            itemTooltip.ShowTooltip(equipableItem);
        }
    }

    private void HideTooltip(ItemSlot itemSlot)
    {
        itemTooltip.HideTooltip();
    }

    private void BeginDrag(ItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            draggedSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.Icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.enabled = true;
        }
    }

    private void EndDrag(ItemSlot itemSlot)
    {
        draggedSlot = null;
        draggableItem.enabled = false;
    }

    private void Drag(ItemSlot itemSlot)
    {
        if (draggableItem.enabled)
        {
            draggableItem.transform.position = Input.mousePosition;
        }

    }

    private void Drop(ItemSlot dropItemSlot)
    {
        if(dropItemSlot.CanReceiveItem(draggedSlot.Item) && draggedSlot.CanReceiveItem(dropItemSlot.Item))
        {
            EquipableItem dragItem = draggedSlot.Item as EquipableItem;
            EquipableItem dropItem = dropItemSlot.Item as EquipableItem;

            if(draggedSlot is EquipmentSlot)
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

            Item draggedItem = draggedSlot.Item;
            draggedSlot.Item = dropItemSlot.Item;
            dropItemSlot.Item = draggedItem;
        }
       
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

    private void OnValidate()
    {
        if (itemTooltip == null)
            itemTooltip = FindObjectOfType<ItemTooltip>();
    }
}
