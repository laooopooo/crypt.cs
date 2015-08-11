/// @file
/// Implementation of the `Crypt.Windows.MainWindow` class.

namespace Crypt.Windows {
  using System;
  using System.Globalization;
  using System.IO;
  using System.Linq;
  using System.Reflection;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;
  using System.Windows.Input;

  using Messages=Properties.Resources;
  using Properties;

  using MiniFramework;
  using MiniFramework.Drawing;
  using MiniFramework.Reflection;
  using MiniFramework.Text;
  using MiniFramework.Windows;

  /// The main window.
  public partial class MainWindow: Window {

    /// Initializes a new instance of the class.
    public MainWindow() {
      this.InitializeComponent();

      // Culture selectors.
      foreach(var item in Settings.Default.SupportedLanguages) this.CultureMenu.Cultures.AddLanguage(item, false);
      this.CultureMenu.SelectedCulture=CultureInfo.CurrentUICulture;
      this.CultureMenu.SelectedCultureChanged+=this.OnSelectedCultureChanged;

      var cultureButton=this.CultureMenu.ToCultureButton();
      cultureButton.Tag="CultureControlStatusTip";
      this.ToolBar.Items.Insert(this.ToolBar.Items.IndexOf(this.AboutButton), cultureButton);

      // Menu items.
      var product=Application.Current.Properties["Product"].ToString();

      foreach(MenuItem topLevelItem in this.MenuBar.Items) {
        foreach(var item in topLevelItem.Items.OfType<MenuItem>()) {
          var property=(item.Tag!=null) ? item.Tag.ToString() : ((RoutedCommand) item.Command).Name+"CommandStatusTip";
          
          var text=Reflector.GetPropertyValue(typeof(Messages), property).ToString();
          if(text.Contains("{0}")) text=string.Format(CultureInfo.CurrentCulture, text, product);

          this.SetStatusTip(item, text);
        }
      }

      // Toolbar buttons.
      foreach(var item in this.ToolBar.Items.OfType<ButtonBase>()) {
        var property=(item.Tag!=null) ? item.Tag.ToString() : ((RoutedCommand) item.Command).Name+"CommandStatusTip";
          
        var text=Reflector.GetPropertyValue(typeof(Messages), property).ToString();
        if(text.Contains("{0}")) text=string.Format(CultureInfo.CurrentCulture, text, product);

        this.SetStatusTip(item, text);
      }

      // Window controls.
      this.SetStatusTip(this.EncodeButton, Messages.EncodeButtonStatusTip);
      this.SetStatusTip(this.InputTextBox, Messages.InputTextBoxStatusTip);
      this.SetStatusTip(this.InputComboBox, Messages.InputComboBoxStatusTip);
      this.SetStatusTip(this.OutputTextBox, Messages.OutputTextBoxStatusTip);
    }
  
    /// @var ShowHideCommand
    /// The "Show/Hide Control" command.
    public static readonly RoutedUICommand ShowHideCommand=new RoutedUICommand();

    /// Shows the "About" box.
    /// @param sender The source of the event.
    /// @param e The event data.
    private void OnAboutExecuted(object sender, ExecutedRoutedEventArgs e) {
      var startupPath=Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

      var license=new License {
        Name="The MIT License",
        Text=File.ReadAllText(Path.Combine(startupPath, "LICENSE.txt"))
      };

      var window=new AboutBox() {
        Authors=Application.Current.Properties["Authors"].ToString(),
        Copyright=string.Format(CultureInfo.CurrentCulture, Messages.ApplicationCopyright, new AssemblyInfo(this.GetType().Assembly).Copyright),
        Description=Messages.ApplicationDescription,
        Logo=Properties.Resources.ApplicationIcon.ToBitmapImage(48),
        License=license,
        Owner=this,
        WebSite=Settings.Default.WebSite
      };

      window.ShowDialog();
    }

    /// Closes the window.
    /// @param sender The source of the event.
    /// @param e The event data.
    private void OnCloseExecuted(object sender, ExecutedRoutedEventArgs e) {
      this.Close();
      e.Handled=true;
    }

    /// Displays in the status bar the text associated to a control.
    /// @param sender The source of the event.
    /// @param e The event data.
    private void OnControlMouseEnter(object sender, MouseEventArgs e) {
      this.StatusImage.Visibility=Visibility.Visible;
    }

    /// Resets the status bar.
    /// @param sender The source of the event.
    /// @param e The event data.
    private void OnControlMouseLeave(object sender, MouseEventArgs e) {
      this.StatusImage.Visibility=Visibility.Hidden;
      this.StatusLabel.Text=string.Empty;
    }

    /// Encodes the string from the input field and puts the results into the output field.
    /// @param sender The source of the event.
    /// @param e The event data.
    private void OnEncodeButtonClick(object sender, RoutedEventArgs e) {
      this.OutputTextBox.Text=EncoderManager.Encoders[this.InputComboBox.SelectedIndex].Encode(this.InputTextBox.Text);
      e.Handled=true;
    }

    /// Opens an URL in the default browser.
    /// @param sender The source of the event.
    /// @param e The event data.
    private void OnOpenUrlExecuted(object sender, ExecutedRoutedEventArgs e) {
      new Uri(Settings.Default.WebSite, e.Parameter as Uri).Open();
      e.Handled=true;
    }

    /// Shows or hides a control.
    /// @param sender The source of the event.
    /// @param e The event data.
    private void OnShowHideExecuted(object sender, ExecutedRoutedEventArgs e) {
      if(e.Parameter!=null) {
        var control=this.FindName(e.Parameter.ToString()) as Control;
        if(control!=null) control.Visibility=(control.IsVisible) ? Visibility.Collapsed : Visibility.Visible;
        e.Handled=true;
      }
    }

    /// Changes the application language and restarts it.
    /// @param sender The source of the event.
    /// @param e The event data.
    private void OnSelectedCultureChanged(object sender, DependencyPropertyChangedEventArgs e) {
      var message=Messages.RestartProgramInfo.Split('|');
      TaskDialog.Show(null, message[1], message[0], null, MessageBoxButton.OK, MessageBoxImage.Information);

      Settings.Default.Culture=this.CultureMenu.SelectedCulture;
      Application.Current.Restart();
    }

    /// Sets the text to display in the status bar when the mouse hovers the specified control.
    /// @param control The control associated to the specified text.
    /// @param text The text to display in the status bar.
    /// @exception System.ArgumentNullException The specified control is `null`.
    private void SetStatusTip(Control control, string text) {
      if(control==null) throw new ArgumentNullException("control");
      control.MouseEnter+=this.OnControlMouseEnter;
      control.MouseEnter+=delegate { this.StatusLabel.Text=text; };
      control.MouseLeave+=this.OnControlMouseLeave;
    }
  }
}