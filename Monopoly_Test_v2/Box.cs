namespace Monopoly_Test_v2
{
    // Представляет коробку, размещённую на паллете.
    public class Box
    {
        // Уникальный идентификатор коробки.
        public long Id { get; set; }

        // Идентификатор паллеты, на которой находится коробка.
        public long? PalletId { get; set; }

        // Ширина коробки (в см или мм — в зависимости от системы).
        public double Width { get; set; }

        // Высота коробки.
        public double Height { get; set; }

        // Глубина (длина) коробки.
        public double Depth { get; set; }

        // Вес коробки.
        public double Weight { get; set; }

        // Дата производства коробки. Может быть null.
        public DateTime? ProductionDate { get; set; }

        // Дата истечения срока годности. Может быть null.
        public DateTime? ExpirationDate { get; set; }

        // Дата создания записи о коробке.
        public DateTime CreatedAt { get; set; }

        // Вычисляемое свойство: объём коробки (Ш × В × Г).
        public double Volume => (Width * Height * Depth) / 1000000;

        // Вычисляемая дата истечения срока годности:
        // если указана ExpirationDate — используется она,
        // иначе вычисляется как ProductionDate + 100 дней.
        public DateTime CalculatedExpirationDate =>
            ExpirationDate ?? (ProductionDate.HasValue ? ProductionDate.Value.AddDays(100) : DateTime.MinValue);
    }
}
