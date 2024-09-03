public interface IState<T>
{
    void OnEnter(T bots);
    void OnExecute(T bots);
    void OnExit(T bots);
}