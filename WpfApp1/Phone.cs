using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataApp {
	public class Phone {
		public int Id { get; set; }
		public string Title { get; set; } // модель телефона
		public string Company { get; set; } // производитель
		public string ImagePath { get; set; } // путь к изображению
	}
}
