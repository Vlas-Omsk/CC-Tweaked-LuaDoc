---@meta

---@class fslib
fs = {}

---A file handle opened with `module!fs.open` with the `"r"`mode.
---
---@source src/main/java/dan200/computercraft/core/apis/handles/EncodedReadableHandle.java:29
---@type ReadHandle
local ReadHandle = {}

---Read a line from the file.
---
---@param withTrailing? boolean Whether to include the newline characters with the returned string. Defaults to `false`.
---@return string|nil . The read line or `nil` if at the end of the file.
---@throws If the file has been closed.
---@changed 1.81.0 Added option to return trailing newline.
---@source src/main/java/dan200/computercraft/core/apis/handles/EncodedReadableHandle.java:55
function ReadHandle.readLine(withTrailing) end

---Read the remainder of the file.
---
---@return nil|string . The remaining contents of the file, or `nil` if we are at the end.
---@throws If the file has been closed.
---@source src/main/java/dan200/computercraft/core/apis/handles/EncodedReadableHandle.java:87
function ReadHandle.readAll() end

---Read a number of characters from this file.
---
---@param count? number The number of characters to read, defaulting to 1.
---@return string|nil . The read characters, or `nil` if at the of the file.
---@throws When trying to read a negative number of characters.
---@throws If the file has been closed.
---@since 1.80pr1.4
---@source src/main/java/dan200/computercraft/core/apis/handles/EncodedReadableHandle.java:122
function ReadHandle.read(count) end

---Close this file, freeing any resources it uses.
---
---Once a file is closed it may no longer be read or written to.
---
---@throws If the file has already been closed.
---@source src/main/java/dan200/computercraft/core/apis/handles/HandleGeneric.java:47
function ReadHandle.close() end

