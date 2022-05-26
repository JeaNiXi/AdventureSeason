using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;

    [SerializeField] public MouseFollower mouseFollower;
    private void Awake()
    {
        if(Instance!=null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        UIInventoryPage.InitializeInventoryUI(testInventorySize);

        if (UIInventoryPage.isActiveAndEnabled)
        {
            UIInventoryPage.Hide();
        }
        if (mouseFollower.isActiveAndEnabled)
        {
            mouseFollower.Toggle(false);
        }
    }

    private int testInventorySize=30;

    [SerializeField] private UIInventoryPage UIInventoryPage;

    private void Start()
    {
        
    }
    private void Update()
    {
        
    }

    public void ToggleInventory()
    {
        if (UIInventoryPage.isActiveAndEnabled == false)
        {
            Debug.Log("InvController Showing");
            UIInventoryPage.Show();
        }
        else
        {
            Debug.Log("InvController Hiding");
            UIInventoryPage.Hide();
        }
    }
}
