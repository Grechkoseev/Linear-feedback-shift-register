namespace Lab_2;

internal class Lfsr
{
    private readonly int[] _indices;
    private readonly int[] _register = new int[8];
    private readonly int _initialValue;

    private int SequenceLength { get; set; }
    private int EvenCounter { get; set; }
    private int OddCounter { get; set; }
    private int ZeroCounter { get; set; }
    private int OneCounter { get; set; }
    private List<int> BinaryOutputSequence { get; }
    private List<int> ByteSequenceList { get; }

    public Lfsr(int[] indices, int initialValue)
    {
        _indices = new int[indices.Length];

        for (var i = 0; i < _indices.Length; i++)
        {
            _indices[i] = indices[i];
        }

        _initialValue = initialValue;
        BinaryOutputSequence = new List<int>();
        ByteSequenceList = new List<int>();

    }

    /// <summary>
    /// Помещаем число в 8-битный регистр
    /// </summary>
    /// <param name="number"></param>
    public void PutNumberToRegister(int number)
    {
        var binaryString = Convert.ToString(number, 2);
        var arr = binaryString.ToCharArray().Select(i => int.Parse(i.ToString())).ToArray();

        for (int i = 0, j = 0; i < _register.Length; i++)
        {
            if (arr.Length >= _register.Length - i)
            {
                _register[i] = arr[j];
                j++;
            }
            else
            {
                _register[i] = 0;
            }
        }
    }

    /// <summary>
    /// Вывод числа, находящегося в регистре
    /// </summary>
    public void PrintNumberFromRegister()
    {
        ConvertRegisterToNumber(out var registerNumber);
        Console.Write("{0} ", registerNumber);
    }

    /// <summary>
    /// Конвертация регистра в число
    /// </summary>
    /// <param name="number"></param>
    private void ConvertRegisterToNumber(out int number)
    {
        var stringRegister = string.Concat(_register);
        number = Convert.ToInt32(stringRegister, 2);
    }

    /// <summary>
    /// Вычисляем значение регистра при очереднйо итерации
    /// </summary>
    public void CalculateNextSequenceMember()
    {
        BinaryOutputSequence.Add(_register[^1]);
        ConvertRegisterToNumber(out var number);
        var firstBit = _indices.Aggregate(0, (current, index) => current ^ _register[index]);
        PutNumberToRegister(number / 2);
        _register[0] = firstBit;
        SequenceLength++;
    }

    /// <summary>
    /// Проверка равенства текущего значения регистра и начального
    /// </summary>
    /// <returns></returns>
    public bool IsCurrentSequenceMemberEqualsInitialNumber()
    {
        ConvertRegisterToNumber(out var currentNumber);

        return currentNumber == _initialValue;
    }

    /// <summary>
    /// Вывод длины псевдослучайной последовательности в битах
    /// </summary>
    public void PrintSequenceLength()
    {
        Console.WriteLine("\n\nДлина периодна псевдослучайной последовательности равна {0} бит", SequenceLength);
    }

    /// <summary>
    /// Вывод псевдослучайной последовательности в битах
    /// </summary>
    public void PrintBinaryOutputSequence()
    {
        Console.WriteLine("\n\nВыходная псевдослучайная последовательность бит: "); 
        foreach (var bit in BinaryOutputSequence)
        {
            Console.Write("{0} ", bit );
        }
    }

    /// <summary>
    /// Вывод однобайтового предствления выходной последовательности
    /// </summary>
    public void PrintByteSequenceRepresentation()
    {
        var byteSequenceCount = 0;

        do
        {
            if (BinaryOutputSequence.Count - byteSequenceCount <= 8)
            {
                ByteSequenceList.Add(ConvertListToNumber(BinaryOutputSequence.GetRange(byteSequenceCount,
                    BinaryOutputSequence.Count - byteSequenceCount)));
                byteSequenceCount += BinaryOutputSequence.Count;
            }
            else
            {
                ByteSequenceList.Add(ConvertListToNumber(BinaryOutputSequence.GetRange(byteSequenceCount, 8)));
                byteSequenceCount += 8;
            }
        } while (BinaryOutputSequence.Count - byteSequenceCount > 0);

        Console.WriteLine("\nОднобайтовое представление выходной последовательности:");

        foreach (var byteSequenceNumber in ByteSequenceList)
        {
            Console.Write("{0} ", byteSequenceNumber);
        }
    }

    /// <summary>
    /// Конвертация листа бит в число
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private static int ConvertListToNumber(List<int> list)
    {
        var stringList = string.Concat(list);
        return Convert.ToInt32(stringList, 2);
    }

    /// <summary>
    /// Вычисление количества четных и нечетных элементов в однобайтовом представлении
    /// </summary>
    public void CalculateEvenOddElements()
    {
        foreach (var number in ByteSequenceList)
        {
            switch (number % 2)
            {
                case 0:
                    EvenCounter++;
                    break;
                case 1:
                    OddCounter++;
                    break;
            }
        }
    }

    /// <summary>
    /// Вычисление количества нулей и единиц в битовом представлении
    /// </summary>
    public void CalculateZerosOnesElements()
    {
        foreach (var bit in BinaryOutputSequence)
        {
            switch (bit)
            {
                case 0:
                    ZeroCounter++;
                    break;
                case 1:
                    OneCounter++;
                    break;
            }
        }
    }

    /// <summary>
    /// Вывод количества четных и нечетных элементов в однобайтовом представлении
    /// </summary>
    public void PrintEvenOddCounters()
    {
        Console.WriteLine("\n\nКоличество четных элементов последовательности {0}", EvenCounter);
        Console.WriteLine("Количество нечетных элементов последовательности {0}", OddCounter);
    }

    /// <summary>
    /// Вывод количества нулей и единиц в битовом представлении
    /// </summary>
    public void PrintZerosOnesCounters()
    {
        Console.WriteLine("\nКоличество нулей в битовом представлении последовательности {0}", ZeroCounter);
        Console.WriteLine("Количество единиц в битовом представлении последовательности {0}", OneCounter);
    }
}