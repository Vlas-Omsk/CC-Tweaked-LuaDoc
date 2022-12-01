---@meta

---@class file_transferlib
file_transfer = {}

---A binary file handle that has been transferred to this computer.
---
---This inherits all methods of `fs.BinaryReadHandle|binary file handles`, meaning
---you can use the standard `fs.BinaryReadHandle.read|read functions` to access the
---contents of the file.
---
---@see fs.BinaryReadHandle
---@source src/main/java/dan200/computercraft/shared/computer/upload/TransferredFile.java:26
---@type TransferredFile
local TransferredFile = {}

---Get the name of this file being transferred.
---
---@return string . The file's name.
---@source src/main/java/dan200/computercraft/shared/computer/upload/TransferredFile.java:42
function TransferredFile.getName() end

