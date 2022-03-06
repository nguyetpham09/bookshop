using System;

namespace BookShop.Data.Infrastructure
{
    public class Disposable : IDisposable
    {
        public bool isDisposable { get; set; }

        ~Disposable()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposable && disposing)
            {
                DisposeCore();
            }

            isDisposable = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeCore() { }
    }
}
