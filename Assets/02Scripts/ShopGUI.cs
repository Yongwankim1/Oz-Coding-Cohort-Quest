using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopGUI : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] PlayerInputReader inputReader;
    [SerializeField] ShopItemTable itemTable;
    [SerializeField] ShopManager shopManager;
    [SerializeField] TextMeshProUGUI itemNameDescription;
    [SerializeField] TextMeshProUGUI itemPriceText;
    [SerializeField] Transform slotParent;
    [SerializeField] ShopSlotUI slotPrefab;

    //배치될 슬롯들 사용하지 않으면 꺼버림
    private ShopSlotUI[] slots;
    [SerializeField] private ShopSlotUI selectSlot;

    public ShopSlotUI SelectSlot => selectSlot;

    public event Action LastSlot;
    private void Awake()
    {
        slots = new ShopSlotUI[0];
    }
    public void Init(ShopItemTable itemTable)
    {
        this.itemTable = itemTable;
        shopManager = itemTable.GetComponent<ShopManager>();
        if(slots.Length < itemTable.Count)
        {
            ShopSlotUI[] slots = new ShopSlotUI[itemTable.Count];
            for (int i = 0; i < this.slots.Length; i++)
            {
                slots[i] = this.slots[i];
            }
            this.slots = slots;
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) slots[i] = Instantiate(slotPrefab, slotParent);
            continue;
        }


        OnEquipSlot();
        LastSlot?.Invoke();
        
    }
    public void SetSelectSlot(ShopSlotUI slotUI)
    {
        if(string.IsNullOrEmpty(slotUI.ItemID)) return;
        if (!ItemCatalogManager.Instance.TryGetItemData(slotUI.ItemID, out var data)) return;
        selectSlot = slotUI;
        itemNameDescription.text = $"{data.DisplayName} : {data.Description}";
        itemPriceText.text = $"가격 : {data.ItemPrice} G";
    }
    private void OnEnable()
    {
        if(itemTable != null) LastSlot?.Invoke();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inputReader.CanMove = false;
    }
    private void OnDisable()
    {
        selectSlot = null;
        itemNameDescription.text = string.Empty;
        itemPriceText.text = string.Empty;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputReader.CanMove = true;
    }
    public void OnBuyBtn()
    {
        if (shopManager == null) return;
        if(selectSlot == null) return;
        if(string.IsNullOrEmpty(selectSlot.ItemID)) return;
        shopManager.Buy(selectSlot.ItemID);
    }
    public void OnEquipSlot()
    {
        SetSlot(itemTable.EquipList);
        LastSlot = null;
        LastSlot += OnEquipSlot;
    }
    public void OnConsumeSlot()
    {
        SetSlot(itemTable.ConsumeList);
        LastSlot = null;
        LastSlot += OnConsumeSlot;
    }
    public void OnMaterialSlot()
    {
        SetSlot(itemTable.MaterialList);
        LastSlot = null;
        LastSlot += OnMaterialSlot;
    }
    private void SetSlot(IReadOnlyDictionary<string,ItemObject> list)
    {
        itemNameDescription.text = string.Empty;
        itemPriceText.text = string.Empty;
        int index = 0;
        foreach (string item in list.Keys)
        {
            slots[index].gameObject.SetActive(true);
            slots[index].Initialize(item, this);
            index++;
        }
        for (int i = index; i < slots.Length; i++)
        {
            slots[i].Initialize();
        }
    }

}
