namespace Fishing.UI {
    public class BaitInventorySlot : BaitUI {
        private BaitInfoMenu _baitInfoMenu;

        private void Awake() {
            _baitInfoMenu = BaitInfoMenu.Instance;
        }

        public void UpdateInfoMenu() {
            _baitInfoMenu.gameObject.SetActive(true);
            _baitInfoMenu.UpdateBaitInfoMenu(_baitScriptable);
        }
    }
}