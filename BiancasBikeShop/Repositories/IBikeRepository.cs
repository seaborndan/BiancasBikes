using BiancasBikeShop.Models;
using System.Collections.Generic;

namespace BiancasBikeShop.Repositories
{
    public interface IBikeRepository
    {
        List<Bike> GetAllBikes();
        Bike GetBikeById(int id);
        int GetBikesInShopCount();
    }
}