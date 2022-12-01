namespace CCTweaked.LuaDoc;

public static class TypeParser
{
    public static int GetTypeEnd(string data, int index)
    {
        if (data[index..(index + "function".Length)] == "function")
        {
            index = GetBracketsEnd(data, index + "function".Length, '(', ')');

            for (var i = index; i < data.Length; i++)
            {
                var ch = data[index];

                if (ch == ' ')
                    continue;

                if (ch == ':')
                {
                    index = GetTypeEnd(data, i + 1);
                    break;
                }
            }

            return index;
        }

        int newIndex = -1;

        for (; index < data.Length; index++)
        {
            var ch = data[index];

            if (ch == ' ')
                continue;

            if (ch == '{')
            {
                newIndex = GetBracketsEnd(data, index, '{', '}');
                break;
            }

            if (ch == '(')
            {
                newIndex = GetBracketsEnd(data, index, '(', ')');
                break;
            }

            if (char.IsLetter(ch))
            {
                newIndex = GetWordEnd(data, index);
                break;
            }
        }

        if (newIndex == -1)
            return -1;

        index = newIndex;

        if (data[index] == '|')
            index = GetTypeEnd(data, index + 1);
        if (data[index] == '&')
            index = GetTypeEnd(data, index + 1);

        return index;
    }

    public static int GetWordEnd(string data, int index)
    {
        for (; index < data.Length; index++)
        {
            var ch = data[index];

            if (!char.IsLetter(ch) && ch != '_' && ch != '.')
                return index;
        }

        return -1;
    }

    public static int GetBracketsEnd(string data, int index, char start, char end)
    {
        var depth = 0;

        for (; index < data.Length; index++)
        {
            var ch = data[index];

            if (ch == ' ')
                continue;

            if (ch == start)
                depth++;

            if (ch == end)
            {
                depth--;

                if (depth == 0)
                    return index + 1;
            }
        }

        return -1;
    }
}
