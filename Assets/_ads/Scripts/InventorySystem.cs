using System;
using System.Collections.Generic;

// Item class representing any item in the game
public class Item
{
    public string Name { get; set; }
    public int Quantity { get; set; }

    public Item(string name, int quantity)
    {
        Name = name;
        Quantity = quantity;
    }
}

// Inventory system that manages a collection of items
public class InventorySystem
{
    private List<Item> inventory;

    public InventorySystem()
    {
        inventory = new List<Item>();
    }

    // Add an item to the inventory
    public void AddItem(Item item)
    {
        Item existingItem = inventory.Find(i => i.Name == item.Name);

        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            inventory.Add(new Item(item.Name, item.Quantity));
        }
    }

    // Remove an item from the inventory
    public void RemoveItem(Item item)
    {
        Item existingItem = inventory.Find(i => i.Name == item.Name);

        if (existingItem != null)
        {
            existingItem.Quantity -= item.Quantity;

            if (existingItem.Quantity <= 0)
            {
                inventory.Remove(existingItem);
            }
        }
        // Handle case where the item to remove is not in the inventory
        else
        {
            Console.WriteLine($"Error: {item.Name} not found in inventory.");
        }
    }

    // Display the current state of the inventory
    public void DisplayInventory()
    {
        Console.WriteLine("Inventory:");

        foreach (var item in inventory)
        {
            Console.WriteLine($"{item.Name}: {item.Quantity}");
        }
    }
}