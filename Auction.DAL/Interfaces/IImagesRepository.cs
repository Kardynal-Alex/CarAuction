using Auction.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    /// <summary>
    /// IImagesRepository
    /// </summary>
    public interface IImagesRepository
    {
        /// <summary>
        /// Add images
        /// </summary>
        /// <param name="addImages"></param>
        /// <returns></returns>
        Task AddImagesAsync(Images addImages);
        /// <summary>
        /// Delete images
        /// </summary>
        /// <param name="id"></param>
        void DeleteImagesById(int id);
        /// <summary>
        /// Update images
        /// </summary>
        /// <param name="updateImages"></param>
        void UpdateImages(Images updateImages);
    }
}
