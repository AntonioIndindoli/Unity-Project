using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public delegate void InventoryUpdated(ArrayList inventoryItemsList);
    public static event InventoryUpdated WhenInventoryUpdated;

    public bool fireKey = false;
    public int batteryCount = 0;
    ArrayList inventoryItems = new ArrayList();

    void OnEnable()
    {
        InventoryItem.WhenItemPicked += OnItemPicked;
        FlashLightController.WhenBatteryRemoved += OnBatteryRemoved;
    }

    void OnDisable()
    {
        InventoryItem.WhenItemPicked -= OnItemPicked;
        FlashLightController.WhenBatteryRemoved -= OnBatteryRemoved;
    }

    void OnItemPicked(int itemType)
    {
        if (itemType == 1 && !fireKey)
        {
            fireKey = true;
            inventoryItems.Add("FireKey");

            if (WhenInventoryUpdated != null)
                WhenInventoryUpdated(inventoryItems);
        }
        if (itemType == 2)
        {
            batteryCount++;
            if (batteryCount == 1)
            {
                inventoryItems.Add("Battery1");
                if (WhenInventoryUpdated != null)
                    WhenInventoryUpdated(inventoryItems);
            }
            if (batteryCount == 2)
            {
                inventoryItems.Remove("Battery1");
                inventoryItems.Add("Battery2");
                if (WhenInventoryUpdated != null)
                    WhenInventoryUpdated(inventoryItems);
            }
            if (batteryCount == 3)
            {
                inventoryItems.Remove("Battery2");
                inventoryItems.Add("Battery3");
                if (WhenInventoryUpdated != null)
                    WhenInventoryUpdated(inventoryItems);
            }
            if (batteryCount == 4)
            {
                inventoryItems.Remove("Battery3");
                inventoryItems.Add("Battery4");
                if (WhenInventoryUpdated != null)
                    WhenInventoryUpdated(inventoryItems);
            }
            if (batteryCount == 5)
            {
                inventoryItems.Remove("Battery4");
                inventoryItems.Add("Battery5");
                if (WhenInventoryUpdated != null)
                    WhenInventoryUpdated(inventoryItems);
            }
            if (batteryCount == 6)
            {
                inventoryItems.Remove("Battery5");
                inventoryItems.Add("Battery6");
                if (WhenInventoryUpdated != null)
                    WhenInventoryUpdated(inventoryItems);
            }
        }
    }

    void OnBatteryRemoved()
    {

        batteryCount--;

        if (batteryCount == 0)
        {
            inventoryItems.Remove("Battery1");
            if (WhenInventoryUpdated != null)
                WhenInventoryUpdated(inventoryItems);
        }
        if (batteryCount == 1)
        {
            inventoryItems.Remove("Battery2");
            inventoryItems.Add("Battery1");
            if (WhenInventoryUpdated != null)
                WhenInventoryUpdated(inventoryItems);
        }
        if (batteryCount == 2)
        {
            inventoryItems.Remove("Battery3");
            inventoryItems.Add("Battery2");
            if (WhenInventoryUpdated != null)
                WhenInventoryUpdated(inventoryItems);
        }
        if (batteryCount == 3)
        {
            inventoryItems.Remove("Battery4");
            inventoryItems.Add("Battery3");
            if (WhenInventoryUpdated != null)
                WhenInventoryUpdated(inventoryItems);
        }
        if (batteryCount == 4)
        {
            inventoryItems.Remove("Battery5");
            inventoryItems.Add("Battery4");
            if (WhenInventoryUpdated != null)
                WhenInventoryUpdated(inventoryItems);
        }
        if (batteryCount == 5)
        {
            inventoryItems.Remove("Battery6");
            inventoryItems.Add("Battery5");
            if (WhenInventoryUpdated != null)
                WhenInventoryUpdated(inventoryItems);
        }
        if (batteryCount == 6)
        {
            inventoryItems.Remove("Battery7");
            inventoryItems.Add("Battery6");
            if (WhenInventoryUpdated != null)
                WhenInventoryUpdated(inventoryItems);
        }

    }

    void UpdateInventory()
    {
        //clearList
        inventoryItems.Clear();

        //add everything again
        if (fireKey)
        {
            inventoryItems.Add("FireKey");
        }
        if (fireKey)
        {
            inventoryItems.Add("FireKey");
        }
    }

}
