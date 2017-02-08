using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Erik.Utilities.Bases;
using Erik.Utilities.Interfaces;

namespace Erik.Utilities.Bases
{
    public class CompositePO :
        BaseProgressiveOperation, ICompositeProgressiveOperation
    {
        protected List<IProgressiveOperation> _operations;
        protected IProgressiveOperation _currentOperation;

        protected CompositePO() { }

        public CompositePO(List<IProgressiveOperation> operations)
        {
            _operations = operations;

            _totalSteps = _operations.Sum<IProgressiveOperation>(po => po.TotalSteps);
        }

        public override void Start()
        {
            _currentStep = 0;
            OnOperationStart(EventArgs.Empty);

            foreach (IProgressiveOperation po in _operations)
            {
                _currentOperation = po;

                po.OperationProgress += 
                    (sender, e) =>
                    {
                        _currentStep++;
                        OnOperationProgress(EventArgs.Empty);
                    };

                OnNewOperation(EventArgs.Empty);

                po.Start();
            }

            OnOperationEnd(EventArgs.Empty);
        }

        protected virtual void OnNewOperation(EventArgs e)
        {
            if (NewOperation != null)
                NewOperation(this, EventArgs.Empty);
        }

        #region ICompositeProgressiveOperation Members

        public virtual IProgressiveOperation CurrentOperation
        {
            get { return _currentOperation; }
        }

        public event EventHandler NewOperation;

        #endregion
    }
}
