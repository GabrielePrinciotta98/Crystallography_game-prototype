using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShopItemsData
{
    private static List<ShopItem> shopitems;
    private const string ZoomDescription = "It allows you to zoom the diffraction pattern in and out.";
    private const string LambdaDescription = "It allows you to increase or reduce the wavelength of X-rays.";

    private const string PowerDescription = "It allows you to increase or reduce the power of X-rays" +
                                            " and therefore change the intensity of the diffraction pattern.";

    private const string RotationDescription = "It allows you to rotate the molecule or the entire crystal along the vertical axis.";
    private const string ZoomInfo = "Zoom info";
    private const string LambdaInfo = "Lambda info";
    private const string PowerInfo = "Power info";
    private const string RotationInfo = "Rotation info";
 
    
    public static List<ShopItem> ShopItems
    {
        get => shopitems;
        set
        {
            shopitems = value;
            
            shopitems.Add(new ShopItem(0, ZoomDescription, 2500, ZoomInfo));
            shopitems.Add(new ShopItem(1, LambdaDescription, 3000, LambdaInfo));
            shopitems.Add(new ShopItem(2, PowerDescription, 1000, PowerInfo));
            shopitems.Add(new ShopItem(3, RotationDescription, 5000, RotationInfo));

            
        }
    }
}
