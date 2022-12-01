---@meta

---Colors converters.
---
---@class colorutilslib
---@source src/main/java/dan200/computercraft/core/apis/ColourUtilsAPI.java:20
colorutils = {}

---Converts RGB bytes to RGB number.
---
---@param r number Red byte.
---@param g number Green byte.
---@param b number Blue byte.
---@return number . RGB number.
---@source src/main/java/dan200/computercraft/core/apis/ColourUtilsAPI.java:40
function colorutils.bytesToInt(r, g, b) end

---Converts RGB chars to RGB number.
---
---@param rgb string RGB chars.
---@return number . RGB number.
---@throws When given string length not equals to 3.
---@source src/main/java/dan200/computercraft/core/apis/ColourUtilsAPI.java:53
function colorutils.stringToInt(rgb) end

---Converts RGB number to RGB bytes.
---
---@param rgb number RGB number.
---@return number . Red byte.
---@return number . Green byte.
---@return number . Blue byte.
---@source src/main/java/dan200/computercraft/core/apis/ColourUtilsAPI.java:74
function colorutils.intToBytes(rgb) end

---Converts RGB number to RGB chars.
---
---@param rgb number RGB number.
---@return string . RGB chars.
---@source src/main/java/dan200/computercraft/core/apis/ColourUtilsAPI.java:87
function colorutils.intToString(rgb) end

