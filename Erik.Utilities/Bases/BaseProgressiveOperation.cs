using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Erik.Utilities.Interfaces;

namespace Erik.Utilities.Bases
{
    public abstract class BaseProgressiveOperation : IProgressiveOperation
    {
        string _title = "Proccessing";
        string _subtitle = "Please wait";

        protected int _totalSteps;
        protected int _currentStep;

        protected virtual void OnOperationStart(EventArgs e)
        {
            if (OperationStart != null)
                OperationStart(this, e);
        }

        protected virtual void OnOperationProgress(EventArgs e)
        {
            if (OperationProgress != null)
                OperationProgress(this, e);
        }

        protected virtual void OnOperationEnd(EventArgs e)
        {
            if (OperationEnd != null)
                OperationEnd(this, e);
        }

        #region IProgressiveOperation Members

        public event EventHandler OperationStart;

        public event EventHandler OperationProgress;

        public event EventHandler OperationEnd;

        public virtual string MainTitle
        {
            get { return _title; }
            set { _title = value; }
        }

        public virtual string SubTitle
        {
            get { return _subtitle; }
            set { _subtitle = value; }
        }

        public virtual int CurrentProgress
        {
            get
            {
                int ret = 0;

                if (_totalSteps > 0)
                    ret = (100 * _currentStep) / _totalSteps;

                return ret;
            }
        }

        public virtual int TotalSteps
        {
            get { return _totalSteps; }
        }

        public virtual int CurrentStep
        {
            get { return _currentStep; }
        }

        public abstract void Start();

        #endregion
    }
}
