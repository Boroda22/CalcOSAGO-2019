using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace CalcOSAGO
{
    /// <summary>
    /// Класс основной модели данных
    /// Содержит константы-перечисления
    /// </summary>
    public class MainModel
    {
        // ПЕРЕМЕННЫЕ КЛАССА
        /// <summary>
        /// Объект для работы с базой данных
        /// </summary>
        private WorkWithDB data_base;
        /// <summary>
        /// объект для работы с печатью
        /// </summary>
        private PrintData print_data;
		/// <summary>
		/// Объект для работы с экспортом
		/// </summary>
		private ExportDataToImage export_data;
        /// <summary>
        /// словарь содержит формулу расчета, все коэффициенты перемножаются
        /// </summary>
        private static Dictionary<string, double> formula_result = new Dictionary<string, double>();
        /// <summary>
        /// Фамилия Имя Отчество страхователя
        /// </summary>
        public string fio_strah { get; set; } = "";
        /// <summary>
        /// Базовый тариф, по умолчанию для самарской обл. = 4118
        /// </summary>
        public decimal tarif_bazovi { get; set; } = 0;
        /// <summary>
        /// юр-физ. лицо, по умолчанию индекс 0 = физлицо
        /// </summary>
        public int ind_ur_fiz_lico { get; set; } = 0;
        /// <summary>
        /// Иностранец - по умолчанию - нет
        /// </summary>
        public bool is_foregin { get; set; } = false;
        /// <summary>
        /// Транспорт следует транзитом, или к месту регистрации, по умолчанию - нет
        /// </summary>
        public bool is_transit { get; set; } = false;
        /// <summary>
        /// Ограничение по водителям, по умолчанию = false
        /// </summary>
        public bool is_driver_limit { get; set; } = false;
        /// <summary>
        /// Транспорт используется как такси, по умолчанию - нет
        /// </summary>
        public bool is_taxi { get; set; } = false;
        /// <summary>
        /// Страхуется с прицепом, по умолчанию - нет
        /// </summary>
        public bool is_pricep { get; set; } = false;
        /// <summary>
        /// Мощность транспортного средства, по умолчанию = 0
        /// </summary>
        public int power_ts { get; set; } = 0;
        /// <summary>
        /// Максимальная разрешенная масса ТС
        /// </summary>
        public int max_mass_ts { get; set; } = 0;
        /// <summary>
        /// Используется в регулярных перевозках, по умолчанию - нет
        /// </summary>
        public bool is_regul_perevoz { get; set; } = false;
        /// <summary>
        /// Количество пассажирских мест
        /// </summary>
        public int count_pasangers { get; set; } = 0;
        /// <summary>
        /// Индекс выбранной категории ТС
        /// </summary>
        public int ind_categori_ts { get; set; } = 1;
        /// <summary>
        /// "Особый" случай, по умолчанию false
        /// </summary>
        public bool is_warning { get; set; } = false;
        /// <summary>
        /// индекс строки срока использования ТС, по умолчанию = 11
        /// </summary>
        public int ind_srok_strah { get; set; } = 11;
        /// <summary>
        /// индекс строки периода использования ТС, по умолчанию = 8
        /// </summary>
        public int ind_srok_use { get; set; } = 8;

		/// <summary>
		/// Коэффициент КБМ
		/// </summary>
		public double koef_kbm { get; set; } = 1.0;

		// старый коэффициент КБМ
        /// <summary>
        /// индекс строки коэффициента бонус-малус
        /// </summary>
        public int ind_kbm { get; set; } = 5;


        /// <summary>
        /// индекс выбранного региона
        /// </summary>
        public int ind_region { get; set; } = 1;
        /// <summary>
        /// индекс под-региона
        /// </summary>
        public int ind_sub_region { get; set; }
        /// <summary>
        /// Индекс категории территории
        /// </summary>
        //public int ind_kategori_teritori { get; set; } = 0;
        /// <summary>
        /// Список допущенных водителей
        /// </summary>
        public List<Driver> Drivers { get; set; } = new List<Driver>();

        // СЛУЖЕБНЫЕ ПРОЦЕДУРЫ И ФУНКЦИИ 
        /// <summary>
        /// Возвращает результирующую страховую премию
        /// </summary>
        /// <returns></returns>
        public decimal CalcStrahPrem()
        {
            // результирующая страховая премия
            decimal result = 1;
            // определяем формулу, по которой будет расчет
            // в определении участвуют показатели: юр./физ. лицо, иностранец, транзит и категория ТС
            // формула записывается в словарь, т.к. все коэффициенты перемножаются
            Dictionary<string, double> main_formula = new Dictionary<string, double>();
            // если это РФ и не следует к месту регистрации тс
            if (!is_foregin && !is_transit)
            {
                // вычисляем для российского физ.лица
                if (ind_ur_fiz_lico == 0)
                {
                    // если категория "B", "BE" в т.ч. такси
                    if(ind_categori_ts == 1)
                    {
                        main_formula.Add("Tb:", 1); //базовый тариф
                        main_formula.Add("Kt:", 1); //коэффициент территориальный
                        main_formula.Add("Kbm:", 1); //коэффициент бонус-малус по водителю
                        main_formula.Add("Kvs:", 1); //коэффициент возраста стажа водителя
                        main_formula.Add("Ko:", 1);  //с ограничением по водителям
                        main_formula.Add("Km:", 1); //коэффициент мощности ТС
                        main_formula.Add("Ks:", 1); //период использования ТС
                        main_formula.Add("Kn:", 1); //"особый случай"
                    }
                    // для остальных категорий
                    else
                    {
                        //
                        main_formula.Add("Tb:", 1); //базовый тариф
                        main_formula.Add("Kt:", 1); //коэффициент территориальный
                        main_formula.Add("Kbm:", 1); //коэффициент бонус-малус по водителю
                        main_formula.Add("Kvs:", 1); //коэффициент возраста стажа водителя
                        main_formula.Add("Ko:", 1);  //с ограничением по водителям
                        main_formula.Add("Ks:", 1); //период использования ТС
                        main_formula.Add("Kn:", 1); //"особый случай"
                        main_formula.Add("Kpr:", 1); //используется с прицепом
                    }
                }
                // вычисляем для российского юр.лица
                else
                {
                    // если категория "B", "BE" в т.ч. такси
                    if (ind_categori_ts == 1)
                    {
                        main_formula.Add("Tb:", 1); //базовый тариф
                        main_formula.Add("Kt:", 1); //коэффициент территориальный
                        main_formula.Add("Kbm:", 1); //коэффициент бонус-малус по водителю
                        main_formula.Add("Ko:", 1);  //с ограничением по водителям
                        main_formula.Add("Km:", 1); //коэффициент мощности ТС
                        main_formula.Add("Ks:", 1); //период использования ТС
                        main_formula.Add("Kn:", 1); //"особый случай"
                        main_formula.Add("Kpr:", 1); //используется с прицепом
                    }
                    // для остальных категорий
                    else
                    {
                        main_formula.Add("Tb:", 1); //базовый тариф
                        main_formula.Add("Kt:", 1); //коэффициент территориальный
                        main_formula.Add("Kbm:", 1); //коэффициент бонус-малус по водителю
                        main_formula.Add("Ko:", 1);  //с ограничением по водителям
                        main_formula.Add("Ks:", 1); //период использования ТС
                        main_formula.Add("Kn:", 1); //"особый случай"
                        main_formula.Add("Kpr:", 1); //используется с прицепом
                    }
                }
            }
            // если это РФ и следует к месту регистрации тс
            else if(!is_foregin && is_transit)
            {
                // вычисляем для физ.лица
                if (ind_ur_fiz_lico == 0)
                {
                    // если категория "B", "BE" в т.ч. такси
                    if (ind_categori_ts == 1)
                    {
                        main_formula.Add("Tb:", 1); //базовый тариф
                        main_formula.Add("Kbm:", 1); //коэффициент бонус-малус по водителю
                        main_formula.Add("Kvs:", 1); //коэффициент возраста стажа водителя
                        main_formula.Add("Ko:", 1);  //с ограничением по водителям
                        main_formula.Add("Km:", 1); //коэффициент мощности ТС
                        main_formula.Add("Kp:", 1); // срок страхования
                    }
                    // для всех остальных
                    else
                    {
                        main_formula.Add("Tb:", 1); //базовый тариф
                        main_formula.Add("Kbm:", 1); //коэффициент бонус-малус по водителю
                        main_formula.Add("Kvs:", 1); //коэффициент возраста стажа водителя
                        main_formula.Add("Ko:", 1);  //с ограничением по водителям
                        main_formula.Add("Kp:", 1); // срок страхования
                        main_formula.Add("Kpr:", 1); //используется с прицепом
                    }
                }
                // вычисляем для юр.рица
                else
                {
                    // если категория "B", "BE" в т.ч. такси
                    if (ind_categori_ts == 1)
                    {
                        main_formula.Add("Tb:", 1); //базовый тариф
                        main_formula.Add("Kbm:", 1); //коэффициент бонус-малус по водителю
                        main_formula.Add("Ko:", 1);  //с ограничением по водителям
                        main_formula.Add("Km:", 1); //коэффициент мощности ТС
                        main_formula.Add("Kp:", 1); // срок страхования
                        main_formula.Add("Kpr:", 1); //используется с прицепом
                    }
                    // для всех остальных категорий
                    else
                    {
                        main_formula.Add("Tb:", 1); //базовый тариф
                        main_formula.Add("Kbm:", 1); //коэффициент бонус-малус по водителю
                        main_formula.Add("Ko:", 1);  //с ограничением по водителям
                        main_formula.Add("Kp:", 1); // срок страхования
                        main_formula.Add("Kpr:", 1); //используется с прицепом
                    }
                }
            }
            // если это иностранец
            else if (is_foregin)
            {
                // вычисляем для физ.лица
                if (ind_ur_fiz_lico == 0)
                {
                    // если категория "B", "BE" в т.ч. такси
                    if (ind_categori_ts == 1)
                    {
                        main_formula.Add("Tb:", 1); //базовый тариф
                        main_formula.Add("Kt:", 1); //коэффициент территориальный
                        main_formula.Add("Kbm:", 1); //коэффициент бонус-малус по водителю
                        main_formula.Add("Kvs:", 1); //коэффициент возраста стажа водителя
                        main_formula.Add("Ko:", 1);  //с ограничением по водителям
                        main_formula.Add("Km:", 1); //коэффициент мощности ТС
                        main_formula.Add("Kp:", 1); // срок страхования
                        main_formula.Add("Kn:", 1); //"особый случай"
                    }
                    // для всех остальных категорий
                    else
                    {
                        main_formula.Add("Tb:", 1); //базовый тариф
                        main_formula.Add("Kt:", 1); //коэффициент территориальный
                        main_formula.Add("Kbm:", 1); //коэффициент бонус-малус по водителю
                        main_formula.Add("Kvs:", 1); //коэффициент возраста стажа водителя
                        main_formula.Add("Ko:", 1);  //с ограничением по водителям
                        main_formula.Add("Kp:", 1); // срок страхования
                        main_formula.Add("Kn:", 1); //"особый случай"
                        main_formula.Add("Kpr:", 1); //используется с прицепом
                    }
                }
                // вычисляем для юр.рица
                else
                {
                    // если категория "B", "BE" в т.ч. такси
                    if (ind_categori_ts == 1)
                    {
                        main_formula.Add("Tb:", 1); //базовый тариф
                        main_formula.Add("Kt:", 1); //коэффициент территориальный
                        main_formula.Add("Kbm:", 1); //коэффициент бонус-малус по водителю
                        main_formula.Add("Ko:", 1);  //с ограничением по водителям
                        main_formula.Add("Km:", 1); //коэффициент мощности ТС
                        main_formula.Add("Kp:", 1); // срок страхования
                        main_formula.Add("Kn:", 1); //"особый случай"
                        main_formula.Add("Kpr:", 1); //используется с прицепом
                    }
                    // для остальных категорий
                    else
                    {
                        main_formula.Add("Tb:", 1); //базовый тариф
                        main_formula.Add("Kt:", 1); //коэффициент территориальный
                        main_formula.Add("Kbm:", 1); //коэффициент бонус-малус по водителю
                        main_formula.Add("Ko:", 1);  //с ограничением по водителям
                        main_formula.Add("Kp:", 1); // срок страхования
                        main_formula.Add("Kn:", 1); //"особый случай"
                        main_formula.Add("Kpr:", 1); //используется с прицепом
                    }
                }
            }

            formula_result = FillFormulaResult(main_formula);

            // перебираем элементы словаря и перемножаем их
            foreach(var tmp_val in formula_result)
            {
                result = result * (decimal)tmp_val.Value;
            }

            /* вычисляем максимальную страховую премию.
              Премия не может превышать значения (3*tmp_tb*tmp_kt) при обычных условиях
              и премия не может превышать значения (5*tmp_tb*tmp_kt) при особых условиях */
            double tmp_tb = GetKoeff_Tb();
            double tmp_kt = GetKoeff_Kt();
            if (!is_warning)
            {
                if(result > (decimal)(3 * tmp_tb * tmp_kt))
                {
                    result = (decimal)(3 * tmp_tb * tmp_kt);
                }
            }
            else
            {
                if(result > (decimal)(5 * tmp_tb * tmp_kt))
                {
                    result = (decimal)(5 * tmp_tb * tmp_kt);
                }
            }

            return result;
        }
        /// <summary>
        /// Функция обрабатывает исходную формулу, и заполняет необходимые коэффициенты
        /// </summary>
        /// <param name="formula">формула, по которой ведется расчет</param>
        /// <returns>Заполненный словарь коэффициентов и их значений</returns>
        private Dictionary<string, double> FillFormulaResult(Dictionary<string, double> formula)
        {
            // результат вычислений
            Dictionary<string, double> result_dict = new Dictionary<string, double>();
            // проверяем каждый элемент словаря и вычисляем необходимые коэффициенты
            foreach(var tmp_record in formula)
            {
                switch (tmp_record.Key)
                {
                    // базовый тариф
                    case "Tb:":
                        {
                            double koeff_Tb = GetKoeff_Tb();
                            // добавляем в словарь коэффициент и его значение
                            result_dict.Add(tmp_record.Key, koeff_Tb);
                            break;
                        }
                    // коэффициент территориальный
                    case "Kt:":
                        {
                            double koeff_Kt = GetKoeff_Kt();
                            // добавляем в словарь коэффициент и его значение
                            result_dict.Add(tmp_record.Key, koeff_Kt);
                            break;
                        }
                    // коэффициент бонус-малус
                    case "Kbm:":
                        {
                            double koeff_Kbm = GetKoeff_Kbm();
                            result_dict.Add(tmp_record.Key, koeff_Kbm);
                            break;
                        }
                    // коэффициент возраста стажа водителя
                    case "Kvs:":
                        {
                            double koeff_Kvs = GetKoeff_Kvs();
                            result_dict.Add(tmp_record.Key, koeff_Kvs);
                            break;
                        }
                    // с ограничением по водителям
                    case "Ko:":
                        {
                            double koeff_Ko = GetKoeff_Ko();
                            result_dict.Add(tmp_record.Key, koeff_Ko);
                            break;
                        }
                    // коэффициент мощности ТС
                    case "Km:":
                        {
                            double koeff_Km = GetKoeff_Km();
                            result_dict.Add(tmp_record.Key, koeff_Km);
                            break;
                        }
                    // период использования ТС
                    case "Ks:":
                        {
                            double koef_Ks = GetKoeff_Ks();
                            result_dict.Add(tmp_record.Key, koef_Ks);
                            break;
                        }
                    // "особый случай"
                    case "Kn:":
                        {
                            double koeff_Kn = GetKoeff_Kn();
                            result_dict.Add(tmp_record.Key, koeff_Kn);
                            break;
                        }
                    // срок страхования
                    case "Kp:":
                        {
                            double koeff_Kp = GetKoeff_Kp();
                            result_dict.Add(tmp_record.Key, koeff_Kp);
                            break;
                        }
                    // использование с прицепом
                    case "Kpr:":
                        {
                            double koeff_Kpr = GetKoeff_Kpr();
                            result_dict.Add(tmp_record.Key, koeff_Kpr);
                            break;
                        }
                }
            }

            return result_dict;
        }
        /// <summary>
        /// Коэффициент использования прицепа
        /// </summary>
        /// <returns></returns>
        private double GetKoeff_Kpr()
        {
            double result = 0;
            if (is_pricep)
            {
                // если категория ТС А или B и принадлежащие юр.лицам
                if ((ind_categori_ts == 0 || ind_categori_ts == 1) & ind_ur_fiz_lico == 1)
                {
                    result = 1.16;
                }
                // если категория ТС С и менне 16 тонн разрешенная масса
                else if (ind_categori_ts == 2 & max_mass_ts <= 16)
                {
                    result = 1.4;
                }
                // если категория ТС С и более 16 тонн разрешенная масса
                else if (ind_categori_ts == 2 & max_mass_ts > 16)
                {
                    result = 1.25;
                }
                // если тракторы, самоходки
                else if(ind_categori_ts == 6)
                {
                    result = 1.24;
                }
                // к другим категориям и типам
                else
                {
                    result = 1;
                }
            }
            else
            {
                result = 1;
            }

            return result;
        }
        /// <summary>
        /// Функция возвращает коэффициент сроков страхования
        /// </summary>
        /// <returns></returns>
        private double GetKoeff_Kp()
        {
            double result = 0;

            // если это россиянин
            if (!is_foregin)
            {
                result = 0.2;
            }
            // если это иностранец
            else
            {
                // выбираем данные из БД, в параметры функции передается индекс записи сроков страхования
                result = data_base._get_koeff_Kp(ind_srok_strah);

            }

            return result;
        }
        /// <summary>
        /// Функция возвращает коэффициент особого случая
        /// </summary>
        /// <returns></returns>
        private double GetKoeff_Kn()
        {
            double result = 0;
            //если это "особый случай"
            if (is_warning)
            {
                result = 1.5;
            }
            else
            {
                result = 1;
            }

            return result;
        }
        /// <summary>
        /// Функция возвращает коэффициент периода использования ТС
        /// </summary>
        /// <returns></returns>
        private double GetKoeff_Ks()
        {
            double result = 0;

            result = data_base._get_koef_Ks(ind_srok_use);

            return result;
        }
        /// <summary>
        /// Функция возвращает коэффициент мощности ТС
        /// </summary>
        /// <returns></returns>
        private double GetKoeff_Km()
        {
            double result = 0;
            if(power_ts <= 50)
            {
                result = 0.6;
            }
            else if(50 < power_ts & power_ts <= 70)
            {
                result = 1;
            }
            else if(70 < power_ts & power_ts <= 100)
            {
                result = 1.1;
            }
            else if(100 < power_ts & power_ts <= 120)
            {
                result = 1.2;
            }
            else if(120 < power_ts & power_ts <= 150)
            {
                result = 1.4;
            }
            else if(power_ts > 150)
            {
                result = 1.6;
            }

            return result;
        }
        /// <summary>
        /// Функция возвращает коэффициент ограничения по водителям
        /// </summary>
        /// <returns></returns>
        private double GetKoeff_Ko()
        {
            double result = 0;

            if (is_driver_limit)
            {
                result = 1;
            }
            else
            {
                result = 1.87;
            }
            // если юр.лицо
            if(ind_ur_fiz_lico == 1)
            {
                result = 1.8;
            }

            return result;
        }
        /// <summary>
        /// Функция возвращает коэффициент возраста-стажа
        /// </summary>
        /// <returns></returns>
        private double GetKoeff_Kvs()
        {
            double result = 0;
			// если это не иностранец
			if (is_driver_limit)
			{				
				if (!is_foregin)
				{
					// получаем максимальный коэффициент по водителям
					result = get_max_kvs(Drivers);
				}
				// для иностранца
				else
				{
					// для физлица
					if (ind_ur_fiz_lico == 0)
					{
						result = 1.7;
					}
					// для юрлица
					else
					{
						result = 1;
					}
				}
			}
			else
			{
				result = 1;
			}
			return result;
        }
		/// <summary>
		/// Функция возвращает максимальный коэффциент по списку водителей
		/// </summary>
		/// <param name="drivers">Список водителей, допущенных к управлению</param>
		/// <returns></returns>
		private double get_max_kvs(List<Driver> drivers)
		{
			double result = 0;

			if(drivers.Count > 0)
			{
				// заполняем список водителей коэффициентами
				fill_drivers_kvs(drivers);
				// получаем максимальный коэффициент				
				double tmp_result = 0; // временный (промежуточный) результат
				foreach (Driver tmp_driver in drivers)
				{
					if(tmp_result < tmp_driver.Koeff)
					{
						tmp_result = tmp_driver.Koeff;
					}
				}
				result = tmp_result;
			}
			else
			{
				result = 0;
			}

			return result;
		}
		/// <summary>
		/// процедура заполняет данные значениями полученных коэффициентов
		/// </summary>
		/// <param name="drivers"></param>
		private void fill_drivers_kvs(List<Driver> drivers)
		{
			// перебираем записи списка водителей и устанавливаем соответствующий коэффициент
			foreach(Driver tmp_driver in drivers)
			{
				// вычисляем возраст водителя
				int age_driver = get_diff_years(tmp_driver.AgeDriver);
				// вычисляем стаж водителя
				int stage_driver = get_diff_years(tmp_driver.StageDriver);
				if(stage_driver < age_driver)
				{
					tmp_driver.Koeff = get_kvs(age_driver, stage_driver);
				}
				else
				{
					tmp_driver.Koeff = 0;
				}
				
			}
		}
		/// <summary>
		/// функция возвращает коэффициент возраста стажа
		/// </summary>
		/// <param name="driver_age"> возраст водителя</param>
		/// <param name="driver_stage"> водительский стаж водителя</param>
		/// <returns></returns>
		private double get_kvs(int driver_age, int driver_stage)
		{
			double result = 0;

			if ((16 <= driver_age & driver_age <= 21) & (driver_stage == 0))
			{
				result = 1.87;
			}
			else if ((16 <= driver_age & driver_age <= 21) & (driver_stage == 1))
			{
				result = 1.87;
			}
			else if ((16 <= driver_age & driver_age <= 21) & (driver_stage == 2))
			{
				result = 1.87;
			}
			else if ((16 <= driver_age & driver_age <= 21) & (3 <= driver_stage & driver_stage <= 4))
			{
				result = 1.66;
			}
			else if ((16 <= driver_age & driver_age <= 21) & (5 <= driver_stage & driver_stage <= 6))
			{
				result = 1.66;
			}
			else if ((22 <= driver_age & driver_age <= 24) & (driver_stage == 0))
			{
				result = 1.77;
			}
			else if ((22 <= driver_age & driver_age <= 24) & (driver_stage == 1))
			{
				result = 1.77;
			}
			else if ((22 <= driver_age & driver_age <= 24) & (driver_stage == 2))
			{
				result = 1.77;
			}
			else if ((22 <= driver_age & driver_age <= 24) & (3 <= driver_stage & driver_stage <= 4))
			{
				result = 1.04;
			}
			else if ((22 <= driver_age & driver_age <= 24) & (5 <= driver_stage & driver_stage <= 6))
			{
				result = 1.04;
			}
			else if ((22 <= driver_age & driver_age <= 24) & (7 <= driver_stage & driver_stage <= 9))
			{
				result = 1.04;
			}
			else if ((25 <= driver_age & driver_age <= 29) & (driver_stage == 0))
			{
				result = 1.77;
			}
			else if ((25 <= driver_age & driver_age <= 29) & (driver_stage == 1))
			{
				result = 1.69;
			}
			else if ((25 <= driver_age & driver_age <= 29) & (driver_stage == 2))
			{
				result = 1.63;
			}
			else if ((25 <= driver_age & driver_age <= 29) & (3 <= driver_stage & driver_stage <= 4))
			{
				result = 1.04;
			}
			else if ((25 <= driver_age & driver_age <= 29) & (5 <= driver_stage & driver_stage <= 6))
			{
				result = 1.04;
			}
			else if ((25 <= driver_age & driver_age <= 29) & (7 <= driver_stage & driver_stage <= 9))
			{
				result = 1.04;
			}
			else if ((25 <= driver_age & driver_age <= 29) & (10 <= driver_stage & driver_stage <= 14))
			{
				result = 1.01;
			}
			else if ((30 <= driver_age & driver_age <= 34) & (driver_stage == 0))
			{
				result = 1.63;
			}
			else if ((30 <= driver_age & driver_age <= 34) & (driver_stage == 1))
			{
				result = 1.63;
			}
			else if ((30 <= driver_age & driver_age <= 34) & (driver_stage == 2))
			{
				result = 1.63;
			}
			else if ((30 <= driver_age & driver_age <= 34) & (3 <= driver_stage & driver_stage <= 4))
			{
				result = 1.04;
			}
			else if ((30 <= driver_age & driver_age <= 34) & (5 <= driver_stage & driver_stage <= 6))
			{
				result = 1.04;
			}
			else if ((30 <= driver_age & driver_age <= 34) & (7 <= driver_stage & driver_stage <= 9))
			{
				result = 1.01;
			}
			else if ((30 <= driver_age & driver_age <= 34) & (10 <= driver_stage & driver_stage <= 14))
			{
				result = 0.96;
			}
			else if ((30 <= driver_age & driver_age <= 34) & (driver_stage > 14))
			{
				result = 0.96;
			}
			else if ((35 <= driver_age & driver_age <= 39) & (driver_stage == 0))
			{
				result = 1.63;
			}
			else if ((35 <= driver_age & driver_age <= 39) & (driver_stage == 1))
			{
				result = 1.63;
			}
			else if ((35 <= driver_age & driver_age <= 39) & (driver_stage == 2))
			{
				result = 1.63;
			}
			else if ((35 <= driver_age & driver_age <= 39) & (3 <= driver_stage & driver_stage <= 4))
			{
				result = 0.99;
			}
			else if ((35 <= driver_age & driver_age <= 39) & (5 <= driver_stage & driver_stage <= 6))
			{
				result = 0.96;
			}
			else if ((35 <= driver_age & driver_age <= 39) & (7 <= driver_stage & driver_stage <= 9))
			{
				result = 0.96;
			}
			else if ((35 <= driver_age & driver_age <= 39) & (10 <= driver_stage & driver_stage <= 14))
			{
				result = 0.96;
			}
			else if ((35 <= driver_age & driver_age <= 39) & (driver_stage > 14))
			{
				result = 0.96;
			}
			else if ((40 <= driver_age & driver_age <= 49) & (driver_stage == 0))
			{
				result = 1.63;
			}
			else if ((40 <= driver_age & driver_age <= 49) & (driver_stage == 1))
			{
				result = 1.63;
			}
			else if ((40 <= driver_age & driver_age <= 49) & (driver_stage == 2))
			{
				result = 1.63;
			}
			else if ((40 <= driver_age & driver_age <= 49) & (3 <= driver_stage & driver_stage <= 4))
			{
				result = 0.96;
			}
			else if ((40 <= driver_age & driver_age <= 49) & (5 <= driver_stage & driver_stage <= 6))
			{
				result = 0.96;
			}
			else if ((40 <= driver_age & driver_age <= 49) & (7 <= driver_stage & driver_stage <= 9))
			{
				result = 0.96;
			}
			else if ((40 <= driver_age & driver_age <= 49) & (10 <= driver_stage & driver_stage <= 14))
			{
				result = 0.96;
			}
			else if ((40 <= driver_age & driver_age <= 49) & (driver_stage > 14))
			{
				result = 0.96;
			}
			else if ((50 <= driver_age & driver_age <= 59) & (driver_stage == 0))
			{
				result = 1.63;
			}
			else if ((50 <= driver_age & driver_age <= 59) & (driver_stage == 1))
			{
				result = 1.63;
			}
			else if ((50 <= driver_age & driver_age <= 59) & (driver_stage == 2))
			{
				result = 1.63;
			}
			else if ((50 <= driver_age & driver_age <= 59) & (3 <= driver_stage & driver_stage <= 4))
			{
				result = 0.96;
			}
			else if ((50 <= driver_age & driver_age <= 59) & (5 <= driver_stage & driver_stage <= 6))
			{
				result = 0.96;
			}
			else if ((50 <= driver_age & driver_age <= 59) & (7 <= driver_stage & driver_stage <= 9))
			{
				result = 0.96;
			}
			else if ((50 <= driver_age & driver_age <= 59) & (10 <= driver_stage & driver_stage <= 14))
			{
				result = 0.96;
			}
			else if ((50 <= driver_age & driver_age <= 59) & (driver_stage > 14))
			{
				result = 0.96;
			}
			else if (driver_age > 59 & driver_stage == 0)
			{
				result = 1.6;
			}
			else if (driver_age > 59 & driver_stage == 1)
			{
				result = 1.6;
			}
			else if (driver_age > 59 & driver_stage == 2)
			{
				result = 1.6;
			}
			else if (driver_age > 59 & (3 <= driver_stage & driver_stage <= 4))
			{
				result = 0.93;
			}
			else if (driver_age > 59 & (5 <= driver_stage & driver_stage <= 6))
			{
				result = 0.93;
			}
			else if (driver_age > 59 & (7 <= driver_stage & driver_stage <= 9))
			{
				result = 0.93;
			}
			else if (driver_age > 59 & (10 <= driver_stage & driver_stage <= 14))
			{
				result = 0.93;
			}
			else if (driver_age > 59 & driver_stage > 14)
			{
				result = 0.93;
			}

			return result;
		}
        /// <summary>
        /// Функция возвращает коэффициет бонус-малус
        /// </summary>
        /// <returns></returns>
        private double GetKoeff_Kbm()
        {
            double result = 0.0;

			//если c ограничениями
			if (is_driver_limit)
			{
				result = get_max_kbm_driver();
			}
			else
			{
				//result = data_base._get_koef_Kbm(ind_kbm);
				result = koef_kbm;
			}
            
            return result;
        }
		/// <summary>
		/// Функция возвращает максимальный коэффициент по водителям
		/// </summary>
		/// <returns></returns>
		private double get_max_kbm_driver()
		{
			double result = 0.0;
			// определяем максимальный коэффициент
			foreach(Driver tmp_driver in Drivers)
			{
				if(tmp_driver.Koeff_kbm.koeff > result)
				{
					result = tmp_driver.Koeff_kbm.koeff;
				}
			}

			return result;
		}
		/// <summary>
		/// Функция возвращает значение территориального коэфициента
		/// </summary>
		/// <returns></returns>
		private double GetKoeff_Kt()
        {
            double result = 0;
            // если это не иностранец
            if (!is_foregin)
            {
                result = data_base._get_koef_Kt(ind_sub_region, ind_categori_ts);
            }
            // для иностранца
            else
            {
                result = 1.7;
            }

            return result;
        }
        /// <summary>
        /// Фунция возвращает значение коэффициента базового тарифа
        /// </summary>
        /// <returns></returns>
        private double GetKoeff_Tb()
        {
            double result = 0;
            //result = (double)tarif_bazovi;
            result = data_base._get_base_tarif(ind_categori_ts, ind_sub_region, ind_ur_fiz_lico, is_taxi, max_mass_ts, count_pasangers, is_regul_perevoz);
            return result;
        }
        /// <summary>
        /// Возвращает формулу расчета в текстовом представлении
        /// </summary>
        /// <returns></returns>
        public string GetResultFormula()
        {
            // переменная хранит результаты расчета
            StringBuilder result = new StringBuilder();
            // счетчик количества записей
            int tmp_ind = 1;
            // перебираем коллекцию и вводим ее в строку
            foreach(var tmp_val in formula_result)
            {
                result = result.Append(tmp_val.Key +"\t" + tmp_val.Value.ToString());
                if(tmp_ind < formula_result.Count)
                {
                    result = result.Append("\n");
                    tmp_ind++;
                }
            }

            return result.ToString();
        }
		/// <summary>
		/// процедура сброса установленных показателей
		/// </summary>
		public void ResetSetters()
        {
            // очищаем словарь основной формулы
            formula_result.Clear();
            // устанавливаем параметры по умолчанию
            fio_strah = "";
			koef_kbm = 1.0;
            tarif_bazovi = 0;
            ind_ur_fiz_lico = 0;
            is_foregin = false;
            is_driver_limit = false;
            ind_kbm = 5;
            is_taxi = false;
            is_pricep = false;
            ind_categori_ts = 1;
            is_transit = false;
            power_ts = 0;
            max_mass_ts = 0;
            is_warning = false;
            is_regul_perevoz = false;
            count_pasangers = 0;
            ind_srok_strah = 11;
            ind_srok_use = 8;
            ind_region = 1;
			ind_sub_region = 0;
			Drivers.Clear();          
        }
		/// <summary>
		/// Функция возвращает количество лет от даты
		/// </summary>
		/// <param name="tmp_date"> дата, от которой ведется отсчет</param>
		/// <returns></returns>
		private int get_diff_years(DateTime tmp_date)
		{
			int result = 0;
			
			if (tmp_date < DateTime.Now)
			{
				DateTime zeroTime = new DateTime(1, 1, 1);
				TimeSpan span = DateTime.Today - tmp_date;
				result = (zeroTime + span).Year - 1;
			}

			return result;
		}
        
        /* ОБЩИЕ ПРОЦЕДУРЫ И ФУНКЦИИ*/

        // Работа с базой данных
        /// <summary>
        /// Функция получает перечисления юр.физ. лицо из базы данных
        /// </summary>
        /// <returns>Список перечислений юр.физ. лицо</returns>
        public ObservableCollection<UrFizLico> GetUrFizLicoList()
        {
            ObservableCollection<UrFizLico> result = new ObservableCollection<UrFizLico>();

            result = data_base._getUrFizLico();
            
            return result;
        }
        /// <summary>
        /// Функция получает список категорий ТС
        /// </summary>
        /// <returns>Коллекция котегорий ТС</returns>
        public ObservableCollection<CategoriaTS> GetCategoriaTs()
        {
            ObservableCollection<CategoriaTS> result = new ObservableCollection<CategoriaTS>();

            result = data_base._getCateoriaTs();

            return result;
        }
        /// <summary>
        /// Функция возвращает коллекцию периодов страхования
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PeriodStrah> GetPeriodStrahTs()
        {
            ObservableCollection<PeriodStrah> result = new ObservableCollection<PeriodStrah>();

            result = data_base._getPeriodStrah();

            return result;
        }
        /// <summary>
        /// Функция возвращает коллекцию периодов использования
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PeriodUse> GetPeriodUseTs()
        {
            ObservableCollection<PeriodUse> result = new ObservableCollection<PeriodUse>();

            result = data_base._getPeriodUse();

            return result;
        }
        /// <summary>
        /// Функция возвращает коллекцию регионов
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Region> GetRegions()
        {
            ObservableCollection<Region> result = new ObservableCollection<Region>();

            result = data_base._getRegions();

            return result;            
        }
        /// <summary>
        /// Функция возвращает под-регионы
        /// </summary>
        /// <param name="subRegions">код(id) региона</param>
        /// <returns></returns>
        public ObservableCollection<SubRegion> GetSubRegions(int subRegions)
        {
            ObservableCollection<SubRegion> result = new ObservableCollection<SubRegion>();

            result = data_base._getSubRegions(subRegions);

            return result;
        }
        /// <summary>
        /// Функция возвращает список КБМ
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<KbmCl> GetKbms()
        {
            ObservableCollection<KbmCl> result = new ObservableCollection<KbmCl>();

            result = data_base._getKbmList();

            return result;
        }
        /// <summary>
        /// Процедура печати документа
        /// </summary>
        public void PrintDocResult()
        {
            // печатаем документ
            print_data.Print(this);
        }
		/// <summary>
		/// Процедура экспорта данных в графический файл
		/// </summary>
		public void ExportDataResultToImage()
		{
			export_data = new ExportDataToImage();
			export_data.ExportToFile(this);
		}
		/// <summary>
		/// Метод получает данные из таблицы базовых тарифов
		/// </summary>
		public DataTable GetDataTableBaseTarif()
		{
			DataTable result = new DataTable();

			result = data_base._get_dt_base_tarif();

			return result;
		}

        // КОНСТРУКТОР КЛАССА
        public MainModel()
        {
            // инициализируем объект для работы с базой данных
            data_base = new WorkWithDB();
            // инициализируем объект для работы с печатью
            print_data = new PrintData();
        }

    }
}
