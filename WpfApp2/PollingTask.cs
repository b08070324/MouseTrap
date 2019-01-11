using System;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp2
{
	public class PollingTask
	{
		private int PollFrequency { get; set; }
		private CancellationTokenSource cancellationTokenSource;
		public Task Task { get; private set; }

		public PollingTask(int pollFrequency)
		{
			PollFrequency = pollFrequency;
		}

		private void ProcessAction(CancellationToken token, Action work, Action finished)
		{
			while (true)
			{
				// Break loop if cancellation requested
				if (token.IsCancellationRequested)
				{
					break;
				}

				// Do work
				work.Invoke();

				// Sleep for a while
				Thread.Sleep(PollFrequency);
			}

			// Invoke final action
			finished.Invoke();
		}

		public bool StartPolling(Action work, Action finished)
		{
			if (cancellationTokenSource == null)
			{
				// Create cancellation token source
				cancellationTokenSource = new CancellationTokenSource();

				// Start task
				Task = Task.Factory.StartNew(
					() => ProcessAction(cancellationTokenSource.Token, work, finished),
					cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

				// Finished
				return true;
			}

			// The task wasn't started
			return false;
		}

		public void StopPolling()
		{
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
				cancellationTokenSource.Dispose();
				cancellationTokenSource = null;
			}
		}

		public bool IsRunning
		{
			get
			{
				return (Task != null && Task.IsCompleted == false);
			}
		}
	}
}
