using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace Soluter
{

    /// <summary>
    /// Данный класс нужен для установки свойства - FontSize - всем текстовым объектам
    /// взависимости от размера экрана. Создаем объект в ресурсах, в ресурсах (в XAML).
    /// После чего в главном Grid, cсылаемся на него с помошью привязок.
    /// </summary>
    public class SizerWindow : INotifyPropertyChanged
    {
        private int _kForTextFont;
        public int kForTextFont
        {
            get { return _kForTextFont; }
            set
            {
                _kForTextFont = value;
                OnPropertyChanged("kForTextFont");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public override string ToString()
        {
            return "Size - " + kForTextFont.ToString();
        }

        /// <summary>
        /// Метод который нужно будет вызываться при изменение окна (event: MainWindow.SizeChanded)
        /// </summary>
        public void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Коэффициент, который позволит настроить, размер шрифта  
            //[впринцепе можно сразу вписать в одну строку, но так понятней (для разбора)]
            float percentK = 0.038f;
            float k = (float)(e.NewSize.Width < e.NewSize.Height ? e.NewSize.Height / e.NewSize.Width : 1);
            float new_kForSizerWindow = MathF.Sqrt(MathF.Pow((float)e.NewSize.Width, 2) + MathF.Pow((float)e.NewSize.Height, 2)) / k;

            kForTextFont = (int)(new_kForSizerWindow * percentK);
        }
    }

    /// <summary>
    /// Уравнение, в котором хранится текст для вывода на экран.
    /// Также хранится формула которую использует уравнение.
    /// </summary>
    public class EquationDataTemplate : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Изменение свойства - используется для привязки
        /// </summary>
        /// <param name="prop">Название свойства.</param>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            ToSolute();
        }

        /// <summary>
        /// Свойство для хранения Text - уравнения
        /// </summary>
        public string text { get; private set; }

        /// <summary>
        /// Метод (формула), для вычисления уравнения
        /// </summary>
        private Func<float[], float> func { get; set; }

        /// <summary>
        /// Конструктор уравнения
        /// </summary>
        /// <param name="text">Укажите текст который нужно вывести в View.</param>
        /// <param name="func">Укажите метод который будет вычисляться этим уравнением
        ///     принимать нужное количество параметров, и возвращать число.
        ///     !Иметь ввиду что в массиве переменные должны быть расположены: a,b,c,x,y .</param>
        /// <param name="degree">Укажите степень числа 10, для возможных значений -c-.</param>
        public EquationDataTemplate(string text, Func<float[], float> func, int degree)
        {
            this.text = text;
            this.func = func;

            for (int i = 1; i <= 5; i++)
                cVarItems.Add(i * (float)MathF.Pow(10, degree));

            cVar = cVarItems[0];

            AddStrokeToTable();

            ToSolute();

        }

        /// <summary>
        /// Имеет публичный модификатор, для тестирования формул.
        /// </summary>
        public float ToSolute(float a, float b, float c, float x, float y)
        {
            return func(new float[] { a, b, c, x, y });
        }
        /// <summary>
        /// Возвращает текст уравнения.
        /// </summary>
        public override string ToString() { return text; }

        private float _bVar;
        /// <summary>
        /// Свойство для работы с переменно -b- .
        /// </summary>
        public string bVar
        {
            get { return _bVar.ToString(); }
            set
            {
                if (Filter.ToFilterInputFloat(ref _bVar, value))
                    OnPropertyChanged("bVar");
            }
        }

        private float _aVar;
        private string _aVarLocal = "";
        /// <summary>
        /// Свойство для работы с переменной -a- .
        /// </summary>
        /// <remarks>Хотел сделать, что бы при записи в переменные дробных числе, 
        /// они изменялись-вычислялись сразу при записи (также как c int).
        /// Но это достаточно проблематично. Костыль получился, хотя иначе пока не представил как это сделать.
        /// Возможно инкапсулировать в структуре какой-то специальной для работы с -textbox- , который будет
        /// работать именно с числами. </remarks>
        public string aVar
        {
            get
            {
                if (_aVarLocal.Length > 0)
                    return _aVarLocal;
                else
                    return _aVar.ToString();
            }
            set
            {
                float item = _aVar;
                if (Filter.ToFilterInputFloat(ref _aVar, value))
                {
                    if (_aVar != item)
                    {
                        _aVarLocal = "";
                        OnPropertyChanged("aVar");
                    }
                }
                else
                    if (_aVarLocal.Length == 0)
                    _aVarLocal = value;
            }
        }

        // Работа с переменной -c-

        /// <summary>
        /// Список для значений переменной -c- .
        /// </summary>
        public ObservableCollection<FloatProperty> cVarItems { get; private set; } = new ObservableCollection<FloatProperty>();
        private FloatProperty _cVar;
        /// <summary>
        /// Свойство выбранной переменной, -c- .
        /// </summary>
        public FloatProperty cVar
        {
            get { return _cVar; }
            set
            {
                _cVar = value;
                OnPropertyChanged("cVar");
            }
        }

        /// <summary>
        /// Список строк, для таблицы.
        /// </summary>
        public ObservableCollection<TableCell> tableCells { get; private set; } = new ObservableCollection<TableCell>();

        /// <summary>
        /// Метод для удаления строки ячеек в таблице.
        /// </summary>
        public void AddStrokeToTable() {tableCells.Add(new TableCell(ToSolute)); OnPropertyChanged("tableCells"); }
        /// <summary>
        /// Метод для добавления строки ячеек в таблице.
        /// </summary>
        public void RemoveStrokeToTable() { if (tableCells.Count > 0) { tableCells.Remove(tableCells[0]); OnPropertyChanged("tableCells"); } }

        /// <summary>
        /// Перегрузка метода ToSolute. Создан по причине (преполагаемой) эффективности использования памяти...
        /// </summary>
        /// <param name="x">Указывается значение -x- .</param>
        /// <param name="y">Указывается значение -y- .</param>
        /// <returns></returns>
        ///<remarks>Пояснение: delegate хранятся, как объекты ссылающиеся на методы -> таким образом, предполагаю, что
        /// он не дублирует код в памяти для каждого -formula- , внутри объекта TableCell, в отличие от того
        /// если бы мы использовали -lambda- выражение.</remarks>
        private float ToSolute(float x, float y)
        {
            return ToSolute(_aVar, _bVar, _cVar.Value, x, y);
        }

        /// <summary>
        /// Для просчета каждой ячейки в таблице, при изменение переменных для всех строк в таблице.
        /// </summary>
        private void ToSolute()
        {
            foreach (var cell in tableCells)
                cell.ToSolute();
        }

        private ButtonCommand addCommand;
        /// <summary>
        /// Комманда для добавления строк в таблицу.
        /// </summary>
        public ButtonCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new ButtonCommand(obj =>
                {
                    AddStrokeToTable();
                }));
            }
        }
        
        private ButtonCommand removeCommand;
        /// <summary>
        /// Комманда для удаления строк в таблице.
        /// </summary>
        public ButtonCommand RemoveCommand
        {
            get
            {
                return removeCommand ?? (removeCommand = new ButtonCommand(obj =>
                {
                    RemoveStrokeToTable();
                }));
            }
        }
    }

    /// <summary>
    /// Ячейка для таблицы.
    /// </summary>
    public class TableCell : INotifyPropertyChanged
    {
        private Func<float, float, float> formula;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

            if (prop != "varF") { ToSolute(); }
        }

        /// <summary>
        /// Метод позволяет высчитать значение для -varF- , из вне.
        /// </summary>
        public void ToSolute()
        {
            varF = formula?.Invoke(_varX, _varY).ToString();
        }

        /// <summary>
        /// Конструктор ячейки для таблицы.
        /// </summary>
        /// <param name="formula">Укажите метод (формулу для вычисления).</param>
        public TableCell(Func<float, float, float> formula)
        {
            this.formula = formula;
        }

        private float _varX;
        private float _varY;
        private float _varF;

        /// <summary>
        /// Свойство для работы с переменной -X- .
        /// </summary>
        public string varX
        {
            get { return _varX.ToString();}
            set {
                if (Filter.ToFilterInputFloat(ref _varX, value))
                    OnPropertyChanged("varX");
            }
        }

        /// <summary>
        /// Свойство для работы с переменной -Y- .
        /// </summary>
        public string varY
        {
            get { return _varY.ToString(); }
            set {
                if (Filter.ToFilterInputFloat(ref _varY, value))
                    OnPropertyChanged("varY");
            }
        }

        /// <summary>
        /// Свойство для работы, с результатом функции -F-
        /// </summary>
        public string varF
        {
            get { return _varF.ToString(); }
            set {
                _varF = float.Parse(value);
                OnPropertyChanged("varF");
            }
        }
    }

    /// <summary>
    /// Структура Int32, для использования чисел в списке, и binding свойств для XAML.
    /// </summary>
    public struct FloatProperty
    {
        private float _val;
        public float Value
        {
            get { return _val; }
            set
            {
                _val = value;
            }
        }
        public FloatProperty(float val) { _val = val; }
        public static explicit operator float(FloatProperty val) { return val.Value; }
        public static implicit operator FloatProperty(float val) { return new FloatProperty(val); }

        public override string ToString()
        {
            return _val.ToString();
        }
    }

}
