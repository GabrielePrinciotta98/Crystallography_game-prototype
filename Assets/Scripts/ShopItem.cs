using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem
{
    private int id;

    public ShopItem(int id, string description, int price, string info)
    {
        this.id = id;
        this.Description = description;
        this.Price = price;
        this.Info = info;
    }

    public string Description { get; set; }

    public int Price { get; set; }

    public bool Sold { get; set; }

    public string Info { get; set; }
}
