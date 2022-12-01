---@meta

---@class fslib
fs = {}

---A file handle opened by `module!fs.open` using the `"wb"` or `"ab"`modes.
---
---@source src/main/java/dan200/computercraft/core/apis/handles/BinaryWritableHandle.java:27
---@type BinaryWriteHandle
local BinaryWriteHandle = {}

---Write a string or byte to the file.
---
---@param The number byte to write.
---@throws If the file has been closed.
---@changed 1.80pr1 Now accepts a string to write multiple bytes.
---@source src/main/java/dan200/computercraft/core/apis/handles/BinaryWritableHandle.java:60
function BinaryWriteHandle.write(The) end

---Save the current file without closing it.
---
---@throws If the file has been closed.
---@source src/main/java/dan200/computercraft/core/apis/handles/BinaryWritableHandle.java:96
function BinaryWriteHandle.flush() end

---Close this file, freeing any resources it uses.
---
---Once a file is closed it may no longer be read or written to.
---
---@throws If the file has already been closed.
---@source src/main/java/dan200/computercraft/core/apis/handles/HandleGeneric.java:47
function BinaryWriteHandle.close() end

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
---@source src/main/java/dan200/computercraft/core/apis/handles/BinaryWritableHandle.java:136
function BinaryWriteHandle.seek(whence, offset) end

