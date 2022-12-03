using CCTweaked.LuaDoc.Entities;

namespace CCTweaked.LuaDoc.Writers;

public interface IDocWriter
{
    void Write(IEnumerable<Module> modules);
}
