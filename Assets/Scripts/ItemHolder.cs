using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHolder : MonoBehaviour, IPointerClickHandler {
    public Item item;
    public List<Item> from;

    private TMP_Text text;

    private void Awake() {
        text = GetComponentInChildren<TMP_Text>();
    }

    public void init() {
        item = null;
        from = null;
        text.text = "";
    }

    public void SetItem(Item i, List<Item> f) {
        item = i;
        from = f;
        text.text = item.name;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            OpenContextMenu();
        }
    }

    public void OpenContextMenu() {
        HideoutManager.instance.OpenContext(item.actions, item, from);
    }
}
