using System.Collections.Generic;

namespace Api.Models
{
    public class Product
    {
        private List<Review> _reviews;
        public string Sku { get; set; }
        public string Name { get; set; }

        public List<Review> Reviews
        {
            get { return _reviews; }
            set { _reviews = value; }
        }

        public Product(string sku, string name)
        {
            Sku = sku;
            Name = name;
            _reviews = new List<Review>();
        }
    }

    public class Review
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public Review(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}
