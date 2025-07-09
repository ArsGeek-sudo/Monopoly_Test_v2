using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Monopoly_Test_v2
{
    internal class InsertData
    {
        const string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1;Database=monopoly_test";
        PostgresCompiler compiler = new PostgresCompiler();

        // Метод для добавления паллеты в таблицу pallets.
        public async Task<bool> AddPallet(Pallet pallet)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    QueryFactory db = new QueryFactory(connection, compiler);

                    var palletId = await db.Query("pallets").InsertGetIdAsync<long>(new
                    {
                        width = pallet.Width,
                        height = pallet.Height,
                        depth = pallet.Depth,
                        created_at = pallet.CreatedAt
                    });

                    if (pallet.Boxes != null && pallet.Boxes.Count > 0)
                    {
                        foreach (var box in pallet.Boxes)
                        {
                            box.PalletId = palletId;
                            await AddBox(box);
                        }
                    }

                    return true;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Ошибка PostgreSQL при добавлении паллеты!\n" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при добавлении паллеты!\n" + ex.Message);
            }

            return false;
        }

        // Метод для добавления коробки в таблицу boxes.
        public async Task<bool> AddBox(Box box)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    QueryFactory db = new QueryFactory(connection, compiler);

                    await db.Query("boxes").InsertAsync(new
                    {
                        pallet_id = box.PalletId,
                        width = box.Width,
                        height = box.Height,
                        depth = box.Depth,
                        weight = box.Weight,
                        production_date = box.ProductionDate,
                        expiration_date = box.ExpirationDate,
                        created_at = box.CreatedAt
                    });

                    return true;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Ошибка PostgreSQL при добавлении коробки!\n" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при добавлении коробки!\n" + ex.Message);
            }

            return false;
        }

        // Метод для присвоения коробке паллеты
        public async Task UpdateBox(Box box)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    QueryFactory db = new QueryFactory(connection, compiler);

                    await db.Query("boxes")
                    .Where("pallet_id", box.PalletId)
                    .UpdateAsync(new
                    {
                        width = box.Width,
                        height = box.Height,
                        depth = box.Depth,
                        weight = box.Weight,
                        production_date = box.ProductionDate,
                        expiration_date = box.ExpirationDate,
                        created_at = box.CreatedAt
                    });
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Ошибка PostgreSQL при добавлении коробки!\n" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при добавлении коробки!\n" + ex.Message);
            }
        }
    }
}
