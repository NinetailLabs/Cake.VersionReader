using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Testing;

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
            _log = new FakeLog();
            var environment = new CakeEnvironment (new CakePlatform(), new CakeRuntime(), _log);
            var globber = new Globber (fileSystem, environment);
            
            var args = new FakeCakeArguments ();
            var processRunner = new ProcessRunner (environment, _log);
            var registry = new WindowsRegistry ();

            var toolLocator = new ToolLocator(environment, new ToolRepository(environment), new ToolResolutionStrategy(fileSystem, environment, globber, new FakeConfiguration()));
            _context = new CakeContext (fileSystem, environment, globber, _log, args, processRunner, registry, toolLocator);
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

