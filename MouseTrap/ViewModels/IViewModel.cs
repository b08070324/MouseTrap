namespace MouseTrap.ViewModels
{
	// Interface for the main content area view
	// This allows us to change the view in MainWindow by swapping the model
	// However DataTemplate->DataType->x:Type only supports classes
	// To use automatic selection of views using ContentControl and DataTemplates
	// the live+design viewmodels inherit from an abstract model
	public interface IViewModel
	{
	}
}
