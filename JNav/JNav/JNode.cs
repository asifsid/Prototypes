using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JNav
{

    public abstract class JNavNode
    {
        
    }

    public abstract class JNavValueNode : JNavNode
    {
        protected JNavSegment segment;

        protected JNavValueNode(JNavSegment segment)
        {
            this.segment = segment;
        }
    }
    
    public class JNavObject : JNavValueNode
    {
        private Dictionary<string, JNavSegment> properties;

        public JNavObject(JNavSegment segment) : base(segment) { }

        private void EnsureParsed()
        {
            if (properties == null)
            {
                properties = new Dictionary<string, JNavSegment>();

                var propEnumerator = segment.SplitItems(':', ',').GetEnumerator();
                while (propEnumerator.MoveNext())
                {
                    if (propEnumerator.Current.Node is JNavString nameNode)
                    {
                        if (!propEnumerator.MoveNext())
                        {
                            throw new Exception();
                        }

                        properties[nameNode.Value] = propEnumerator.Current;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public JNavSegment this[string name]
        {
            get
            {
                EnsureParsed();   

                return properties[name];
            }
        }
    }

    public class JNavArray : JNavValueNode, IEnumerable<JNavSegment>
    {
        private IList<JNavSegment> items;

        public JNavArray(JNavSegment segment) : base(segment)
        {
        }

        private void EnsureParsed()
        {
            if (items == null)
            {
                items = segment.SplitItems(',').ToList();
            }
        }

        public int Count
        {
            get
            {
                EnsureParsed();
                return items.Count;
            }
        }

        public JNavSegment this[int index]
        {
            get
            {
                EnsureParsed();
                return items[index];
            }
        }

        public IEnumerator<JNavSegment> GetEnumerator()
        {
            EnsureParsed();
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            EnsureParsed();
            return items.GetEnumerator();
        }
    }

    public abstract class JNavValue<T> : JNavValueNode
    {
        private T value;
        private bool assigned;

        public T Value
        {
            get
            {
                if (!assigned)
                {
                    value = Parse();
                    assigned = true;
                }
                return value;
            }
            set
            {
                this.value = value;
                assigned = true;
            }
        }
        
        public JNavValue(JNavSegment segment) : base(segment)
        {
        }

        protected abstract T Parse();
    }

    public class JNavString : JNavValue<string>
    {
        public JNavString(JNavSegment segment) : base(segment) { }

        protected override string Parse()
        {
            return segment.GetString(new char[] { '\\' }, 
                index => {
                            if (index >= this.segment.Length - 1)
                            {
                                throw new Exception(); // Invalid Escape Character
                            }

                            switch (this.segment[index + 1])
                            {
                                case '\b': return ("\b", 1);
                                case '\f': return ("\f", 1);
                                case '\n': return ("\n", 1);
                                case '\r': return ("\r", 1);
                                case '\t': return ("\t", 1);
                                case '\"': 
                                case '\'': 
                                case '\\':
                                    return (this.segment[index + 1].ToString(), 1);
                                default:
                                    throw new Exception(); //Invalid Escape Sequence
                            }
                        });
        }
    }

    public class JNavNumber : JNavValue<decimal>
    {
        public JNavNumber(JNavSegment segment) : base(segment) { }

        protected override decimal Parse()
        {
            throw new NotImplementedException();
        }
    }

    public class JNavBool : JNavValue<bool>
    {
        public JNavBool(JNavSegment segment) : base(segment) { }

        public JNavBool(JNavSegment segment, bool value) : base(segment)
        {
            this.Value = value;
        }

        protected override bool Parse()
        {
            throw new NotImplementedException();
        }
    }

    public class JNavNull : JNavNode
    {
        public static readonly JNavNull Value = new JNavNull();

        private JNavNull() { }
    }

}
