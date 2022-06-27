using PDFGenerator.Models;

namespace PDFGenerator.Utils
{
    public static class CarBrandUtils
    {
        public static string GetCarBrandName(CarBrand carBrand) =>
            carBrand switch
            {
                CarBrand.Tesla => "Tesla",
                CarBrand.BMW => "BMV",
                CarBrand.Ferrari => "Ferrari",
                CarBrand.Ford => "Ford",
                CarBrand.Porsche => "Porshe",
                CarBrand.Honda => "Honda",
                CarBrand.Toyota => "Toyota",
                CarBrand.Audi => "Audi",
                CarBrand.Jeep => "Jeep",
                CarBrand.Lexus => "Lexus",
                CarBrand.Chevrolet => "Chevrolet",
                CarBrand.Mercedes => "Mercedes",
                CarBrand.Volkswagen => "Volkswagen",
                CarBrand.Peugeot => "Peugeot",
                CarBrand.Kia => "KIA"
            };
    }
}
