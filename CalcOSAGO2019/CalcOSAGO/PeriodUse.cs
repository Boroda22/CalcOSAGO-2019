using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalcOSAGO
{
    /// <summary>
    /// Класс представляет период использования ТС
    /// </summary>
    public class PeriodUse
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Текстовое представление записи
        /// </summary>
        public string text_value { get; set; }
    }
}
