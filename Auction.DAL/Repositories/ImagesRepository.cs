using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Auction.DAL.Repositories
{
    /// <summary>
    /// ImageRepository
    /// </summary>
    public class ImagesRepository : IImagesRepository
    {
        private readonly ApplicationContext context;
        /// <summary>
        /// Repository ctor
        /// </summary>
        public ImagesRepository(ApplicationContext ctx)
        {
            context = ctx;
        }
        /// <summary>
        /// Add images
        /// </summary>
        /// <param name="addImages"></param>
        /// <returns></returns>
        public async Task AddImagesAsync(Images addImages)
        {
            await context.Images.AddAsync(addImages);
        }
        /// <summary>
        /// Delete images by id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteImagesById(int id)
        {
            context.Images.Remove(new Images { Id = id });
        }
        /// <summary>
        /// Update Images
        /// </summary>
        /// <param name="updateImages"></param>
        public void UpdateImages(Images updateImages)
        {
            context.Images.Update(updateImages);
        }
        /// <summary>
        /// Delete lot images using reflection 
        /// </summary>
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
        /// <summary>
        /// Delete image physically by path in folder
        /// </summary>
        /// <param name="path"></param>
        public void DeleteImagePhysicallyByPath(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}
