namespace Letters.Rest.Extensions
{
    public static class FormFileExtensions
    {
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            if (formFile == null)
                return null;

            using var memoryStream = new MemoryStream();

            await formFile.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }
    }
}
