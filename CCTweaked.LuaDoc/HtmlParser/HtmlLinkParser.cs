using System.Text.RegularExpressions;
using CCTweaked.LuaDoc.Entities.Description;

namespace CCTweaked.LuaDoc.HtmlParser;

public static class HtmlLinkParser
{
    private readonly static Uri _baseUri = new Uri("https://tweaked.cc");

    public static LinkNode ParseLink(string basePath, string href, string name)
    {
        var match = Regex.Match(href, @"\/?([^/]+)\.html(#(v|ty):(.+))?");

        var link = match.Groups[1].Value;

        if (match.Groups[2].Success)
            link += '.' + match.Groups[4].Value.Replace(':', '.');

        name = name.Replace(':', '.');

        if (
            name.Equals(link, StringComparison.InvariantCultureIgnoreCase) ||
            name.Equals(match.Groups[4].Value.Replace(':', '.'), StringComparison.InvariantCultureIgnoreCase)
        )
        {
            return new LinkNode(LinkNodeType.TypeLink, link, name);
        }
        else
        {
            Uri uri;

            if (!Uri.TryCreate(href, UriKind.Absolute, out uri))
            {
                uri = new Uri(_baseUri, basePath + '/');
                uri = new Uri(uri, href);
            }

            return new LinkNode(LinkNodeType.ExternalLink, uri.ToString(), name);
        }
    }
}
