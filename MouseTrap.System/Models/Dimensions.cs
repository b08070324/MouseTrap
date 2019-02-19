namespace MouseTrap.Models
{
	public class Dimensions
	{
		public double Left { get; set; }
		public double Top { get; set; }
		public double Right { get; set; }
		public double Bottom { get; set; }

		public Dimensions GetPaddedDimensions(Dimensions padding)
		{
			return new Dimensions
			{
				Left = Left + padding.Left,
				Top = Top + padding.Top,
				Right = Right - padding.Right,
				Bottom = Bottom - padding.Bottom
			};
		}

		public override string ToString()
		{
			return $"{Left}, {Top}, {Right}, {Bottom}";
		}
	}
}
