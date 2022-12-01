---@meta

---A computer or turtle wrapped as a peripheral.
---
---This allows for basic interaction with adjacent computers. Computers wrapped as
---peripherals will have the type `computer` while turtles will be `turtle`.
---
---@class computerlib
---@source src/main/java/dan200/computercraft/shared/computer/blocks/ComputerPeripheral.java:23
computer = {}

---Turn the other computer on.
---
---@source src/main/java/dan200/computercraft/shared/computer/blocks/ComputerPeripheral.java:44
function computer.turnOn() end

---Shutdown the other computer.
---
---@source src/main/java/dan200/computercraft/shared/computer/blocks/ComputerPeripheral.java:53
function computer.shutdown() end

---Reboot or turn on the other computer.
---
---@source src/main/java/dan200/computercraft/shared/computer/blocks/ComputerPeripheral.java:62
function computer.reboot() end

---Get the other computer's ID.
---
---@return number . The computer's ID.
---@see module!os.getComputerID To get your computer's ID.
---@source src/main/java/dan200/computercraft/shared/computer/blocks/ComputerPeripheral.java:74
function computer.getID() end

---Determine if the other computer is on.
---
---@return boolean . If the computer is on.
---@source src/main/java/dan200/computercraft/shared/computer/blocks/ComputerPeripheral.java:85
function computer.isOn() end

---Get the other computer's label.
---
---@return string|nil . The computer's label.
---@see module!os.getComputerLabel To get your label.
---@source src/main/java/dan200/computercraft/shared/computer/blocks/ComputerPeripheral.java:97
function computer.getLabel() end

