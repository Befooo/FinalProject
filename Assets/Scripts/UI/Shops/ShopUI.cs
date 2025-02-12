using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Shops;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        private Shopper shopper = null;
        Shop currentShop = null;
        // Start is called before the first frame update
        void Start()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
            if(shopper == null) return;
            
            shopper.activeShopChange += ShopChanged;
            
            ShopChanged();
        }

        void ShopChanged()
        {
            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);
        }
    }
}
