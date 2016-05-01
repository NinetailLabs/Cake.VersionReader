using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.VersionReader.Tests.Fakes
{
    public class FakeCakeContext
    {
        private readonly ICakeContext _context;
        private readonly FakeLog _log;
        private readonly DirectoryPath _testsDir;

        public FakeCakeContext ()
        {
            _testsDir = new DirectoryPath (
                System.IO.Path.GetFullPath (AppDomain.CurrentDomain.BaseDirectory));
            
            var fileSystem = new FileSystem ();
            var environment = new CakeEnvironment ();
            var globber = new Globber (fileSystem, environment);
            _log = new FakeLog ();
            var args = new FakeCakeArguments ();
            var processRunner = new ProcessRunner (environment, _log);
            var registry = new WindowsRegistry ();

            _context = new CakeContext (fileSystem, environment, globber, _log, args, processRunner, registry);
            _context.Environment.WorkingDirectory = _testsDir;
        }

        public DirectoryPath WorkingDirectory => _testsDir;

        public ICakeContext CakeContext => _context;

        public string GetLogs ()
        {
            return string.Join(Environment.NewLine, _log.Messages);
        }

        public void DumpLogs ()
        {
            foreach (var m in _log.Messages)
                Console.WriteLine (m);
        }
    }
}

