using Auction.DAL.Entities;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    public interface IImagesRepository
    {
        Task AddImagesAsync(Images addImages);
        void DeleteImagesById(int id);
        void UpdateImages(Images updateImages);
        void DeleteImagesPhysically(Images images);
        void DeleteImagePhysicallyByPath(string path);
    }
}
