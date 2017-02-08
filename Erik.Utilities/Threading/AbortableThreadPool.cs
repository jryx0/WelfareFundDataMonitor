using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Erik.Utilities.Interfaces;

namespace Erik.Utilities.Threading
{
    public sealed class AbortableThreadPool : 
        IAbortable, IDisposable
    {
        #region Static part

        /// <summary>
        /// No more than 25 threads will be running simultaneously, and this
        /// value does not depend on the number of pools you create. For example,
        /// if you create three pools, the sum of executing threads in the three
        /// pools will never be greater than MaxSimultaneousThreads value
        /// </summary>
        const int MaxSimultaneousThreads = 25;
        const int TimeoutMilliseconds = 10;

        static Semaphore _semaphore;
        static List<AbortableThreadPool> _instances;

        static TimeSpan _timeout;

        static AbortableThreadPool()
        {
            _semaphore = new Semaphore(MaxSimultaneousThreads, MaxSimultaneousThreads);
            _instances = new List<AbortableThreadPool>();

            _timeout = TimeSpan.FromMilliseconds(TimeoutMilliseconds);
        }

        public static AbortableThreadPool NewInstance()
        {
            return NewInstance(MaxSimultaneousThreads);
        }

        public static AbortableThreadPool NewInstance(int concurrentThreads)
        {
            if (concurrentThreads > MaxSimultaneousThreads || concurrentThreads < 1)
                throw new ArgumentException();

            AbortableThreadPool pool = new AbortableThreadPool();
            pool._maxThreadsRunning = concurrentThreads;

            lock(_instances)
                _instances.Add(pool);

            return pool;
        }

        public static void AbortAll()
        {
            AbortableThreadPool[] array = new AbortableThreadPool[_instances.Count];

            lock (_instances)
                _instances.CopyTo(array);

            foreach (AbortableThreadPool pool in array)
                pool.Abort();
        }

        #endregion

        #region Instance part

        object _lock;

        int _maxThreadsRunning;
        Dictionary<int, Thread> _runningThreads;
        Queue<ThreadStart> _waitingCallbacks;

        System.Timers.Timer _tmr;
        bool _isDisposed = false;

        private AbortableThreadPool() 
        {
            _runningThreads = new Dictionary<int, Thread>();
            _waitingCallbacks = new Queue<ThreadStart>();

            _lock = new object();

            _tmr = new System.Timers.Timer(_timeout.TotalMilliseconds);
            _tmr.Elapsed += new System.Timers.ElapsedEventHandler(_tmr_Elapsed);

            _tmr.Start();
        }

        ~AbortableThreadPool()
        {
            lock (_lock)
            {
                if (_runningThreads.Count != 0 || _waitingCallbacks.Count != 0)
                    Abort();

                _isDisposed = true;
                _tmr.Dispose();
            }
        }

        public void AddNewOperation(ThreadStart callback)
        {
            CheckDisposed();

            lock (_lock)
                _waitingCallbacks.Enqueue(callback);
        }

        void _tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_lock)
            {
                _tmr.Stop();

                // Check for waiting callbacks
                if (_waitingCallbacks.Count != 0)
                {
                    if (_runningThreads.Count < _maxThreadsRunning)
                    {
                        // I can run a new callback if _semaphore is free
                        if (_semaphore.WaitOne(_timeout))
                        {
                            ThreadStart callback = _waitingCallbacks.Dequeue();

                            Thread th = new Thread(
                                new ParameterizedThreadStart(RunThread));
                            th.IsBackground = true;

                            _runningThreads.Add(th.ManagedThreadId, th);

                            th.Start(callback);
                        }
                    }
                }

                if (!_isDisposed)
                    _tmr.Start();
            }
        }

        void RunThread(object obj)
        {
            ThreadStart callback = obj as ThreadStart;

            // Just make the callback
            callback();

            // When callback is done, just remove the thread
            lock (_lock)
                _runningThreads.Remove(Thread.CurrentThread.ManagedThreadId);

            // And signal the semaphore
            _semaphore.Release(1);
        }

        void CheckDisposed()
        {
            lock (_lock)
            {
                if (_isDisposed)
                    throw new ObjectDisposedException("AbortableThreadPool");
            }
        }

        #region IAbortable members

        public event EventHandler Aborted;

        public void Abort()
        {
            CheckDisposed();

            lock (_lock)
            {
                // Clear the waiting calbacks queue
                _waitingCallbacks.Clear();

                // And abort all of the running threads
                foreach (Thread th in _runningThreads.Values)
                {
                    // Abort the thread
                    th.Abort();
                    // And signal the semaphore
                    _semaphore.Release(1);
                }

                _runningThreads.Clear();

                // Dispose the pool
                Dispose(false);

                // Raise Aborted event
                if (Aborted != null)
                    Aborted(this, EventArgs.Empty);

                // Clean Aborted event
                Aborted = null;
            }
        }
        #endregion

        #endregion

        #region IDisposable Members

        void Dispose(bool cleanAbortedEvent)
        {
            lock (_lock)
            {
                if (!_isDisposed)
                {
                    if (_waitingCallbacks.Count != 0 || _runningThreads.Count != 0)
                        throw new InvalidOperationException("The thread pool is still running");

                    GC.SuppressFinalize(this);

                    _tmr.Dispose();
                    _isDisposed = true;

                    lock (_instances)
                        _instances.Remove(this);

                    if (cleanAbortedEvent)
                        Aborted = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
