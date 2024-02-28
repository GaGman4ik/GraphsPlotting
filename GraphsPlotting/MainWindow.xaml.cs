using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZedGraph;


namespace GraphsPlotting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        //double minX = 100000;
        //double maxX = -100000;

        //double minY = 100000;
        //double maxY = -100000;

        //double MapFromPixel(double pixelV, double pixelMax, double minV) =>
        //    minV + (pixelV / pixelMax) * (maxX - minX);

        //double MapToPixel(double v, double minV, double maxV, double pixelMax) =>
        //    (v - minV) / (maxV - minV) * pixelMax;

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {

            //minX = Double.Parse(TextBoxDownBound.Text);
            //maxX = Double.Parse(TextBoxUpBound.Text);
            Parcer parser = new Parcer(TextBoxInput.Text);
            var result = parser.Parse();
            //var pixelWidth = Graph.ActualWidth;
            //var pixelHeight = Graph.ActualHeight;
            //PointCollection points = new PointCollection((int)pixelWidth + 1);
            //for (double pixelX = minX; pixelX < maxX; pixelX += (maxX - minX) / pixelWidth)
            //{
            //    var y = parser.Calculate(result, pixelX);
            //    minY = Math.Min(minY, y);
            //    maxY = Math.Max(maxY, y);
            //}
            //for (int pixelX = 0; pixelX < pixelWidth; pixelX++)
            //{
            //    var x = MapFromPixel(pixelX, pixelWidth, minX);
            //    var y = parser.Calculate(result, x);
            //    var pixelY = pixelHeight - MapToPixel(y, minY, maxY, pixelHeight);
            //    points.Add(new System.Windows.Point(pixelX, pixelY));
            //}
            //Graph.Points = points;

            // Получим панель для рисования
            GraphPane pane = zgc.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList list = new PointPairList();

            double xmin = Double.Parse(TextBoxDownBound.Text);
            double xmax = Double.Parse(TextBoxUpBound.Text);

            // Заполняем список точек
            for (double x = xmin; x <= xmax; x += (xmax - xmin)/100000)
            {
                // добавим в список точку
                list.Add(x, parser.Calculate(result, x));
            }

            // Создадим кривую с названием "Sinc",
            // которая будет рисоваться голубым цветом (Color.Blue),
            // Опорные точки выделяться не будут (SymbolType.None)
            LineItem myCurve = pane.AddCurve("Sinc", list, System.Drawing.Color.Blue, SymbolType.None);

            // Вызываем метод AxisChange (), чтобы обновить данные об осях.
            // В противном случае на рисунке будет показана только часть графика,
            // которая умещается в интервалы по осям, установленные по умолчанию
            zgc.AxisChange();

            // Обновляем график
            zgc.Invalidate();
        }

        private void zedGraph_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Сюда будут записаны координаты в системе координат графика
            double x, y;

            // Пересчитываем пиксели в координаты на графике
            // У ZedGraph есть несколько перегруженных методов ReverseTransform.
            zgc.GraphPane.ReverseTransform(e.Location, out x, out y);

            // Выводим результат
            string text = string.Format("X: {0};    Y: {1}", x, y);
            zgc.Text = text;
        }


        private void TextBoxInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
            
        }
    }
}