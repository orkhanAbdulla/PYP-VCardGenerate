using PYP_QrCodeGeneration.Models;

namespace PYP_QrCodeGeneration.Services
{
    public interface IVCardService
    {
        string CreateVCard(VCard vCard);
        string GenerateQr(string vCard);
        Task AddVCardAsync();
        Task<IEnumerable<VCard>> GetAll();
    }
}
