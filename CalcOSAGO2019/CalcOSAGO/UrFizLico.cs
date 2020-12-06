using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalcOSAGO
{
    public class UrFizLico
    {
        /// <summary>
        /// идентификатор перечисления
        /// </summary>
        public int id_cl { get; set; } = 0;
        /// <summary>
        /// значение перечисления
        /// </summary>
        public string value_cl { get; set; } = "физ. лицо";

        /// <summary>
        /// переопределенная функция возврата в строковом представлении
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return value_cl;
        }
    }
}
