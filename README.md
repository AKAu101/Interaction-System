# Inventory System Documentation

A flexible, slot-based inventory system for Unity with drag-and-drop functionality and event-driven architecture.

## Features

- **Slot-based organization** - Up to 20 slots with stackable items (max 99 per stack)
- **Drag-and-drop UI** - Intuitive item management with visual feedback
- **Event-driven architecture** - Decoupled components using observer pattern
- **ScriptableObject items** - Easy-to-configure item definitions
- **World item pickups** - Seamless integration with 3D game objects

## Architecture Overview

The system uses a slot-based approach where items are organized in numbered slots. The architecture separates data management (Inventory) from visual representation (InventoryGUI), connected through an event system.

## Core Components

### 1. ItemSO.cs (Item Definition)

ScriptableObject that defines item properties:

```csharp
- icon          // Visual sprite representation
- name          // Item display name
- description   // Item description text
- isUsable      // Whether the item can be used/consumed
```

### 2. ItemStack.cs (Data Structure)

Simple container pairing an item type with its quantity:

```csharp
public class ItemStack {
    public ItemSO ItemType;
    public int Amount;
}
```

### 3. Inventory.cs (Core Logic - Singleton)

**Data Structure:** `Dictionary<int, ItemStack> slotToStack`

**Key Methods:**
- `AddItem(ItemSO itemType)` - Finds stackable or empty slot and adds item
- `RemoveItem(ItemSO itemType)` - Removes one item from first matching slot
- `TryMoveItem(int sourceSlot, int targetSlot)` - Handles drag-drop moves and swaps

**Events System:**
- `OnItemAdded(ItemSO, int slot)`
- `OnItemRemoved(ItemSO, int slot)`
- `OnItemMoved(int source, int target)`
- `OnItemSwapped(int slot1, int slot2)`

### 4. InventoryGUI.cs (UI Controller - Singleton)

Manages visual representation:
- `slotIndexToContainer` - Maps slot indices to UI container elements
- `slotToView` - Maps slot indices to ItemView instances
- Listens to Inventory events and updates UI accordingly

### 5. ItemView.cs (Draggable Item UI)

Implements Unity's drag-and-drop interfaces:
- `IBeginDragHandler` - Initiates drag, lifts item visually
- `IDragHandler` - Updates position to follow mouse cursor
- `IEndDragHandler` - Raycasts to detect drop slot, calls `Inventory.TryMoveItem()`

### 6. SlotView.cs (Slot Marker)

Simple component that marks valid drop zones with a slot index reference.

### 7. WorldItem.cs (Item Pickup)

Component attached to 3D items in the game world. Calls `Inventory.Instance.AddItem()` when picked up by the player.

## Data Flow Examples

### Adding an Item

1. `WorldItem.PickUp()` → `Inventory.AddItem()`
2. Inventory finds stackable or empty slot, updates Dictionary
3. Fires `OnItemAdded` event
4. InventoryGUI receives event, instantiates ItemView prefab
5. ItemView displays item sprite

### Moving Items (Drag-Drop)

1. User drags ItemView component
2. On drop, raycasts to detect target SlotView
3. Calls `Inventory.TryMoveItem(sourceSlot, targetSlot)`
4. Inventory either moves (empty slot) or swaps (occupied slot)
5. Fires `OnItemMoved` or `OnItemSwapped` event
6. InventoryGUI updates view positions

## Design Patterns

- **Singleton Pattern** - Inventory and InventoryGUI provide global access points
- **Observer Pattern** - Event-driven architecture where UI observes inventory state changes
- **Dictionary for O(1) Lookup** - Fast slot-to-item mapping using hash-based data structure

## Getting Started

1. Create item definitions using the ItemSO ScriptableObject
2. Set up your UI with SlotView components for each inventory slot
3. Attach WorldItem components to pickable objects in your scene
4. The Inventory and InventoryGUI singletons will handle the rest automatically

## Requirements

- Unity 6000.2.7f2 or later
- TextMeshPro (for UI text elements)
