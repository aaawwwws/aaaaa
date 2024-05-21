using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler
{
    public class Path
    {
        private readonly string _path;
        public string path { get => _path; }
        public Path(string path)
        {
            if (path == string.Empty) throw new ArgumentNullException(path);
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException(path);
            var correction = path.EndsWith('\\') ? path : path += '\\';
            this._path = correction;
        }
    }
}
