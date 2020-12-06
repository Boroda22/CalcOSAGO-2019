using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalcOSAGO
{
    /// <summary>
    /// Класс предназначен для работы с водителем
    /// </summary>
    public class Driver
    {
        /// <summary>
        /// ФИО водителя
        /// </summary>
        public string NameDriver { get; set; } = "";
        /// <summary>
        /// Возраст водителя
        /// </summary>
        public DateTime AgeDriver { get; set; } = new DateTime(1930,1,1);
        /// <summary>
        /// Водительский стаж
        /// </summary>
        public DateTime StageDriver { get; set; } = new DateTime(1948, 1, 1);
        /// <summary>
        /// Коэффициент возраста стажа
        /// </summary>
        public double Koeff { get; set; }
		/// <summary>
		/// Коэффициент бонсу-малус
		/// </summary>
		public KbmCl Koeff_kbm { get; set; } = new KbmCl();
    }
}
