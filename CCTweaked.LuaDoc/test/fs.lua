---@meta

---@class fslib
fs = {}

---Returns true if a path is mounted to the parent filesystem.
---
---The root filesystem "/" is considered a mount, along with disk folders and the
---rom folder. Other programs (such as network shares) can exstend this to make
---other mount types by correctly assigning their return value for getDrive.
---
---@param path string The path to check.
---@return boolean . If the path is mounted, rather than a normal file/folder.
---@throws If the path does not exist.
---@see getDrive
---@since 1.87.0
function fs.isDriveRoot(path) end

---Provides completion for a file or directory name, suitable for use
---with`_G.read`.
---
---When a directory is a possible candidate for completion, two entries are
---included - one with a trailing slash (indicating that entries within this
---directory exist) and one without it (meaning this entry is an immediate
---completion candidate). `include_dirs` can be set to `false` to only include
---those with a trailing slash.
---
---@param path string The path to complete.
---@param location string The location where paths are resolved from.
---@param include_files? boolean When `false`, only directories willbe included in the returned list.
---@param include_dirs? boolean When `false`, "raw" directories willnot be included in the returned list.
---@return { [number]: string } . A list of possible completion candidates.
---@since 1.74
---@changed 1.101.0
---@usage Complete files in the root directory.  read(nil, nil, function(str) return fs.complete(str, "", true, false) end)
---@usage Complete files in the root directory, hiding hidden files by default.  read(nil, nil, function(str) return fs.complete(str, "", { include_files = true, include_dirs = false, included_hidden = false, }) end)
function fs.complete(path, location, include_files, include_dirs) end

