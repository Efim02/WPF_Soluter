using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Soluter
{
    /// <summary>
    /// Класс для фильтрации текста в число
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Преобразование из текста в Int32 число
        /// </summary>
        public static int ToFilterInt(int oldValue, string newValue)
        {
            int result;
            if (newValue == "")
                return 0;
            if (!Int32.TryParse(newValue, out result))
                return oldValue;
            return result;
        }
        /// <summary>
        /// Преобразование из текста в Float число
        /// </summary>
        /// <param name="newValue">Укажите новое значение.</param>
        /// <param name="variable">Укажите ссылку на переменную, в которую нужно установить значение.</param>
        /// <returns>Возвращаем логическое значение, нужно ли изменять в View.</returns>
        public static bool ToFilterInputFloat(ref float variable, string newValue)
        {
            if (newValue == "")
            { variable = 0; return true; }

            char replaceText = '.';
            char replaceFloat = ',';
            if (System.Globalization.CultureInfo.CurrentCulture.ToString() == "en-EN")
            {
                replaceText = ',';
                replaceFloat = '.';
            }

            float result;
            if (float.TryParse(newValue = newValue.Replace(replaceText, replaceFloat), out result))
            {
                if (newValue.Split(replaceFloat).Length > 2)
                {
                    return true;
                }
                else if (newValue[newValue.Length - 1] == replaceFloat) return false;
                else if (newValue.Split(replaceFloat)[0].Length == 0) return false;

                variable = result;
                return true;
            }
            else
            {
                return true;
            }

        }
    }
    /// <summary>
    /// Класс для создания собставенных команд, реализации привязки
    /// </summary>
    public class ButtonCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        /// <summary>
        /// Вызывается при изменении условий, с указанием может ли команда выполняться
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        /// <summary>
        /// Конструктор для создания команды.
        /// </summary>
        /// <param name="execute">Метод выполняющий команду</param>
        /// <param name="canExecute">Метод с проверкой, может ли команда выполняться</param>
        public ButtonCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        /// <summary>
        /// Метод, позволяющий понять может ли команда выполняться.
        /// </summary>
        public bool CanExecute(object parametr) { return canExecute == null || canExecute(parametr); }
        /// <summary>
        /// Выполняет команду.
        /// </summary>
        public void Execute(object parametr) { execute(parametr); }
    }
}
