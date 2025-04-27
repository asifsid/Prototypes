using System;
using System.Collections.Generic;
using System.Text;

namespace JNav
{
    public abstract class Wrapper
    {
        public static T Wrap<T>(JNavSegment segment)
            where T : Wrapper, new()
        {
            return new T { Segment = segment };
        }

        protected JNavSegment Segment { get; private set; }
    }

    public abstract class ObjectWrapper : Wrapper
    {
        protected JNavSegment this[string property] 
            => this.Segment.ObjectNode[property]; 
    }

    public abstract class CollectionWrapper : Wrapper
    {
        public int Count => this.Segment.ArrayNode.Count; 
    }

    public class ObjectCollection<T> : CollectionWrapper
        where T : ObjectWrapper, new()
    {
        public T this[int index] => Wrap<T>(this.Segment.ArrayNode[index]);
    }
}
