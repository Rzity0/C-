using System;

public class Money
{
    private uint rubles; // Поле для рублей
    private byte kopeks; // Поле для копеек

    // Конструктор
    public Money(uint rubles, byte kopeks)
    {
        if (kopeks >= 100)
            throw new ArgumentOutOfRangeException(nameof(kopeks));

        this.rubles = rubles;
        this.kopeks = kopeks;
    }

    // Свойства
    public uint Rubles
    {
        get => rubles;
        set => rubles = value;
    }

    public byte Kopeks
    {
        get => kopeks;
        set
        {
            if (value >= 100)
            {
                rubles += (uint)(value / 100);
                kopeks = (byte)(value % 100);
            }
            else
            {
                kopeks = value;
            }
        }
    }

    // Метод для вычитания
    public Money Subtract(Money other)
    {
        int totalKopeksThis = (int)(rubles * 100 + kopeks);
        int totalKopeksOther = (int)(other.rubles * 100 + other.kopeks);

        if (totalKopeksThis < totalKopeksOther)
            throw new InvalidOperationException("Результат вычитания не может быть меньше 0.");

        int resultKopeks = totalKopeksThis - totalKopeksOther;

        return new Money((uint)(resultKopeks / 100), (byte)(resultKopeks % 100));
    }

    // Переопределение ToString
    public override string ToString()
    {
        return $"{rubles} руб. {kopeks} коп.";
    }

    // Унарная операция ++
    public static Money operator ++(Money m)
    {
        m.Kopeks++;
        if (m.Kopeks >= 100)
        {
            m.Rubles++;
            m.Kopeks = 0;
        }
        return m;
    }

    // Унарная операция -
    public static Money operator -(Money m)
    {
        if (m.kopeks == 0)
        {
            if (m.rubles == 0)
                throw new InvalidOperationException("Невозможно вычесть копейку из нуля.");
            m.rubles--;
            m.kopeks = 99;
        }
        else
        {
            m.kopeks--;
        }
        return m;
    }

    // Неявное приведение к uint
    public static implicit operator uint(Money m)
    {
        return m.rubles;
    }

    // Явное приведение к double
    public static explicit operator double(Money m)
    {
        return m.rubles + m.kopeks / 100.0;
    }

    // Бинарные операции с беззнаковым целым числом (левостороннее)
    public static Money operator -(Money m, uint amount)
    {
        return m.Subtract(new Money(amount, 0));
    }

    // Бинарные операции с беззнаковым целым числом (правостороннее)
    public static Money operator -(uint amount, Money m)
    {
        return new Money(amount, 0).Subtract(m);
    }

    // Бинарные операции между двумя объектами Money
    public static Money operator -(Money m1, Money m2)
    {
        return m1.Subtract(m2);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // Ввод первой суммы денег
            Console.WriteLine("Введите первую сумму денег:");
            uint rubles1 = GetRubles("Рубли: ");
            byte kopeks1 = GetKopeks("Копейки: ");
            Money money1 = new Money(rubles1, kopeks1);

            // Ввод второй суммы денег
            Console.WriteLine("Введите вторую сумму денег:");
            uint rubles2 = GetRubles("Рубли: ");
            byte kopeks2 = GetKopeks("Копейки: ");
            Money money2 = new Money(rubles2, kopeks2);

            Console.WriteLine($"\nПервая сумма: {money1}");
            Console.WriteLine($"Вторая сумма: {money2}");

            // Вычитание
            Money result = money1 - money2;
            Console.WriteLine($"\nРезультат вычитания: {result}");

            // Тестирование унарной операции ++
            Console.WriteLine($"Исходная сумма: {money1}");
            money1++;
            Console.WriteLine($"После добавления копейки: {money1}");

            // Тестирование унарной операции -
            Console.WriteLine($"Сумма перед вычитанием копейки: {money2}");
            money2 = -money2;
            Console.WriteLine($"После вычитания копейки: {money2}");


            // Тестирование приведения типов
            uint rubles = (uint)money1; // Неявное приведение
            Console.WriteLine($"Количество рублей: {rubles}");

            // Тестирование приведения типов

            uint kopeks = (uint)kopeks1; // Неявное приведение
            Console.WriteLine($"Количество копеек: {kopeks}");

            Console.WriteLine($"Сумма: {money2}");
            double rublesDouble = (double)money2; // Явное приведение
            Console.WriteLine($"Сумма в рублях с копейками: {rublesDouble}");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static uint GetRubles(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (uint.TryParse(input, out uint rubles))
            {
                return rubles;
            }
            Console.WriteLine("Ошибка: введено некорректное число рублей. Попробуйте снова.");
        }
    }

    private static byte GetKopeks(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (byte.TryParse(input, out byte kopeks) && kopeks < 100)
            {
                return kopeks;
            }
            Console.WriteLine("Ошибка: введено некорректное число копеек. Копейки должны быть от 0 до 99.");
        }
    }
}