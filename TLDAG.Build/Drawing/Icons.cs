using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using TLDAG.Core.Drawing;
using TLDAG.Core.IO;
using static TLDAG.Core.Primitives;
using static TLDAG.Core.IO.Streams;

namespace TLDAG.Build.Drawing
{
    public static class Icons
    {
        public static void Create(IEnumerable<Image> images, FileInfo output)
            { output.WriteAllBytes(Create(images)); }

        public static void Create(IEnumerable<FileInfo> images, FileInfo output)
            { Create(Images.Load(images), output); }

        public static byte[] Create(IEnumerable<FileInfo> images)
            => Create(Images.Load(images));

        public static byte[] Create(IEnumerable<Image> images)
            => Save(Convert(images));

        private static byte[] Save(List<Tuple<byte, byte[]>> images)
        {
            MemoryStream output = new();
            int offset = 6 + (16 * images.Count);

            output.Write(UShortToBytes(0)); // 0..1 reserved
            output.Write(UShortToBytes(1)); // 2..3 image type. 1 = icon
            output.Write(UShortToBytes((ushort)images.Count)); // 4..5 image count

            for (int i = 0; i < images.Count; ++i)
            {
                output.WriteByte(images[i].Item1); // width
                output.WriteByte(images[i].Item1); // height
                output.WriteByte(0); // color count
                output.WriteByte(0); // reserved

                output.Write(UShortToBytes(0)); // color planes
                output.Write(UShortToBytes(0)); // bits per pixel
                output.Write(IntToBytes(images[i].Item2.Length)); // size of image data
                output.Write(IntToBytes(offset)); // offset of image data
                offset += images[i].Item2.Length;
            }

            for (int i = 0; i < images.Count; ++i)
            {
                output.Write(images[i].Item2);
            }

            return output.ToArray();
        }

        private static List<Tuple<byte, byte[]>> Convert(IEnumerable<Image> images)
            => images.Where(IsConvertable).Select(Convert).OrderBy(t => t.Item1).ToList();

        private static Tuple<byte, byte[]> Convert(Image image)
        {
            MemoryStream stream = new();

            image.Save(stream, ImageFormat.Png);
            return Tuple.Create((byte)image.Width, stream.ToArray());
        }

        private static bool IsConvertable(Image image)
        {
            if (image.Width < 1 || image.Width > 255) return false;
            if (image.Height < 1 || image.Height > 255) return false;
            if (image.Width != image.Height) return false;

            return true;
        }
    }
}
