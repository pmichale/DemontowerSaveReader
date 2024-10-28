using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace State.Redux;

public class Store<TState> : IStore<TState>
{
    private readonly object _syncRoot = new();
    private readonly Dispatcher _dispatcher;
    private readonly Reducer<TState> _reducer;
    private TState _lastState;
    private System.Action? _stateChanged;
    private readonly Subject<StoreAction> _actionSubject = new();
    public IObservable<StoreAction> Actions => _actionSubject.AsObservable();

    public Store(Reducer<TState> reducer, TState initialState, params Middleware<TState>[] middlewares)
    {
        _reducer = reducer;
        _dispatcher = ApplyMiddlewares(middlewares);

        _lastState = initialState;
    }

    public event System.Action StateChanged
    {
        add
        {
            value();
            _stateChanged += value;
        }
        remove
        {
            _stateChanged -= value;
        }
    }

    public object Dispatch(StoreAction storeAction)
    {
        var result = _dispatcher(storeAction);

        _actionSubject.OnNext(storeAction);

        return result;
    }

    public TState GetState()
    {
        return _lastState;
    }

    private Dispatcher ApplyMiddlewares(params Middleware<TState>[] middlewares)
    {
        Dispatcher dispatcher = InnerDispatch;
        foreach (var middleware in middlewares)
        {
            dispatcher = middleware(this)(dispatcher);
        }
        return dispatcher;
    }

    private object InnerDispatch(StoreAction storeAction)
    {
        lock (_syncRoot)
        {
            _lastState = _reducer(_lastState, storeAction);
        }

        _stateChanged?.Invoke();

        return storeAction;
    }
}
