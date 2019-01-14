namespace MouseTrap.Models
{
	public class TrapMargin : ModelWithNotification
	{
		private int value = 0;

		public int Value
		{
			get { return value; }
			set { SetField(ref this.value, value); }
		}
	}
}
