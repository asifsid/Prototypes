using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace JNav
{
    /// <summary>
    /// 
    /// </summary>
    partial class NavRoot : ObjectWrapper
    {
        public static NavRoot Parse(string json)
        {
            return Wrap<NavRoot>(new JNavSegment(json));
        }

        public ObjectCollection<NavModel> Models
        {
            get
            {
                return Wrap<ObjectCollection<NavModel>>(this["models"]);
            }
        }

        public string Serialize()
        {
            return this.Segment.ToString();
        }
    }
    
    partial class NavModel : ObjectWrapper
    {
        public string Name
        {
            get => this["name"].StringNode.Value;
        }

        public ObjectCollection<NavChild> Children
        {
            get => Wrap<ObjectCollection<NavChild>>(this["children"]); 
        }
    }

    partial class NavChild : ObjectWrapper
    {
        public int Id => (int)this["id"].NumberNode.Value;

        public string Path
        {
            get => this["path"].StringNode.Value;
            set => this["path"].StringNode.Value = value;
        }

        public bool Active
        {
            get => this["active"].BoolNode.Value;
            set => this["active"].BoolNode.Value = value;
        }
    }

}
