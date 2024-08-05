using System.Text;

namespace PhinaMart.Helpers
{
    public class MyUtil
    {
        public static string UploadImage(IFormFile Image, string folder)
        {
            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", folder, Image.FileName);
                using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    Image.CopyTo(myfile);
                }
                return Image.FileName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string GenerateRanDomKey(int length = 5)
        {
            var parttern = @"asdsadsaxccvcxvsdhfgASDSAVCXVBASDASD!";
            var sb = new StringBuilder();
            var rd = new Random();
            for (int i = 0; i < length; i++)
            {
                sb.Append(parttern[rd.Next(0, parttern.Length)]);
            }
            return sb.ToString();
        }
    }
}
