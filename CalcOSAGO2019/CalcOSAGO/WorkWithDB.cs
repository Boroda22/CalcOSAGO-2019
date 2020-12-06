using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace CalcOSAGO
{
    /// <summary>
    /// Класс предназначен для работы с базой данных
    /// </summary>
    class WorkWithDB
    {
        /// <summary>
        /// Соединение для работы с файлом - источником данных
        /// </summary>
        private SQLiteConnection connect;

        /// <summary>
        /// метод получения данных юр.физ. лицо
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<UrFizLico> _getUrFizLico()
        {
            //коллекция для временного хранения данных
            ObservableCollection<UrFizLico> tmp_collection_data = new ObservableCollection<UrFizLico>();
            // получаем данные из таблицы
            if (connect != null & connect.State == System.Data.ConnectionState.Open)
            {
                //создаем команду чтения данных
                string sql_command = "SELECT * FROM UrFizLico";
                SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);
                //считываем построчно данные и записываем их в коллекцию
                try
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        UrFizLico tmp_data = new UrFizLico();
                        tmp_data.id_cl = Int32.Parse((reader["id_urfiz_lico"].ToString()));
                        tmp_data.value_cl = reader["text_urfiz_lico"].ToString();
                        tmp_collection_data.Add(tmp_data);
                    }
                }
                catch (SQLiteException e)
                {
                    //MessageBox.Show(e.Message);
                }
            }

            return tmp_collection_data;
        }
        /// <summary>
        /// метод получает список категорий ТС
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<CategoriaTS> _getCateoriaTs()
        {
            //коллекция для временного хранения данных
            ObservableCollection<CategoriaTS> tmp_collection_data = new ObservableCollection<CategoriaTS>();
            // получаем данные из таблицы
            if (connect != null & connect.State == System.Data.ConnectionState.Open)
            {
                //создаем команду чтения данных
                string sql_command = "SELECT * FROM KategoriaTS";
                SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);
                //считываем построчно данные и записываем их в коллекцию
                try
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CategoriaTS tmp_data = new CategoriaTS();
                        tmp_data.id_categoria_ts = Int32.Parse((reader["id_categoria_ts"].ToString()));
                        tmp_data.kategoria_ts = reader["kategoria_ts"].ToString();
                        tmp_collection_data.Add(tmp_data);
                    }
                }
                catch (SQLiteException e)
                {
                    //MessageBox.Show(e.Message);
                }
            }

            return tmp_collection_data;
        }
        /// <summary>
        /// метод получает коллекцию периодов страхования
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PeriodStrah> _getPeriodStrah()
        {
            //коллекция для временного хранения данных
            ObservableCollection<PeriodStrah> tmp_collection_data = new ObservableCollection<PeriodStrah>();
            // получаем данные из таблицы
            if (connect != null & connect.State == System.Data.ConnectionState.Open)
            {
                //создаем команду чтения данных
                string sql_command = "SELECT * FROM PeriodStrahTS";
                SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);
                //считываем построчно данные и записываем их в коллекцию
                try
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        PeriodStrah tmp_data = new PeriodStrah();
                        tmp_data.id = Int32.Parse((reader["id"].ToString()));
                        tmp_data.text_value = reader["text_period_strah_ts"].ToString();
                        tmp_collection_data.Add(tmp_data);
                    }
                }
                catch (SQLiteException e)
                {
                    //MessageBox.Show(e.Message);
                }
            }
            return tmp_collection_data;
        }
        /// <summary>
        /// метод получает коллекцию периодов использования
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PeriodUse> _getPeriodUse()
        {
            //коллекция для временного хранения данных
            ObservableCollection<PeriodUse> tmp_collection_data = new ObservableCollection<PeriodUse>();
            // получаем данные из таблицы
            if (connect != null & connect.State == System.Data.ConnectionState.Open)
            {
                //создаем команду чтения данных
                string sql_command = "SELECT * FROM PeriodUseTS";
                SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);
                //считываем построчно данные и записываем их в коллекцию
                try
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        PeriodUse tmp_data = new PeriodUse();
                        tmp_data.id = Int32.Parse((reader["id"].ToString()));
                        tmp_data.text_value = reader["text_period_use_ts"].ToString();
                        tmp_collection_data.Add(tmp_data);
                    }
                }
                catch (SQLiteException e)
                {
                    //MessageBox.Show(e.Message);
                }
            }
            return tmp_collection_data;
        }
        /// <summary>
        /// метод получает коллекцию регионов
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Region> _getRegions()
        {
            //коллекция для временного хранения данных
            ObservableCollection<Region> tmp_collection_data = new ObservableCollection<Region>();
            // получаем данные из таблицы
            if (connect != null & connect.State == System.Data.ConnectionState.Open)
            {
                //создаем команду чтения данных
                string sql_command = "SELECT * FROM Regions";
                SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);
                //считываем построчно данные и записываем их в коллекцию
                try
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Region tmp_data = new Region();
                        tmp_data.id = Int32.Parse(reader["id_region"].ToString());
                        tmp_data.text_value = reader["region_name"].ToString();
                        //tmp_data.id_categori_teritory = Int32.Parse((reader["id_categoty_teritor"].ToString()));
                        tmp_collection_data.Add(tmp_data);
                    }
                }
                catch (SQLiteException e)
                {
                    //MessageBox.Show(e.Message);
                }
            }
            return tmp_collection_data;
        }
		/// <summary>
		/// метод получает коллекцию под-регионов
		/// </summary>
		/// <param name="ind_main_region">индекс основного региона</param>
		/// <returns></returns>
		public ObservableCollection<SubRegion> _getSubRegions(int ind_main_region)
        {
            //коллекция для временного хранения данных
            ObservableCollection<SubRegion> tmp_collection_data = new ObservableCollection<SubRegion>();
            // получаем данные из таблицы
            if (connect != null & connect.State == System.Data.ConnectionState.Open)
            {
                //создаем команду чтения данных
                string sql_command = "SELECT sub_regions.id_sub_region," +
                    "sub_regions.region," +
                    "sub_regions.sub_region_name " +
                    "FROM SubRegions sub_regions " +
                    "WHERE sub_regions.region =" + ind_main_region.ToString();
                SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);
                //считываем построчно данные и записываем их в коллекцию
                try
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SubRegion tmp_data = new SubRegion();
                        tmp_data.id = Int32.Parse((reader["id_sub_region"].ToString()));
                        tmp_data.text_value = reader["sub_region_name"].ToString();
                        tmp_collection_data.Add(tmp_data);
                    }
                }
                catch (SQLiteException e)
                {
                    //MessageBox.Show(e.Message);
                }
            }
            return tmp_collection_data;
        }
        /// <summary>
        /// Функция возвращает список коэффициентов КБМ
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<KbmCl> _getKbmList()
        {
            //коллекция для временного хранения данных
            ObservableCollection<KbmCl> tmp_collection_data = new ObservableCollection<KbmCl>();

            // получаем данные из таблицы
            if (connect != null & connect.State == System.Data.ConnectionState.Open)
            {
                //создаем команду чтения данных
                string sql_command = "SELECT * FROM KBM";
                SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);
                //считываем построчно данные и записываем их в коллекцию
                try
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        KbmCl tmp_data = new KbmCl();
                        tmp_data.id = Int32.Parse((reader["id"].ToString()));
                        tmp_data.text_value = reader["text_value"].ToString();
						string tmp_value = reader["koef"].ToString().Replace(".", ",");
						tmp_data.koeff = Convert.ToDouble(tmp_value);
                        tmp_collection_data.Add(tmp_data);
                    }
                }
                catch (SQLiteException e)
                {
                    //MessageBox.Show(e.Message);
                }
            }

            return tmp_collection_data;
        }
		/// <summary>
		/// Функция возвращает таблицу данных базовых тарифов
		/// </summary>
		/// <returns></returns>
		public DataTable _get_dt_base_tarif()
		{
			DataTable result = new DataTable();
			if (connect != null & connect.State == System.Data.ConnectionState.Open)
			{
				// создаем команду чтения данных из таблицы базовых тарифов
				string sql_command = "SELECT * FROM BaseTarif";
				SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);
				// пытаемся прочитать данные
				try
				{
					SQLiteDataReader reader = cmd.ExecuteReader();
					// выгружаем выбранные данные в таблицу
					result.Load(reader);
				}
				catch (SQLiteException e)
				{
					//MessageBox.Show(e.Message);
				}
			}

			return result;
		}

        // ОПРЕДЕЛЕНИЕ ЗНАЧЕНИЙ КОЭФИЦИЕНТОВ
        //
        /// <summary>
        /// Функция возвращает значение коэффициента, в зависимости от индекса записи
        /// </summary>
        /// <param name="index">индекс записи</param>
        /// <returns></returns>
        public double _get_koeff_Kp(int index)
        {
            double result = 0;
            // получаем данные из таблицы
            if (connect != null & connect.State == System.Data.ConnectionState.Open)
            {
                //создаем команду чтения данных
                string sql_command = "SELECT period_strah.id," +
                    "period_strah.koef " +
                    "FROM PeriodStrahTS period_strah " +
                    "WHERE period_strah.id =" + index.ToString();
                SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);
                //считываем построчно данные и записываем их в коллекцию
                try
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = (double)reader["koef"];
                    }
                }
                catch (SQLiteException e)
                {
                    //MessageBox.Show(e.Message);
                }
            }

                return result;
        }
        /// <summary>
        /// Функция возвращает значение коэффициента периода использования
        /// </summary>
        /// <param name="index">индекс периода использования</param>
        /// <returns></returns>
        public double _get_koef_Ks(int index)
        {
            double result = 0;
            // получаем данные из таблицы
            if (connect != null & connect.State == System.Data.ConnectionState.Open)
            {
                //создаем команду чтения данных
                string sql_command = "SELECT period_use.id," +
                    "period_use.koef " +
                    "FROM PeriodUseTS period_use " +
                    "WHERE period_use.id =" + index.ToString();
                SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);
                //считываем построчно данные и записываем их в коллекцию
                try
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = (double)reader["koef"];
                    }
                }
                catch (SQLiteException e)
                {
                    //MessageBox.Show(e.Message);
                }
            }

            return result;
        }
        /// <summary>
        /// Функция возвращает коэффициент бонус-малус
        /// </summary>
        /// <param name="index">индекс необходимой записи</param>
        /// <returns></returns>
        public double _get_koef_Kbm(int index)
        {
            double result = 0;
            // получаем данные из таблицы
            if (connect != null & connect.State == System.Data.ConnectionState.Open)
            {
                //создаем команду чтения данных
                string sql_command = "SELECT kbm.id," +
                    "kbm.koef " +
                    "FROM KBM kbm " +
                    "WHERE kbm.id =" + index.ToString();
                SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);
                //считываем построчно данные и записываем их в коллекцию
                try
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = (double)reader["koef"];
                    }
                }
                catch (SQLiteException e)
                {
                    //MessageBox.Show(e.Message);
                }
            }

            return result;
        }
        /// <summary>
        /// Функция возвращает коэффициент территории использования
        /// </summary>
        /// <param name="index">индекс под-региона территории использования</param>
        /// <param name="ind_kategori">индекс категории, по умолчанию = 1</param>
        /// <returns></returns>
        public double _get_koef_Kt(int index_sub_region, int ind_kategori = 1)
        {
            double result = 0;

            // получаем данные из таблицы
            if (connect != null & connect.State == System.Data.ConnectionState.Open)
            {
                //создаем команду чтения данных
                string sql_command = "SELECT subregion.id_sub_region," +
                    "subregion.koef_all, " +
                    "subregion.koef_traktor " +
                    "FROM SubRegions subregion " +
                    "WHERE subregion.id_sub_region =" + index_sub_region.ToString();
                SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);
                //считываем построчно данные и записываем их в коллекцию
                try
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        // если это категория тракторов и самоходок
                        if(ind_kategori == 6)
                        {
                            result = (double)reader["koef_traktor"];
                        }
                        else
                        {
                            result = (double)reader["koef_all"];
                        }
                        
                    }
                }
                catch (SQLiteException e)
                {
                    //MessageBox.Show(e.Message);
                }
            }

            return result;
        }
        /// <summary>
        /// Функция возвращает значение базового тарифа
        /// </summary>
        /// <param name="ind_categor_ts">индекс категории ТС</param>
        /// <param name="ind_kategor_teritor">индекс категории территории</param>
        /// <param name="ind_urfiz_lico">индекс юр-физ. лица</param>
        /// <param name="use_taxi">использование в качестве такси</param>
        /// <param name="weght">разрешенная масса ТС</param>
        /// <param name="seat">количество пассажирских мест</param>
        /// <param name="regular_perevoz">используется в регулярных перевозках</param>
        /// <returns></returns>
        public double _get_base_tarif(int ind_categor_ts, int ind_sub_region, int ind_urfiz_lico, bool use_taxi, int weght, int seat, bool regular_perevoz)
        {
            double result = 0;
            // получаем данные из таблицы
            if (connect != null & connect.State == System.Data.ConnectionState.Open)
            {
                //создаем команду чтения данных
                string sql_command = "SELECT * " +
                    "FROM BaseTarif base_tarif " +
                    "WHERE base_tarif.id_categir_ts =" + ind_categor_ts.ToString() + " AND base_tarif.id_sub_region =" + ind_sub_region.ToString();
                SQLiteCommand cmd = new SQLiteCommand(sql_command, connect);
                //считываем построчно данные и записываем их в коллекцию
                try
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        // для категорий
                        if(ind_categor_ts == 0)
                        {
                            result = (double)reader["koef"];
                            break;
                        }
                        else if(ind_categor_ts == 1)
                        {
                            if(ind_urfiz_lico == 0 & !use_taxi)
                            {
                                result = (double)reader["fiz_lico"];
                            }
                            else if(ind_urfiz_lico == 1 & !use_taxi)
                            {
                                result = (double)reader["ur_lico"];
                            }
                            else if (use_taxi)
                            {
                                result = (double)reader["taxi"];
                            }
                        }
                        else if(ind_categor_ts == 2)
                        {
                            if(weght <= 16)
                            {
                                result = (double)reader["min_weght"];
                            }
                            else
                            {
                                result = (double)reader["max_weght"];
                            }
                        }
                        else if(ind_categor_ts == 3)
                        {
                            if(seat <= 16 & !regular_perevoz)
                            {
                                result = (double)reader["min_seat"];
                            }
                            else if(seat > 16 & !regular_perevoz)
                            {
                                result = (double)reader["max_seat"];
                            }
                            else if (regular_perevoz)
                            {
                                result = (double)reader["regular_perevoz"];
                            }
                        }
                        else if(ind_categor_ts == 4)
                        {
                            result = (double)reader["koef"];
                        }
                        else if (ind_categor_ts == 5)
                        {
                            result = (double)reader["koef"];
                        }
                        else if (ind_categor_ts == 6)
                        {
                            result = (double)reader["koef"];
                        }
                    }
                }
                catch (SQLiteException e)
                {
                    //MessageBox.Show(e.Message);
                }
            }

            return result;
        }

        //КОНСТРУКТОР КЛАССА
        /// <summary>
        /// В конструкторе производим подключение к базе данных
        /// </summary>
        public WorkWithDB()
        {
            //подключаемся к бд
            try
            {
                //получаем относительный путь к файлу с базой данных
                string path_to_file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "main_db.db");
                connect = new SQLiteConnection("Data Source=" + path_to_file + "; Version=3;");
                //открываем базу данных
                connect.Open();
            }
            catch(SQLiteException e)
            {
                //MessageBox.Show(e.Message);
            }
        }
    }
}
