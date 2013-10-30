using System;

namespace sabatoast_puller.Jenkins
{
    public class JenkinsFailedException : Exception
    {
        public JenkinsFailedException(string message) : base(message) { }
    }
}