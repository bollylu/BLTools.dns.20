using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools
{
    public class TActionManager
    {
        protected readonly List<AActionManagerItem> _Actions = new List<AActionManagerItem>();

        #region --- Constructor(s) ---------------------------------------------------------------------------------
        public TActionManager() { } 
        #endregion --- Constructor(s) ------------------------------------------------------------------------------

        private readonly object _LockActions = new object();

        public bool IsEmpty()
        {
            lock ( _LockActions )
            {
                return _Actions.IsEmpty();
            }
        }

        public bool Any()
        {
            lock ( _LockActions )
            {
                return _Actions.Any();
            }
        }

        public int Count()
        {
            lock ( _LockActions )
            {
                return _Actions.Count();
            }
        }

        #region --- Add actions --------------------------------------------
        public void AddAction<X>(string name, Predicate<X> predicate, Action<X> action)
        {
            lock ( _LockActions )
            {
                _Actions.Add(new TActionManagerActionItem<X>(name, predicate, action));
            }
        }

        public void AddAction<X, Y>(string name, Predicate<X> predicate, Action<X, Y> action)
        {
            lock ( _LockActions )
            {
                _Actions.Add(new TActionManagerActionItem<X, Y>(name, predicate, action));
            }
        }

        public void AddAction<X, Y, Z>(string name, Predicate<X> predicate, Action<X, Y, Z> action)
        {
            lock ( _LockActions )
            {
                _Actions.Add(new TActionManagerActionItem<X, Y, Z>(name, predicate, action));
            }
        }
        #endregion --- Add actions -----------------------------------------

        #region --- Add functions --------------------------------------------
        /// <summary>
        /// Add a function to calculate some results if a predicate is true
        /// </summary>
        /// <typeparam name="X">The type of the value passed to the predicate (and the function)</typeparam>
        /// <typeparam name="R">The type of the returned value</typeparam>
        /// <param name="name">A name (for display only)</param>
        /// <param name="predicate">The predicate that determine if the function is processed</param>
        /// <param name="func">The function to process when the predicate is true</param>
        public void AddFunc<X, R>(string name, Predicate<X> predicate, Func<X, R> func) where R : IActionManagerReturnValue<R>
        {
            lock ( _LockActions )
            {
                _Actions.Add(new TActionManagerFuncItem<X, R>(name, predicate, func));
            }
        }

        /// <summary>
        /// Add a function to calculate some results if a predicate is true
        /// </summary>
        /// <typeparam name="X1">The type of the value passed to the predicate (and the function)</typeparam>
        /// <typeparam name="X2">The type of the second parameter for the function</typeparam>
        /// <typeparam name="R">The type of the returned value</typeparam>
        /// <param name="name">A name (for display only)</param>
        /// <param name="predicate">The predicate that determine if the function is processed</param>
        /// <param name="func">The function to process when the predicate is true</param>
        public void AddFunc<X1, X2, R>(string name, Predicate<X1> predicate, Func<X1, X2, R> func) where R : IActionManagerReturnValue<R>
        {
            lock ( _LockActions )
            {
                _Actions.Add(new TActionManagerFuncItem<X1, X2, R>(name, predicate, func));
            }
        }

        /// <summary>
        /// Add a function to calculate some results if a predicate is true
        /// </summary>
        /// <typeparam name="X1">The type of the value passed to the predicate (and the function)</typeparam>
        /// <typeparam name="X2">The type of the second parameter for the function</typeparam>
        /// <typeparam name="X3">The type of the third parameter for the function</typeparam>
        /// <typeparam name="R">The type of the returned value</typeparam>
        /// <param name="name">A name (for display only)</param>
        /// <param name="predicate">The predicate that determine if the function is processed</param>
        /// <param name="func">The function to process when the predicate is true</param>
        public void AddFunc<X1, X2, X3, R>(string name, Predicate<X1> predicate, Func<X1, X2, X3, R> func) where R : IActionManagerReturnValue<R>
        {
            lock ( _LockActions )
            {
                _Actions.Add(new TActionManagerFuncItem<X1, X2, X3, R>(name, predicate, func));
            }
        }

        /// <summary>
        /// Add a function to calculate some results if a predicate is true
        /// </summary>
        /// <typeparam name="X1">The type of the value passed to the predicate (and the function)</typeparam>
        /// <typeparam name="X2">The type of the second parameter for the function</typeparam>
        /// <typeparam name="X3">The type of the third parameter for the function</typeparam>
        /// <typeparam name="X4">The type of the third parameter for the function</typeparam>
        /// <typeparam name="R">The type of the returned value</typeparam>
        /// <param name="name">A name (for display only)</param>
        /// <param name="predicate">The predicate that determine if the function is processed</param>
        /// <param name="func">The function to process when the predicate is true</param>
        public void AddFunc<X1, X2, X3, X4, R>(string name, Predicate<X1> predicate, Func<X1, X2, X3, X4, R> func) where R : IActionManagerReturnValue<R>
        {
            lock ( _LockActions )
            {
                _Actions.Add(new TActionManagerFuncItem<X1, X2, X3, X4, R>(name, predicate, func));
            }
        }
        #endregion --- Add functions -----------------------------------------

        #region --- Execute actions --------------------------------------------
        public int ExecuteAction<X>(X value, EActionManagerExecutionType executionType = EActionManagerExecutionType.First)
        {
            lock ( _LockActions )
            {
                if ( _Actions.IsEmpty() )
                {
                    return 0;
                }

                switch ( executionType )
                {
                    case EActionManagerExecutionType.All:
                        int Counter = 0;
                        foreach ( TActionManagerActionItem<X> ActionManagerActionItemItem in _Actions.OfType<TActionManagerActionItem<X>>() )
                        {
                            ActionManagerActionItemItem.ConditionalExecute(value);
                            Counter++;
                        }
                        return Counter;

                    case EActionManagerExecutionType.Last:
                        foreach ( TActionManagerActionItem<X> ActionManagerActionItemItem in _Actions.OfType<TActionManagerActionItem<X>>().Reverse() )
                        {
                            if ( ActionManagerActionItemItem.ConditionalExecute(value).WasExecuted )
                            {
                                return 1;
                            }
                        }
                        return 0;

                    case EActionManagerExecutionType.First:
                    default:
                        foreach ( TActionManagerActionItem<X> ActionManagerActionItemItem in _Actions.OfType<TActionManagerActionItem<X>>() )
                        {
                            if ( ActionManagerActionItemItem.ConditionalExecute(value).WasExecuted )
                            {
                                return 1;
                            }
                        }
                        return 0;
                }
            }
        }

        public int ExecuteAction<X1, X2>(X1 arg1, X2 arg2, EActionManagerExecutionType executionType = EActionManagerExecutionType.First)
        {
            lock ( _LockActions )
            {
                if ( _Actions.IsEmpty() )
                {
                    return 0;
                }

                switch ( executionType )
                {
                    case EActionManagerExecutionType.All:
                        int Counter = 0;
                        foreach ( TActionManagerActionItem<X1, X2> ActionManagerActionItemItem in _Actions.OfType<TActionManagerActionItem<X1, X2>>() )
                        {
                            ActionManagerActionItemItem.ConditionalExecute(arg1, arg2);
                            Counter++;
                        }
                        return Counter;

                    case EActionManagerExecutionType.Last:
                        foreach ( TActionManagerActionItem<X1, X2> ActionManagerActionItemItem in _Actions.OfType<TActionManagerActionItem<X1, X2>>().Reverse() )
                        {
                            if ( ActionManagerActionItemItem.ConditionalExecute(arg1, arg2).WasExecuted )
                            {
                                return 1;
                            }
                        }
                        return 0;

                    case EActionManagerExecutionType.First:
                    default:
                        foreach ( TActionManagerActionItem<X1, X2> ActionManagerActionItemItem in _Actions.OfType<TActionManagerActionItem<X1, X2>>() )
                        {
                            if ( ActionManagerActionItemItem.ConditionalExecute(arg1, arg2).WasExecuted )
                            {
                                return 1;
                            }
                        }
                        return 0;
                }
            }
        }

        public int ExecuteAction<X1, X2, X3>(X1 arg1, X2 arg2, X3 arg3, EActionManagerExecutionType executionType = EActionManagerExecutionType.First)
        {
            lock ( _LockActions )
            {
                if ( _Actions.IsEmpty() )
                {
                    return 0;
                }

                switch ( executionType )
                {
                    case EActionManagerExecutionType.All:
                        int Counter = 0;
                        foreach ( TActionManagerActionItem<X1, X2, X3> ActionManagerActionItemItem in _Actions.OfType<TActionManagerActionItem<X1, X2, X3>>() )
                        {
                            ActionManagerActionItemItem.ConditionalExecute(arg1, arg2, arg3);
                            Counter++;
                        }
                        return Counter;

                    case EActionManagerExecutionType.Last:
                        foreach ( TActionManagerActionItem<X1, X2, X3> ActionManagerActionItemItem in _Actions.OfType<TActionManagerActionItem<X1, X2, X3>>().Reverse() )
                        {
                            if ( ActionManagerActionItemItem.ConditionalExecute(arg1, arg2, arg3).WasExecuted )
                            {
                                return 1;
                            }
                        }
                        return 0;

                    case EActionManagerExecutionType.First:
                    default:
                        foreach ( TActionManagerActionItem<X1, X2, X3> ActionManagerActionItemItem in _Actions.OfType<TActionManagerActionItem<X1, X2, X3>>() )
                        {
                            if ( ActionManagerActionItemItem.ConditionalExecute(arg1, arg2, arg3).WasExecuted )
                            {
                                return 1;
                            }
                        }
                        return 0;
                }
            }
        }

        public int ExecuteAction<X1, X2, X3, X4>(X1 arg1, X2 arg2, X3 arg3, X4 arg4, EActionManagerExecutionType executionType = EActionManagerExecutionType.First)
        {
            lock ( _LockActions )
            {
                if ( _Actions.IsEmpty() )
                {
                    return 0;
                }

                switch ( executionType )
                {
                    case EActionManagerExecutionType.All:
                        int Counter = 0;
                        foreach ( TActionManagerActionItem<X1, X2, X3, X4> ActionManagerActionItemItem in _Actions.OfType<TActionManagerActionItem<X1, X2, X3, X4>>() )
                        {
                            ActionManagerActionItemItem.ConditionalExecute(arg1, arg2, arg3, arg4);
                            Counter++;
                        }
                        return Counter;

                    case EActionManagerExecutionType.Last:
                        foreach ( TActionManagerActionItem<X1, X2, X3, X4> ActionManagerActionItemItem in _Actions.OfType<TActionManagerActionItem<X1, X2, X3, X4>>().Reverse() )
                        {
                            if ( ActionManagerActionItemItem.ConditionalExecute(arg1, arg2, arg3, arg4).WasExecuted )
                            {
                                return 1;
                            }
                        }
                        return 0;

                    case EActionManagerExecutionType.First:
                    default:
                        foreach ( TActionManagerActionItem<X1, X2, X3, X4> ActionManagerActionItemItem in _Actions.OfType<TActionManagerActionItem<X1, X2, X3, X4>>() )
                        {
                            if ( ActionManagerActionItemItem.ConditionalExecute(arg1, arg2, arg3, arg4).WasExecuted )
                            {
                                return 1;
                            }
                        }
                        return 0;
                }
            }
        }
        #endregion --- Execute actions -----------------------------------------

        #region --- Execute functions --------------------------------------------
        public R ExecuteFunc<X1, R>(X1 value, EActionManagerExecutionType executionType = EActionManagerExecutionType.First) where R : IActionManagerReturnValue<R>
        {
            if ( _Actions.IsEmpty() )
            {
                return default(R);
            }

            switch ( executionType )
            {
                case EActionManagerExecutionType.Last:
                    foreach ( TActionManagerFuncItem<X1, R> ActionManagerFuncItemItem in _Actions.OfType<TActionManagerFuncItem<X1, R>>().Reverse() )
                    {
                        IActionManagerReturnValue<R> RetVal = ActionManagerFuncItemItem.ConditionalExecute(value);
                        if ( RetVal.WasExecuted )
                        {
                            return RetVal.Value;
                        }
                    }
                    return default(R);

                case EActionManagerExecutionType.First:
                default:
                    foreach ( TActionManagerFuncItem<X1, R> ActionManagerFuncItemItem in _Actions.OfType<TActionManagerFuncItem<X1, R>>() )
                    {
                        IActionManagerReturnValue<R> RetVal = ActionManagerFuncItemItem.ConditionalExecute(value);
                        if ( RetVal.WasExecuted )
                        {
                            return RetVal.Value;
                        }
                    }
                    return default(R);
            }
        }

        public R ExecuteFunc<X1, X2, R>(X1 arg1, X2 arg2, EActionManagerExecutionType executionType = EActionManagerExecutionType.First) where R : IActionManagerReturnValue<R>
        {
            if ( _Actions.IsEmpty() )
            {
                return default(R);
            }

            switch ( executionType )
            {
                case EActionManagerExecutionType.Last:
                    foreach ( TActionManagerFuncItem<X1, X2, R> ActionManagerFuncItemItem in _Actions.OfType<TActionManagerFuncItem<X1, X2, R>>().Reverse() )
                    {
                        IActionManagerReturnValue<R> RetVal = ActionManagerFuncItemItem.ConditionalExecute(arg1, arg2);
                        if ( RetVal.WasExecuted )
                        {
                            return RetVal.Value;
                        }
                    }
                    return default(R);

                case EActionManagerExecutionType.First:
                default:
                    foreach ( TActionManagerFuncItem<X1, X2, R> ActionManagerFuncItemItem in _Actions.OfType<TActionManagerFuncItem<X1, X2, R>>() )
                    {
                        IActionManagerReturnValue<R> RetVal = ActionManagerFuncItemItem.ConditionalExecute(arg1, arg2);
                        if ( RetVal.WasExecuted )
                        {
                            return RetVal.Value;
                        }
                    }
                    return default(R);
            }
        }

        public R ExecuteFunc<X1, X2, X3, R>(X1 arg1, X2 arg2, X3 arg3, EActionManagerExecutionType executionType = EActionManagerExecutionType.First) where R : IActionManagerReturnValue<R>
        {
            if ( _Actions.IsEmpty() )
            {
                return default(R);
            }

            switch ( executionType )
            {
                case EActionManagerExecutionType.Last:
                    foreach ( TActionManagerFuncItem<X1, X2, X3, R> ActionManagerFuncItemItem in _Actions.OfType<TActionManagerFuncItem<X1, X2, X3, R>>().Reverse() )
                    {
                        IActionManagerReturnValue<R> RetVal = ActionManagerFuncItemItem.ConditionalExecute(arg1, arg2, arg3);
                        if ( RetVal.WasExecuted )
                        {
                            return RetVal.Value;
                        }
                    }
                    return default(R);

                case EActionManagerExecutionType.First:
                default:
                    foreach ( TActionManagerFuncItem<X1, X2, X3, R> ActionManagerFuncItemItem in _Actions.OfType<TActionManagerFuncItem<X1, X2, X3, R>>() )
                    {
                        IActionManagerReturnValue<R> RetVal = ActionManagerFuncItemItem.ConditionalExecute(arg1, arg2, arg3);
                        if ( RetVal.WasExecuted )
                        {
                            return RetVal.Value;
                        }
                    }
                    return default(R);
            }
        }

        public R ExecuteFunc<X1, X2, X3, X4, R>(X1 arg1, X2 arg2, X3 arg3, X4 arg4, EActionManagerExecutionType executionType = EActionManagerExecutionType.First) where R : IActionManagerReturnValue<R>
        {
            if ( _Actions.IsEmpty() )
            {
                return default(R);
            }

            switch ( executionType )
            {
                case EActionManagerExecutionType.Last:
                    foreach ( TActionManagerFuncItem<X1, X2, X3, X4, R> ActionManagerFuncItemItem in _Actions.OfType<TActionManagerFuncItem<X1, X2, X3, X4, R>>().Reverse() )
                    {
                        IActionManagerReturnValue<R> RetVal = ActionManagerFuncItemItem.ConditionalExecute(arg1, arg2, arg3, arg4);
                        if ( RetVal.WasExecuted )
                        {
                            return RetVal.Value;
                        }
                    }
                    return default(R);

                case EActionManagerExecutionType.First:
                default:
                    foreach ( TActionManagerFuncItem<X1, X2, X3, X4, R> ActionManagerFuncItemItem in _Actions.OfType<TActionManagerFuncItem<X1, X2, X3, X4, R>>() )
                    {
                        IActionManagerReturnValue<R> RetVal = ActionManagerFuncItemItem.ConditionalExecute(arg1, arg2, arg3, arg4);
                        if ( RetVal.WasExecuted )
                        {
                            return RetVal.Value;
                        }
                    }
                    return default(R);
            }
        }
        #endregion --- Execute functions -----------------------------------------
    }
}
