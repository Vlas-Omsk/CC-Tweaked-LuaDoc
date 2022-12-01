---@meta

---Turtles are a robotic device, which can break and place blocks, attack mobs, and
---move about the world. They havean internal inventory of 16 slots, allowing them
---to store blocks they have broken or would like to place.
---
---## Movement Turtles are capable of moving through the world. As turtles are
---blocks themselves, they are confined to Minecraft's grid, moving a single block
---at a time.
---
---`turtle.forward` and `turtle.back` move the turtle in the direction it is
---facing, while `turtle.up` and `turtle.down` move it up and down (as one might
---expect!). In order to move left or right, you first need to turn the turtle
---using `turtle.turnLeft`/`turtle.turnRight` and then move forward or backwards.
---
---:::info The name "turtle" comes from [Turtle graphics], which originated from
---the Logo programming language. Here you'd move a turtle with various commands
---like "move 10" and "turn left", much like ComputerCraft's turtles! :::
---
---Moving a turtle (though not turning it) consumes *fuel*. If a turtle does not
---have any `turtle.refuel|fuel`, it won't move, and the movement functions will
---return `false`. If your turtle isn't going anywhere, the first thing to check is
---if you've fuelled your turtle.
---
---:::tip Handling errors Many turtle functions can fail in various ways. For
---instance, a turtle cannot move forward if there's already a block there. Instead
---of erroring, functions which can fail either return `true` if they succeed, or
---`false` and some error message if they fail.
---
---Unexpected failures can often lead to strange behaviour. It's often a good idea
---to check the return values of these functions, or wrap them in `assert` (for
---instance, use `assert(turtle.forward())` rather than `turtle.forward()`), so the
---program doesn't misbehave. :::
---
---## Turtle upgrades While a normal turtle can move about the world and place
---blocks, its functionality is limited. Thankfully, turtles can be upgraded with
---*tools* and `peripheral|peripherals`. Turtles have two upgrade slots, one on the
---left and right sides. Upgrades can be equipped by crafting a turtle with the
---upgrade, or calling the `turtle.equipLeft`/`turtle.equipRight` functions.
---
---Turtle tools allow you to break blocks (`turtle.dig`) and attack entities
---(`turtle.attack`). Some tools are more suitable to a task than others. For
---instance, a diamond pickaxe can break every block, while a sword does more
---damage. Other tools have more niche use-cases, for instance hoes can til dirt.
---
---Peripherals (such as the `modem|wireless modem` or `speaker`) can also be
---equipped as upgrades. These are then accessible by accessing the `"left"` or
---`"right"` peripheral.
---
---[Turtle Graphics]: https://en.wikipedia.org/wiki/Turtle_graphics "Turtle
---graphics"
---
---@class turtlelib
---@since 1.3
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:74
turtle = {}

---Craft a recipe based on the turtle's inventory.
---
---The turtle's inventory should set up like a crafting grid. For instance, to
---craft sticks, slots 1 and 5 should contain planks. _All_ other slots should be
---empty, including those outside the crafting "grid".
---
---@param limit? number The maximum number of crafting steps to run.
---@return true . If crafting succeeds.
---@return string . A string describing why crafting failed.
---@throws When limit is less than 1 or greater than 64.
---@since 1.4
function turtle.craft(limit) end

---Move the turtle forward one block.
---
---@return boolean . Whether the turtle could successfully move.
---@return string|nil . The reason the turtle could not move.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:104
function turtle.forward() end

---Move the turtle backwards one block.
---
---@return boolean . Whether the turtle could successfully move.
---@return string|nil . The reason the turtle could not move.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:117
function turtle.back() end

---Move the turtle up one block.
---
---@return boolean . Whether the turtle could successfully move.
---@return string|nil . The reason the turtle could not move.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:130
function turtle.up() end

---Move the turtle down one block.
---
---@return boolean . Whether the turtle could successfully move.
---@return string|nil . The reason the turtle could not move.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:143
function turtle.down() end

---Rotate the turtle 90 degress to the left.
---
---@return boolean . Whether the turtle could successfully turn.
---@return string|nil . The reason the turtle could not turn.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:156
function turtle.turnLeft() end

---Rotate the turtle 90 degress to the right.
---
---@return boolean . Whether the turtle could successfully turn.
---@return string|nil . The reason the turtle could not turn.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:169
function turtle.turnRight() end

---Attempt to break the block in front of the turtle.
---
---This requires a turtle tool capable of breaking the block. Diamond pickaxes
---(mining turtles) can break any vanilla block, but other tools (such as axes) are
---more limited.
---
---@param side? string The specific tool to use. Should be "left" or "right".
---@return boolean . Whether a block was broken.
---@return string|nil . The reason no block was broken.
---@changed 1.6 Added optional side argument.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:188
function turtle.dig(side) end

---Attempt to break the block above the turtle. See `dig` for full details.
---
---@param side? string The specific tool to use.
---@return boolean . Whether a block was broken.
---@return string|nil . The reason no block was broken.
---@changed 1.6 Added optional side argument.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:204
function turtle.digUp(side) end

---Attempt to break the block below the turtle. See `dig` for full details.
---
---@param side? string The specific tool to use.
---@return boolean . Whether a block was broken.
---@return string|nil . The reason no block was broken.
---@changed 1.6 Added optional side argument.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:220
function turtle.digDown(side) end

---Place a block or item into the world in front of the turtle.
---
---"Placing" an item allows it to interact with blocks and entities in front of the
---turtle. For instance, buckets can pick up and place down fluids, and wheat can
---be used to breed cows. However, you cannot use `place` to perform arbitrary
---block interactions, such as clicking buttons or flipping levers.
---
---@param text? string When placing a sign, set its contents to this text.
---@return boolean . Whether the block could be placed.
---@return string|nil . The reason the block was not placed.
---@since 1.4
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:241
function turtle.place(text) end

---Place a block or item into the world above the turtle.
---
---@param text? string When placing a sign, set its contents to this text.
---@return boolean . Whether the block could be placed.
---@return string|nil . The reason the block was not placed.
---@since 1.4
---@see place For more information about placing items.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:258
function turtle.placeUp(text) end

---Place a block or item into the world below the turtle.
---
---@param text? string When placing a sign, set its contents to this text.
---@return boolean . Whether the block could be placed.
---@return string|nil . The reason the block was not placed.
---@since 1.4
---@see place For more information about placing items.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:275
function turtle.placeDown(text) end

---Drop the currently selected stack into the inventory in front of the turtle, or
---as an item into the world ifthere is no inventory.
---
---@param count? number The number of items to drop. If not given, the entire stack will be dropped.
---@return boolean . Whether items were dropped.
---@return string|nil . The reason the no items were dropped.
---@throws If dropping an invalid number of items.
---@since 1.31
---@see select
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:293
function turtle.drop(count) end

---Drop the currently selected stack into the inventory above the turtle, or as an
---item into the world if there isno inventory.
---
---@param count? number The number of items to drop. If not given, the entire stack will be dropped.
---@return boolean . Whether items were dropped.
---@return string|nil . The reason the no items were dropped.
---@throws If dropping an invalid number of items.
---@since 1.4
---@see select
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:311
function turtle.dropUp(count) end

---Drop the currently selected stack into the inventory in front of the turtle, or
---as an item into the world ifthere is no inventory.
---
---@param count? number The number of items to drop. If not given, the entire stack will be dropped.
---@return boolean . Whether items were dropped.
---@return string|nil . The reason the no items were dropped.
---@throws If dropping an invalid number of items.
---@since 1.4
---@see select
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:329
function turtle.dropDown(count) end

---Change the currently selected slot.
---
---The selected slot is determines what slot actions like `drop` or `getItemCount`
---act on.
---
---@param slot number The slot to select.
---@return true . When the slot has been selected.
---@throws If the slot is out of range.
---@see getSelectedSlot
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:347
function turtle.select(slot) end

---Get the number of items in the given slot.
---
---@param slot? number The slot we wish to check. Defaults to the `select|selected slot`.
---@return number . The number of items in this slot.
---@throws If the slot is out of range.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:364
function turtle.getItemCount(slot) end

---Get the remaining number of items which may be stored in this stack.
---
---For instance, if a slot contains 13 blocks of dirt, it has room for another 51.
---
---@param slot? number The slot we wish to check. Defaults to the `select|selected slot`.
---@return number . The space left in in this slot.
---@throws If the slot is out of range.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:380
function turtle.getItemSpace(slot) end

---Check if there is a solid block in front of the turtle. In this case, solid
---refers to any non-air or liquidblock.
---
---@return boolean . If there is a solid block in front.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:395
function turtle.detect() end

---Check if there is a solid block above the turtle. In this case, solid refers to
---any non-air or liquid block.
---
---@return boolean . If there is a solid block in front.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:407
function turtle.detectUp() end

---Check if there is a solid block below the turtle. In this case, solid refers to
---any non-air or liquid block.
---
---@return boolean . If there is a solid block in front.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:419
function turtle.detectDown() end

---Check if the block in front of the turtle is equal to the item in the currently
---selected slot.
---
---@return boolean . If the block and item are equal.
---@since 1.31
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:432
function turtle.compare() end

---Check if the block above the turtle is equal to the item in the currently
---selected slot.
---
---@return boolean . If the block and item are equal.
---@since 1.31
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:445
function turtle.compareUp() end

---Check if the block below the turtle is equal to the item in the currently
---selected slot.
---
---@return boolean . If the block and item are equal.
---@since 1.31
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:458
function turtle.compareDown() end

---Attack the entity in front of the turtle.
---
---@param side? string The specific tool to use.
---@return boolean . Whether an entity was attacked.
---@return string|nil . The reason nothing was attacked.
---@since 1.4
---@changed 1.6 Added optional side argument.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:474
function turtle.attack(side) end

---Attack the entity above the turtle.
---
---@param side? string The specific tool to use.
---@return boolean . Whether an entity was attacked.
---@return string|nil . The reason nothing was attacked.
---@since 1.4
---@changed 1.6 Added optional side argument.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:490
function turtle.attackUp(side) end

---Attack the entity below the turtle.
---
---@param side? string The specific tool to use.
---@return boolean . Whether an entity was attacked.
---@return string|nil . The reason nothing was attacked.
---@since 1.4
---@changed 1.6 Added optional side argument.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:506
function turtle.attackDown(side) end

---Suck an item from the inventory in front of the turtle, or from an item floating
---in the world.
---
---This will pull items into the first acceptable slot, starting at the
---`select|currently selected` one.
---
---@param count? number The number of items to suck. If not given, up to a stack of items will be picked up.
---@return boolean . Whether items were picked up.
---@return string|nil . The reason the no items were picked up.
---@throws If given an invalid number of items.
---@since 1.4
---@changed 1.6 Added an optional limit argument.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:525
function turtle.suck(count) end

---Suck an item from the inventory above the turtle, or from an item floating in
---the world.
---
---@param count? number The number of items to suck. If not given, up to a stack of items will be picked up.
---@return boolean . Whether items were picked up.
---@return string|nil . The reason the no items were picked up.
---@throws If given an invalid number of items.
---@since 1.4
---@changed 1.6 Added an optional limit argument.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:542
function turtle.suckUp(count) end

---Suck an item from the inventory below the turtle, or from an item floating in
---the world.
---
---@param count? number The number of items to suck. If not given, up to a stack of items will be picked up.
---@return boolean . Whether items were picked up.
---@return string|nil . The reason the no items were picked up.
---@throws If given an invalid number of items.
---@since 1.4
---@changed 1.6 Added an optional limit argument.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:559
function turtle.suckDown(count) end

---Get the maximum amount of fuel this turtle currently holds.
---
---@return number . The current amount of fuel a turtle this turtle has.
---@since 1.4
---@see getFuelLimit
---@see refuel
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:575
function turtle.getFuelLevel() end

---Refuel this turtle.
---
---While most actions a turtle can perform (such as digging or placing blocks) are
---free, moving consumes fuel from the turtle's internal buffer. If a turtle has no
---fuel, it will not move.
---
---`refuel` refuels the turtle, consuming fuel items (such as coal or lava buckets)
---from the currently selected slot and converting them into energy. This finishes
---once the turtle is fully refuelled or all items have been consumed.
---
---@param count? number The maximum number of items to consume. One can pass `0` to check if an item is combustable or not.
---@return true . If the turtle was refuelled.
---@throws If the refuel count is out of range.
---@usage Refuel a turtle from the currently selected slot.```lua local level = turtle.getFuelLevel() if new_level == "unlimited" then error("Turtle does not need fuel", 0) end  local ok, err = turtle.refuel() if ok then local new_level = turtle.getFuelLevel() print(("Refuelled %d, current level is %d"):format(new_level - level, new_level)) else printError(err) end ```
---@usage Check if the current item is a valid fuel source.```lua local is_fuel, reason = turtle.refuel(0) if not is_fuel then printError(reason) end ```
---@since 1.4
---@see getFuelLevel
---@see getFuelLimit
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:618
function turtle.refuel(count) end

---Compare the item in the currently selected slot to the item in another slot.
---
---@param slot number The slot to compare to.
---@return boolean . If the two items are equal.
---@throws If the slot is out of range.
---@since 1.4
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:635
function turtle.compareTo(slot) end

---Move an item from the selected slot to another one.
---
---@param slot number The slot to move this item to.
---@param count? number The maximum number of items to move.
---@return boolean . If some items were successfully moved.
---@throws If the slot is out of range.
---@throws If the number of items is out of range.
---@since 1.45
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:652
function turtle.transferTo(slot, count) end

---Get the currently selected slot.
---
---@return number . The current slot.
---@since 1.6
---@see select
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:667
function turtle.getSelectedSlot() end

---Get the maximum amount of fuel this turtle can hold.
---
---By default, normal turtles have a limit of 20,000 and advanced turtles of
---100,000.
---
---@return number . The maximum amount of fuel a turtle can hold.
---@since 1.6
---@see getFuelLevel
---@see refuel
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:685
function turtle.getFuelLimit() end

---Equip (or unequip) an item on the left side of this turtle.
---
---This finds the item in the currently selected slot and attempts to equip it to
---the left side of the turtle. The previous upgrade is removed and placed into the
---turtle's inventory. If there is no item in the slot, the previous upgrade is
---removed, but no new one is equipped.
---
---@return true . If the item was equipped.
---@since 1.6
---@see equipRight
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:705
function turtle.equipLeft() end

---Equip (or unequip) an item on the right side of this turtle.
---
---This finds the item in the currently selected slot and attempts to equip it to
---the right side of the turtle. The previous upgrade is removed and placed into
---the turtle's inventory. If there is no item in the slot, the previous upgrade is
---removed, but no new one is equipped.
---
---@return true . If the item was equipped.
---@since 1.6
---@see equipLeft
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:725
function turtle.equipRight() end

---Get information about the block in front of the turtle.
---
---@return boolean . Whether there is a block in front of the turtle.
---@return table|string . Information about the block in front, or a message explaining that there is no block.
---@since 1.64
---@changed 1.76 Added block state to return value.
---@usage ```lualocal has_block, data = turtle.inspect() if has_block then print(textutils.serialise(data)) -- { --   name = "minecraft:oak_log", --   state = { axis = "x" }, --   tags = { ["minecraft:logs"] = true, ... }, -- } else print("No block in front of the turtle") end ```
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:752
function turtle.inspect() end

---Get information about the block above the turtle.
---
---@return boolean . Whether there is a block above the turtle.
---@return table|string . Information about the above below, or a message explaining that there is no block.
---@since 1.64
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:766
function turtle.inspectUp() end

---Get information about the block below the turtle.
---
---@return boolean . Whether there is a block below the turtle.
---@return table|string . Information about the block below, or a message explaining that there is no block.
---@since 1.64
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:780
function turtle.inspectDown() end

---Get detailed information about the items in the given slot.
---
---@param slot? number The slot to get information about. Defaults to the `select|selected slot`.
---@param detailed? boolean Whether to include "detailed" information. When `true` the method will contain muchmore information about the item at the cost of taking longer to run.
---@return nil|table . Information about the given slot, or `nil` if it is empty.
---@throws If the slot is out of range.
---@since 1.64
---@usage Print the current slot, assuming it contains 13 dirt.  ```lua print(textutils.serialise(turtle.getItemDetail())) -- => { --  name = "minecraft:dirt", --  count = 13, -- } ```
---@see generic_peripheral!inventory.getItemDetail Describes the information returned by a detailed query.
---@source src/main/java/dan200/computercraft/shared/turtle/apis/TurtleAPI.java:808
function turtle.getItemDetail(slot, detailed) end

