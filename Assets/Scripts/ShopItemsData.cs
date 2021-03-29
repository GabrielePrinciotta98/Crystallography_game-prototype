using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShopItemsData
{
    private static List<ShopItem> shopitems;
    /*
    private const string ZoomDescription = "It allows you to zoom the diffraction pattern in and out.";
    private const string LambdaDescription = "It allows you to increase or reduce the wavelength of X-rays.";

    private const string PowerDescription = "It allows you to increase or reduce the power of X-rays" +
                                            " and therefore change the intensity of the diffraction pattern.";

    private const string RotationDescription = "It allows you to rotate the molecule or the entire crystal along the vertical axis.";
    private const string ZoomInfo = "Zoom info";
    private const string LambdaInfo = "Lambda info";
    private const string PowerInfo = "Power info";
    private const string RotationInfo = "Rotation info";
 */
    private const string ZoomDescription = "Permette di ingrandire o rimpicciolire il pattern di diffrazione.";
    private const string LambdaDescription = "Permette di aumentare o ridurre la lunghezza d'onda dei raggi X.";

    private const string PowerDescription = "Permette di aumentare o ridurre la potenza dei raggi X," +
                                            " e quindi cambiare l'intensità del pattern di diffrazione.";

    private const string RotationDescription = "Permette di ruotare la molecola o l'intero cristallo rispetto all'asse verticale.";
    private const string SwapDescription = "Permette di proiettare il pattern della soluzione anche sul detector" +
                                           " di gioco per confrontare meglio i due pattern di diffrazione." ;

    private const string ZoomInfo = "Zoom info";
    private const string LambdaInfo = "Lambda info";
    private const string PowerInfo = "Power info";
    private const string RotationInfo = "Rotation info";
    private const string SwapInfo = "Swap info";

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
            shopitems.Add(new ShopItem(4, SwapDescription, 500, SwapInfo));
        }
    }
}
