using System;

namespace ModelViewController.DAL.Entities
{
    public class Award
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
