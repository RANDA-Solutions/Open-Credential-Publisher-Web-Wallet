using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OpenCredentialPublisher.Services.Drawing
{
    class VerdanaFontResolver : IFontResolver
    {
        private const string ResourceLocation = "OpenCredentialPublisher.Services.Resources.Fonts.Verdana";
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Ignore case of font names.
            var name = familyName.ToLower().TrimEnd('#');

            // Deal with the fonts we know.
            switch (name)
            {
                case "verdana":
                    if (isBold)
                    {
                        if (isItalic)
                            return new FontResolverInfo("Verdana#z");
                        return new FontResolverInfo("Verdana#b");
                    }
                    if (isItalic)
                        return new FontResolverInfo("Verdana#i");
                    return new FontResolverInfo("Verdana#");
            }

            // We pass all other font requests to the default handler.
            // When running on a web server without sufficient permission, you can return a default font at this stage.
            return PlatformFontResolver.ResolveTypeface(familyName, isBold, isItalic);
        }

        public byte[] GetFont(string faceName)
        {
            switch (faceName)
            {
                case "Verdana#":
                    return LoadFontData($"{ResourceLocation}.verdana.ttf");

                case "Verdana#b":
                    return LoadFontData($"{ResourceLocation}.verdanab.ttf");

                case "Verdana#i":
                    return LoadFontData($"{ResourceLocation}.verdanai.ttf");

                case "Verdana#bi":
                    return LoadFontData($"{ResourceLocation}.verdanaz.ttf");
            }

            return null;
        }

        /// <summary>
        /// Returns the specified font from an embedded resource.
        /// </summary>
        private byte[] LoadFontData(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Test code to find the names of embedded fonts - put a watch on "ourResources"
            //var ourResources = assembly.GetManifestResourceNames();

            using (var stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                    throw new ArgumentException("No resource with name " + name);

                int count = (int)stream.Length;
                byte[] data = new byte[count];
                stream.Read(data, 0, count);
                return data;
            }
        }

        internal static VerdanaFontResolver OurGlobalFontResolver = null;

        /// <summary>
        /// Ensure the font resolver is only applied once (or an exception is thrown)
        /// </summary>
        internal static void Apply()
        {
            if (OurGlobalFontResolver == null || GlobalFontSettings.FontResolver == null)
            {
                if (OurGlobalFontResolver == null)
                    OurGlobalFontResolver = new VerdanaFontResolver();

                GlobalFontSettings.FontResolver = OurGlobalFontResolver;
            }
        }
    }
}
