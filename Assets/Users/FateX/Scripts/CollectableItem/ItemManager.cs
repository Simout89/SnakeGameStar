using System.Collections.Generic;

namespace Users.FateX.Scripts.CollectableItem
{
    public class ItemManager
    {
        private List<XpItem> _xpItems = new List<XpItem>();
        public IReadOnlyList<XpItem> XpItems => _xpItems;
        
        public XpItem[] GetXpItemsArray()
        {
            return _xpItems.ToArray();
        }

        public void AddXpItem(XpItem xpItem)
        {
            _xpItems.Add(xpItem);
        }

        public void RemoveXpItem(XpItem xpItem)
        {
            _xpItems.Remove(xpItem);
        }
    }
}