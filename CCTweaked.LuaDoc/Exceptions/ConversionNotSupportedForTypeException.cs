namespace CCTweaked.LuaDoc;

public class ConversionNotSupportedForTypeException : Exception
{
    public ConversionNotSupportedForTypeException(Type type) : base("Conversion not supported for type " + type)
    {
    }
}
