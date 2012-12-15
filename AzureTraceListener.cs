using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;

namespace AzureTraceListener
{
    public class AzureTraceListener : TraceListener
    {
        private string _fileName;

        public AzureTraceListener(string fileName)
        {
            FileInfo info = new FileInfo(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("/"), fileName));

            _fileName = info.FullName;

            if (!info.Directory.Exists)
            {
                info.Directory.Create();
            }
        }

        public override void Write(string message)
        {
            using (FileStream stream = new FileStream(_fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);

                stream.Write(bytes, 0, bytes.Length);

                stream.Flush();
            }
        }

        public override void WriteLine(string message)
        {
            using (FileStream stream = new FileStream(_fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message + "\r\n");

                stream.Write(bytes, 0, bytes.Length);

                stream.Flush();
            }
        }

        //public AzureTraceListener(string fileName)
        //{
        //    FileInfo info = new FileInfo(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("/"), fileName));
        //    _fileName = info.FullName;
        //    if (!info.Directory.Exists)
        //    {
        //        info.Directory.Create();
        //    }

        //    _stream = new FileStream(_fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
        //}

        //public override void Write(string message)
        //{
        //    byte[] bytes = Encoding.UTF8.GetBytes(message);
        //    _stream.Write(bytes, 0, bytes.Length);
        //    _stream.Flush();
        //    FlushFileBuffers(_stream.SafeFileHandle.DangerousGetHandle());
        //}

        //public override void WriteLine(string message)
        //{
        //    byte[] bytes = Encoding.UTF8.GetBytes(message + "\r\n");
        //    _stream.Write(bytes, 0, bytes.Length);
        //    _stream.Flush();
        //    FlushFileBuffers(_stream.SafeFileHandle.DangerousGetHandle());
        //}

        //[DllImport("kernel32", SetLastError = true)]
        //private static extern bool FlushFileBuffers(IntPtr handle);
    }
}
