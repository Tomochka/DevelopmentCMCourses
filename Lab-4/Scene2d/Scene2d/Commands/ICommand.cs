namespace Scene2d.Commands
{
    public interface ICommand
    {
        void Apply(Scene scene);
        string FriendlyResultMessage { get; }
    }
}