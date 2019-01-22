using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChest : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] Inventory inventory;
    [SerializeField] SpriteRenderer SpriteRenderer;
    [SerializeField] Color emptyColor;
    [SerializeField] KeyCode itemPickupKeyCode = KeyCode.E;


    private bool isInRange;
    private bool isEmpty;

    private void Start()
    {

        SpriteRenderer.sprite = item.Icon;
        SpriteRenderer.enabled = false;
    }

    private void OnValidate()
    {
        if (inventory == null)
            inventory = FindObjectOfType<Inventory>();

        if (SpriteRenderer == null)
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

    private void Update()
    {

        if (isInRange && Input.GetKeyDown(itemPickupKeyCode))
        {
            if (!isEmpty)
            {
                inventory.AddItem(item);
                isEmpty = true;
                SpriteRenderer.color = emptyColor;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckCollision(other.gameObject, true);

    }

    private void OnTriggerExit(Collider other)
    {
        CheckCollision(other.gameObject, false);

    }

    private void CheckCollision(GameObject gameObject,bool state)
    {
        if (gameObject.CompareTag("Player"))
        {
            isInRange = state;
            SpriteRenderer.enabled = state;
        }
    }
}
