using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Erik.Utilities.Bases;
using Erik.Utilities.Interfaces;
using Erik.Utilities.Threading;

namespace Erik.Utilities.Bases
{
    public class BackgroundCompositePO:
        BaseProgressiveOperation, IAbortable
    {
        Mutex _curStepMutex;
        AbortableThreadPool _pool;

        protected List<IProgressiveOperation> _operations;

        protected BackgroundCompositePO() 
        {
            _curStepMutex = new Mutex(false);
        }

        public BackgroundCompositePO(
            List<IProgressiveOperation> operations) : this()
        {
            _operations = operations;
            _totalSteps = _operations.Sum<IProgressiveOperation>(po => po.TotalSteps);
        }

        void CreateThreadPool()
        {
            if (_pool != null)
                _pool.Dispose();

            _pool = AbortableThreadPool.NewInstance();

            _pool.Aborted += (sender, e) => OnAborted(e);
        }

        public override void Start()
        {
            // If the operation is already running, this
            // will throw an InvalidOperationException
            // due to the invocation to its Dispose method
            CreateThreadPool();

            _currentStep = 0;
            OnOperationStart(EventArgs.Empty);

            foreach (IProgressiveOperation po in _operations)
            {
                po.OperationProgress +=
                    (sender, e) =>
                    {
                        IncreaseCurrentStep();
                        OnOperationProgress(EventArgs.Empty);
                    };

                RunOperation(po);
            }
        }

        protected void RunOperation(IProgressiveOperation po)
        {
            _pool.AddNewOperation(new ThreadStart(po.Start));
        }

        protected void IncreaseCurrentStep()
        {
            _curStepMutex.WaitOne();
            _currentStep++;

            if (_currentStep == _totalSteps)
            {
                OnOperationEnd(EventArgs.Empty);
                Thread th = new Thread(new ThreadStart(DisposePool));
                th.Start();
            }

            _curStepMutex.ReleaseMutex();
        }

        void DisposePool()
        {
            bool disposed = false;
            while (!disposed)
            {
                try
                {
                    _pool.Dispose();
                    disposed = true;
                }
                catch { Thread.Sleep(10); }
            }
        }

        protected virtual void OnAborted(EventArgs e)
        {
            if (Aborted != null)
                Aborted(this, EventArgs.Empty);
        }

        #region IAbortable Members

        public event EventHandler  Aborted;

        public void Abort()
        {
            _pool.Abort();
        }

        #endregion
    }
}
