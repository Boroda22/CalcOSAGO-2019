using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalcOSAGO
{
    /// <summary>
    /// Класс для хранения коэффициента бонус-малус
    /// </summary>
    public class KbmCl
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Текстовое предствление
        /// </summary>
        public string text_value { get; set; }
		/// <summary>
		/// Значение коэффициента КБМ
		/// </summary>
		public double koeff { get; set; } = 0;
	}
}
