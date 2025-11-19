using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace GeekShoping.Web.Models
{
    public class ProductViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string ImageURL { get; set; }

        public string Description {  get; set; }

        [Range(1, 100)]
        public int Count { get; set; } = 1;
        

        public string SubstringName()
        {
            if (Name.Length < 22) return Name;
            return $"{Name.Substring(0, 19)} ...";
        }

        public string SubstringDescription()
        {
            if (Description.Length < 352) return Description;
            return $"{Description.Substring(0, 349)} ...";
        }
        public string PriceByCountry(string country = "pt-BR")
        {
            var culture = new System.Globalization.CultureInfo(country);
            return string.Format(culture, "{0:C2}", Price);
        }
    }
}
