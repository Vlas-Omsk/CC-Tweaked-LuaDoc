---@meta

---
---
---@class commandslib
---@since 1.7
---@source src/main/java/dan200/computercraft/shared/computer/apis/CommandAPI.java:33
commands = {}

---Execute a specific command.
---
---@param command string The command to execute.
---@return boolean . Whether the command executed successfully.
---@return { [number]: string } . The output of this command, as a list of lines.
---@return number|nil . The number of "affected" objects, or `nil` if the command failed. The definition of thisvaries from command to command.
---@changed 1.71 Added return value with command output.
---@changed 1.85.0 Added return value with the number of affected objects.
---@usage Set the block above the command computer to stone.```lua commands.exec("setblock ~ ~1 ~ minecraft:stone") ```
---@source src/main/java/dan200/computercraft/shared/computer/apis/CommandAPI.java:104
function commands.exec(command) end

---Asynchronously execute a command.
---
---Unlike `exec`, this will immediately return, instead of waiting for the command
---to execute. This allows you to run multiple commands at the same time.
---
---When this command has finished executing, it will queue a `task_complete` event
---containing the result of executing this command (what `exec` would return).
---
---@param command string The command to execute.
---@return number . The "task id". When this command has been executed, it will queue a `task_complete` event with a matching id.
---@usage Asynchronously sets the block above the computer to stone.```lua commands.execAsync("setblock ~ ~1 ~ minecraft:stone") ```
---@see parallel One may also use the parallel API to run multiple commands at once.
---@source src/main/java/dan200/computercraft/shared/computer/apis/CommandAPI.java:131
function commands.execAsync(command) end

---List all available commands which the computer has permission to execute.
---
---@param ... string The sub-command to complete.
---@return { [number]: string } . A list of all available commands
---@source src/main/java/dan200/computercraft/shared/computer/apis/CommandAPI.java:145
function commands.list(...) end

---Get the position of the current command computer.
---
---@return number . This computer's x position.
---@return number . This computer's y position.
---@return number . This computer's z position.
---@see gps.locate To get the position of a non-command computer.
---@source src/main/java/dan200/computercraft/shared/computer/apis/CommandAPI.java:176
function commands.getBlockPosition() end

---Get information about a range of blocks.
---
---This returns the same information as `getBlockInfo`, just for multiple blocks at
---once.
---
---Blocks are traversed by ascending y level, followed by z and x - the returned
---table may be indexed using `x + z*width + y*depth*depth`.
---
---@param minX number The start x coordinate of the range to query.
---@param minY number The start y coordinate of the range to query.
---@param minZ number The start z coordinate of the range to query.
---@param maxX number The end x coordinate of the range to query.
---@param maxY number The end y coordinate of the range to query.
---@param maxZ number The end z coordinate of the range to query.
---@param dimension? string The dimension to query (e.g. "minecraft:overworld"). Defaults to the current dimension.
---@return { [number]: table } . A list of information about each block.
---@throws If the coordinates are not within the world.
---@throws If trying to get information about more than 4096 blocks.
---@since 1.76
---@changed 1.99 Added `dimension` argument.
---@source src/main/java/dan200/computercraft/shared/computer/apis/CommandAPI.java:206
function commands.getBlockInfos(minX, minY, minZ, maxX, maxY, maxZ, dimension) end

---Get some basic information about a block.
---
---The returned table contains the current name, metadata and block state (as with
---`turtle.inspect`). If there is a tile entity for that block, its NBT will also
---be returned.
---
---@param x number The x position of the block to query.
---@param y number The y position of the block to query.
---@param z number The z position of the block to query.
---@param dimension? string The dimension to query (e.g. "minecraft:overworld"). Defaults to the current dimension.
---@return table . The given block's information.
---@throws If the coordinates are not within the world, or are not currently loaded.
---@changed 1.76 Added block state info to return value
---@changed 1.99 Added `dimension` argument.
---@source src/main/java/dan200/computercraft/shared/computer/apis/CommandAPI.java:261
function commands.getBlockInfo(x, y, z, dimension) end

