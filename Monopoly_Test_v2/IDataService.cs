namespace Monopoly_Test_v2
{
    public interface IDataService
    {
        Task<List<Pallet>?> GetPallets();
        Task<List<Box>?> GetBoxes();
        Task<Pallet?> GetPalletById(long id);
    }
}
