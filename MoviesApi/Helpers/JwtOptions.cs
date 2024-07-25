namespace MoviesApi.Helpers
{
    public class JwtOptions 
    {
        public string KeySecrect { get; set; }
        public string Key { get; set; }

        public string ValideIssure { get; set; }
        public string ValideAudience { get; set; }           
        public double DurationInHours { get; set; }           
    }
}
