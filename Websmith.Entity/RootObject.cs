public class Configuration
{
    public string price { get; set; }
    public string recharge_number { get; set; }
}

public class MetaData
{
    public string protection_url { get; set; }
}

public class CartItem
{
    public int product_id { get; set; }
    public int qty { get; set; }
    public Configuration configuration { get; set; }
    public MetaData meta_data { get; set; }
}

public class RootObject
{
    public List<CartItem> cart_items { get; set; }
}