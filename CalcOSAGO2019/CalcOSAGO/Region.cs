using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalcOSAGO
{
    public class Region
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Текстовое представление записи
        /// </summary>
        public string text_value { get; set; }
        /// <summary>
        /// индекс категории территории
        /// </summary>
        public int id_categori_teritory { get; set; }
    }
}
