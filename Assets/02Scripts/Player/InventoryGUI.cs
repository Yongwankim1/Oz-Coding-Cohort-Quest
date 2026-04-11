using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventoryGUI : MonoBehaviour
{

    [Header("Ref")]
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] PlayerInventory inventory;
    [SerializeField] PlayerInventoryGrid inventoryGrid;
    [SerializeField] InventorySlotUI slotPrefab;
    [SerializeField] Transform parentTransform;

    [SerializeField] InventorySlotUI[,] inventorySlotUIs = new InventorySlotUI[0, 0];
    [SerializeField] TextMeshProUGUI goldText;

    private void Awake()
    {
        if (inventory == null)
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();

        if (inventoryGrid == null && inventory != null)
            inventoryGrid = inventory.InventoryGrid;

        Init();
    }

    public void Init()
    {
        if (slotPrefab == null) return;
        if (inventory == null) return;
        if (inventoryGrid == null) return;
        if (parentTransform == null) return;
        if (gridLayoutGroup == null)
        {
            gridLayoutGroup = GetComponent<GridLayoutGroup>();
        }
        else
        {
            gridLayoutGroup.constraintCount = inventory.RowCount;
        }
        inventorySlotUIs = new InventorySlotUI[inventory.RowCount, inventory.ColumnCount];

        for (int col = 0; col < inventory.ColumnCount; col++)
        {
            for (int row = 0; row < inventory.RowCount; row++)
            {
                inventorySlotUIs[row, col] = Instantiate(slotPrefab, parentTransform);
                inventorySlotUIs[row, col].Initialized(row, col);
            }
        }
    }

    private void OnEnable()
    {
        if (inventory != null)
            inventory.OnItemAmountChanged += ReDrawAllUI;
        if(inventoryGrid != null)
            inventoryGrid.OnSlotChangedAction += ReDrawAllUI;
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.OnItemAmountChanged -= ReDrawAllUI;
        if (inventoryGrid != null)
            inventoryGrid.OnSlotChangedAction -= ReDrawAllUI;
    }

    void ReDrawAllUI()
    {
        if (inventory == null) return;
        if (inventoryGrid == null) return;
        if (inventoryGrid.InventoryGrid == null) return;

        for (int col = 0; col < inventory.ColumnCount; col++)
        {
            for (int row = 0; row < inventory.RowCount; row++)
            {
                inventorySlotUIs[row, col].DrawSlot(inventoryGrid.InventoryGrid[row, col], row, col);
            }
        }
        goldText.text = inventory.Gold.ToString() + " G";
    }
}