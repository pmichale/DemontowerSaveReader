namespace State.Redux;

public interface IStore<TState>
{
    object Dispatch(StoreAction storeAction);

    TState GetState();

    event System.Action StateChanged;
    IObservable<StoreAction> Actions { get; }
}

