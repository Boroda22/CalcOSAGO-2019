using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CalcOSAGO
{
    /// <summary>
    /// Класс служит печати данных
    /// </summary>
    public class PrintData
    {
        /// <summary>
        /// Процедура вызова диалога печати и печать документа
        /// </summary>
        public void Print(MainModel model_data)
        {
            PrintDialog print_dialog = new PrintDialog();
            if (print_dialog.ShowDialog() == true)
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
                        new Typeface("Calibri"), 20, Brushes.Black);
                    // Указать максимальную ширину, в пределах которой выполнять перенос текста, 
                    //text_fio.MaxTextWidth = print_dialog.PrintableAreaWidth / 2;
                    // Получить размер выводимого текста.
                    //Size textSize = new Size(text_fio.Width, text_fio.Height);
                    // Найти верхний левый угол, куда должен быть помещен текст. 
                    //double margin = 96 * 0.25;
                    Point point_company = new Point(450, 80);
                    Point point_text_caption = new Point(220, 180);
                    Point point_fio = new Point(50, 280);
                    // Нарисовать содержимое
                    dc.DrawText(text_company, point_company);
                    dc.DrawText(text_result_caption, point_text_caption);
                    dc.DrawText(text_fio, point_fio);

                }
                // Напечатать визуальный элемент. 
                print_dialog.PrintVisual(visual, "Печать результатов расчета");
            }
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public PrintData()
        {

        }
    }
}
