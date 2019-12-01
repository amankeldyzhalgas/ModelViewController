using System;
using System.Collections.Generic;
using System.Text;

namespace ModelViewController.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public string Photo { get; set; }
    }
}
