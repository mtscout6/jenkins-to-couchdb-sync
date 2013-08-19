namespace sabatoast_puller.Sabatoast
{
    public class Job
    {
        private readonly string _name;
        private readonly string _url;

        public Job(string name, string url)
        {
            _name = name;
            _url = url;
        }
    }
}