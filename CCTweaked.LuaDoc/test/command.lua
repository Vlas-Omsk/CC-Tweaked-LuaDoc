---@meta

---This peripheral allows you to interact with command blocks.
---
---Command blocks are only wrapped as peripherals if the `enable_command_block`
---option is true within the config.
---
---This API is <em>not</em> the same as the `module!commands` API, which is exposed
---on command computers.
---
---@class commandlib
---@source src/main/java/dan200/computercraft/shared/peripheral/commandblock/CommandBlockPeripheral.java:39
command = {}

---Get the command this command block will run.
---
---@return string . The current command.
---@source src/main/java/dan200/computercraft/shared/peripheral/commandblock/CommandBlockPeripheral.java:64
function command.getCommand() end

---Set the command block's command.
---
---@param command string The new command.
---@source src/main/java/dan200/computercraft/shared/peripheral/commandblock/CommandBlockPeripheral.java:75
function command.setCommand(command) end

---Execute the command block once.
---
---@return boolean . If the command completed successfully.
---@return string|nil . A failure message.
---@source src/main/java/dan200/computercraft/shared/peripheral/commandblock/CommandBlockPeripheral.java:89
function command.runCommand() end

