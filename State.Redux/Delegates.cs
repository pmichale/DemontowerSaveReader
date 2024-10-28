namespace State.Redux;

public delegate object Dispatcher(StoreAction storeAction);
public delegate Func<Dispatcher, Dispatcher> Middleware<TState>(IStore<TState> store);
public delegate TState Reducer<TState>(TState previousState, StoreAction storeAction);
