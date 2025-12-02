using System;
using System.Collections.Generic;
using Users.FateX.Scripts.Data;

namespace Users.FateX.Scripts.Shop
{
    [Serializable]
    public class ShopSaveData
    {
        public List<ShopProductProgress> ProductsProgress = new List<ShopProductProgress>();
    }
}