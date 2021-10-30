using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace TLDAG.Core.Drawing
{
    public static class Images
    {
        public static IEnumerable<Image> Load(IEnumerable<FileInfo> files)
        {
            List<Image> images = new();

            foreach (FileInfo file in files)
            {
                if (TryLoad(file, out Image image))
                    images.Add(image);
            }

            return images;
        }

        public static bool TryLoad(FileInfo file, out Image image)
        {
            try
            {
                image = Image.FromFile(file.FullName);
            }
            catch (Exception)
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                image = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            }

            return image is not null;
        }
    }
}
