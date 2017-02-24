using System.Collections.Generic;
using Cake.Core.Diagnostics;

namespace Cake.VersionReader.Tests.Fakes
{
    /// <summary>
    /// Implementation of a <see cref="ICakeLog"/> that saves all messages written to it.
    /// </summary>
    public sealed class FakeLog : ICakeLog
    {
        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <value>The messages.</value>
        public List<string> Messages { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeLog"/> class.
        /// </summary>
        public FakeLog()
        {
            Messages = new List<string>();
        }

        /// <summary>
        /// Gets the verbosity.
        /// </summary>
        /// <value>The verbosity.</value>
        public Verbosity Verbosity
        {
            get { return Verbosity.Diagnostic; }
            set { }
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects to the
        /// log using the specified verbosity, log level and format information.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The log level.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            Messages.Add(string.Format(format, args));
        }
    }
}