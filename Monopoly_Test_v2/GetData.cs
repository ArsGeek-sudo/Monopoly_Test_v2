using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Monopoly_Test_v2
{
    public class GetData : IDataService
    {
        const string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1;Database=monopoly_test";
        PostgresCompiler compiler = new PostgresCompiler();

        // Получает все паллеты из таблицы pallets.
        public async Task<List<Pallet>?> GetPallets()
        {
            List<Pallet> pallets = new List<Pallet>();

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    QueryFactory db = new QueryFactory(connection, compiler);

                    var palletsData = await db.Query("pallets").GetAsync();

                    List<Box> boxes = await GetBoxesById(palletsData);

                    foreach (var pallet in palletsData)
                    {
                        var sortedBoxes = boxes.Where(box => box.PalletId == pallet.id).ToList();

                        pallets.Add(new Pallet
                        {
                            Id = pallet.id,
                            Width = pallet.width,
                            Height = pallet.height,
                            Depth = pallet.depth,
                            CreatedAt = pallet.created_at,
                            Boxes = sortedBoxes
                        });
                    }

                    return pallets;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Ошибка PostgreSQL при получении палет!\n" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении палет!\n" + ex.Message);
            }

            return pallets;
        }

        public async Task<Pallet?> GetPalletById(long id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    QueryFactory db = new QueryFactory(connection, compiler);

                    // Выполняем запрос с условием по id
                    var palletData = await db.Query("pallets").Where("id", id).FirstOrDefaultAsync();

                    // Если паллета не найдена, возвращаем null
                    if (palletData == null)
                        return null;

                    // Создаем список из одной паллеты для передачи в GetBoxesById
                    var palletList = new List<dynamic> { palletData };
                    List<Box> boxes = await GetBoxesById(palletList);

                    // Фильтруем коробки для этой паллеты (хотя в списке boxes должны быть только коробки этой паллеты)
                    var sortedBoxes = boxes.Where(box => box.PalletId == palletData.id).ToList();

                    return new Pallet
                    {
                        Id = palletData.id,
                        Width = palletData.width,
                        Height = palletData.height,
                        Depth = palletData.depth,
                        CreatedAt = palletData.created_at,
                        Boxes = sortedBoxes
                    };
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Ошибка PostgreSQL при получении паллеты по id {id}!\n" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении паллеты по id - {id}!\n" + ex.Message);
            }

            return null;
        }

        // Получает все коробки из таблицы boxes.
        public async Task<List<Box>?> GetBoxes()
        {
            List<Box> boxes = new List<Box>();

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    QueryFactory db = new QueryFactory(connection, compiler);

                    var result = await db.Query("boxes").GetAsync();

                    foreach (var row in result)
                    {
                        boxes.Add(new Box
                        {
                            Id = row.id,
                            PalletId = row.pallet_id,
                            Width = row.width,
                            Height = row.height,
                            Depth = row.depth,
                            Weight = row.weight,
                            CreatedAt = row.created_at,
                            ProductionDate = row.production_date,
                            ExpirationDate = row.expiration_date
                        });
                    }

                    return boxes;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Ошибка PostgreSQL при получении коробок!\n" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении коробок!\n" + ex.Message);
            }

            return boxes;
        }

        public async Task<List<Box>> GetBoxesById(IEnumerable<dynamic> palletRows)
        {
            List<Box> boxes = new List<Box>();

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    QueryFactory db = new QueryFactory(connection, compiler);

                    // Извлекаем все pallet_id
                    var palletIds = palletRows.Select(palletData => (int)palletData.id).ToList();

                    if (palletIds.Count == 0)
                        return boxes;

                    // Получаем коробки, у которых pallet_id содержится в palletIds
                    var boxResult = await db.Query("boxes")
                        .WhereIn("pallet_id", palletIds)
                        .GetAsync();

                    foreach (var row in boxResult)
                    {
                        boxes.Add(new Box
                        {
                            Id = row.id,
                            PalletId = row.pallet_id,
                            Width = row.width,
                            Height = row.height,
                            Depth = row.depth,
                            Weight = row.weight,
                            ProductionDate = row.production_date,
                            ExpirationDate = row.expiration_date
                        });
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Ошибка PostgreSQL при получении коробок!\n" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении коробок!\n" + ex.Message);
            }

            return boxes;
        }
    }
}
