---@meta

---The printer peripheral allows pages and books to be printed.
---
---## Recipe <div class="recipe-container"> <mc-recipe
---recipe="computercraft:printer"></mc-recipe> </div>
---
---@class printerlib
---@source src/main/java/dan200/computercraft/shared/peripheral/printer/PrinterPeripheral.java:28
printer = {}

---Writes text to the current page.
---
---@param ... string|number The values to write to the page.
---@throws If any values couldn't be converted to a string, or if no page is started.
---@source src/main/java/dan200/computercraft/shared/peripheral/printer/PrinterPeripheral.java:57
function printer.write(...) end

---Returns the current position of the cursor on the page.
---
---@return number . The X position of the cursor.
---@return number . The Y position of the cursor.
---@throws If a page isn't being printed.
---@source src/main/java/dan200/computercraft/shared/peripheral/printer/PrinterPeripheral.java:74
function printer.getCursorPos() end

---Sets the position of the cursor on the page.
---
---@param x number The X coordinate to set the cursor at.
---@param y number The Y coordinate to set the cursor at.
---@throws If a page isn't being printed.
---@source src/main/java/dan200/computercraft/shared/peripheral/printer/PrinterPeripheral.java:90
function printer.setCursorPos(x, y) end

---Returns the size of the current page.
---
---@return number . The width of the page.
---@return number . The height of the page.
---@throws If a page isn't being printed.
---@source src/main/java/dan200/computercraft/shared/peripheral/printer/PrinterPeripheral.java:105
function printer.getPageSize() end

---Starts printing a new page.
---
---@return boolean . Whether a new page could be started.
---@source src/main/java/dan200/computercraft/shared/peripheral/printer/PrinterPeripheral.java:119
function printer.newPage() end

---Finalizes printing of the current page and outputs it to the tray.
---
---@return boolean . Whether the page could be successfully finished.
---@throws If a page isn't being printed.
---@source src/main/java/dan200/computercraft/shared/peripheral/printer/PrinterPeripheral.java:131
function printer.endPage() end

---Sets the title of the current page.
---
---@param title? string The title to set for the page.
---@throws If a page isn't being printed.
---@source src/main/java/dan200/computercraft/shared/peripheral/printer/PrinterPeripheral.java:144
function printer.setPageTitle(title) end

---Returns the amount of ink left in the printer.
---
---@return number . The amount of ink available to print with.
---@source src/main/java/dan200/computercraft/shared/peripheral/printer/PrinterPeripheral.java:156
function printer.getInkLevel() end

---Returns the amount of paper left in the printer.
---
---@return number . The amount of paper available to print with.
---@source src/main/java/dan200/computercraft/shared/peripheral/printer/PrinterPeripheral.java:167
function printer.getPaperLevel() end

