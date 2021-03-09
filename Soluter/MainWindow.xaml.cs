using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Soluter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Устанавливаем наш ViewModel
            var viewModel = new CalculatorViewModel(Resources);
            DataContext = viewModel;

            //Подписываем метод (изменения пропорции FontSize), на событие изменения окна
            SizeChanged += viewModel.refSizerWindow.OnWindowSizeChanged;
        }
    }

    /// <summary>
    /// Модель представления
    /// </summary>
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private SizerWindow _refSizerWindow;
        /// <summary>
        /// Класс для установки -FontSize-, для окна.
        /// </summary>
        public SizerWindow refSizerWindow { get { return _refSizerWindow; } }

        private EquationDataTemplate _selectedEquation;
        /// <summary>
        /// Свойство коллекции с уравнениями.
        /// </summary>
        public ObservableCollection<EquationDataTemplate> Equations { get; private set; }
        /// <summary>
        /// Текущее выбранное уравнение в списке.
        /// </summary>
        public EquationDataTemplate SelectedEquation
        {
            get { return _selectedEquation; }
            set
            {
                _selectedEquation = value;
                OnPropertyChanged("SelectedEquation");
            }
        }

        /// <summary>
        /// Конструктор ViewMovel.
        /// </summary>
        /// <param name="Resources">Укажите ресурсы MainWindow, для установки SizerWindow.</param>
        public CalculatorViewModel(ResourceDictionary Resources)
        {
            _refSizerWindow = ((SizerWindow)Resources["SizerKey"]);

            Equations = new ObservableCollection<EquationDataTemplate>()
            {
                new EquationDataTemplate("ax+by"+((char)8304).ToString()+"+c", (float[] mas) => {
                    return mas[0] * mas[3]+mas[1] + mas[2];
                }, 0),
                new EquationDataTemplate($"ax"+((char)0178).ToString()+"+by+c", (float[] mas) => {
                    return (float)( mas[0] * MathF.Pow(mas[3],2)+mas[1]*mas[4] + mas[2]);
                }, 1),
                new EquationDataTemplate("ax"+((char)0179).ToString()+"+by"+((char)0178).ToString()+"+c", (float[] mas) => {
                    return (float)( mas[0] * MathF.Pow(mas[3],3)+mas[1]*MathF.Pow(mas[4], 2) + mas[2]);
                }, 2),
                new EquationDataTemplate("ax⁴"+"+by"+((char)0179).ToString()+"+c", (float[] mas) => {
                     return (float)( mas[0] * MathF.Pow(mas[3],4)+mas[1]*MathF.Pow(mas[4], 3) + mas[2]);
                }, 3),
                new EquationDataTemplate("ax⁵+by⁴+c", (float[] mas) => {
                     return (float)( mas[0] * MathF.Pow(mas[3],5)+mas[1]*MathF.Pow(mas[4], 4) + mas[2]);
                }, 4)
            };

            _selectedEquation = Equations[0];
        }

        /// <summary>
        /// Событие для биндинга свойств.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Метод реализующий обновление свойств.
        /// </summary>
        /// <param name="prop"></param>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        
    }

}
