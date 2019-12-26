
namespace Azarashi.Utilities.HTML
{
    public class HTMLParser
    {
        string Source { get; }

        public HTMLParser(string source)
        {
            Source = source;
        }

        public TagList Parse()
        {
            TagList tagList = new TagList();

            for (int i = 0; i < Source.Length; i++)
            {
                if (Source[i] != '<') continue;
                i++;
                if (!IsEffectiveCharacter(Source[i])) continue;

                //不格好. 変えたい.
                if (IsCommentBegin(i))
                {
                    i = SkipComment(i);
                    i--;
                    continue;
                }

                var findTagResult = FindTagName(i);
                i = findTagResult.index;
                Tag tag = new Tag(findTagResult.tagName);

                i = SkipWhiteSpace(i);

                i = ParseTag(i, tag);

                tagList.Add(tag);
                continue;
            }

            return tagList;
        }

        bool IsEffectiveCharacter(char character)
        {
            return IsAlphabet(character) || character == '!' || character == '/';
        }

        bool IsAlphabet(char character)
        {
            character = char.ToLower(character);
            return character >= 'a' && character <= 'z';
        }

        int SkipComment(int index)
        {
            if (!IsCommentBegin(index)) return index;

            for (; index + 2 < Source.Length; index++)
            {
                if (IsCommentEnd(index)) break;
            }
            index += 3;
            return index;
        }

        bool IsCommentBegin(int offset)
        {
            return Source[offset] == '!' && Source[offset + 1] == '-' && Source[offset + 2] == '-';
        }
        bool IsCommentEnd(int offset)
        {
            return Source[offset] == '-' && Source[offset + 1] == '-' && Source[offset + 2] == '>';
        }

        (int index, string tagName) FindTagName(int index)
        {
            string tagName = string.Empty;
            for (; index < Source.Length; index++)
            {
                if (Source[index] == ' ' || Source[index] == '>') break;
                tagName += Source[index];
            }

            return (index, tagName);
        }

        int SkipWhiteSpace(int index)
        {
            for (; index < Source.Length; index++)
                if (Source[index] != ' ')
                    break;

            return index;
        }

        int ParseTag(int index, Tag tag)
        {
            while (Source[index] != '>')
            {
                string attributeName = string.Empty;
                string attributeValue = string.Empty;

                var parseAttributeNameResult = ParseAttributeName(index);
                index = parseAttributeNameResult.index;
                attributeName = parseAttributeNameResult.attributeName;

                if (Source[index] != '>')
                {
                    var parseAttributeValueResult = ParseAttributeValue(index);
                    index = parseAttributeValueResult.index;
                    attributeValue = parseAttributeValueResult.attributeValue;
                }

                Attribute attribute = new Attribute(attributeName, attributeValue);
                tag.Add(attribute.Name.ToLower(), attribute);
            }

            return index;
        }

        public (int index, string attributeName) ParseAttributeName(int index)
        {
            string attributeName = string.Empty;
            index = SkipWhiteSpace(index);
            for (; index < Source.Length; index++)
            {
                if (Source[index] == ' ' ||
                  Source[index] == '=' ||
                  Source[index] == '>')
                    break;
                attributeName += Source[index];
            }
            index = SkipWhiteSpace(index);

            return (index, attributeName);
        }

        public (int index,string attributeValue) ParseAttributeValue(int index)
        {
            if (Source[index] != '=') return (index++, string.Empty);

            index++;
            string attributeValue = string.Empty;
            index = SkipWhiteSpace(index);
            if (Source[index] == '\'' || Source[index] == '\"')
            {
                char deliminator = Source[index];
                index++;
                for (; index < Source.Length; index++)
                {
                    if (Source[index] == deliminator) break;

                    attributeValue += Source[index];
                }
                index++;
            }
            else
            {
                for (; index < Source.Length; index++)
                {
                    if (Source[index] == ' ' || Source[index] == '>') break;

                    attributeValue += Source[index];
                }
            }
            index = SkipWhiteSpace(index);
        
            return (index, attributeValue);
        }
    }
}