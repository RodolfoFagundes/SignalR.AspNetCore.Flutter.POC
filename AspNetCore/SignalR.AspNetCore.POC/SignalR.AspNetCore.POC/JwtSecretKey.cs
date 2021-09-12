namespace SignalR.AspNetCore.POC
{
    public static class JwtSecretKey
    {
        public static readonly byte[] SecretKey =
            System.Text.Encoding.UTF8.GetBytes("INFORM THE SECRET KEY");
    }
}
