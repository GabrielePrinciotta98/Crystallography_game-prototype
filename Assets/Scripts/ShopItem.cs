public class ShopItem
{
    private int id;

    public ShopItem(int id, string description, int price, string info, bool unlocked)
    {
        this.id = id;
        Description = description;
        Price = price;
        Info = info;
    }

    public string Description { get; set; }

    public int Price { get; set; }

    public bool Sold { get; set; }
    
    public bool Unlocked { get; set; }
    
    public bool Buyable { get; set; }

    public string Info { get; set; }
}
