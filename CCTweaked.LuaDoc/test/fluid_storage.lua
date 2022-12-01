---@meta

---Methods for interacting with tanks and other fluid storage blocks.
---
---@class fluid_storagelib
---@since 1.94.0
---@source src/main/java/dan200/computercraft/shared/peripheral/generic/methods/FluidMethods.java:40
fluid_storage = {}

---Get all "tanks" in this fluid storage.
---
---Each tank either contains some amount of fluid or is empty. Tanks with fluids
---inside will return some basic information about the fluid, including its name
---and amount.
---
---The returned table is sparse, and so empty tanks will be `nil` - it is
---recommended to loop over using `pairs` rather than `ipairs`.
---
---@return { [number]: (table|nil) } . All tanks in this fluid storage.
---@source src/main/java/dan200/computercraft/shared/peripheral/generic/methods/FluidMethods.java:69
function fluid_storage.tanks() end

---Move a fluid from one fluid container to another connected one.
---
---This allows you to pull fluid in the current fluid container to another
---container <em>on the same wired network</em>. Both containers must attached to
---wired modems which are connected via a cable.
---
---@param toName string The name of the peripheral/container to push to. This is the string given to `peripheral.wrap`,and displayed by the wired modem.
---@param limit? number The maximum amount of fluid to move.
---@param fluidName? string The fluid to move. If not given, an arbitrary fluid will be chosen.
---@return number . The amount of moved fluid.
---@throws If the peripheral to transfer to doesn't exist or isn't an fluid container.
---@see peripheral.getName Allows you to get the name of a @{peripheral.wrap|wrapped} peripheral.
---@source src/main/java/dan200/computercraft/shared/peripheral/generic/methods/FluidMethods.java:99
function fluid_storage.pushFluid(toName, limit, fluidName) end

---Move a fluid from a connected fluid container into this oneone.
---
---This allows you to pull fluid in the current fluid container from another
---container <em>on the same wired network</em>. Both containers must attached to
---wired modems which are connected via a cable.
---
---@param fromName string The name of the peripheral/container to push to. This is the string given to `peripheral.wrap`,and displayed by the wired modem.
---@param limit? number The maximum amount of fluid to move.
---@param fluidName? string The fluid to move. If not given, an arbitrary fluid will be chosen.
---@return number . The amount of moved fluid.
---@throws If the peripheral to transfer to doesn't exist or isn't an fluid container.
---@see peripheral.getName Allows you to get the name of a @{peripheral.wrap|wrapped} peripheral.
---@source src/main/java/dan200/computercraft/shared/peripheral/generic/methods/FluidMethods.java:140
function fluid_storage.pullFluid(fromName, limit, fluidName) end

