---@meta

---Functions in the global environment, defined in `bios.lua`. This does not
---include standard Lua functions.
---
---@class _Glib
_G = {}

---Pauses execution for the specified number of seconds.
---
---As it waits for a fixed amount of world ticks, `time` will automatically be
---rounded up to the nearest multiple of 0.05 seconds. If you are using coroutines
---or the `parallel|parallel API`, it will only pause execution of the current
---thread, not the whole program.
---
---:::tip Because sleep internally uses timers, it is a function that yields. This
---means that you can use it to prevent "Too long without yielding" errors.
---However, as the minimum sleep time is 0.05 seconds, it will slow your program
---down. :::
---
---:::caution Internally, this function queues and waits for a timer event (using
---`os.startTimer`), however it does not listen for any other events. This means
---that any event that occurs while sleeping will be entirely discarded. If you
---need to receive events while sleeping, consider using `os.startTimer|timers`, or
---the `parallel|parallel API`. :::
---
---@param time number The number of seconds to sleep for, rounded up to thenearest multiple of 0.05.
---@see os.startTimer
---@usage Sleep for three seconds.  print("Sleeping for three seconds") sleep(3) print("Done!")
function _G.sleep(time) end

---Writes a line of text to the screen without a newline at the end, wrappingtext
---if necessary.
---
---@param text string The text to write to the string
---@return number . The number of lines written
---@see print A wrapper around write that adds a newline and accepts multiple arguments
---@usage write("Hello, world")
function _G.write(text) end

---Prints the specified values to the screen separated by spaces, wrapping
---ifnecessary. After printing, the cursor is moved to the next line.
---
---@param ... any The values to print on the screen
---@return number . The number of lines written
---@usage print("Hello, world!")
function _G.print(...) end

---Prints the specified values to the screen in red, separated by spaces,wrapping
---if necessary. After printing, the cursor is moved to the next line.
---
---@param ... any The values to print on the screen
---@usage printError("Something went wrong!")
function _G.printError(...) end

---Reads user input from the terminal. This automatically handles arrow
---keys,pasting, character replacement, history scrollback, auto-completion, and
---default values.
---
---@param replaceChar? string A character to replace each typed character with.This can be used for hiding passwords, for example.
---@param history? table A table holding history items that can be scrolledback to with the up/down arrow keys. The oldest item is at index 1, while the newest item is at the highest index.
---@param completeFn? fun(partial: string):({ [number]: string }|nil) A functionto be used for completion. This function should take the partial text typed so far, and returns a list of possible completion options.
---@param default? string Default text which should already be entered intothe prompt.
---@return string . The text typed in.
---@see cc.completion For functions to help with completion.
---@usage Read a string and echo it back to the user  write("> ") local msg = read() print(msg)
---@usage Prompt a user for a password.  while true do write("Password> ") local pwd = read("*") if pwd == "let me in" then break end print("Incorrect password, try again.") end print("Logged in!")
---@usage A complete example with completion, history and a default value.  local completion = require "cc.completion" local history = { "potato", "orange", "apple" } local choices = { "apple", "orange", "banana", "strawberry" } write("> ") local msg = read(nil, history, function(text) return completion.choice(text, choices) end, "app") print(msg)
---@changed 1.74 Added `completeFn` parameter.
---@changed 1.80pr1 Added `default` parameter.
function _G.read(replaceChar, history, completeFn, default) end

---Stores the current ComputerCraft and Minecraft versions.
---
---Outside of Minecraft (for instance, in an emulator) `_HOST` will contain the
---emulator's version instead.
---
---For example, `ComputerCraft 1.93.0 (Minecraft 1.15.2)`.
---
---@usage Print the current computer's environment.  print(_HOST)
---@since 1.76
_HOST = _HOST

---The default computer settings as defined in the ComputerCraftconfiguration.
---
---This is a comma-separated list of settings pairs defined by the mod
---configuration or server owner. By default, it is empty.
---
---An example value to disable autocompletion:
---
---shell.autocomplete=false,lua.autocomplete=false,edit.autocomplete=false
---
---@usage _CC_DEFAULT_SETTINGS
---@since 1.77
_CC_DEFAULT_SETTINGS = _CC_DEFAULT_SETTINGS

