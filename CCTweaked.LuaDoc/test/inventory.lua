---@meta

---Methods for interacting with inventories.
---
---@class inventorylib
---@since 1.94.0
---@source src/main/java/dan200/computercraft/shared/peripheral/generic/methods/InventoryMethods.java:42
inventory = {}

---Get the size of this inventory.
---
---@return number . The number of slots in this inventory.
---@source src/main/java/dan200/computercraft/shared/peripheral/generic/methods/InventoryMethods.java:64
function inventory.size() end

---List all items in this inventory. This returns a table, with an entry for each
---slot.
---
---Each item in the inventory is represented by a table containing some basic
---information, much like `module!turtle.getItemDetail` includes. More information
---can be fetched with `getItemDetail`. The table contains the item `name`, the
---`count` and an a (potentially nil) hash of the item's `nbt.` This NBT data
---doesn't contain anything useful, but allows you to distinguish identical items.
---
---The returned table is sparse, and so empty slots will be `nil` - it is
---recommended to loop over using `pairs` rather than `ipairs`.
---
---@return { [number]: (table|nil) } . All items in this inventory.
---@usage Find an adjacent chest and print all items in it.  ```lua local chest = peripheral.find("minecraft:chest") for slot, item in pairs(chest.list()) do print(("%d x %s in slot %d"):format(item.count, item.name, slot)) end ```
---@source src/main/java/dan200/computercraft/shared/peripheral/generic/methods/InventoryMethods.java:94
function inventory.list() end

---Get detailed information about an item.
---
---The returned information contains the same information as each item in `list`,
---as well as additional details like the display name (`displayName`), item groups
---(`itemGroups`), which are the creative tabs an item will appear under, and item
---and item durability (`damage`, `maxDamage`, `durability`).
---
---Some items include more information (such as enchantments) - it is recommended
---to print it out using `textutils.serialize` or in the Lua REPL, to explore what
---is available.
---
---@param slot number The slot to get information about.
---@return table . Information about the item in this slot, or `nil` if not present.
---@throws If the slot is out of range.
---@usage Print some information about the first in a chest.  ```lua local chest = peripheral.find("minecraft:chest") local item = chest.getItemDetail(1) if not item then print("No item") return end  print(("%s (%s)"):format(item.displayName, item.name)) print(("Count: %d/%d"):format(item.count, item.maxCount))  for _, group in pairs(item.itemGroups) do print(("Group: %s"):format(group.displayName)) end  if item.damage then print(("Damage: %d/%d"):format(item.damage, item.maxDamage)) end ```
---@source src/main/java/dan200/computercraft/shared/peripheral/generic/methods/InventoryMethods.java:145
function inventory.getItemDetail(slot) end

---Get the maximum number of items which can be stored in this slot.
---
---Typically this will be limited to 64 items. However, some inventories (such as
---barrels or caches) can store hundreds or thousands of items in one slot.
---
---@param slot number The slot
---@return number . The maximum number of items in this slot.
---@throws If the slot is out of range.
---@usage Count the maximum number of items an adjacent chest can hold.```lua local chest = peripheral.find("minecraft:chest") local total = 0 for i = 1, chest.size() do total = total + chest.getItemLimit(i) end print(total) ```
---@since 1.96.0
---@source src/main/java/dan200/computercraft/shared/peripheral/generic/methods/InventoryMethods.java:176
function inventory.getItemLimit(slot) end

---Push items from one inventory to another connected one.
---
---This allows you to push an item in an inventory to another inventory <em>on the
---same wired network</em>. Both inventories must attached to wired modems which
---are connected via a cable.
---
---@param toName string The name of the peripheral/inventory to push to. This is the string given to `peripheral.wrap`,and displayed by the wired modem.
---@param fromSlot number The slot in the current inventory to move items to.
---@param limit? number The maximum number of items to move. Defaults to the current stack limit.
---@param toSlot? number The slot in the target inventory to move to. If not given, the item will be inserted into any slot.
---@return number . The number of transferred items.
---@throws If the peripheral to transfer to doesn't exist or isn't an inventory.
---@throws If either source or destination slot is out of range.
---@see peripheral.getName Allows you to get the name of a @{peripheral.wrap|wrapped} peripheral.
---@usage Wrap two chests, and push an item from one to another.```lua local chest_a = peripheral.wrap("minecraft:chest_0") local chest_b = peripheral.wrap("minecraft:chest_1")  chest_a.pushItems(peripheral.getName(chest_b), 1) ```
---@source src/main/java/dan200/computercraft/shared/peripheral/generic/methods/InventoryMethods.java:208
function inventory.pushItems(toName, fromSlot, limit, toSlot) end

---Pull items from a connected inventory into this one.
---
---This allows you to transfer items between inventories <em>on the same wired
---network</em>. Both this and the source inventory must attached to wired modems
---which are connected via a cable.
---
---@param fromName string The name of the peripheral/inventory to pull from. This is the string given to `peripheral.wrap`,and displayed by the wired modem.
---@param fromSlot number The slot in the source inventory to move items from.
---@param limit? number The maximum number of items to move. Defaults to the current stack limit.
---@param toSlot? number The slot in current inventory to move to. If not given, the item will be inserted into any slot.
---@return number . The number of transferred items.
---@throws If the peripheral to transfer to doesn't exist or isn't an inventory.
---@throws If either source or destination slot is out of range.
---@see peripheral.getName Allows you to get the name of a @{peripheral.wrap|wrapped} peripheral.
---@usage Wrap two chests, and push an item from one to another.```lua local chest_a = peripheral.wrap("minecraft:chest_0") local chest_b = peripheral.wrap("minecraft:chest_1")  chest_a.pullItems(peripheral.getName(chest_b), 1) ```
---@source src/main/java/dan200/computercraft/shared/peripheral/generic/methods/InventoryMethods.java:255
function inventory.pullItems(fromName, fromSlot, limit, toSlot) end

