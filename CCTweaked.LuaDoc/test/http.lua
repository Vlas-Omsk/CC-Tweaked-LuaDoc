---@meta

---Make HTTP requests, sending and receiving data to a remote web server.
---
---@class httplib
---@since 1.1
---@see local_ips To allow accessing servers running on your local network.
http = {}

---Asynchronously make a HTTP request to the given url.
---
---This returns immediately, a `http_success` or `http_failure` will be queued once
---the request has completed.
---
---@param url string   The url to request
---@param body? string  An optional string containing the body of therequest. If specified, a `POST` request will be made instead.
---@param headers? { [string]: string } Additional headers to send as partof this request.
---@param binary? boolean Whether to make a binary HTTP request. If true,the body will not be UTF-8 encoded, and the received response will not be decoded.
---@see http.get  For a synchronous way to make GET requests.
---@see http.post For a synchronous way to make POST requests.
---@changed 1.63 Added argument for headers.
---@changed 1.80pr1 Added argument for binary handles.
---@changed 1.80pr1.6 Added support for table argument.
---@changed 1.86.0 Added PATCH and TRACE methods.
function http.request(url, body, headers, binary) end

---Make a HTTP GET request to the given url.
---
---@param url string   The url to request
---@param headers? { [string]: string } Additional headers to send as partof this request.
---@param binary? boolean Whether to make a binary HTTP request. If true,the body will not be UTF-8 encoded, and the received response will not be decoded.
---@return Response . The resulting http response, which can be read from.
---@return string . A message detailing why the request failed.
---@return Response|nil . The failing http response, if available.
---@changed 1.63 Added argument for headers.
---@changed 1.80pr1 Response handles are now returned on error if available.
---@changed 1.80pr1 Added argument for binary handles.
---@changed 1.80pr1.6 Added support for table argument.
---@changed 1.86.0 Added PATCH and TRACE methods.
---@usage Make a request to [example.tweaked.cc](https://example.tweaked.cc),and print the returned page. ```lua local request = http.get("https://example.tweaked.cc") print(request.readAll()) -- => HTTP is working! request.close() ```
function http.get(url, headers, binary) end

---Make a HTTP POST request to the given url.
---
---@param url string   The url to request
---@param body string  The body of the POST request.
---@param headers? { [string]: string } Additional headers to send as partof this request.
---@param binary? boolean Whether to make a binary HTTP request. If true,the body will not be UTF-8 encoded, and the received response will not be decoded.
---@return Response . The resulting http response, which can be read from.
---@return string . A message detailing why the request failed.
---@return Response|nil . The failing http response, if available.
---@since 1.31
---@changed 1.63 Added argument for headers.
---@changed 1.80pr1 Response handles are now returned on error if available.
---@changed 1.80pr1 Added argument for binary handles.
---@changed 1.80pr1.6 Added support for table argument.
---@changed 1.86.0 Added PATCH and TRACE methods.
function http.post(url, body, headers, binary) end

---Asynchronously determine whether a URL can be requested.
---
---If this returns `true`, one should also listen for `http_check` which will
---container further information about whether the URL is allowed or not.
---
---@param url string The URL to check.
---@return true . When this url is not invalid. This does not imply that it isallowed - see the comment above.
---@return string . A reason why this URL is not valid (for instance, if it ismalformed, or blocked).
---@see http.checkURL For a synchronous version.
function http.checkURLAsync(url) end

---Determine whether a URL can be requested.
---
---If this returns `true`, one should also listen for `http_check` which will
---container further information about whether the URL is allowed or not.
---
---@param url string The URL to check.
---@return true . When this url is valid and can be requested via `http.request`.
---@return string . A reason why this URL is not valid (for instance, if it ismalformed, or blocked).
---@see http.checkURLAsync For an asynchronous version.
---@usage ```lua print(http.checkURL("https://example.tweaked.cc/")) -- => true print(http.checkURL("http://localhost/")) -- => false Domain not permitted print(http.checkURL("not a url")) -- => false URL malformed ```
function http.checkURL(url) end

---Open a websocket.
---
---@param url string The websocket url to connect to. This should have the`ws://` or `wss://` protocol.
---@param headers? { [string]: string } Additional headers to send as partof the initial websocket connection.
---@return Websocket . The websocket connection.
---@return string . An error message describing why the connection failed.
---@since 1.80pr1.1
---@changed 1.80pr1.3 No longer asynchronous.
---@changed 1.95.3 Added User-Agent to default headers.
function http.websocket(url, headers) end

---Asynchronously open a websocket.
---
---This returns immediately, a `websocket_success` or `websocket_failure` will be
---queued once the request has completed.
---
---@param url string The websocket url to connect to. This should have the`ws://` or `wss://` protocol.
---@param headers? { [string]: string } Additional headers to send as partof the initial websocket connection.
---@since 1.80pr1.3
---@changed 1.95.3 Added User-Agent to default headers.
function http.websocketAsync(url, headers) end

