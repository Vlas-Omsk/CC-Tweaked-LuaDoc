---@meta

---Methods for interacting with blocks using Forge's energy storage system.
---
---This works with energy storage blocks, as well as generators and machines which
---consume energy.
---
---:::note Due to limitations with Forge's energy API, it is not possible to
---measure throughput (i.e. RF used/generated per tick). :::
---
---@class energy_storagelib
---@since 1.94.0
---@source src/main/java/dan200/computercraft/shared/peripheral/generic/methods/EnergyMethods.java:30
energy_storage = {}

---Get the energy of this block.
---
---@return number . The energy stored in this block, in FE.
---@source src/main/java/dan200/computercraft/shared/peripheral/generic/methods/EnergyMethods.java:52
function energy_storage.getEnergy() end

---Get the maximum amount of energy this block can store.
---
---@return number . The energy capacity of this block.
---@source src/main/java/dan200/computercraft/shared/peripheral/generic/methods/EnergyMethods.java:64
function energy_storage.getEnergyCapacity() end

