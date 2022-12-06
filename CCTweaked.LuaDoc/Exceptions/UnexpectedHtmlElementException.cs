namespace CCTweaked.LuaDoc;

public class UnexpectedHtmlElementException : Exception
{
    public UnexpectedHtmlElementException() : base("Unexpected HTML element. Probably the HTML document does not match the template.")
    {
    }
}
