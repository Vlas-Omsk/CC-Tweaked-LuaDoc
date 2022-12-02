using HtmlAgilityPack;

namespace CCTweaked.LuaDoc;

public static class IEnumeratorHtmlNode
{
    public static bool MoveToNextTaggedNode(this IEnumerator<HtmlNode> self)
    {
        while (true)
        {
            if (!self.MoveNext())
                return false;

            if (self.Current.Name != "#text")
                return true;
        }
    }
}
