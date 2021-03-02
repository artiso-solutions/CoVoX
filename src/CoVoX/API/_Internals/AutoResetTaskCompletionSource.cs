using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Covox
{
    internal class AutoResetTaskCompletionSource<TResult>
    {
        private readonly TaskCreationOptions _creationOptions;
        private TaskCompletionSource<TResult> _tcs;

        public AutoResetTaskCompletionSource()
        {
            _creationOptions = TaskCreationOptions.RunContinuationsAsynchronously;
            _tcs = new TaskCompletionSource<TResult>(_creationOptions);
        }

        public Task<TResult> Task => _tcs.Task;

        private void InitTcs() => _tcs = new TaskCompletionSource<TResult>(_creationOptions);

        public bool TrySetCanceled()
        {
            var ok = _tcs.TrySetCanceled();
            InitTcs();
            return ok;
        }

        public bool TrySetCanceled(CancellationToken cancellationToken)
        {
            var ok = _tcs.TrySetCanceled(cancellationToken);
            InitTcs();
            return ok;
        }

        public bool TrySetException(IEnumerable<Exception> exceptions)
        {
            var ok = _tcs.TrySetException(exceptions);
            InitTcs();
            return ok;
        }

        public bool TrySetException(Exception exception)
        {
            var ok = _tcs.TrySetException(exception);
            InitTcs();
            return ok;
        }

        public bool TrySetResult(TResult result)
        {
            var ok = _tcs.TrySetResult(result);
            InitTcs();
            return ok;
        }
    }
}
