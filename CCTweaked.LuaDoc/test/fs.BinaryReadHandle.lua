---@meta

---@class fslib
fs = {}

---A file handle opened with `module!fs.open` with the `"rb"`mode.
---
---@source src/main/java/dan200/computercraft/core/apis/handles/BinaryReadableHandle.java:27
---@type BinaryReadHandle
local BinaryReadHandle = {}

---Read a number of bytes from this file.
---
---@param count? number The number of bytes to read. When absent, a single byte will be read <em>as a number</em>. Thismay be 0 to determine we are at the end of the file.
---@return nil . If we are at the end of the file.
---@throws When trying to read a negative number of bytes.
---@throws If the file has been closed.
---@changed 1.80pr1 Now accepts an integer argument to read multiple bytes, returning a string instead of a number.
---@source src/main/java/dan200/computercraft/core/apis/handles/BinaryReadableHandle.java:66
function BinaryReadHandle.read(count) end

---Read the remainder of the file.
---
---@return string|nil . The remaining contents of the file, or `nil` if we are at the end.
---@throws If the file has been closed.
---@since 1.80pr1
---@source src/main/java/dan200/computercraft/core/apis/handles/BinaryReadableHandle.java:151
function BinaryReadHandle.readAll() end

---Read a line from the file.
---
---@param withTrailing? boolean Whether to include the newline characters with the returned string. Defaults to `false`.
---@return string|nil . The read line or `nil` if at the end of the file.
---@throws If the file has been closed.
---@since 1.80pr1.9
---@changed 1.81.0 `\r` is now stripped.
---@source src/main/java/dan200/computercraft/core/apis/handles/BinaryReadableHandle.java:190
function BinaryReadHandle.readLine(withTrailing) end

---Close this file, freeing any resources it uses.
---
---Once a file is closed it may no longer be read or written to.
---
---@throws If the file has already been closed.
---@source src/main/java/dan200/computercraft/core/apis/handles/HandleGeneric.java:47
function BinaryReadHandle.close() end

---Seek to a new position within the file, changing where bytes are written to. The
---new position is an offsetgiven by `offset`, relative to a start position
---determined by `whence`:
---
---- `"set"`: `offset` is relative to the beginning of the file. - `"cur"`:
---Relative to the current position. This is the default. - `"end"`: Relative to
---the end of the file.
---
---In case of success, `seek` returns the new file position from the beginning of
---the file.
---
---@param whence? string Where the offset is relative to.
---@param offset? number The offset to seek to.
---@return number . The new position.
---@return string . The reason seeking failed.
---@throws If the file has been closed.
---@since 1.80pr1.9
---@source src/main/java/dan200/computercraft/core/apis/handles/BinaryReadableHandle.java:268
function BinaryReadHandle.seek(whence, offset) end

