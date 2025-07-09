namespace Monopoly_Test_v2
{
    public class Menu
    {
        GetData getData = new GetData();
        InsertData insertData = new InsertData();

        int selectedIndex = 0;

        ConsoleKey key;

        public void PalletDialogue(List<Pallet> pallets)
        {
            if (pallets != null && pallets.Count > 0)
            {
                // Сортировка по ExpirationDate от самой дальней даты к ближайшей
                pallets = pallets
                    .OrderByDescending(pallet => pallet.ExpirationDate ?? DateTime.MinValue)
                    .ToList();

                do
                {
                    Console.Clear();
                    Console.WriteLine("Выберите палету (стрелками ↑ ↓, Enter для выбора):\n");

                    for (int i = 0; i < pallets.Count; i++)
                    {
                        if (i == selectedIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }

                        Console.WriteLine($"  Паллета {pallets[i].Id}");

                        Console.ResetColor();
                    }

                    key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.UpArrow)
                    {
                        selectedIndex = (selectedIndex == 0) ? pallets.Count - 1 : selectedIndex - 1;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        selectedIndex = (selectedIndex == pallets.Count - 1) ? 0 : selectedIndex + 1;
                    }
                    else if (key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        Console.WriteLine($"Вы выбрали паллету {pallets[selectedIndex].Id}\n");
                        Console.WriteLine($"Ширина: {pallets[selectedIndex].Width} см");
                        Console.WriteLine($"Высота: {pallets[selectedIndex].Height} см");
                        Console.WriteLine($"Глубина: {pallets[selectedIndex].Depth} см");
                        Console.WriteLine($"Собственный вес: {pallets[selectedIndex].OwnWeight} кг");
                        Console.WriteLine($"Общий вес: {pallets[selectedIndex].TotalWeight} кг");
                        Console.WriteLine($"Общий объём: {pallets[selectedIndex].TotalVolume} м3");
                        Console.WriteLine($"Срок годности: {(pallets[selectedIndex].ExpirationDate.HasValue ?
                            pallets[selectedIndex].ExpirationDate.Value.ToShortDateString() : "Нет")}\n");

                        if (pallets[selectedIndex].Boxes.Count > 0)
                        {
                            Console.WriteLine("Коробки на паллете:");
                            foreach (var box in pallets[selectedIndex].Boxes)
                            {
                                Console.WriteLine($"  - Коробка {box.Id}: {box.Width}x{box.Height}x{box.Depth} cм," +
                                    $" вес {box.Weight} кг, срок годности {box.CalculatedExpirationDate.ToShortDateString()}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("На паллете нет коробок.");
                        }

                        Console.WriteLine("\nНажмите любую клавишу чтобы продолжить");
                        Console.ReadKey();
                        break;
                    }

                } while (true);
            }
            else
            {
                Console.WriteLine($"Паллеты отсутствуют");
            }
        }

        public void ShowGroupedPallets(List<Pallet> pallets)
        {
            if (pallets == null || pallets.Count == 0)
            {
                Console.WriteLine("Паллеты отсутствуют.");
                Console.WriteLine("\nНажмите любую клавишу чтобы продолжить");
                Console.ReadKey();
                return;
            }

            // Группировка и сортировка
            var groupedPallets = pallets
                .GroupBy(p => p.ExpirationDate)
                .Select(g => new
                {
                    ExpirationDate = g.Key,
                    Pallets = g.OrderBy(p => p.TotalWeight).ToList()
                })
                .OrderBy(g => g.ExpirationDate ?? DateTime.MaxValue) // null идут последними
                .ToList();

            Console.Clear();
            Console.WriteLine("Группированные паллеты (по сроку годности, внутри группы по весу):\n");

            foreach (var group in groupedPallets)
            {
                string groupHeader = group.ExpirationDate.HasValue
                    ? $"Группа со сроком годности: {group.ExpirationDate.Value.ToShortDateString()}"
                    : "Группа без срока годности";

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(groupHeader);
                Console.ResetColor();

                foreach (var pallet in group.Pallets)
                {
                    Console.WriteLine($"  Паллета {pallet.Id}: " +
                        $"Вес = {pallet.TotalWeight} кг; " +
                        $"Объем = {pallet.TotalVolume} м3; " +
                        $"Коробок = {pallet.Boxes.Count}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\nНажмите любую клавишу чтобы продолжить");
            Console.ReadKey();
        }

        public void ShowTopPallets(List<Pallet> pallets)
        {
            if (pallets == null || pallets.Count == 0)
            {
                Console.WriteLine("Паллеты отсутствуют.");
                return;
            }

            // Фильтрация паллет, которые содержат коробки
            var filteredPallets = pallets
                .Where(pallet => pallet.Boxes != null && pallet.Boxes.Count > 0)
                .Select(pallet => new
                {
                    Pallet = pallet,
                    MaxExpirationDate = pallet.Boxes.Max(box => box.CalculatedExpirationDate)
                })
                .OrderByDescending(pallet => pallet.MaxExpirationDate) // Сортировка по убыванию максимального срока годности
                .ThenBy(pallet => pallet.Pallet.TotalVolume)          // Сортировка по возрастанию объема
                .Take(3)                                   // Выбор 3 паллет
                .ToList();

            if (filteredPallets.Count < 3)
            {
                Console.WriteLine("Нет паллет с коробками или паллет меньше трёх.");
                Console.WriteLine("\nНажмите любую клавишу чтобы продолжить");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Топ-3 паллеты с коробками с наибольшим сроком годности, отсортированные по объему:\n");

            foreach (var item in filteredPallets)
            {
                var pallet = item.Pallet;
                Console.WriteLine($"Паллета {pallet.Id}:");
                Console.WriteLine($"  Общий объем: {pallet.TotalVolume} м3");
                Console.WriteLine($"  Палета годна до: {item.MaxExpirationDate.ToShortDateString()}");
                Console.WriteLine($"  Количество коробок: {pallet.Boxes.Count}\n");
            }

            Console.WriteLine("\nНажмите любую клавишу чтобы продолжить");
            Console.ReadKey();
        }

        public void BoxDialogue(List<Box> boxes)
        {
            if (boxes != null && boxes.Count > 0)
            {
                int selectedIndex = 0;
                ConsoleKey key;

                do
                {
                    Console.Clear();
                    Console.WriteLine("Выберите коробку (стрелками ↑ ↓, Enter для выбора):\n");

                    for (int i = 0; i < boxes.Count; i++)
                    {
                        if (i == selectedIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }

                        Console.WriteLine($"  Коробка {boxes[i].Id}");

                        Console.ResetColor();
                    }

                    key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.UpArrow)
                    {
                        selectedIndex = (selectedIndex == 0) ? boxes.Count - 1 : selectedIndex - 1;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        selectedIndex = (selectedIndex == boxes.Count - 1) ? 0 : selectedIndex + 1;
                    }
                    else if (key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        Console.WriteLine($"Вы выбрали коробку {boxes[selectedIndex].Id}\n");
                        Console.WriteLine($"Ширина: {boxes[selectedIndex].Width} см");
                        Console.WriteLine($"Высота: {boxes[selectedIndex].Height} см");
                        Console.WriteLine($"Глубина: {boxes[selectedIndex].Depth} см");
                        Console.WriteLine($"Вес: {boxes[selectedIndex].Weight} кг");
                        Console.WriteLine($"Объём: {boxes[selectedIndex].Volume} м3");
                        Console.WriteLine($"Срок годности: {boxes[selectedIndex].CalculatedExpirationDate.ToShortDateString()}\n");

                        if (boxes[selectedIndex].PalletId != null)
                            Console.WriteLine($"Коробка размещена на паллете {boxes[selectedIndex].PalletId}\n");
                        else
                        {
                            AddBoxToPallet(boxes[selectedIndex]);
                        }

                        Console.WriteLine("\nНажмите любую клавишу чтобы продолжить");
                        Console.ReadKey();
                        break;
                    }
                } while (true);
            }
            else
            {
                Console.WriteLine("Коробки отсутствуют.");
                Console.WriteLine("\nНажмите любую клавишу чтобы продолжить");
                Console.ReadKey();
            }
        }

        public async void AddBoxToPallet(Box addBox)
        {
            Console.Write("Хотите разместить коробку на паллете? (y/n): ");

            string? plaseBox = Console.ReadLine()?.Trim().ToLower();

            if (plaseBox == "y" || plaseBox == "н")
            {
                while (true)
                {
                    Console.Write("Введите номер палеты целым числом: ");
                    if (long.TryParse(Console.ReadLine(), out long id) && id > 0)
                    {
                        addBox.PalletId = id;

                        Pallet pallet = getData.GetPalletById(id).Result;

                        if (pallet != null && pallet.CanContain(addBox))
                        {
                            await insertData.UpdateBox(addBox);
                            break;
                        }
                        else if (pallet != null)
                            Console.WriteLine("Коробка не помещается на паллету!");
                        else Console.WriteLine("Выбраной вами паллеты не существует!");
                    }
                    else
                    {
                        Console.WriteLine("Вы ввели неправильное значение, попробуйте ещё раз!");
                    }
                }
            }
        }
    }
}
