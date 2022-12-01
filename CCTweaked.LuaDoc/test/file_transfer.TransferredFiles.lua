---@meta

---@class file_transferlib
file_transfer = {}

---A list of files that have been transferred to this computer.
---
---@source src/main/java/dan200/computercraft/shared/computer/upload/TransferredFiles.java:22
---@type TransferredFiles
local TransferredFiles = {}

---All the files that are being transferred to this computer.
---
---@return { [number]: event!file_transfer.TransferredFile } . The list of files.
---@source src/main/java/dan200/computercraft/shared/computer/upload/TransferredFiles.java:42
function TransferredFiles.getFiles() end

