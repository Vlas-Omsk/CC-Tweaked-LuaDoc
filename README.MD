# CC-Tweaked-LuaDoc

Type generator for CC-Tweaked.

## Using

Download types from [releases](https://github.com/Vlas-Omsk/CC-Tweaked-LuaDoc/releases) or generate them (See [Generating Types](https://github.com/Vlas-Omsk/CC-Tweaked-LuaDoc#generating-types)).

### Lua

For using with lua install [lua-language-server](https://marketplace.visualstudio.com/items?itemName=sumneko.lua) extension for VS Code and modify `.luarc.json` in workspace root directory

```diff
{
  "runtime.version": "Lua 5.1",
  "workspace.library": [
+   "/path/to/CC-Tweaked-LuaDoc/CCTweaked.LuaDoc/cc_libs_lua"
  ]
}
```

or add path to VS Code Settings

```diff
{
  "Lua.runtime.version": "Lua 5.1",
  "Lua.workspace.library": [
+   "/path/to/CC-Tweaked-LuaDoc/CCTweaked.LuaDoc/cc_libs_lua"
  ]
}
```

For more information see [lua-language-server Wiki](https://github.com/sumneko/lua-language-server/wiki/Libraries#manually-applying)

### TypeScript

For using with TypeScript install [TypeScriptToLua](https://typescripttolua.github.io) and modify `tsconfig.json` in workspace root directory

```diff
{
  "compilerOptions": {
+   "types": ["/path/to/CC-Tweaked-LuaDoc/CCTweaked.LuaDoc/cc_libs_ts"]
  }
}
```

For more information see [TypeScript Dosc](https://www.typescriptlang.org/tsconfig#types)

## Generating Types

1. Download [CC-Tweaked-LuaDoc](https://github.com/Vlas-Omsk/CC-Tweaked-LuaDoc)

2. Download [CC-Tweaked-RGB](https://github.com/Vlas-Omsk/CC-Tweaked-RGB) (or [CC-Tweaked](https://github.com/cc-tweaked/CC-Tweaked)) and follow the [development instructions](https://github.com/Vlas-Omsk/CC-Tweaked-RGB/blob/mc-1.18.x/CONTRIBUTING.md#developing).

3. After downloading CC-Tweaked source code and running `./gradlew build`, run `./gradlew illuminateDocs`

4. Run CC-Tweaked-LuaDoc with path to directory `build/illuaminate/module` in arguments, for example

```
./CC-Tweaked-LuaDoc /path/to/CC-Tweaked/build/illuaminate/module
```

5. After that, directories `cc_libs_<target language>` will be generated.
