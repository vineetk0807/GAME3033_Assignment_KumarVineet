using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class IconSlot : MonoBehaviour
{
    private ItemScript Item;

    private Button ItemButton;
    private TMP_Text ItemText;

    [SerializeField] private ItemSlotAmountCanvas AmountWidget;
    [SerializeField] private ItemSlotEquippedCanvas EquippedWidget;
    [SerializeField] private Image itemIconImage;

    void Awake()
    {
        ItemButton = GetComponent<Button>();
        ItemText = GetComponentInChildren<TMP_Text>();
    }

    public void Initialize(ItemScript item)
    {
        Item = item;
        ItemText.text = Item.name;
        AmountWidget.Initialize(item);
        EquippedWidget.Initialize(item);

        // Image
        itemIconImage.sprite = item.iconImage;

        ItemButton.onClick.AddListener(UseItem);
        Item.OnItemDestroyed += OnItemDestroyed;
    }

    public void UseItem()
    {
        Debug.Log($"{Item.name} used!");
        Item.UseItem(Item.controller);
    }

    private void OnItemDestroyed()
    {
        Item = null;
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        if (Item) Item.OnItemDestroyed -= OnItemDestroyed;
    }
}