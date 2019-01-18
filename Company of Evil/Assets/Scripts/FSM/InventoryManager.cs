using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{

    //[SerializeField] Inventory inventory;
    //[SerializeField] EquipmentPanel equipmentPanel;
    //[SerializeField] StatPanel statPanel;

    //public CharacterStat Strength;
    //public CharacterStat Agility;
    //public CharacterStat Intelligence;
    //public CharacterStat Vitality;

    //private void Awake()
    //{

    //    statPanel.SetStats(Strength, Agility, Intelligence, Vitality);
    //    statPanel.UpdateStatValues();

    //    inventory.OnItemRightClickedEvent += EquipFromInventory;
    //    equipmentPanel.OnItemRightClickedEvent += UnequipFromEquipPanel;
    //}

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

