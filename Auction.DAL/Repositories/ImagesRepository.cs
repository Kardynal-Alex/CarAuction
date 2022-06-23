using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Auction.DAL.Repositories
{
    public class ImagesRepository : IImagesRepository
    {
        private readonly ApplicationContext context;
        public ImagesRepository(ApplicationContext ctx)
        {
            context = ctx;
        }
       
        public async Task AddImagesAsync(Images addImages)
        {
            await context.Images.AddAsync(addImages);
        }
     
        public void DeleteImagesById(int id)
        {
            context.Images.Remove(new Images { Id = id });
        }
      
        public void UpdateImages(Images updateImages)
        {
            context.Images.Update(updateImages);
        }
      
        public void DeleteImagesPhysically(Images images)
        {
            Type t = typeof(Images);
            PropertyInfo[] props = t.GetProperties();
            for (int i = 1; i < props.Length; i++)
            {
                var path = props[i].GetValue(images).ToString();
                DeleteImagePhysicallyByPath(path);
            }
        }
        
        public void DeleteImagePhysicallyByPath(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}
