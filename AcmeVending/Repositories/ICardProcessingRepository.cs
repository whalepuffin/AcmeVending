namespace AcmeVending.Repositories
{
    public interface ICardProcessingRepository
    {
        bool ProcessCard(string cardNumber);
    }
}