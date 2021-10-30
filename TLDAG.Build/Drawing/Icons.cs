using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using static TLDAG.Core.Primitives;

namespace TLDAG.Build.Drawing
{
    public static class Icons
    {
        public static byte[] Create(IEnumerable<Bitmap> bitmaps)
        {
            List<Tuple<byte, byte[]>> images = Convert(bitmaps);

            return Save(images);
        }

        private static byte[] Save(List<Tuple<byte, byte[]>> images)
        {
            MemoryStream output = new();
            byte[] bytes;

            bytes = UShortToBytes(0); // 0..1 reserved
            output.Write(bytes, 0, bytes.Length);

            bytes = UShortToBytes(1); // 2..3 image type. 1 = icon
            output.Write(bytes, 0, bytes.Length);

            bytes = UShortToBytes((ushort)images.Count);
            output.Write(bytes, 0, bytes.Length);

            int offset = 6 + (16 * images.Count);

            for (int i = 0; i < images.Count; ++i)
            {
                output.WriteByte(images[i].Item1); // width
                output.WriteByte(images[i].Item1); // height
                output.WriteByte(0); // color count
                output.WriteByte(0); // reserved

                bytes = UShortToBytes(0);
                output.Write(bytes, 0, bytes.Length); // color planes
                bytes = UShortToBytes(0);
                output.Write(bytes, 0, bytes.Length); // bits per pixel
                bytes = IntToBytes(images[i].Item2.Length);
                output.Write(bytes, 0, bytes.Length); // size of image data
                bytes = IntToBytes(offset);
                output.Write(bytes, 0, bytes.Length); // offset of image data
                offset += images[i].Item2.Length;
            }

            for (int i = 0; i < images.Count; ++i)
            {
                bytes = images[i].Item2;
                output.Write(bytes, 0, bytes.Length);
            }

            return output.ToArray();
        }

        private static List<Tuple<byte, byte[]>> Convert(IEnumerable<Bitmap> bitmaps)
        {
            return bitmaps
                .Where(IsConvertable)
                .Select(Convert)
                .OrderBy(t => t.Item1)
                .ToList();
        }

        private static Tuple<byte, byte[]> Convert(Bitmap bitmap)
        {
            MemoryStream stream = new();

            bitmap.Save(stream, ImageFormat.Png);
            return Tuple.Create((byte)bitmap.Width, stream.ToArray());
        }

        private static bool IsConvertable(Bitmap bitmap)
        {
            if (bitmap.Width < 1 || bitmap.Width > 255) return false;
            if (bitmap.Height < 1 || bitmap.Height > 255) return false;
            if (bitmap.Width != bitmap.Height) return false;

            return true;
        }
    }
}
