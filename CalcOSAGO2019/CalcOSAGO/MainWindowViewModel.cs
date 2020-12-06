using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CalcOSAGO
{
    //наследуется от интерфейса INotifyPropertyChanged
	/// <summary>
	/// ViewModel основного потока данных
	/// </summary>
    public class ViewModelMainWindow : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        // ПЕРЕМЕННЫЕ КЛАССА        

        /// <summary>
        /// Основная модель данных
        /// </summary>
        private MainModel model_data;

                
        /// <summary>
        /// ФИО страхователя
        /// </summary>
        public string FioStrah
        {
            get
            {
                return model_data.fio_strah;
            }
            set
            {
                model_data.fio_strah = value;
                OnPropertyChanged("FioStrah");
            }
        }

		/// <summary>
		/// Список водителей, допущенных к управлению
		/// </summary>
		public ObservableCollection<Driver> Drivers { get; set; } = new ObservableCollection<Driver>();
		/// <summary>
		/// Выбранный водитель в списке
		/// </summary>
		public Driver SelectedDriver { get; set; }

        /// <summary>
        /// Коллекция хранит перечисления - юр.физ. лицо
        /// </summary>
        public ObservableCollection<UrFizLico> DictUrFizLico { get; set; }
        /// <summary>
        /// Свойство отвечает за индекс выбранного юр.физ. лица, индекс 0 = физлицо
        /// Для юр.лица есть определнные рамки
        /// </summary>
        public UrFizLico SelectedUrFizLicoIndex
        {
            get
            {
                if (model_data.ind_ur_fiz_lico == 1)
                {
                    return DictUrFizLico[0];
                }
                else
                {
                    return DictUrFizLico[1];
                }
            }            
            set
            {
                model_data.ind_ur_fiz_lico = value.id_cl;
                OnPropertyChanged("SelectedUrFizLicoIndex");
                //устанавливаем значения по умолчанию для юр.лиц
                if(value.id_cl == 1)
                {
                    // периоды страхования и использования устанавливаются 10 мес. и более
                    SelectedPeriodUse = get_period_use_by_index(8);
					BlockPeriodUse = false;
					OnPropertyChanged("BlockPeriodUse");
                    SelectedPeriodStrah = get_srok_strah_by_index(11);
                    OnPropertyChanged("SelectedPeriodUse");
                    // без ограничения
                    IsDriverLimit = false;
                    OnPropertyChanged("IsDriverLimit");
					BlockDriverLimit = false;
					OnPropertyChanged("BlockDriverLimit");
				}
				else
				{
					BlockPeriodUse = true;
					OnPropertyChanged("BlockPeriodUse");
					BlockDriverLimit = true;
					OnPropertyChanged("BlockDriverLimit");
				}
            }
        }

        /// <summary>
        /// Коллекция категорий ТС
        /// </summary>
        public ObservableCollection<CategoriaTS> ListCategoriaTs { get; set; }
        /// <summary>
        /// Выбранная категория ТС
        /// </summary>
        public CategoriaTS SelectedCategoriaTS
        {
            get
            {
                return get_categori_ts_by_index(model_data.ind_categori_ts);
            }
            set
            {
                model_data.ind_categori_ts = value.id_categoria_ts;
                OnPropertyChanged("SelectedCategoriaTS");
            }
        }
        /// <summary>
        /// Функция осуществляет поиск записи в коллекции категорий тс по индексу
        /// </summary>
        /// <param name="index">индекс, по которому необходимо найти записть в колекции</param>
        /// <returns></returns>
        private CategoriaTS get_categori_ts_by_index(int index)
        {
            CategoriaTS tmp_result = new CategoriaTS();

            // перебираем все элементы массива, пока не найдем нужный
            foreach(CategoriaTS tmp_value in ListCategoriaTs)
            {
                // если находим равный указанному
                if(tmp_value.id_categoria_ts == index)
                {
                    tmp_result = tmp_value;
                    break;
                }
            }

            return tmp_result;
        }

        /// <summary>
        /// Период страхования
        /// </summary>
        public ObservableCollection<PeriodStrah> ListPeriodStrah { get; set; }
        /// <summary>
        /// Выбранный период страхования
        /// </summary>
        public PeriodStrah SelectedPeriodStrah
        {
            get
            {
                return get_srok_strah_by_index(model_data.ind_srok_strah);
            }
            set
            {
                model_data.ind_srok_strah = value.id;
                OnPropertyChanged("SelectedPeriodStrah");
            }
        }
        /// <summary>
        /// Функция осуществляет поиск записи в коллекции периодов страхования
        /// </summary>
        /// <param name="index">Индекс записи</param>
        /// <returns></returns>
        private PeriodStrah get_srok_strah_by_index(int index)
        {
            PeriodStrah result = new PeriodStrah();
            // перебираем все элементы массива, пока не найдем нужный
            foreach (PeriodStrah tmp_value in ListPeriodStrah)
            {
                // если находим равный указанному
                if (tmp_value.id == index)
                {
                    result = tmp_value;
                    break;
                }
            }

            return result;
        }        

        /// <summary>
        /// Период использования ТС
        /// </summary>
        public ObservableCollection<PeriodUse> ListPeriodUse { get; set; }
        /// <summary>
        /// выбранный период использования
        /// </summary>
        public PeriodUse SelectedPeriodUse
        {
            get
            {
                return get_period_use_by_index(model_data.ind_srok_use);
            }
            set
            {
                model_data.ind_srok_use = value.id;
                OnPropertyChanged("SelectedPeriodUse");
            }
        }
        /// <summary>
        /// Функция осуществляет поиск периода использования по индексу
        /// </summary>
        /// <param name="index">индекс периода использования</param>
        /// <returns></returns>
        private PeriodUse get_period_use_by_index(int index)
        {
            PeriodUse result = new PeriodUse();
            // перебираем все элементы массива, пока не найдем нужный
            foreach (PeriodUse tmp_value in ListPeriodUse)
            {
                // если находим равный указанному
                if (tmp_value.id == index)
                {
                    result = tmp_value;
                    break;
                }
            }

            return result;
        }
		/// <summary>
		/// Признак блокирования списка периода использования ТС
		/// </summary>
		public bool BlockPeriodUse { get; set; } = true;

        /// <summary>
        /// Список регионов
        /// </summary>
        public ObservableCollection<Region> ListRegions { get; set; }
        /// <summary>
        /// выбранный регион
        /// </summary>
        public Region SelectedRegion
        {
            get { return get_selected_region(model_data.ind_region); }
            set
            {
                model_data.ind_region = value.id;
                // устанавливаем уточнение территории, выбираем подчиненные записи
                ListSubRegions = model_data.GetSubRegions(model_data.ind_region);
                OnPropertyChanged("ListSubRegions");
                // устанавливаем первый элемент
                SelectedSubRegion = ListSubRegions[0];
                OnPropertyChanged("SelectedSubRegion");
            }
        }
        /// <summary>
        /// выбранный регион, необходим для установки под-регионов
        /// </summary>
        private Region get_selected_region(int index)
        {
            Region result = new Region();

            // перебираем все элементы массива, пока не найдем нужный
            foreach (Region tmp_value in ListRegions)
            {
                // если находим равный указанному
                if (tmp_value.id == index)
                {
                    result = tmp_value;
                    break;
                }
            }

            return result;
        }
		/// <summary>
		/// Признак блокирования списка регионов
		/// </summary>
		public bool EnableRegions { get; set; } = true;

        /// <summary>
        /// коллекция под-регионов
        /// </summary>
        public ObservableCollection<SubRegion> ListSubRegions { get; set; } = new ObservableCollection<SubRegion>();
        /// <summary>
        /// выбранный под-регион
        /// </summary>
        public SubRegion SelectedSubRegion
        {
            get
            {
                return get_subregion_by_index(model_data.ind_sub_region);
            }
            set
            {
				if(value != null)
				{
					model_data.ind_sub_region = value.id;
					OnPropertyChanged("SelectedSubRegion");
				}
				// устанавливаем индекс подкатегории

            }
        }
        /// <summary>
        /// функция возвращает объект класса по индексу списка
        /// </summary>
        /// <param name="index">индекс под-региона</param>
        /// <returns></returns>
        private SubRegion get_subregion_by_index(int index)
        {
            SubRegion result = new SubRegion();
            // перебираем все элементы массива, пока не найдем нужный
            foreach (SubRegion tmp_value in ListSubRegions)
            {
                // если находим равный указанному
                if (tmp_value.id == index)
                {
                    result = tmp_value;
                    break;
                }
            }

            return result;
        }
		/// <summary>
		/// Признак блокирования списка под-регионов
		/// </summary>
		public bool EnableSubRegions { get; set; } = true;
		
		/// <summary>
		/// Коэффициент КБМ
		/// </summary>
		//public double Koef_kbm
		//{
		//	get
		//	{
		//		return model_data.koef_kbm;
		//	}
		//	set
		//	{
		//		model_data.koef_kbm = value;
		//		OnPropertyChanged("Koef_kbm");
		//	}
		//}
		// КОЭФФИЦИЕНТ КБМ старый
        /// <summary>
        /// Коллекция коэффициентов бонус-малуса
        /// </summary>
        public ObservableCollection<KbmCl> ListKbm { get; set; }
		public double NewKbm
		{
			get { return model_data.koef_kbm; }
			set
			{
				model_data.koef_kbm = value;
				OnPropertyChanged("NewKbm");
			}
		}
		/// <summary>
		/// Значение коэффициента КБМ
		/// </summary>
		public double ValueKbm { get; set; }
		/// <summary>
		/// выбранный коэффициент бонус-малуса
		/// </summary>
		public KbmCl SelectedKbm
		{
			get
			{
				return get_kbm_by_index(model_data.ind_kbm);
			}
			set
			{
				model_data.ind_kbm = value.id;
				OnPropertyChanged("SelectedKbm");
			}
		}
		/// <summary>
		/// функция возвращает элемент коллекции КБМ
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private KbmCl get_kbm_by_index(int index)
        {
            KbmCl result = new KbmCl();
            // перебираем все элементы массива, пока не найдем нужный
            foreach(KbmCl tmp_value in ListKbm)
            {
                // если находим равный указанному
                if (tmp_value.id == index)
                {
                    result = tmp_value;
                    break;
                }
            }

            return result;
        }		
		/// <summary>
		/// Признак блокирования выбора коэффициента КБМ
		/// </summary>
		public bool BlockKbm { get; set; } = true;

        /// <summary>
        /// Транспорт следует транзитом, или к месту регистрации
        /// </summary>
        public bool IsTransit
        {
            get
            {
                return model_data.is_transit;
            }
            set
            {
                model_data.is_transit = value;
                // если это транзит, тогда устанавливаем сроки страхования
                if (value == true)
                {
                    SelectedPeriodStrah = get_srok_strah_by_index(1);
					IsForegin = false;
					OnPropertyChanged("IsForegin");
                }
                OnPropertyChanged("IsTransit");
                OnPropertyChanged("SelectedPeriodStrah");
            }
        }
        /// <summary>
        /// Признак иностранца
        /// </summary>
        public bool IsForegin
        {
            get
            {
                return model_data.is_foregin;
            }
            set
            {
                model_data.is_foregin = value;
                OnPropertyChanged("IsForegin");
				if(value == true)
				{
					IsTransit = false;
					OnPropertyChanged("IsTransit");
					EnableRegions = false;
					OnPropertyChanged("EnableRegions");
					EnableSubRegions = false;
					OnPropertyChanged("EnableSubRegions");
					// устанавливаем регион иностранца
					//model_data.ind_region = 87;
					SelectedRegion = get_selected_region(87);
					OnPropertyChanged("SelectedRegion");
				}
				else
				{
					EnableRegions = true;
					OnPropertyChanged("EnableRegions");
					EnableSubRegions = true;
					OnPropertyChanged("EnableSubRegions");
					// устанавливаем регион
					SelectedRegion = get_selected_region(1);
					OnPropertyChanged("SelectedRegion");
				}
            }
        }
        /// <summary>
        /// Признак ограничения по водителям
        /// </summary>
        public bool IsDriverLimit
        {
            get
            {
                return model_data.is_driver_limit;
            }
            set
            {
                model_data.is_driver_limit = value;
                OnPropertyChanged("IsDriverLimit");
				// если снимаем флаг, тогда список водителей очищается
				if (!value)
				{
					model_data.Drivers.Clear();
					Drivers.Clear();
					OnPropertyChanged("Drivers");
					// блокируем выбор коэффициента КБМ
					BlockKbm = true;
				}
				else
				{
					BlockKbm = false;
				}
				OnPropertyChanged("BlockKbm");
            }
        }
		/// <summary>
		/// Признак блокирования "галки" ограничения по водителям
		/// </summary>
		public bool BlockDriverLimit { get; set; } = true;
		/// <summary>
		/// Используется в качестве такси
		/// </summary>
		public bool IsTaxi
        {
            get
            {
                return model_data.is_taxi;
            }
            set
            {
                model_data.is_taxi = value;
                OnPropertyChanged("IsTaxi");
            }
        }
        /// <summary>
        /// Может использоваться с прицепом
        /// </summary>
        public bool IsPricep
        {
            get
            {
                return model_data.is_pricep;
            }
            set
            {
                model_data.is_pricep = value;
                OnPropertyChanged("IsPricep");
            }
        }
        /// <summary>
        /// Мощьность ТС в л.с.
        /// </summary>
        public int PowerTS
        {
            get
            {
                return model_data.power_ts;
            }
            set
            {
				// если валидное значение, удаляем сообщения по ошибкам
				if (!(SelectedCategoriaTS.id_categoria_ts == 1 && value == 0))
				{
					ClearErrors("PowerTS");
				}
				model_data.power_ts = value;
                OnPropertyChanged("PowerTS");
            }
        }
        /// <summary>
        /// Максимально разрешенная масса ТС
        /// </summary>
        public int MaxMassaTS
        {
            get
            {
                return model_data.max_mass_ts;
            }
            set
            {
				if(!(SelectedCategoriaTS.id_categoria_ts == 2 && MaxMassaTS == 0))
				{
					ClearErrors("MaxMassaTS");
				}
                model_data.max_mass_ts = value;
                OnPropertyChanged("MaxMassaTS");
            }
        }
        /// <summary>
        /// Используется в регулярных перевозках
        /// </summary>
        public bool IsRegularPerevoz
        {
            get
            {
                return model_data.is_regul_perevoz;
            }
            set
            {
                model_data.is_regul_perevoz = value;
                OnPropertyChanged("IsRegularPerevoz");
            }
        }
        /// <summary>
        /// Количество пассажирских мест
        /// </summary>
        public int CountPassangers
        {
            get
            {
                return model_data.count_pasangers;
            }
            set
            {
				if(!(SelectedCategoriaTS.id_categoria_ts == 3 && CountPassangers == 0))
				{
					ClearErrors("CountPassangers");
				}
                model_data.count_pasangers = value;
                OnPropertyChanged("CountPassangers");
            }
        }      
        /// <summary>
        /// "Особый" случай
        /// </summary>
        public bool IsWarinig
        {
            get
            {
                return model_data.is_warning;
            }
            set
            {
                model_data.is_warning = value;
                OnPropertyChanged("IsWarinig");
            }
        }
        /// <summary>
        /// служебная переменная хранит формулу расчета
        /// </summary>
        private string _formula_rascheta { get; set; } = "";
        /// <summary>
        /// Представление результатов расчета
        /// </summary>
        public string ResultRascheta
        {
            get
            {
                return _formula_rascheta;
            }
            set
            {
                _formula_rascheta = value;
                OnPropertyChanged("ResultRascheta");
            }
            //string tmp_result = model_data.GetResultFormula();
        }
        /// <summary>
        /// Результирующая страховая премия
        /// </summary>
        public string StrahPrem { get; set; } = "0.00";
        
        // ОБРАБОТЧИКИ КОМАНД        
        /// <summary>
        /// Команда запуска расчетов
        /// </summary>
        public ICommand CommandCalc
        {
            get { return new DelegateCommand(CalcAllSumm); }
        }
        /// <summary>
        /// Команда сброса показателей
        /// </summary>
        public ICommand CommandReset
        {
            get { return new DelegateCommand(ResetData); }
        }
        /// <summary>
        /// Команда печати документа
        /// </summary>
        public ICommand CommandPrintData
        {
            get { return new DelegateCommand(PrintData); }
        }
		/// <summary>
		/// Команда экспорта данных в графический файл
		/// </summary>
		public ICommand CommandExportImage
		{
			get { return new DelegateCommand(ExportImage); }
		}
		/// <summary>
		/// Команда добавления водителя
		/// </summary>
		public ICommand CommandAddDriver
		{
			get { return new DelegateCommand(AddDriver); }
		}
		/// <summary>
		/// Команда удаления водителя
		/// </summary>
		public ICommand CommandDelDriver
		{
			get { return new DelegateCommand(DelDriver); }
		}

		// РЕАЛИЗАЦИЯ КОМАНД
		/// <summary>
		/// Процедура запускает процесс калькуляции стоимости полиса
		/// </summary>
		private void CalcAllSumm()
        {
			// Очищаем все ошибки
			ClearErrors("PowerTS");
			ClearErrors("MaxMassaTS");
			ClearErrors("CountPassangers");
			ClearErrors("ListSubRegions");
			// наличие ошибок
			bool is_errors = false;
			/* Для определенных категорий ТС должны быть заполнены определенные поля
            Для категории B - мощность*/
			if (SelectedCategoriaTS.id_categoria_ts == 1 && PowerTS == 0)
            {				
				// добавляем новый список ошибок по свойству
				List<string> text_errors = new List<string>();
				text_errors.Add("Укажите мощность ТС");
				SetErrors("PowerTS", text_errors);
				is_errors = true;
			}
			// Для категории С - разрешенная масса
            if (SelectedCategoriaTS.id_categoria_ts == 2 && MaxMassaTS == 0)
            {
				// добавляем новый список ошибок по свойству
				List<string> text_errors = new List<string>();
				text_errors.Add("Укажите массу ТС");
				SetErrors("MaxMassaTS", text_errors);
				is_errors = true;
			}
			// Для категории D - количество пассажирских мест
			if (SelectedCategoriaTS.id_categoria_ts == 3 && CountPassangers == 0)
            {
				// добавляем новый список ошибок по свойству
				List<string> text_errors = new List<string>();
				text_errors.Add("Укажите количество пассажирских мест ТС");
				SetErrors("CountPassangers", text_errors);
				is_errors = true;
			}
			// Если не выбрано уточнения территории
			if (ListSubRegions.Count == 0 && EnableSubRegions)
			{
				// добавляем новый список ошибок по свойству
				List<string> text_errors = new List<string>();
				text_errors.Add("Необходимо выбрать уточнение территории");
				SetErrors("ListSubRegions", text_errors);
				is_errors = true;
			}

			// если есть ошибки, тогда выходим
			if (is_errors) { return; }

			// заполняем водителей, но перед этим очищаем
			model_data.Drivers.Clear();
			foreach(Driver tmp_driver in Drivers)
			{
				model_data.Drivers.Add(tmp_driver);
			}
			// результат вычисления
			decimal result = model_data.CalcStrahPrem();
            // форматированный вывод с разделением на тысячи
            var numberFormatInfo = new System.Globalization.CultureInfo("ru-Ru", false).NumberFormat;
            StrahPrem = result.ToString("N", numberFormatInfo).Replace(",", ".");
            OnPropertyChanged("StrahPrem");

            // формула расчета с показателями
            ResultRascheta = model_data.GetResultFormula();
        }
        /// <summary>
        /// Процедура сбрасывает текущие показатели
        /// </summary>
        private void ResetData()
        {
            model_data.ResetSetters();
            ResultRascheta = "";
            OnPropertyChanged("FioStrah");
            OnPropertyChanged("ResultRascheta");
            OnPropertyChanged("SelectedUrFizLicoIndex");
            OnPropertyChanged("IsForegin");
            OnPropertyChanged("IsDriverLimit");
            OnPropertyChanged("CountAge");
            OnPropertyChanged("CountStage");
			OnPropertyChanged("NewKbm");
            OnPropertyChanged("IsTaxi");
            OnPropertyChanged("IsPricep");
            OnPropertyChanged("SelectedCategoriaTS");
            OnPropertyChanged("IsTransit");
            OnPropertyChanged("PowerTS");
            OnPropertyChanged("MaxMassaTS");
            OnPropertyChanged("IsWarinig");
            OnPropertyChanged("IsRegularPerevoz");
            OnPropertyChanged("CountPassangers");
            OnPropertyChanged("SelectedPeriodUse");
            OnPropertyChanged("SelectedPeriodStrah");
			SelectedRegion = get_selected_region(1);
			OnPropertyChanged("SelectedRegion");
            StrahPrem = "0.00";
            OnPropertyChanged("StrahPrem");
            model_data.Drivers.Clear();
			Drivers.Clear();
			OnPropertyChanged("Drivers");
			BlockKbm = true;
			OnPropertyChanged("BlockKbm");
			BlockPeriodUse = true;
			OnPropertyChanged("BlockPeriodUse");
			BlockDriverLimit = true;
			OnPropertyChanged("BlockDriverLimit");
			EnableRegions = true;
			OnPropertyChanged("EnableRegions");
			EnableSubRegions = true;
			OnPropertyChanged("EnableSubRegions");
			ClearErrors("ListSubRegions");
			OnPropertyChanged("ListSubRegions");
		}
        /// <summary>
        /// Процедура печати результатов расчета
        /// </summary>
        private void PrintData()
        {
            //если есть результат, печатаем данные
            if(StrahPrem != "0.00")
            {
                model_data.PrintDocResult();
            }
            else
            {
                MessageBox.Show("Премия не рассчитана!");
            }
        }        		
		/// <summary>
		/// Процедура экспорта результатов расчета в графический файл
		/// </summary>
		private void ExportImage()
		{
			//если есть результат, печатаем данные
			if (StrahPrem != "0.00")
			{
				model_data.ExportDataResultToImage();
			}
			else
			{
				MessageBox.Show("Премия не рассчитана!");
			}
		}
		/// <summary>
		/// Процедура добавления водителя в список
		/// </summary>
		private void AddDriver()
		{
			if(Drivers.Count() < 4)
			{
				Driver tmp_driver = new Driver();
				//int current_driver_num = Drivers.Count() + 1;
				tmp_driver.NameDriver = "Допущенный водитель";
				Drivers.Add(tmp_driver);
				OnPropertyChanged("Drivers");
			}
		}
		/// <summary>
		/// Процедура удаления водителя из списка
		/// </summary>
		private void DelDriver()
		{
			if(SelectedDriver != null)
			{
				Drivers.Remove(SelectedDriver);
			}
			OnPropertyChanged("Drivers");
		}


		// реализация интерфейса INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


		// реализация интерфейса INotifyDataErrorInfo
		/// <summary>
		/// Список ошибок, которые происходили
		/// </summary>
		private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
		/// <summary>
		/// событие для вызова ошибки
		/// </summary>
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
		/// <summary>
		/// возвращает true, если имеются ошибки
		/// </summary>
		public bool HasErrors
		{
			get
			{
				// имеются ли ошибки
				return (errors.Count > 0);
			}
		}
		/// <summary>
		/// Фукция возвращает перечисление коллекции ошибок по свойству
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public IEnumerable GetErrors(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				// Предоставить всю коллекцию ошибок
				return errors.Values;
			}
			else
			{
				// Предоставить коллекцию ошибок для запрашиваемого свойства
				if (errors.ContainsKey(propertyName))
				{
					return errors[propertyName];
				}
				else
				{
					return null;
				}
			}
		}
		/// <summary>
		/// Обработчик добавления ошибок
		/// </summary>
		/// <param name="propertyName"> имя свойства с ошибкой</param>
		/// <param name="propertyErrors"> список ошибок по свойству</param>
		private void SetErrors(string propertyName, List<string> propertyErrors)
		{
			// очищаем ошибки, которые существуют по этому свойству
			errors.Remove(propertyName);
			// добавляем в список причину ошибки и необходимый элемент
			errors.Add(propertyName, propertyErrors);
			// вызываем обработчик
			if (ErrorsChanged != null)
			{
				ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
			}
			// очищаем ошибки, которые существуют по этому свойству
			errors.Remove(propertyName);
		}
		/// <summary>
		/// Обработчик удаления ошибок
		/// </summary>
		/// <param name="propertyName"></param>
		private void ClearErrors(string propertyName)
		{
			// очищаем ошибки, которые существуют по этому свойству
			errors.Remove(propertyName);
			// вызываем обработчик
			if (ErrorsChanged != null)
			{
				ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
			}
		}



		// КОНСТРУКТОР КЛАССА
		//-----------------------------------
		public ViewModelMainWindow()
        {
            // инициализируем модель данный
            model_data = new MainModel();           
            
            // получаем перечисление юр.физ. лицо
            DictUrFizLico = model_data.GetUrFizLicoList();
            OnPropertyChanged("DictUrFizLico");
            
            // получаем список категорий ТС
            ListCategoriaTs = model_data.GetCategoriaTs();
            OnPropertyChanged("ListCategoriaTs");

            // получаем список сроков страхования ТС
            ListPeriodStrah = model_data.GetPeriodStrahTs();
            OnPropertyChanged("ListPeriodStrah");

            // получаем список периодов использования ТС
            ListPeriodUse = model_data.GetPeriodUseTs();
            OnPropertyChanged("ListPeriodUse");

            // получаем список регионов
            ListRegions = model_data.GetRegions();
            OnPropertyChanged("ListRegions");
			SelectedRegion = get_selected_region(1);
			OnPropertyChanged("SelectedRegion");

            // получаем список КБМ
            ListKbm = model_data.GetKbms();
            OnPropertyChanged("ListKbm");
		}
    }
}
