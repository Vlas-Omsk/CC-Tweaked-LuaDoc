---@meta

---Make HTTP requests, sending and receiving data to a remote web server.
---
---@see Allowing access to local IPs To allow accessing servers running on your local network.
---
---@class httplib
http = {}

---Asynchronously make a HTTP request to the given url.
---
---This returns immediately, a http_successor http_failurewill be queued once the
---request has completed.
---
---@see http.get For a synchronous way to make GET requests.
---@see http.post For a synchronous way to make POST requests.
---
---@overload fun(request : { url: string, body?: string, headers?: { [string]: string }, binary?: boolean, method?: string, redirect?: boolean })
---@param url string The url to request
---@param body? string An optional string containing the body of the request. If specified, a `POST`request will be made instead.
---@param headers? { [string]: string } Additional headers to send as part of this request.
---@param binary? boolean Whether to make a binary HTTP request. If true, the body will not be UTF-8 encoded, and the received response will not be decoded.
---@param request { url: string, body?: string, headers?: { [string]: string }, binary?: boolean, method?: string, redirect?: boolean } Options for the request.  This table form is an expanded version of the previous syntax. All arguments from above are passed in as fields instead (for instance, `http.request(&quot;https://example.com&quot;)`becomes `http.request { url = &quot;https://example.com&quot; }`).  This table also accepts several additional options:   -  `method`: Which HTTP method to use, for instance `&quot;PATCH&quot;`or `&quot;DELETE&quot;`.  -  `redirect`: Whether to follow HTTP redirects. Defaults to true.
function http.request(url, body, headers, binary) end

---Make a HTTP GET request to the given url.
---
---@overload fun(url : string, headers? : { [string]: string }, binary? : boolean) : nil, string, Response | nil
---@overload fun(request : { url: string, headers?: { [string]: string }, binary?: boolean, method?: string, redirect?: boolean }) : Response
---@overload fun(request : { url: string, headers?: { [string]: string }, binary?: boolean, method?: string, redirect?: boolean }) : nil, string, Response | nil
---@param url string The url to request
---@param headers? { [string]: string } Additional headers to send as part of this request.
---@param binary? boolean Whether to make a binary HTTP request. If true, the body will not be UTF-8 encoded, and the received response will not be decoded.
---@param request { url: string, headers?: { [string]: string }, binary?: boolean, method?: string, redirect?: boolean } Options for the request. See http.requestfor details on how these options behave.
---@return Response . The resulting http response, which can be read from.
function http.get(url, headers, binary) end

---Make a HTTP POST request to the given url.
---
---@overload fun(url : string, body : string, headers? : { [string]: string }, binary? : boolean) : nil, string, Response | nil
---@overload fun(request : { url: string, body?: string, headers?: { [string]: string }, binary?: boolean, method?: string, redirect?: boolean }) : Response
---@overload fun(request : { url: string, body?: string, headers?: { [string]: string }, binary?: boolean, method?: string, redirect?: boolean }) : nil, string, Response | nil
---@param url string The url to request
---@param body string The body of the POST request.
---@param headers? { [string]: string } Additional headers to send as part of this request.
---@param binary? boolean Whether to make a binary HTTP request. If true, the body will not be UTF-8 encoded, and the received response will not be decoded.
---@param request { url: string, body?: string, headers?: { [string]: string }, binary?: boolean, method?: string, redirect?: boolean } Options for the request. See http.requestfor details on how these options behave.
---@return Response . The resulting http response, which can be read from.
function http.post(url, body, headers, binary) end

---Asynchronously determine whether a URL can be requested.
---
---If this returns `true`, one should also listen for http_checkwhich will
---container further information about whether the URL is allowed or not.
---
---@see http.checkURL For a synchronous version.
---
---@overload fun(url : string) : false, string
---@param url string The URL to check.
---@return true . When this url is not invalid. This does not imply that it is allowed - see the comment above.
function http.checkURLAsync(url) end

---Determine whether a URL can be requested.
---
---If this returns `true`, one should also listen for http_checkwhich will
---container further information about whether the URL is allowed or not.
---
---@see http.checkURLAsync For an asynchronous version.
---
---@overload fun(url : string) : false, string
---@param url string The URL to check.
---@return true . When this url is valid and can be requested via http.request.
function http.checkURL(url) end

---Open a websocket.
---
---@overload fun(url : string, headers? : { [string]: string }) : false, string
---@param url string The websocket url to connect to. This should have the `ws://`or `wss://`protocol.
---@param headers? { [string]: string } Additional headers to send as part of the initial websocket connection.
---@return Websocket . The websocket connection.
function http.websocket(url, headers) end

---Asynchronously open a websocket.
---
---This returns immediately, a websocket_successor websocket_failurewill be queued
---once the request has completed.
---
---@param url string The websocket url to connect to. This should have the `ws://`or `wss://`protocol.
---@param headers? { [string]: string } Additional headers to send as part of the initial websocket connection.
function http.websocketAsync(url, headers) end

---A websocket, which can be used to send an receive messages with a web server.
---
---@see http.websocket On how to open a websocket.
---
---@class Websocket
Websocket = {}

---Wait for a message from the server.
---
---@overload fun(timeout? : number) : nil
---@param timeout? number The number of seconds to wait if no message is received.
---@return string . The received message.
---@return boolean . If this was a binary message.
function Websocket.receive(timeout) end

---Send a websocket message to the connected server.
---
---@param message any The message to send.
---@param binary? boolean Whether this message should be treated as a
function Websocket.send(message, binary) end

---Close this websocket. This will terminate the connection, meaning messages can
---no longer be sent or received along it.
---
function Websocket.close() end

---A http response. This provides the same methods as a file(or binary fileif the
---request used binary mode), though provides several request specific methods.
---
---@see http.request On how to make a http request.
---
---@class Response
Response = {}

---Returns the response code and response message returned by the server.
---
---@return number . The response code (i.e. 200)
---@return string . The response message (i.e. &quot;OK&quot;)
function Response.getResponseCode() end

---Get a table containing the response's headers, in a format similar to that
---required by http.request. If multiple headers are sent with the same name, they
---will be combined with a comma.
---
---@return { [string]: string } . The response's headers.
function Response.getResponseHeaders() end

