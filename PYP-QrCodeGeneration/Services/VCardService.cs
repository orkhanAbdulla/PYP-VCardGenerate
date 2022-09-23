using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PYP_QrCodeGeneration.DAL;
using PYP_QrCodeGeneration.Models;
using System.Net;
using System.Web;


namespace PYP_QrCodeGeneration.Services
{
    public class VCardService : IVCardService
    {
        private readonly AppDbContext _context;

        public VCardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddVCardAsync()
        {
           
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://randomuser.me/api?results=50");
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                Stream responseStream = response.GetResponseStream();
                StreamReader readerStream = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                string json = readerStream.ReadToEnd();
                readerStream.Close();
                var result = JsonConvert.DeserializeObject<dynamic>(json);
                foreach (var item in result.results)
                {
                    VCard vCard = new VCard()
                    {
                        Firstname = item.name.first.ToString(),
                        Surname = item.name.last.ToString(),
                        Email = item.email.ToString(),
                        Phone = item.phone.ToString(),
                        Country = item.location.country.ToString(),
                        City = item.location.city.ToString(),
                    };
                    vCard.QrCode = GenerateQr(CreateVCard(vCard));
                    await _context.AddAsync(vCard);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception){}
        }

        public string CreateVCard(VCard vCard)
        {
            return "BEGIN:VCARD\r\n" +
                   "VERSION:3.0\r\n" +
                   $"N:{vCard.Firstname};{vCard.Surname};\r\n" +
                   $"FN:{vCard.Surname + vCard.Firstname};\r\n" +
                   $"EMAIL:{vCard.Email};\r\n" +
                   $"TEL:{vCard.Phone};\r\n" +
                   $"END:VCARD";
        }

        public string GenerateQr(string vCard)
        {
            string data = HttpUtility.UrlEncode(vCard);
            return $"https://chart.googleapis.com//chart?cht=qr&chs=200x200&chl={data}";
        }

        public async Task<IEnumerable<VCard>> GetAll()
        {
            return await _context.VCards.OrderByDescending(x => x.Id).Take(10).ToListAsync();
        }
    }
}
