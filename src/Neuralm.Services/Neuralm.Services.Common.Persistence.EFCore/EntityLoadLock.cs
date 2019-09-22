using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.Common.Persistence.EFCore
{
    /// <summary>
    /// Represents the <see cref="EntityLoadLock"/> class.
    /// Basically a copy of the AsyncLock class from EntityFrameWorkCore internal usage.
    /// </summary>
    public sealed class EntityLoadLock
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        private readonly Releaser _releaser;
        private readonly Task<Releaser> _releaserTask;
        private static EntityLoadLock _sharedLoadLock;

        /// <summary>
        /// Gets the shared <see cref="EntityLoadLock"/> instance.
        /// </summary>
        public static EntityLoadLock Shared => (_sharedLoadLock ??= new EntityLoadLock());

        /// <summary>
        /// Initializes an instance of the <see cref="EntityLoadLock"/> class.
        /// </summary>
        private EntityLoadLock()
        {
            _releaser = new Releaser(this);
            _releaserTask = Task.FromResult(_releaser);
        }

        /// <summary>
        /// Locks the lock asynchronously and returns a disposable struct for releasing the lock.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with type parameter <see cref="Releaser"/>, a disposable struct for releasing the lock.</returns>
        public Task<Releaser> LockAsync(CancellationToken cancellationToken = default)
        {
            Task task = _semaphore.WaitAsync(cancellationToken);
            return !task.IsCompleted
                ? task.ContinueWith((_, state) => ((EntityLoadLock)state)._releaser, this, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default)
                : _releaserTask;
        }

        /// <summary>
        /// Locks the lock synchronously and returns a disposable struct for releasing the lock.
        /// </summary>
        /// <returns>Returns a disposable struct for releasing the lock.</returns>
        public Releaser Lock()
        {
            _semaphore.Wait();
            return _releaser;
        }

        /// <summary>
        /// Represents the <see cref="Releaser"/> struct, a disposable lock releaser.
        /// </summary>
        public readonly struct Releaser : IDisposable
        {
            private readonly EntityLoadLock _toRelease;

            /// <summary>
            /// Initializes the <see cref="Releaser"/> struct.
            /// </summary>
            /// <param name="toRelease"></param>
            internal Releaser(EntityLoadLock toRelease)
            {
                _toRelease = toRelease;
            }

            /// <summary>
            /// Disposes the lock, i.e. releases the lock.
            /// </summary>
            public void Dispose()
            {
                _toRelease._semaphore.Release();
            }
        }
    }
}
