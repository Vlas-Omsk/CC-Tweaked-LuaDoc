---@meta

---@class fslib
fs = {}

---A file handle opened by `module!fs.open` using the `"w"` or `"a"` modes.
---
---@source src/main/java/dan200/computercraft/core/apis/handles/EncodedWritableHandle.java:29
---@type WriteHandle
local WriteHandle = {}

---Write a string of characters to the file.
---
---@param value any The value to write to the file.
---@throws If the file has been closed.
---@source src/main/java/dan200/computercraft/core/apis/handles/EncodedWritableHandle.java:46
function WriteHandle.write(value) end

---Write a string of characters to the file, follwing them with a new line
---character.
---
---@param value any The value to write to the file.
---@throws If the file has been closed.
---@source src/main/java/dan200/computercraft/core/apis/handles/EncodedWritableHandle.java:68
function WriteHandle.writeLine(value) end

---Save the current file without closing it.
---
---@throws If the file has been closed.
---@source src/main/java/dan200/computercraft/core/apis/handles/EncodedWritableHandle.java:89
function WriteHandle.flush() end

---Close this file, freeing any resources it uses.
---
---Once a file is closed it may no longer be read or written to.
---
---@throws If the file has already been closed.
---@source src/main/java/dan200/computercraft/core/apis/handles/HandleGeneric.java:47
function WriteHandle.close() end

