---@meta

---@class httplib
http = {}

---A http response. This provides the same methods as a `fs.ReadHandle|file`
---(or`fs.BinaryReadHandle|binary file` if the request used binary mode), though
---provides several request specific methods.
---
---@see module!http.request On how to make a http request.
---@source src/main/java/dan200/computercraft/core/apis/http/request/HttpResponseHandle.java:28
---@type Response
local Response = {}

---Returns the response code and response message returned by the server.
---
---@return number . The response code (i.e. 200)
---@return string . The response message (i.e. "OK")
---@changed 1.80pr1.13 Added response message return value.
---@source src/main/java/dan200/computercraft/core/apis/http/request/HttpResponseHandle.java:51
function Response.getResponseCode() end

---Get a table containing the response's headers, in a format similar to that
---required by `module!http.request`.If multiple headers are sent with the same
---name, they will be combined with a comma.
---
---@return { [string]: string } . The response's headers.
---@usage Make a request to [example.tweaked.cc](https://example.tweaked.cc), and print thereturned headers. ```lua local request = http.get("https://example.tweaked.cc") print(textutils.serialize(request.getResponseHeaders())) -- => { --  [ "Content-Type" ] = "text/plain; charset=utf8", --  [ "content-length" ] = 17, --  ... -- } request.close() ```
---@source src/main/java/dan200/computercraft/core/apis/http/request/HttpResponseHandle.java:75
function Response.getResponseHeaders() end

