---@meta

---Monitors are a block which act as a terminal, displaying information on one
---side. This allows them to be read andinteracted with in-world without opening a
---GUI.
---
---Monitors act as `term.Redirect|terminal redirects` and so expose the same
---methods, as well as several additional ones, which are documented below.
---
---Like computers, monitors come in both normal (no colour) and advanced (colour)
---varieties.
---
---## Recipes <div class="recipe-container"> <mc-recipe
---recipe="computercraft:monitor_normal"></mc-recipe> <mc-recipe
---recipe="computercraft:monitor_advanced"></mc-recipe> </div>
---
---@class monitorlib
---@usage Write "Hello, world!" to an adjacent monitor:  ```lua local monitor = peripheral.find("monitor") monitor.setCursorPos(1, 1) monitor.write("Hello, world!") ```
---@source src/main/java/dan200/computercraft/shared/peripheral/monitor/MonitorPeripheral.java:43
monitor = {}

---Set the scale of this monitor. A larger scale will result in the monitor having
---a lower resolution, but displaytext much larger.
---
---@param scale number The monitor's scale. This must be a multiple of 0.5 between 0.5 and 5.
---@throws If the scale is out of range.
---@see getTextScale
---@source src/main/java/dan200/computercraft/shared/peripheral/monitor/MonitorPeripheral.java:67
function monitor.setTextScale(scale) end

---Get the monitor's current text scale.
---
---@return number . The monitor's current scale.
---@throws If the monitor cannot be found.
---@since 1.81.0
---@source src/main/java/dan200/computercraft/shared/peripheral/monitor/MonitorPeripheral.java:82
function monitor.getTextScale() end

---Write `text` at the current cursor position, moving the cursor to the end of the
---text.
---
---Unlike functions like `write` and `print`, this does not wrap the text - it
---simply copies the text to the current terminal line.
---
---@param text any The text to write.
---@source src/main/java/dan200/computercraft/core/apis/TermMethods.java:37
function monitor.write(text) end

---Move all positions up (or down) by `y` pixels.
---
---Every pixel in the terminal will be replaced by the line `y` pixels below it. If
---`y` is negative, it will copy pixels from above instead.
---
---@param y number The number of lines to move up by. This may be a negative number.
---@source src/main/java/dan200/computercraft/core/apis/TermMethods.java:58
function monitor.scroll(y) end

---Get the position of the cursor.
---
---@return number . The x position of the cursor.
---@return number . The y position of the cursor.
---@source src/main/java/dan200/computercraft/core/apis/TermMethods.java:72
function monitor.getCursorPos() end

---Set the position of the cursor. `write|terminal writes` will begin from this
---position.
---
---@param x number The new x position of the cursor.
---@param y number The new y position of the cursor.
---@source src/main/java/dan200/computercraft/core/apis/TermMethods.java:86
function monitor.setCursorPos(x, y) end

---Checks if the cursor is currently blinking.
---
---@return boolean . If the cursor is blinking.
---@since 1.80pr1.9
---@source src/main/java/dan200/computercraft/core/apis/TermMethods.java:103
function monitor.getCursorBlink() end

---Sets whether the cursor should be visible (and blinking) at the current
---`getCursorPos|cursor position`.
---
---@param blink boolean Whether the cursor should blink.
---@source src/main/java/dan200/computercraft/core/apis/TermMethods.java:115
function monitor.setCursorBlink(blink) end

---Get the size of the terminal.
---
---@return number . The terminal's width.
---@return number . The terminal's height.
---@source src/main/java/dan200/computercraft/core/apis/TermMethods.java:133
function monitor.getSize() end

---Clears the terminal, filling it with the `getBackgroundColour|current background
---colour`.
---
---@source src/main/java/dan200/computercraft/core/apis/TermMethods.java:145
function monitor.clear() end

---Clears the line the cursor is currently on, filling it with the
---`getBackgroundColour|current backgroundcolour`.
---
---@source src/main/java/dan200/computercraft/core/apis/TermMethods.java:157
function monitor.clearLine() end

---Return the colour that new text will be written as.
---
---@return number . The current text colour.
---@see colors For a list of colour constants, returned by this function.
---@since 1.74
---@source src/main/java/dan200/computercraft/core/apis/TermMethods.java:171
function monitor.getTextColour() end

---Set the colour that new text will be written as.
---
---@param colour number The new text colour.
---@see colors For a list of colour constants.
---@since 1.45
---@changed 1.80pr1 Standard computers can now use all 16 colors, being changed to grayscale on screen.
---@source src/main/java/dan200/computercraft/core/apis/TermMethods.java:186
function monitor.setTextColour(colour) end

---Return the current background colour. This is used when `write|writing text` and
---`clear|clearing`the terminal.
---
---@return number . The current background colour.
---@see colors For a list of colour constants, returned by this function.
---@since 1.74
---@source src/main/java/dan200/computercraft/core/apis/TermMethods.java:205
function monitor.getBackgroundColour() end

---Set the current background colour. This is used when `write|writing text` and
---`clear|clearing` theterminal.
---
---@param colour number The new background colour.
---@see colors For a list of colour constants.
---@since 1.45
---@changed 1.80pr1 Standard computers can now use all 16 colors, being changed to grayscale on screen.
---@source src/main/java/dan200/computercraft/core/apis/TermMethods.java:221
function monitor.setBackgroundColour(colour) end

---Determine if this terminal supports colour.
---
---Terminals which do not support colour will still allow writing coloured
---text/backgrounds, but it will be displayed in greyscale.
---
---@return boolean . Whether this terminal supports colour.
---@since 1.45
---@source src/main/java/dan200/computercraft/core/apis/TermMethods.java:241
function monitor.isColour() end

---Writes `text` to the terminal with the specific foreground and background
---characters.
---
---As with `write`, the text will be written at the current cursor location, with
---the cursor moving to the end of the text.
---
---`textColour` and `backgroundColour` must both be strings the same length as
---`text`. All characters represent a single hexadecimal digit, which is converted
---to one of CC's colours. For instance, `"a"` corresponds to purple.
---
---@param text string The text to write.
---@param textColour string The corresponding text colours.
---@param backgroundColour string The corresponding background colours.
---@throws If the three inputs are not the same length.
---@see colors For a list of colour constants, and their hexadecimal values.
---@since 1.74
---@changed 1.80pr1 Standard computers can now use all 16 colors, being changed to grayscale on screen.
---@usage Prints "Hello, world!" in rainbow text.```lua term.blit("Hello, world!","01234456789ab","0000000000000") ```
---@source src/main/java/dan200/computercraft/core/apis/TermMethods.java:269
function monitor.blit(text, textColour, backgroundColour) end

