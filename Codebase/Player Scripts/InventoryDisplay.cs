using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    Image[] images;
    public Sprite emptySprite;

    public Sprite fireKeySprite;
    public Sprite batterySprite1;
    public Sprite batterySprite2;
    public Sprite batterySprite3;
    public Sprite batterySprite4;
    public Sprite batterySprite5;
    public Sprite batterySprite6;
    public ArrayList inventoryManagerItems;

    // Start is called before the first frame update
    void Start()
    {
        images = this.GetComponentsInChildren<Image>(false);
    }

    void OnEnable()
    {
        InventoryManager.WhenInventoryUpdated += OnUpdate;
    }

    void OnDisable()
    {
        InventoryManager.WhenInventoryUpdated -= OnUpdate;
    }

    void OnUpdate(ArrayList inventoryManagerItemsUpdated)
    {
        inventoryManagerItems = inventoryManagerItemsUpdated;

        for (int i = 0; i < images.Length-1; i++)
        {
            images[i + 1].sprite = emptySprite;
        }

        for (int i = 0; i < inventoryManagerItems.Count; i++)
        {
            if (inventoryManagerItems[i].Equals("FireKey"))
            {
                images[i + 1].sprite = fireKeySprite;
            }
            else if (inventoryManagerItems[i].Equals("Battery1"))
            {
                images[i + 1].sprite = batterySprite1;
            }
            else if (inventoryManagerItems[i].Equals("Battery2"))
            {
                images[i + 1].sprite = batterySprite2;
            }
            else if (inventoryManagerItems[i].Equals("Battery3"))
            {
                images[i + 1].sprite = batterySprite3;
            }
            else if (inventoryManagerItems[i].Equals("Battery4"))
            {
                images[i + 1].sprite = batterySprite4;
            }
            else if (inventoryManagerItems[i].Equals("Battery5"))
            {
                images[i + 1].sprite = batterySprite5;
            }
            else if (inventoryManagerItems[i].Equals("Battery6"))
            {
                images[i + 1].sprite = batterySprite6;
            }
        }
    }

}
