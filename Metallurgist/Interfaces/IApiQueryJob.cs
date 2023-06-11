namespace Metallurgist.Interfaces
{
    public interface IApiQueryJob
    {
        string Url { get; }
        int UpdateFrequency { get; set; }
        public Task ExecuteAsync();
    }
}
