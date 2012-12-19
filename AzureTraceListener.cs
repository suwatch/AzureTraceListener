using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System;

namespace AzureTraceListener
{
    public class AzureTraceListener : TraceListener
    {
        private DirectoryInfo _directory;
        private string _fileName;
        private int _traceCount;

        public AzureTraceListener(string path)
        {
            _directory = new DirectoryInfo(Path.Combine(HostingEnvironment.MapPath("/"), path));

            if (!_directory.Exists)
            {
                _directory.Create();
            }

            CleanUpOldFiles();

            EnsureMaxFiles();
        }

        private void CleanUpOldFiles()
        {
            DateTime now = DateTime.UtcNow;
            foreach (FileInfo file in _directory.GetFiles())
            {
                if (now.Subtract(file.LastWriteTimeUtc) > TimeSpan.FromMinutes(10))
                {
                    SafeDelete(file);
                }
            }
        }

        private void EnsureMaxFiles()
        {
            FileInfo[] files = _directory.GetFiles(Environment.MachineName + "-*.txt")
                .OrderBy(f => f.LastWriteTimeUtc).ToArray();
            for (int i = 0; i < files.Length - 3; ++i)
            {
                SafeDelete(files[i]);
            }

            _fileName = Path.Combine(_directory.FullName, String.Format("{0}-{1}.txt", Environment.MachineName, DateTime.UtcNow.Ticks));
            _traceCount = 0;
        }

        private void SafeDelete(FileInfo file)
        {
            try
            {
                file.Delete();
            }
            catch (Exception)
            {
            }
        }

        public override void Write(string message)
        {
            if (++_traceCount > 100)
            {
                EnsureMaxFiles();
            }

            using (FileStream stream = new FileStream(_fileName, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);

                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public override void WriteLine(string message)
        {
            Write(message + Environment.NewLine);
        }
    }
}