using System.Linq.Expressions;
using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace State.Redux;

public static class StoreExtensions
{
    public static IObservable<T> ObserveState<T>(this IStore<T> store)
    {
        return Observable
            .FromEvent(
                h => store.StateChanged += h,
                h => store.StateChanged -= h)
            .Select(_ => store.GetState());
    }

    public static void SubscribeToProperty<TState, TProperty>(
            this IStore<TState> store,
            Expression<Func<TState, TProperty>> propertySelector,
            Action<TProperty> action)
    {
        store.ObserveState()
             .Select(propertySelector.Compile())
             .DistinctUntilChanged()
             .Subscribe(action);
    }
    
    public static IDisposable SubscribeToAction<TState, TAction>(
        this IStore<TState> store,
        Action<TAction> action) where TAction : StoreAction
    {
        return store.Actions
            .OfType<TAction>()
            .Subscribe(action);
    }
    
    public static Task<TAction> SubscribeToActionAsync<TState, TAction>(
        this IStore<TState> store) where TAction : StoreAction
    {
        var tcs = new TaskCompletionSource<TAction>();

        IDisposable? subscription = null;
        subscription = store.Actions
            .OfType<TAction>()
            .Subscribe(action =>
            {
                tcs.SetResult(action);
                subscription?.Dispose();
            });

        return tcs.Task;
    }

    public static IServiceCollection AddScopedStore<TState>(this IServiceCollection services, Reducer<TState> reducer, TState initialState, params Middleware<TState>[] middlewares)
    {
        services.AddScoped<IStore<TState>>(_ => new Store<TState>(reducer, initialState, middlewares));
        return services;
    }

    public static IServiceCollection AddSingletonStore<TState>(this IServiceCollection services, Reducer<TState> reducer, TState initialState, params Middleware<TState>[] middlewares)
    {
        services.AddSingleton<IStore<TState>>(_ => new Store<TState>(reducer, initialState, middlewares));
        return services;
    }

}
