---@meta

---Disk drives are a peripheral which allow you to read and write to floppy disks
---and other "mountable media" (such ascomputers or turtles). They also allow you
---to `playAudio|play records`.
---
---When a disk drive attaches some mount (such as a floppy disk or computer), it
---attaches a folder called `disk`, `disk2`, etc... to the root directory of the
---computer. This folder can be used to interact with the files on that disk.
---
---When a disk is inserted, a `disk` event is fired, with the side peripheral is
---on. Likewise, when the disk is detached, a `disk_eject` event is fired.
---
---## Recipe <div class="recipe-container"> <mc-recipe
---recipe="computercraft:disk_drive"></mc-recipe> </div>
---
---@class drivelib
---@source src/main/java/dan200/computercraft/shared/peripheral/diskdrive/DiskDrivePeripheral.java:40
drive = {}

---Returns whether a disk is currently inserted in the drive.
---
---@return boolean . Whether a disk is currently inserted in the drive.
---@source src/main/java/dan200/computercraft/shared/peripheral/diskdrive/DiskDrivePeripheral.java:61
function drive.isDiskPresent() end

---Returns the label of the disk in the drive if available.
---
---@return string . The label of the disk, or `nil` if either no disk is inserted or the disk doesn't have a label.
---@source src/main/java/dan200/computercraft/shared/peripheral/diskdrive/DiskDrivePeripheral.java:73
function drive.getDiskLabel() end

---Sets or clears the label for a disk.
---
---If no label or `nil` is passed, the label will be cleared.
---
---If the inserted disk's label can't be changed (for example, a record), an error
---will be thrown.
---
---@param label? string The new label of the disk, or `nil` to clear.
---@throws If the disk's label can't be changed.
---@source src/main/java/dan200/computercraft/shared/peripheral/diskdrive/DiskDrivePeripheral.java:92
function drive.setDiskLabel(label) end

---Returns whether a disk with data is inserted.
---
---@return boolean . Whether a disk with data is inserted.
---@source src/main/java/dan200/computercraft/shared/peripheral/diskdrive/DiskDrivePeripheral.java:113
function drive.hasData() end

---Returns the mount path for the inserted disk.
---
---@return string|nil . The mount path for the disk, or `nil` if no data disk is inserted.
---@source src/main/java/dan200/computercraft/shared/peripheral/diskdrive/DiskDrivePeripheral.java:125
function drive.getMountPath() end

---Returns whether a disk with audio is inserted.
---
---@return boolean . Whether a disk with audio is inserted.
---@source src/main/java/dan200/computercraft/shared/peripheral/diskdrive/DiskDrivePeripheral.java:137
function drive.hasAudio() end

---Returns the title of the inserted audio disk.
---
---@return string|nil|false . The title of the audio, `false` if no disk is inserted, or `nil` if the disk has no audio.
---@source src/main/java/dan200/computercraft/shared/peripheral/diskdrive/DiskDrivePeripheral.java:151
function drive.getAudioTitle() end

---Plays the audio in the inserted disk, if available.
---
---@source src/main/java/dan200/computercraft/shared/peripheral/diskdrive/DiskDrivePeripheral.java:163
function drive.playAudio() end

---Stops any audio that may be playing.
---
---@see playAudio
---@source src/main/java/dan200/computercraft/shared/peripheral/diskdrive/DiskDrivePeripheral.java:174
function drive.stopAudio() end

---Ejects any disk that may be in the drive.
---
---@source src/main/java/dan200/computercraft/shared/peripheral/diskdrive/DiskDrivePeripheral.java:183
function drive.ejectDisk() end

---Returns the ID of the disk inserted in the drive.
---
---@return number . The The ID of the disk in the drive, or `nil` if no disk with an ID is inserted.
---@since 1.4
---@source src/main/java/dan200/computercraft/shared/peripheral/diskdrive/DiskDrivePeripheral.java:196
function drive.getDiskID() end

