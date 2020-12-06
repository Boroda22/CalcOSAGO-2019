using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CalcOSAGO
{
	/// <summary>
	/// Класс служит для экспорта данных в графический файл
	/// </summary>
	public class ExportDataToImage
	{
		/// <summary>
		/// Процедура экспорта данных в формат .png
		/// </summary>
		/// <param name="model">модель данных</param>
		public void ExportToFile(MainModel model_data)
		{
			SaveFileDialog save_dialog = new SaveFileDialog();
			save_dialog.Filter = "Файлы картинок(*.png)| *.png";
			save_dialog.AddExtension = true;
			string tmp_title = "Экспорт даных расчета: " + model_data.fio_strah;
			// заголовок окна
			save_dialog.Title = tmp_title;
			// устанавливаем по умолчанию имя файла
			save_dialog.FileName = model_data.fio_strah;
			if (save_dialog.ShowDialog() == true)
			{
				
				// Создать визуальный элемент для страницы
				DrawingVisual visual = new DrawingVisual();
				// Получить контекст рисования
				using (DrawingContext dc = visual.RenderOpen())
				{
					// Определить текст, который необходимо печатать
					string company = "ООО \"Поволжский страховой альянс\"";
					FormattedText text_company = new FormattedText(company,
						System.Globalization.CultureInfo.CurrentCulture,
						FlowDirection.LeftToRight,
						new Typeface("Calibri"), 20, Brushes.Black);
					// ЗАГОЛОВОК
					string result_caption = "Результаты расчета страховой премии";
					FormattedText text_result_caption = new FormattedText(result_caption,
						System.Globalization.CultureInfo.CurrentCulture,
						FlowDirection.LeftToRight,
						new Typeface("Calibri"), 24, Brushes.Black);
					// ФИО СТРАХОВАТЕЛЯ
					string fio = "Страхователь: ";
					fio = fio + model_data.fio_strah;
					FormattedText text_fio = new FormattedText(fio,
						System.Globalization.CultureInfo.CurrentCulture,
						FlowDirection.LeftToRight,
						new Typeface("Calibri"), 24, Brushes.Black);
					// Указать максимальную ширину, в пределах которой выполнять перенос текста, 
					//text_fio.MaxTextWidth = print_dialog.PrintableAreaWidth / 2;
					// Получить размер выводимого текста.
					//Size textSize = new Size(text_fio.Width, text_fio.Height);
					// Найти верхний левый угол, куда должен быть помещен текст. 
					//double margin = 96 * 0.25;

					// КОЭФФИЦИЕНТЫ И РЕЗУЛЬТАТЫ РАСЧЕТА
					string result_formula = model_data.GetResultFormula();
					FormattedText text_result_formula = new FormattedText(result_formula,
						System.Globalization.CultureInfo.CurrentCulture,
						FlowDirection.LeftToRight,
						new Typeface("Calibri"), 22, Brushes.Black);
					// СТРАХОВАЯ ПРЕМИЯ
					decimal result_calc = model_data.CalcStrahPrem();
					// форматированный вывод с разделением на тысячи
					var numberFormatInfo = new System.Globalization.CultureInfo("ru-Ru", false).NumberFormat;
					string strah_prem = "Страховая премия: ";
					strah_prem = strah_prem + result_calc.ToString("N", numberFormatInfo).Replace(",", ".") + " руб.";
					FormattedText text_result_strah_prem = new FormattedText(strah_prem,
						System.Globalization.CultureInfo.CurrentCulture,
						FlowDirection.LeftToRight,
						new Typeface("Calibri"), 24, Brushes.Black);

					Point point_company = new Point(450, 80);
					Point point_text_caption = new Point(220, 180);
					Point point_fio = new Point(50, 280);
					Point point_result = new Point(50, 330);
					Point point_prem = new Point(50, 560);
					// Нарисовать содержимое
					dc.DrawText(text_company, point_company);
					dc.DrawText(text_result_caption, point_text_caption);
					dc.DrawText(text_fio, point_fio);
					dc.DrawText(text_result_formula, point_result);
					dc.DrawText(text_result_strah_prem, point_prem);
					dc.Close();

					// Создаём изображение и отрисовываем в него DrawingVisual.
					var Bitmap = new RenderTargetBitmap(1024, 960, 96, 96, PixelFormats.Pbgra32);
					Bitmap.Render(visual);
					// Создаём кодировщик и добавляем изображение в его коллекцию фреймов.
					var encoder = new PngBitmapEncoder();
					encoder.Frames.Add(BitmapFrame.Create(Bitmap));
					//Сохраняем изображение.
					using (Stream fileStream = File.Create(save_dialog.FileName))
					{
						encoder.Save(fileStream);
					}
				}				
			}
		}
		/// <summary>
		/// Конструктор класса
		/// </summary>
		public ExportDataToImage()
		{

		}
	}
}
