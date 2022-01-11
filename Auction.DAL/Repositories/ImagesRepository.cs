using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using System;
using System.Collections.Generic;
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
    }
}
