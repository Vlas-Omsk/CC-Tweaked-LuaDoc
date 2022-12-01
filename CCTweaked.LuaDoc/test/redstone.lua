---@meta

---Get and set redstone signals adjacent to this computer.
---
---The `module!redstone` library exposes three "types" of redstone control: -
---Binary input/output (`setOutput`/`getInput`): These simply check if a redstone
---wire has any input or output. A signal strength of 1 and 15 are treated the
---same. - Analogue input/output (`setAnalogOutput`/`getAnalogInput`): These work
---with the actual signal strength of the redstone wired, from 0 to 15. - Bundled
---cables (`setBundledOutput`/`getBundledInput`): These interact with "bundled"
---cables, such as those from Project:Red. These allow you to send 16 separate
---on/off signals. Each channel corresponds to a colour, with the first being
---`colors.white` and the last `colors.black`.
---
---Whenever a redstone input changes, a `event!redstone` event will be fired. This
---may be used instead of repeativly polling.
---
---This module may also be referred to as `rs`. For example, one may call
---`rs.getSides()` instead of `getSides`.
---
---@class redstonelib
---@usage Toggle the redstone signal above the computer every 0.5 seconds.  ```lua while true do redstone.setOutput("top", not redstone.getOutput("top")) sleep(0.5) end ```
---@usage Mimic a redstone comparator in [subtraction mode][comparator].  ```lua while true do local rear = rs.getAnalogueInput("back") local sides = math.max(rs.getAnalogueInput("left"), rs.getAnalogueInput("right")) rs.setAnalogueOutput("front", math.max(rear - sides, 0))  os.pullEvent("redstone") -- Wait for a change to inputs. end ```  [comparator]: https://minecraft.gamepedia.com/Redstone_Comparator#Subtract_signal_strength "Redstone Comparator on the Minecraft wiki."
---@source src/main/java/dan200/computercraft/core/apis/RedstoneAPI.java:55
redstone = {}

---Returns a table containing the six sides of the computer. Namely, "top",
---"bottom", "left", "right", "front" and"back".
---
---@return { [number]: string } . A table of valid sides.
---@since 1.2
---@source src/main/java/dan200/computercraft/core/apis/RedstoneAPI.java:77
function redstone.getSides() end

---Turn the redstone signal of a specific side on or off.
---
---@param side string The side to set.
---@param on boolean Whether the redstone signal should be on or off. When on, a signal strength of 15 is emitted.
---@source src/main/java/dan200/computercraft/core/apis/RedstoneAPI.java:89
function redstone.setOutput(side, on) end

---Get the current redstone output of a specific side.
---
---@param side string The side to get.
---@return boolean . Whether the redstone output is on or off.
---@see setOutput
---@source src/main/java/dan200/computercraft/core/apis/RedstoneAPI.java:102
function redstone.getOutput(side) end

---Get the current redstone input of a specific side.
---
---@param side string The side to get.
---@return boolean . Whether the redstone input is on or off.
---@source src/main/java/dan200/computercraft/core/apis/RedstoneAPI.java:114
function redstone.getInput(side) end

---Set the redstone signal strength for a specific side.
---
---@param side string The side to set.
---@param value number The signal strength between 0 and 15.
---@throws If `value` is not betwene 0 and 15.
---@since 1.51
---@source src/main/java/dan200/computercraft/core/apis/RedstoneAPI.java:128
function redstone.setAnalogOutput(side, value) end

---Get the redstone output signal strength for a specific side.
---
---@param side string The side to get.
---@return number . The output signal strength, between 0 and 15.
---@since 1.51
---@see setAnalogOutput
---@source src/main/java/dan200/computercraft/core/apis/RedstoneAPI.java:143
function redstone.getAnalogOutput(side) end

---Get the redstone input signal strength for a specific side.
---
---@param side string The side to get.
---@return number . The input signal strength, between 0 and 15.
---@since 1.51
---@source src/main/java/dan200/computercraft/core/apis/RedstoneAPI.java:156
function redstone.getAnalogInput(side) end

---Set the bundled cable output for a specific side.
---
---@param side string The side to set.
---@param output number The colour bitmask to set.
---@see colors.subtract For removing a colour from the bitmask.
---@see colors.combine For adding a color to the bitmask.
---@source src/main/java/dan200/computercraft/core/apis/RedstoneAPI.java:170
function redstone.setBundledOutput(side, output) end

---Get the bundled cable output for a specific side.
---
---@param side string The side to get.
---@return number . The bundle cable's output.
---@source src/main/java/dan200/computercraft/core/apis/RedstoneAPI.java:182
function redstone.getBundledOutput(side) end

---Get the bundled cable input for a specific side.
---
---@param side string The side to get.
---@return number . The bundle cable's input.
---@see testBundledInput To determine if a specific colour is set.
---@source src/main/java/dan200/computercraft/core/apis/RedstoneAPI.java:195
function redstone.getBundledInput(side) end

---Determine if a specific combination of colours are on for the given side.
---
---@param side string The side to test.
---@param mask number The mask to test.
---@return boolean . If the colours are on.
---@usage Check if @{colors.white} and @{colors.black} are on above the computer.```lua print(redstone.testBundledInput("top", colors.combine(colors.white, colors.black))) ```
---@see getBundledInput
---@source src/main/java/dan200/computercraft/core/apis/RedstoneAPI.java:213
function redstone.testBundledInput(side, mask) end

