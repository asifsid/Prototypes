using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JNav
{
    // Object       { ... }
    // Array        [ ... ]
    // Value
    //  - null      null
    //  - string    " ... "
    //  - number    (decimal)
    //  - bool      true/false
    /* the json is a segment
     *   - segment start/end marks the unparsed start, end
     *   - segment.Parse() should return a node with it's specific type 
     *   - If a node is
     *        - null, is null
     *        - string, number, bool - value 
     *        - array
     *        
     *  1. Segment Create
     *  2. Trim - skip whitespace
     *  3. ParseNode
     *  3.1. Create typed node based on enclosure, Object/Array/String 
     *  3.2. Else create typed node based on keyword null,true,false
     *  3.3. Else number 
     *  3.4. Else Invalid
     *  
     *  Split: array of split chars assumed to appear in the array order (if not matched, error out)
     *  
     *  1. Set segment start to cur index.
     *  2. Find next sep-char - at current level
     *  2.1. If ", skip until string closed
     *  2.2. If { or [, skip until all closed
     *  2.3. else, stop if found sep char.
     *  2.4. Segment end is cur index - 1
     *  2.5. Until end of parent seg
     *  
     *  Content Parse: Object.Properties/Values, Array.Items
     *  Use inner-content and call segment.Split([ ',' ] or [ ':' , ',' ])
     *  
     *  Object: Enumerate Items to property 
     *  1. Segment[0] is Key (JNavString.Value) 
     *  2. Segment[1] is Value (Don't Parse until used)
     *  
     *  Array: Enumerate Items of Segments, don't parse until accessed
     *  
     *        
     *            { "x" : [ { "a" : "f00", "b": 123 }, "Test", true, [ ... ] ], "y" : null }    
     * seg 1    ^----------------------------------------------------------------------------^
     * trimmed    ^------------- trim -----------------------------------------------------^
     * Node: Object
     * seg 2..5     ^--^:^---------------------------------------------------^,^---^:^----^
     * seg 6..9            ^------------------------^,^-----^,^---^,^-------^
     * 
     */

    public partial struct JNavSegment
    {
        // 0 - 1 - 2 - 3 - 4 - 5 - 6 - 7 - 8 - 9
        //         | ------- 6 ------  |
        private string json;
        private int start;
        private int length;
        private JNavNode node;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        public JNavSegment(string json)
        {
            this.json = json;
            this.start = 0;
            this.length = json.Length;
            this.node = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private JNavSegment(string json, int start, int end)
        {
            this.json = json;
            this.start = start;
            this.length = end - start + 1;
            this.node = null;
        }

        public int Start => start;

        public int Length => length;

        public char this[int index] => json[start + index];

        public JNavSegment Segment(int start, int end)
        {
            return new JNavSegment(this.json, this.start + start, this.start + end);
        }

        private JNavNode Parse()
        {
            JNavNode parsedNode;

            var firstCharPos = 0;
            while (char.IsWhiteSpace(this[firstCharPos]) && firstCharPos < length)
            {
                firstCharPos++;
            }

            var lastCharPos = this.Length - 1;
            while (char.IsWhiteSpace(this[lastCharPos]) && lastCharPos >= 0)
            {
                lastCharPos--;
            }

            bool contentEquals(JNavSegment seg, string with)
                => string.Compare(seg.json, seg.start + firstCharPos, with, 0, lastCharPos - firstCharPos + 1) == 0;

            switch (this[firstCharPos])
            {
                case '{':
                    if (this[lastCharPos] != '}')
                    {
                        throw new Exception(); // <-- not closed properly
                    }

                    parsedNode = new JNavObject(this);
                    break;
                case '[':
                    if (this[lastCharPos] != ']')
                    {
                        throw new Exception(); // <-- not closed properly
                    }

                    parsedNode = new JNavArray(this);
                    break;
                case '"':
                    if (this[lastCharPos] != '"')
                    {
                        throw new Exception(); // <-- not closed properly
                    }

                    parsedNode = new JNavString(this.Segment(firstCharPos + 1, lastCharPos - 1));
                    break;
                case var _ when contentEquals(this, "null"):
                    parsedNode = JNavNull.Value;
                    break;
                case var _ when contentEquals(this, "true"):
                    parsedNode = new  JNavBool(this, true);
                    break;
                case var _ when contentEquals(this, "false"):
                    parsedNode = new JNavBool(this, false);
                    break;
                default:
                    // basic number validation
                    parsedNode = new JNavNumber(this.Segment(firstCharPos, lastCharPos));

                    throw new Exception(); // <- Invalid Token
            }

            return parsedNode;
        }

        public JNavNode Node => this.node ?? (this.node = Parse());
       
        private T NodeOfType<T>()
            where T : JNavNode
        {
            if (this.Node is T typedNode)
            {
                return typedNode;
            }
            throw new Exception();
        }

        public JNavObject ObjectNode => NodeOfType<JNavObject>();

        public JNavArray ArrayNode => NodeOfType<JNavArray>();

        public JNavString StringNode => NodeOfType<JNavString>();

        public JNavBool BoolNode => NodeOfType<JNavBool>();

        public JNavNumber NumberNode => NodeOfType<JNavNumber>();

        internal string GetString(char[] lookup, Func<int, (string replaceWith, int skip)> filter)
        {
            var builder = new StringBuilder(length);
            
            // 0 1 2 3 4 5 6 7 8 9 0 <-- json index
            //   " T h a t \ ' s "    
            //     ^       |   ^
            //     0 1 2 3 4 5 6     <-- relative index
            
            int pos = start;
            int count = length;
            
            int i;
            while ((i = json.IndexOfAny(lookup, pos, count)) != -1)
            {
                builder.Append(json, pos, i - pos);

                var (replaceWith, skip) = filter(i - start); // Relative Index
                builder.Append(replaceWith);

                pos = i + 1 + skip;
                count = start + length - pos;
            }

            builder.Append(json, pos, count);
            return builder.ToString();
        }

        internal IEnumerable<JNavSegment> SplitItems(params char[] separators)
        {
            var itemStart = 0;
            var sepIndex = 0;

            int depth = 0;
            for (int i = itemStart; i < this.Length && depth >= 0; i++)
            {
                switch (this[i])
                {
                    case '"':
                        bool foundEnd = false;
                        // skip to end of string
                        for (int j = i + 1; j < this.Length; j++)
                        {
                            if (this[j] == '\\')
                            {
                                j++;
                            }
                            else if (this[j] == '"')
                            {
                                i = j;
                                foundEnd = true;
                                break;
                            }
                        }

                        if (!foundEnd)
                        {
                            throw new Exception(); // <--- string not closed

                        }

                        break;
                    case '[':
                    case '{':
                        depth++;
                        if (depth == 1)
                        {
                            itemStart = i + 1;
                        }
                        break;
                    case ']':
                    case '}':
                        depth--;

                        if (depth == 0)
                        {
                            yield return this.Segment(itemStart, i - 1);
                        }

                        break;
                    case var ch when depth == 1 && ch == separators[sepIndex]:
                        yield return this.Segment(itemStart, i - 1);

                        itemStart = i + 1;
                        if (++sepIndex >= separators.Length)
                        {
                            sepIndex = 0;
                        }

                        break;
                }
            }

            if (depth > 0)
            {
                throw new Exception(); // <--- collection not closed
            }
        }  
    }
}
