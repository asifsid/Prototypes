using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallaxEditorExtension
{
    internal static class ContentTypes
    {
        [Export]
        [Name("parallax")]
        [BaseDefinition("plaintext")]
        internal static ContentTypeDefinition ParallaxConfigContentTypeDefinition;

        [Export]
        [FileExtension(".ini")]
        [ContentType("parallax")]
        internal static FileExtensionToContentTypeDefinition ParallaxConfigExtensionDefinition;
    }
}
