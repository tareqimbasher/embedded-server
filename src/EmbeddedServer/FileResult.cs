using EmbeddedServer.Hosting;
using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace EmbeddedServer
{
    public class FileResult : IActionResult
    {
        public FileResult(string filePath)
        {
            FilePath = filePath;
            ReadFromDisk = true;
        }

        public FileResult(string fileName, byte[] fileData)
        {
            FileName = fileName;
            FileData = fileData;
        }

        public FileResult(string fileName, byte[] fileData, string contentType) : this(fileName, fileData)
        {
            FileContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
        }

        public bool ReadFromDisk { get; set; }
        public string FileName { get; }
        public byte[] FileData { get; }
        public string FilePath { get; }
        public string FileContentType { get; set; } = MediaTypeNames.Application.Octet;

        public async Task WriteToResponseAsync(HttpListenerResponse response, IWebHostConfiguration configuration)
        {
            if (ReadFromDisk)
            {
                using (FileStream fs = File.OpenRead(FilePath))
                {
                    var fileName = Path.GetFileName(FilePath);

                    response.ContentLength64 = fs.Length;
                    response.SendChunked = false;
                    response.ContentType = FileContentType;
                    response.AddHeader("Content-disposition", $"attachment; filename={fileName}");

                    byte[] buffer = new byte[64 * 1024];
                    int read;
                    using (BinaryWriter bw = new BinaryWriter(response.OutputStream))
                    {
                        while ((read = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            bw.Write(buffer, 0, read);
                            bw.Flush(); //seems to have no effect
                        }

                        bw.Close();
                    }

                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.StatusDescription = "OK";
                    response.OutputStream.Close();
                }
            }
            else
            {
                var fileName = FileName;

                response.ContentLength64 = FileData.Length;
                response.SendChunked = false;
                response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet;
                response.AddHeader("Content-disposition", $"attachment; filename={fileName}");

                using (BinaryWriter bw = new BinaryWriter(response.OutputStream))
                {
                    bw.Write(FileData, 0, FileData.Length);
                    bw.Flush(); //seems to have no effect

                    bw.Close();
                }

                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "OK";
                response.OutputStream.Close();
            }
        }
    }
}
