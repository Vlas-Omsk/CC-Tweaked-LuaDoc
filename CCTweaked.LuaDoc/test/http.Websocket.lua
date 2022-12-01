---@meta

---@class httplib
http = {}

---A websocket, which can be used to send an receive messages with a web server.
---
---@see module!http.websocket On how to open a websocket.
---@source src/main/java/dan200/computercraft/core/apis/http/websocket/WebsocketHandle.java:34
---@type Websocket
local Websocket = {}

---Wait for a message from the server.
---
---@param timeout? number The number of seconds to wait if no message is received.
---@return string . The received message.
---@return boolean . If this was a binary message.
---@throws If the websocket has been closed.
---@changed 1.80pr1.13 Added return value indicating whether the message was binary.
---@changed 1.87.0 Added timeout argument.
---@source src/main/java/dan200/computercraft/core/apis/http/websocket/WebsocketHandle.java:61
function Websocket.receive(timeout) end

---Send a websocket message to the connected server.
---
---@param message any The message to send.
---@param binary? boolean Whether this message should be treated as a
---@throws If the message is too large.
---@throws If the websocket has been closed.
---@changed 1.81.0 Added argument for binary mode.
---@source src/main/java/dan200/computercraft/core/apis/http/websocket/WebsocketHandle.java:81
function Websocket.send(message, binary) end

---Close this websocket. This will terminate the connection, meaning messages can
---no longer be sent or receivedalong it.
---
---@source src/main/java/dan200/computercraft/core/apis/http/websocket/WebsocketHandle.java:107
function Websocket.close() end

