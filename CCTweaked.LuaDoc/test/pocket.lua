---@meta

---Control the current pocket computer, adding or removing upgrades.
---
---This API is only available on pocket computers. As such, you may use its
---presence to determine what kind of computer you are using:
---
---```lua if pocket then print("On a pocket computer") else print("On something
---else") end ```
---
---@class pocketlib
---@source src/main/java/dan200/computercraft/shared/pocket/apis/PocketAPI.java:38
pocket = {}

---Search the player's inventory for another upgrade, replacing the existing one
---with that item if found.
---
---This inventory search starts from the player's currently selected slot, allowing
---you to prioritise upgrades.
---
---@return boolean . If an item was equipped.
---@return string|nil . The reason an item was not equipped.
---@source src/main/java/dan200/computercraft/shared/pocket/apis/PocketAPI.java:62
function pocket.equipBack() end

---Remove the pocket computer's current upgrade.
---
---@return boolean . If the upgrade was unequipped.
---@return string|nil . The reason an upgrade was not unequipped.
---@source src/main/java/dan200/computercraft/shared/pocket/apis/PocketAPI.java:106
function pocket.unequipBack() end

