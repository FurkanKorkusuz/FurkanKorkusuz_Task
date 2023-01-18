using System;
using Core.Entities.Concrete;
using System.Collections.Generic;

namespace Core.Entities.DTOs
{
    public class NewAddDto
    {

        public int ID { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string UrlLink { get; set; }
        public bool IsActive { get; set; }
    }
}

        