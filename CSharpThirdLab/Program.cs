using System;
using System.IO;

namespace CSharpThirdLab
{
    class Program
    {
        private const int widthStartIndexPNG = 16;
        private const int heightStartIndexPNG = 20;
        private const int widthStartIndexBMP = 17;
        private const int heightStartIndexBMP = 21;
        static void Main(string[] args)
        {
            byte[] data = new byte[0];
              
            try
            { 
                var fileStream = new FileStream(args[0], FileMode.Open);
                var fileSize = (int)fileStream.Length;
                data = new byte[fileSize];
                fileStream.Read(data, 0, fileSize);
    
                fileStream.Close();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("“File not found.”");
                return;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Empty parameters");
                return;
            }

            if (IsPortableNetworkGraphicsFile(data))
            {
                var widthBytes = new byte[4];
                Array.Copy(data, widthStartIndexPNG, widthBytes, 0, 4);
                Array.Reverse(widthBytes);
                var heightBytes = new byte[4];
                Array.Copy(data, heightStartIndexPNG, heightBytes, 0, 4);
                Array.Reverse(heightBytes);
                int width = BitConverter.ToInt32(widthBytes, 0);
                int height = BitConverter.ToInt32(heightBytes, 0);

                Console.WriteLine($"”This is a .png image. Resolution: {width}x{height} pixels”");
            }
            else if (IsBitmapFile(data))
            {
                var widthBytes = new byte[4];
                Array.Copy(data, widthStartIndexBMP, widthBytes, 0, 4);
                Array.Reverse(widthBytes);
                var heightBytes = new byte[4];
                Array.Copy(data, heightStartIndexBMP, heightBytes, 0, 4);
                Array.Reverse(heightBytes);
                int width = BitConverter.ToInt32(widthBytes, 0);
                int height = BitConverter.ToInt32(heightBytes, 0);

                Console.WriteLine($"”This is a .bmp image. Resolution: {width}x{height} pixels”");
            }
            else
            {
                Console.WriteLine("”This is not a valid .bmp or.png file!”");
            }
        }

        private static bool IsPortableNetworkGraphicsFile(byte[] data)
        {
            byte[] pngFile = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

            for (int i = 0; i < 8; i++)
            {
                if (pngFile[i] != data[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsBitmapFile(byte[] data)
        {
            byte[] bmpFile = { 0x42, 0x4D };

            for (int i = 0; i < 2; i++)
            {
                if (bmpFile[i] != data[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}