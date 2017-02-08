using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Erik.Utilities.Bases;
using Erik.Utilities.Interfaces;

namespace Erik.Utilities.Bases
{
    // This class is just to show a tiny implementation
    // using the Framework's ThreadPool. However, it
    // will not perform very well
    public class BackgroundCompositePO_ThreadPool : 
        BaseProgressiveOperation
    {
        Mutex _curStepMutex;
        protected List<IProgressiveOperation> _operations;

        protected BackgroundCompositePO_ThreadPool()
        {
            _curStepMutex = new Mutex(false);
        }

        public BackgroundCompositePO_ThreadPool(
            List<IProgressiveOperation> operations) : this()
        {
            _operations = operations;
            _totalSteps = _operations.Sum<IProgressiveOperation>(po => po.TotalSteps);
        }

        public override void Start()
        {
            if (!Monitor.TryEnter(this))
                throw new InvalidOperationException(
                    "Operation is already running");

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

                ThreadPool.QueueUserWorkItem(new WaitCallback(InitOperation), po);
            }
        }

        void InitOperation(object operation)
        {
            ((IProgressiveOperation)operation).Start();
        }

        protected void IncreaseCurrentStep()
        {
            _curStepMutex.WaitOne();
            _currentStep++;

            if (_currentStep == _totalSteps)
            {
                OnOperationEnd(EventArgs.Empty);
                Monitor.Exit(this);
            }

            _curStepMutex.ReleaseMutex();
        }
    }
}
