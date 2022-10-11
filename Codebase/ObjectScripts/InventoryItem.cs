using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public delegate void ItemPicked(int itemType);
    public static event ItemPicked WhenItemPicked;

    public int itemType;

    public void isInteracted()
    {
        Destroy(this.gameObject);

        if (WhenItemPicked != null)
            WhenItemPicked(itemType);
    }


}
