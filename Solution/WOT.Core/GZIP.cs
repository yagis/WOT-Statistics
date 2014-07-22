using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace WOTStatistics.Core
{
    class GZIP
    {
        public static byte[] Compress(byte[] file)
        {
            using (Stream fs = new MemoryStream(file))
            using (MemoryStream fd = new MemoryStream())
            using (Stream csStream = new GZipStream(fd, CompressionMode.Compress))
            {
                byte[] buffer = new byte[1024];
                int nRead;
                while ((nRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    csStream.Write(buffer, 0, nRead);
                }

                return fd.ToArray();
            }
        }


        public static byte[] Decompress(byte[] file)
        {
            using (Stream fd = new MemoryStream(file))
            using (MemoryStream fs = new MemoryStream())
            using (Stream csStream = new GZipStream(fd, CompressionMode.Decompress))
            {
                byte[] buffer = new byte[1024];
                int nRead;
                while ((nRead = csStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, nRead);
                }

                return fs.ToArray();
            }
        }
    }
}
