namespace Monopoly_Test_v2
{
    // Представляет паллету на складе, содержащую коробки.  
    public class Pallet
    {
        // Уникальный идентификатор паллеты.  
        public long Id { get; set; }

        // Ширина паллеты.  
        public double Width { get; set; }

        // Высота паллеты.  
        public double Height { get; set; }

        // Глубина паллеты.  
        public double Depth { get; set; }

        // Дата создания паллеты.  
        public DateTime CreatedAt { get; set; }

        // Список коробок, находящихся на паллете.  
        public List<Box> Boxes { get; set; } = new List<Box>();

        // Собственный вес паллеты (30 кг по условию).  
        public double OwnWeight = 30.0;

        // Общий вес паллеты = собственный вес + вес всех коробок.  
        public double TotalWeight => OwnWeight + Boxes.Sum(box => box.Weight);

        // Объём паллеты без учёта коробок.  
        public double OwnVolume => Width * Height * Depth;

        // Общий объём паллеты = объём самой паллеты + объём всех коробок.  
        public double TotalVolume => (OwnVolume + GetBoxesVolume()) / 1000000;

        // Срок годности паллеты: минимальный срок годности среди всех коробок.  
        // Если коробок нет, то null.  
        public DateTime? ExpirationDate =>
            Boxes.Any() ? Boxes.Min(box => box.CalculatedExpirationDate) : null;

        // Проверка, может ли коробка поместиться на паллету по габаритам (ширина и глубина).  
        public bool CanContain(Box box) =>
            box.Width <= Width && box.Height <= Depth;

        // Метод для вычисления объёма всех коробок.  
        private double GetBoxesVolume()
        {
            double boxesTotalVolume = 0.0;

            foreach (Box box in Boxes)
                boxesTotalVolume += box.Volume;

            return boxesTotalVolume;
        }
    }
}
