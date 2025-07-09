using SqlKata.Compilers;

namespace Monopoly_Test_v2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] options = { "Просмотреть паллеты", "Отобразить палеты с наибольшим сроком годности\n" +
                    "  отсортированные по возрастанию объёма",
                "Отобразить группы паллет", "Просмотреть коробки", "Добавить паллету", "Добавить коробку", "Выход" };
            int selectedIndex = 0;
            GetData getData = new GetData();
            InsertData insertData = new InsertData();
            Menu menu = new Menu();

            bool isWorks = true;

            ConsoleKey key;

            while (isWorks)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Выберите опцию (стрелками ↑ ↓, Enter для выбора):\n");

                    for (int i = 0; i < options.Length; i++)
                    {
                        if (i == selectedIndex)
                        {
                            // Подсветка текущего выбора
                            Console.ForegroundColor = ConsoleColor.Green;
                        }

                        Console.WriteLine($"  {options[i]}");

                        // Сброс цвета
                        Console.ResetColor();
                    }

                    key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.UpArrow)
                    {
                        selectedIndex = (selectedIndex == 0) ? options.Length - 1 : selectedIndex - 1;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        selectedIndex = (selectedIndex == options.Length - 1) ? 0 : selectedIndex + 1;
                    }

                } while (key != ConsoleKey.Enter);

                Console.Clear();
                Console.WriteLine($"Вы выбрали: {options[selectedIndex]}\n");

                switch (selectedIndex)
                {
                    case 0:
                        Console.WriteLine("Загрузка паллет...");

                        var pallets = getData.GetPallets().Result;

                        if (pallets != null)
                            menu.PalletDialogue(pallets);
                        break;
                    case 1:
                        Console.WriteLine("Загрузка топ-3 паллет...");

                        pallets = getData.GetPallets().Result;

                        if (pallets != null)
                        {
                            // Сортировка по ExpirationDate от самой дальней даты к ближайшей
                            pallets = pallets
                                .OrderByDescending(pallet => pallet.ExpirationDate ?? DateTime.MinValue)
                                .ToList();

                            menu.ShowTopPallets(pallets);
                        }
                        break;
                    case 2:
                        Console.WriteLine("Загрузка групп паллет...");

                        pallets = getData.GetPallets().Result;

                        if (pallets != null)
                            menu.ShowGroupedPallets(pallets); // Заменяем PalletDialogue на ShowGroupedPallets
                        break;
                    case 3:
                        Console.WriteLine("Загрузка коробок...");

                        var boxes = getData.GetBoxes().Result;

                        if (boxes != null)
                            menu.BoxDialogue(boxes);
                        break;
                    case 4:
                        // Добавление паллеты
                        Pallet newPallet = new Pallet();

                        Console.WriteLine("Вы хотите использовать стандартные значения для паллеты? (y/n):");
                        string? useDefaults = Console.ReadLine()?.Trim().ToLower();

                        if (useDefaults == "y" || useDefaults == "н")
                        {
                            newPallet.Width = 80;
                            newPallet.Height = 14.4;
                            newPallet.Depth = 120;
                            newPallet.CreatedAt = DateTime.Now;
                        }
                        else
                        {
                            Console.WriteLine("Введите ширину паллеты (см):");
                            double width;
                            while (!double.TryParse(Console.ReadLine(), out width) || width <= 0)
                            {
                                Console.WriteLine("Некорректное значение. Введите положительное число для ширины:");
                            }
                            newPallet.Width = width;

                            Console.WriteLine("Введите высоту паллеты (см):");
                            double height;
                            while (!double.TryParse(Console.ReadLine(), out height) || height <= 0)
                            {
                                Console.WriteLine("Некорректное значение. Введите положительное число для высоты:");
                            }
                            newPallet.Height = height;

                            Console.WriteLine("Введите глубину паллеты (см):");
                            double depth;
                            while (!double.TryParse(Console.ReadLine(), out depth) || depth <= 0)
                            {
                                Console.WriteLine("Некорректное значение. Введите положительное число для глубины:");
                            }
                            newPallet.Depth = depth;

                            Console.WriteLine("Введите дату создания паллеты (в формате ГГГГ-ММ-ДД):");
                            DateTime createdAt;
                            while (!DateTime.TryParse(Console.ReadLine(), out createdAt))
                            {
                                Console.WriteLine("Некорректное значение. Введите дату в формате ГГГГ-ММ-ДД:");
                            }
                            newPallet.CreatedAt = createdAt;
                        }

                        var result = insertData.AddPallet(newPallet).Result;
                        if (result)
                            Console.WriteLine("Паллета успешно добавлена!");
                        else
                            Console.WriteLine("Ошибка при добавлении паллеты.");

                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                    case 5:
                        // Добавление коробки
                        Box newBox = new Box();

                        Console.WriteLine("Введите ширину коробки (см):");
                        double boxWidth;
                        while (!double.TryParse(Console.ReadLine(), out boxWidth) || boxWidth <= 0)
                        {
                            Console.WriteLine("Некорректное значение. Введите положительное число для ширины:");
                        }
                        newBox.Width = boxWidth;

                        Console.WriteLine("Введите высоту коробки (см):");
                        double boxHeight;
                        while (!double.TryParse(Console.ReadLine(), out boxHeight) || boxHeight <= 0)
                        {
                            Console.WriteLine("Некорректное значение. Введите положительное число для высоты:");
                        }
                        newBox.Height = boxHeight;

                        Console.WriteLine("Введите глубину коробки (см):");
                        double boxDepth;
                        while (!double.TryParse(Console.ReadLine(), out boxDepth) || boxDepth <= 0)
                        {
                            Console.WriteLine("Некорректное значение. Введите положительное число для глубины:");
                        }
                        newBox.Depth = boxDepth;

                        Console.WriteLine("Введите вес коробки (кг):");
                        double boxWeight;
                        while (!double.TryParse(Console.ReadLine(), out boxWeight) || boxWeight <= 0)
                        {
                            Console.WriteLine("Некорректное значение. Введите положительное число для веса:");
                        }
                        newBox.Weight = boxWeight;

                        Console.WriteLine("Введите дату производства коробки (в формате ГГГГ-ММ-ДД) или оставьте пустым:");
                        string? productionDateInput = Console.ReadLine()?.Trim();
                        DateTime? productionDate = null;

                        if (!string.IsNullOrEmpty(productionDateInput))
                        {
                            while (!DateTime.TryParse(productionDateInput, out DateTime parsedProductionDate))
                            {
                                Console.WriteLine("Некорректное значение. Введите дату в формате ГГГГ-ММ-ДД или оставьте пустым:");
                                productionDateInput = Console.ReadLine()?.Trim();
                                if (string.IsNullOrEmpty(productionDateInput))
                                {
                                    break;
                                }
                            }
                            productionDate = string.IsNullOrEmpty(productionDateInput) ? null : DateTime.Parse(productionDateInput);
                        }

                        Console.WriteLine("Введите дату истечения срока годности коробки (в формате ГГГГ-ММ-ДД) или оставьте пустым:");
                        string? expirationDateInput = Console.ReadLine()?.Trim();
                        DateTime? expirationDate = null;

                        if (!string.IsNullOrEmpty(expirationDateInput))
                        {
                            while (!DateTime.TryParse(expirationDateInput, out DateTime parsedExpirationDate))
                            {
                                Console.WriteLine("Некорректное значение. Введите дату в формате ГГГГ-ММ-ДД или оставьте пустым:");
                                expirationDateInput = Console.ReadLine()?.Trim();
                                if (string.IsNullOrEmpty(expirationDateInput))
                                {
                                    break;
                                }
                            }
                            expirationDate = string.IsNullOrEmpty(expirationDateInput) ? null : DateTime.Parse(expirationDateInput);
                        }

                        // Проверка: хотя бы одна из дат должна быть указана
                        if (productionDate == null && expirationDate == null)
                        {
                            Console.WriteLine("Вы должны указать хотя бы одну дату (производства или истечения срока годности)!");
                            Console.WriteLine("Попробуйте снова.");
                            Console.ReadKey();
                            break;
                        }

                        newBox.ProductionDate = productionDate;
                        newBox.ExpirationDate = expirationDate;

                        newBox.PalletId = null; // Изначально коробка не привязана к паллете

                        menu.AddBoxToPallet(newBox);

                        var boxResult = insertData.AddBox(newBox).Result;
                        if (boxResult)
                            Console.WriteLine("Коробка успешно добавлена!");
                        else
                            Console.WriteLine("Ошибка при добавлении коробки.");

                        Console.WriteLine("\nНажмите любую клавишу чтобы продолжить");
                        Console.ReadKey();
                        break;
                    case 6:
                        isWorks = false;
                        break;
                }
            }
        }
    }
}
