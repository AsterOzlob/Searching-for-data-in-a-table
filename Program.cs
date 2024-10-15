/* Вариант: 11
Разработать консольное приложение, осуществляющее поиск в таблице
данных деятельности ломбарда. Человек обращается в ломбард в том случае, 
если ему срочно нужны деньги. В ломбарде с клиентом заключается договор. В нем
оговариваются следующие условия: до какого срока выкуп вещи возможен без
процентов, с какого времени будет взыматься процент, по истечении какого
срока выкуп вещи невозможен, и она поступает в собственность ломбарда. 
Невыкупленные вещи ломбард выставляет на продажу
*/

using System;
using System.Collections;
using System.Collections.Generic;

//Класс, реализующий договор
class Contract
{
    //Атрибуты класса
    private string name; //Наименование предмета
    private double amount; //Цена предмета
    private DateTime noProcentDate; //До какого срока нет процентов
    private DateTime expiryDate; //Срок, после которого выкуп невозможен

    //get, set методы
    public string Name { get => name; set => name = value; }
    public double Amount { get => amount; set => amount = value; }
    public DateTime NoProcentDate { get => noProcentDate; set => noProcentDate = value; }
    public DateTime ExpiryDate { get => expiryDate; set => expiryDate = value; }

    public Contract(string name, double amount, DateTime noProcentDate, DateTime expiryDate)
    {
        Name = name;
        Amount = amount;
        NoProcentDate = noProcentDate;
        ExpiryDate = expiryDate;
    }

    //Переопределение метода ToString
    public override string ToString()
    {
        return $"Название предмета: {Name}\n" +
            $"Сумма залога: {Amount}\n" +
            $"Дата окончания беспроцентного периода: {noProcentDate}\n" +
            $"Дата окончания залога: {ExpiryDate}\n";
    }
}

//Класс, реализующий методы поиска данных в таблице
class SearchMethods
{
    //Метод последовательного поиска
    //Аргументы: список договоров, наименование искомого предмета
    //Возвращает: найденный договор
    public static Contract? IncrementalSearch(List<Contract> contracts, string name)
    {
        Contract foundContract = contracts.Find(x => x.Name.ToLower() == name.ToLower());

        return foundContract;
    }

    //Метод бинарного поиска
    //Аргументы: список договоров, наименование искомого предмета,
    //Аргументы: границы массива/области поиска (left, right)
    //Возвращает: найденный договор
    public static Contract? BinarySearch(List<Contract> contracts, string name,
        int left, int right)
    {
        while (left <= right)
        {
            //Индекс среднего элемента
            var middle = (left + right) / 2;

            if (name == contracts[middle].Name)
            {
                return contracts[middle];
            }
            else if (string.Compare(name, contracts[middle].Name) < 0)
            {
                //Сужаем рабочую область с правой стороны
                right = middle - 1;
            }
            else
            {
                //Сужаем рабочую область с левой стороны
                left = middle + 1;
            }
        }

        return null;
    }

    //Метод поиска с помощью хэш-таблицы
    //Аргументы: список договор, наименование искомого предмета
    //Возвращает: найденный договор
    public static Contract? HashTableSearch(List<Contract> contracts, string name)
    {
        Hashtable contractTable = new();

        //Заполнение хэш-таблицы
        foreach (Contract contract in contracts)
        {
            contractTable.Add(contract.Name, contract);
        }

        //Поиска данных в хэщ-таблице
        if(contractTable.ContainsKey(name))
        {
            return (Contract)contractTable[name];
        }

        return null;
    }
}


class Program
{
    static void Main()
    {
        //Заголовок программы
        Console.WriteLine("Программа поиска данных в таблице\n");

        //Список, где будут храниться договоры
        List<Contract> contracts = new();

        //Формирование базовых данных (для тестирования программы)
        for (int i = 0; i < 10; i++)
        {
            Random random = new();
            string name = "Предмет " + (i + 1).ToString();
            double amount = random.Next(100, 1000);
            DateTime noProcentDate = DateTime.Now.AddDays(random.Next(1, 30));
            DateTime expiryDate = DateTime.Now.AddMonths(random.Next(1, 12));

            contracts.Add(new Contract(name, amount, noProcentDate, expiryDate));
        }

        //Цикл программы
        while (true)
        {
            //Меню пользователя
            Console.WriteLine("<<Меню>>");
            Console.WriteLine("1 - Добавить данные");
            Console.WriteLine("2 - Вывести данные");
            Console.WriteLine("3 - Последовательный поиск");
            Console.WriteLine("4 - Бинарный поиск");
            Console.WriteLine("5 - Поиск с помощью хэш-таблицы");
            Console.WriteLine("6 - Завершение работы\n");

            int choice = GetChoice(1, 6);

            switch(choice)
            {
                case 1: //Добавление новой вещи
                    Console.WriteLine("\n<<Добавление вещи>>\n");

                    Console.Write("Введите название вещи: ");
                    string name = Console.ReadLine();
                    Console.Write("Введите сумму залога: ");
                    double amount = Convert.ToDouble(Console.ReadLine());
                    Console.WriteLine("Введите дату окончания беспроцентного периода (гггг-мм-дд): ");
                    DateTime noProcentDate = Convert.ToDateTime(Console.ReadLine());
                    Console.WriteLine("Введите дату окончания залога (гггг-мм-дд): ");
                    DateTime expiryDate = Convert.ToDateTime(Console.ReadLine());

                    contracts.Add(new Contract(name, amount, noProcentDate, expiryDate));

                    break;

                case 2: //Вывод заложенных вещей
                    if(contracts.Any())
                    {
                        Console.WriteLine("\n<<Таблица заложенных вещей>>\n");

                        foreach (Contract contract in contracts)
                        {
                            Console.WriteLine(contract);
                        }

                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Таблица пуста\n");
                    }

                    break;

                case 3: //Последовательный поиск
                    Console.WriteLine("\n<<Последовательный поиск в таблице>>\n");

                    Console.Write("Введите название предмета: ");
                    name = Console.ReadLine();

                    Contract? foundContract = SearchMethods.IncrementalSearch(contracts, name);

                    if(foundContract == null)
                    {
                        Console.WriteLine("Предмет не найден\n");
                    }
                    else
                    {
                        Console.WriteLine($"\n{foundContract}");
                    }

                    break;

                case 4: //Бинарный поиск
                    Console.WriteLine("\n<<Бинарный поиск в таблице>>\n");

                    //Сортировка списка заложенных вещей по названию
                    contracts.Sort((x, y) => string.Compare(x.Name, y.Name));

                    Console.Write("Введите название предмета: ");
                    name = Console.ReadLine();

                    foundContract = SearchMethods.BinarySearch(contracts, name, 0, contracts.Count);

                    if (foundContract == null)
                    {
                        Console.WriteLine("Предмет не найден\n");
                    }
                    else
                    {
                        Console.WriteLine($"\n{foundContract}");
                    }

                    break;

                case 5: //Поиск с помощью хэш-таблицы
                    Console.WriteLine("\n<<Поиск с помощью хэш-таблицы>>\n");

                    Console.Write("Введите название предмета: ");
                    name = Console.ReadLine();

                    foundContract = SearchMethods.HashTableSearch(contracts, name);

                    if (foundContract == null)
                    {
                        Console.WriteLine("Предмет не найден\n");
                    }
                    else
                    {
                        Console.WriteLine($"\n{foundContract}");
                    }

                    break;
                    
                case 6: //Завершение работы
                    Console.WriteLine("\nЗавершение работы");
                    return;
            }
        }
    }

    //Функция проверки корректности выбранной операции
    //Аргументы: диапазон допустимых значений
    //Возвращает выбранное значение
    static int GetChoice(int min, int max)
    {
        int choice = 0;
        bool isCorrect = false;

        while (!isCorrect)
        {
            Console.Write($"Выберите операцию ({min}-{max}): "); ;

            isCorrect = int.TryParse(Console.ReadLine(), out choice);

            if (isCorrect && (choice < min || choice > max))
            {
                isCorrect = false;
            }

            if (!isCorrect)
            {
                Console.WriteLine("Вы ввели некорректное значение. Попробуйте снова.\n");
            }
        }

        return choice;
    }
}

