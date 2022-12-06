namespace CCTweaked.LuaDoc;

public class UnexpectedEndOfHtmlElementContentException : Exception
{
    public UnexpectedEndOfHtmlElementContentException() : base("Unexpected end of HTML element content. Probably the HTML document does not match the template.")
    {
    }
}
