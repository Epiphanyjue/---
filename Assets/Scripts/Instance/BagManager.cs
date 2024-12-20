using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    public static BagManager Instance { get; private set; }

    // 使用字典来存储方块名称和数量
    private Dictionary<string, int> items = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    // 增加指定名称方块的数量
    public void AddItem(string itemName, int quantity)
    {
        if (quantity <= 0) return;

        if (items.ContainsKey(itemName))
        {
            items[itemName] += quantity;  // 如果已经有这个方块，增加数量
        }
        else
        {
            items.Add(itemName, quantity); // 否则添加新的方块
        }
        Debug.Log(itemName);
    }

    // 减少指定名称方块的数量
    public void RemoveItem(string itemName, int quantity)
    {
        if (quantity <= 0 || !items.ContainsKey(itemName)) return;

        items[itemName] -= quantity;  // 减少数量

        if (items[itemName] <= 0)
        {
            items.Remove(itemName);  // 如果数量为零或负数，删除这个方块
        }

    }

    // 查询指定名称方块的数量
    public int GetItemCount(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            return items[itemName];
        }
        return 0;  // 如果没有这种方块，返回0
    }
}
