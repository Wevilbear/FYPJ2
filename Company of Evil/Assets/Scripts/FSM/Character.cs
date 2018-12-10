using UnityEngine;
using System.Collections.Generic;
using FSM;

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

    //[SerializeField] Inventory inventory;
    //[SerializeField] EquipmentPanel equipmentPanel;
    //[SerializeField] StatPanel statPanel;

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
        //statPanel.SetStats(Strength, Agility, Intelligence, Vitality);
        //statPanel.UpdateStatValues();

        //inventory.OnItemRightClickedEvent += EquipFromInventory;
        //equipmentPanel.OnItemRightClickedEvent += UnequipFromEquipPanel;
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

            foreach(Node pathNode in path)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(GridManager.Instance.GetWorldPosition(pathNode), Vector3.one); 
            }

            Node startPoint = GridManager.Instance.GetClosestNodeToWorldPosition(myTransform.position);

            Gizmos.DrawWireCube(GridManager.Instance.GetWorldPosition(startPoint), Vector3.one);
        }
    }

    //private void EquipFromInventory(Item item)
    //{
    //    if (item is EquipableItem)
    //    {
    //        Equip((EquipableItem)item);
    //    }
    //}

    //private void UnequipFromEquipPanel(Item item)
    //{
    //    if (item is EquipableItem)
    //    {
    //        Unequip((EquipableItem)item);
    //    }
    //}

    //public void Equip(EquipableItem item)
    //{
    //    if (inventory.RemoveItem(item))
    //    {
    //        EquipableItem previousItem;
    //        if (equipmentPanel.AddItem(item, out previousItem))
    //        {
    //            if (previousItem != null)
    //            {
    //                inventory.AddItem(previousItem);
    //                previousItem.Unequip(this);
    //                statPanel.UpdateStatValues();
    //            }
    //            item.Equip(this);
    //            statPanel.UpdateStatValues();
    //        }
    //        else
    //        {
    //            inventory.AddItem(item);
    //        }
    //    }
    //}

    //public void Unequip(EquipableItem item)
    //{
    //    if (!inventory.IsFull() && equipmentPanel.RemoveItem(item))
    //    {
    //        item.Unequip(this);
    //        statPanel.UpdateStatValues();
    //        inventory.AddItem(item);
    //    }
    //}
}
