---@meta

---Modems allow you to send messages between computers over long distances.
---
---:::tip Modems provide a fairly basic set of methods, which makes them very
---flexible but often hard to work with. The `rednet` API is built on top of
---modems, and provides a more user-friendly interface. :::
---
---## Sending and receiving messages Modems operate on a series of channels, a bit
---like frequencies on a radio. Any modem can send a message on a particular
---channel, but only those which have `open|opened` the channel and are "listening
---in" can receive messages.
---
---Channels are represented as an integer between 0 and 65535 inclusive. These
---channels don't have any defined meaning, though some APIs or programs will
---assign a meaning to them. For instance, the `gps` module sends all its messages
---on channel 65534 (`gps.CHANNEL_GPS`), while `rednet` uses channels equal to the
---computer's ID.
---
---- Sending messages is done with the `transmit` message. - Receiving messages is
---done by listening to the `modem_message` event.
---
---## Types of modem CC: Tweaked comes with three kinds of modem, with different
---capabilities.
---
---
---- <strong>Wireless modems:</strong> Wireless modems can send messages to any
---other wireless modem. They can be placed next to a computer, or equipped as a
---pocket computer or turtle upgrade.
---
---Wireless modems have a limited range, only sending messages to modems within 64
---blocks. This range increases linearly once the modem is above y=96, to a maximum
---of 384 at world height. - <strong>Ender modems:</strong> These are upgraded
---versions of normal wireless modems. They do not have a distance limit, and can
---send messages between dimensions. - <strong>Wired modems:</strong> These send
---messages to other any other wired modems connected to the same network (using
---<em>Networking Cable</em>). They also can be used to attach additional
---peripherals to a computer.
---
---@class modemlib
---@see modem_message Queued when a modem receives a message on an @{open|open channel}.
---@see rednet A networking API built on top of the modem peripheral.
---@usage Wrap a modem and a message on channel 15, requesting a response on channel 43. Then wait for a message toarrive on channel 43 and print it.  ```lua local modem = peripheral.find("modem") or error("No modem attached", 0) modem.open(43) -- Open 43 so we can receive replies  -- Send our message modem.transmit(15, 43, "Hello, world!")  -- And wait for a reply local event, side, channel, replyChannel, message, distance repeat event, side, channel, replyChannel, message, distance = os.pullEvent("modem_message") until channel == 43  print("Received a reply: " .. tostring(message)) ```  ## Recipes <div class="recipe-container"> <mc-recipe recipe="computercraft:wireless_modem_normal"></mc-recipe> <mc-recipe recipe="computercraft:wireless_modem_advanced"></mc-recipe> <mc-recipe recipe="computercraft:wired_modem"></mc-recipe> <mc-recipe recipe="computercraft:cable"></mc-recipe> <mc-recipe recipe="computercraft:wired_modem_full_from"></mc-recipe> </div>
---@source src/main/java/dan200/computercraft/shared/peripheral/modem/ModemPeripheral.java:88
modem = {}

---Open a channel on a modem. A channel must be open in order to receive messages.
---Modems can have up to 128channels open at one time.
---
---@param channel number The channel to open. This must be a number between 0 and 65535.
---@throws If the channel is out of range.
---@throws If there are too many open channels.
---@source src/main/java/dan200/computercraft/shared/peripheral/modem/ModemPeripheral.java:176
function modem.open(channel) end

---Check if a channel is open.
---
---@param channel number The channel to check.
---@return boolean . Whether the channel is open.
---@throws If the channel is out of range.
---@source src/main/java/dan200/computercraft/shared/peripheral/modem/ModemPeripheral.java:189
function modem.isOpen(channel) end

---Close an open channel, meaning it will no longer receive messages.
---
---@param channel number The channel to close.
---@throws If the channel is out of range.
---@source src/main/java/dan200/computercraft/shared/peripheral/modem/ModemPeripheral.java:201
function modem.close(channel) end

---Close all open channels.
---
---@source src/main/java/dan200/computercraft/shared/peripheral/modem/ModemPeripheral.java:210
function modem.closeAll() end

---Sends a modem message on a certain channel. Modems listening on the channel will
---queue a `modem_message`event on adjacent computers.
---
---:::note The channel does not need be open to send a message. :::
---
---@param channel number The channel to send messages on.
---@param replyChannel number The channel that responses to this message should be sent on. This can be the same as`channel` or entirely different. The channel must have been `open|opened` on the sending computer in order to receive the replies.
---@param payload any The object to send. This can be any primitive type (boolean, number, string) as well astables. Other types (like functions), as well as metatables, will not be transmitted.
---@throws If the channel is out of range.
---@usage Wrap a modem and a message on channel 15, requesting a response on channel 43.  ```lua local modem = peripheral.find("modem") or error("No modem attached", 0) modem.transmit(15, 43, "Hello, world!") ```
---@source src/main/java/dan200/computercraft/shared/peripheral/modem/ModemPeripheral.java:238
function modem.transmit(channel, replyChannel, payload) end

---Determine if this is a wired or wireless modem.
---
---Some methods (namely those dealing with wired networks and remote peripherals)
---are only available on wired modems.
---
---@return boolean . `true` if this is a wireless modem.
---@source src/main/java/dan200/computercraft/shared/peripheral/modem/ModemPeripheral.java:269
function modem.isWireless() end

---List all remote peripherals on the wired network.
---
---If this computer is attached to the network, it _will not_ be included in this
---list.
---
---:::note This function only appears on wired modems. Check `isWireless` returns
---false before calling it. :::
---
---@return { [number]: string } . Remote peripheral names on the network.
---@source src/main/java/dan200/computercraft/shared/peripheral/modem/wired/WiredModemPeripheral.java:97
function modem.getNamesRemote() end

---Determine if a peripheral is available on this wired network.
---
---:::note This function only appears on wired modems. Check `isWireless` returns
---false before calling it. :::
---
---@param name string The peripheral's name.
---@return boolean . boolean If a peripheral is present with the given name.
---@see module!peripheral.isPresent
---@source src/main/java/dan200/computercraft/shared/peripheral/modem/wired/WiredModemPeripheral.java:115
function modem.isPresentRemote(name) end

---Get the type of a peripheral is available on this wired network.
---
---:::note This function only appears on wired modems. Check `isWireless` returns
---false before calling it. :::
---
---@param name string The peripheral's name.
---@return string|nil . The peripheral's type, or `nil` if it is not present.
---@changed 1.99 Peripherals can have multiple types - this function returns multiple values.
---@see module!peripheral.getType
---@source src/main/java/dan200/computercraft/shared/peripheral/modem/wired/WiredModemPeripheral.java:135
function modem.getTypeRemote(name) end

---Check a peripheral is of a particular type.
---
---:::note This function only appears on wired modems. Check `isWireless` returns
---false before calling it. :::
---
---@param name string The peripheral's name.
---@param type string The type to check.
---@return boolean|nil . If a peripheral has a particular type, or `nil` if it is not present.
---@since 1.99
---@see module!peripheral.getType
---@source src/main/java/dan200/computercraft/shared/peripheral/modem/wired/WiredModemPeripheral.java:157
function modem.hasTypeRemote(name, type) end

---Get all available methods for the remote peripheral with the given name.
---
---:::note This function only appears on wired modems. Check `isWireless` returns
---false before calling it. :::
---
---@param name string The peripheral's name.
---@return { [number]: string }|nil . A list of methods provided by this peripheral, or `nil` if it is not present.
---@see module!peripheral.getMethods
---@source src/main/java/dan200/computercraft/shared/peripheral/modem/wired/WiredModemPeripheral.java:177
function modem.getMethodsRemote(name) end

---Call a method on a peripheral on this wired network.
---
---:::note This function only appears on wired modems. Check `isWireless` returns
---false before calling it. :::
---
---@param remoteName string The name of the peripheral to invoke the method on.
---@param method string The name of the method
---@param ... any Additional arguments to pass to the method
---@return string . The return values of the peripheral method.
---@see module!peripheral.call
---@source src/main/java/dan200/computercraft/shared/peripheral/modem/wired/WiredModemPeripheral.java:204
function modem.callRemote(remoteName, method, ...) end

---Returns the network name of the current computer, if the modem is on. Thismay be
---used by other computers on the network to wrap this computer as a peripheral.
---
---:::note This function only appears on wired modems. Check `isWireless` returns
---false before calling it. :::
---
---@return string|nil . The current computer's name on the wired network.
---@since 1.80pr1.7
---@source src/main/java/dan200/computercraft/shared/peripheral/modem/wired/WiredModemPeripheral.java:228
function modem.getNameLocal() end

